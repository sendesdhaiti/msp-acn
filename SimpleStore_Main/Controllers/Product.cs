using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using System.Net.Http;
using Microsoft.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace SimpleStore_Main.Controllers
{
    [EnableCors("MyAllowAllOrigins")]
    [ApiController]
    [Route("api/[controller]")]
    public class Product : ControllerBase
    {
        private IConfiguration config;
        private string printful_api_url;
        private string printful_api_key;
        private readonly actions.Imsactions actions;
        private readonly logic.IAccountLogic accounts;
        private readonly logic.IProductLogic products;
        static readonly HttpClient client = new HttpClient();

        public Product(
            actions.Imsactions a,
            logic.IAccountLogic acc,
            logic.IProductLogic p,
            IConfiguration iConfig
        )
        {
            actions = a;
            accounts = acc;
            products = p;
            config = iConfig;
            printful_api_url = config
                .GetSection("ConnectionStrings")
                .GetSection("Printful_URL")
                .Value;
            printful_api_key = config
                .GetSection("ConnectionStrings")
                .GetSection("Printful_API")
                .Value;
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await this.products.GetProducts());
        }

        [HttpGet("get-store-products")]
        public async Task GetPrintfulProducts(string url)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    printful_api_key
                );
                using HttpResponseMessage response = await client.GetAsync(this.printful_api_url + "/" + url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // return Ok(await this.products.GetProducts());
        }

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct(
            string? category,
            string? name,
            string? image,
            decimal? price,
            string? description,
            models.ProductStatus? productstatus,
            int? size,
            string? color,
            models.ProductType? productType,
            models.ProductDeliveryMethod? deliveryMethod,
            models.ProductSubscriptionType? productSubscriptionType,
            int? inventory,
            string? accountid
        )
        {
            var request = Request.Headers["request"];
            if (!string.IsNullOrEmpty(request))
            {
                var product = this.actions.ConvertJsonObj<models.Product>(request);
                if (product != null)
                {
                    var cat = this.actions.DecryptFromClient(product.category);
                    var n = this.actions.DecryptFromClient(product.name);
                    var i = product.image;
                    var p = product.price;
                    var d = this.actions.DecryptFromClient(product.description);
                    var ps = product.productstatus;
                    var s = product.size;
                    var c = product.color;
                    var pt = product.productType;
                    var dm = product.deliveryMethod;
                    var pst = product.productSubscriptionType;
                    var inv = product.inventory;
                    var acc = product.accountid;
                    return Created(
                        "create-product",
                        await this.products.CreateProduct(
                            cat,
                            n,
                            i,
                            p,
                            d,
                            ps,
                            s,
                            c,
                            pt,
                            dm,
                            pst,
                            inv,
                            acc
                        )
                    );
                }
                else
                {
                    return BadRequest(null);
                }
            }
            else
            {
                var cat = this.actions.DecryptFromClient(category);
                var n = this.actions.DecryptFromClient(name);
                var i = image;
                var p = price;
                var d = this.actions.DecryptFromClient(description);
                var ps = productstatus;
                var s = size;
                var c = color;
                var pt = productType;
                var dm = deliveryMethod;
                var pst = productSubscriptionType;
                var inv = inventory;
                var acc = accountid;
                return Created(
                    "create-product",
                    await this.products.CreateProduct(
                        category: cat,
                        name: n,
                        image: i,
                        price: (decimal?)p,
                        description: d,
                        productstatus: ps,
                        size: s,
                        color: c,
                        productType: pt,
                        deliveryMethod: dm,
                        productSubscriptionType: pst,
                        inventory: inv,
                        accountid: acc
                    )
                );
            }
        }
    }
}
