using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC
{
    public class ProductLogic : BASELOGICCLASS, IProductLogic
    {
        public ProductLogic(actions.Imsactions a, repo.IAccount_REPO acc, repo.IProduct_REPO p)
            : base(a, acc, p) { }

        public async Task<IEnumerable<models.Product>> GetProducts()
        {
            var products = await prod.GetProducts();
            return products;
        }

        public async Task<bool> CreateProduct(
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
            if (
                !string.IsNullOrWhiteSpace(category)
                && !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(image)
                && price != null 
                && !string.IsNullOrWhiteSpace(description)
                && productstatus != null
                && size != null
                && color != null
                && productType != null
                && deliveryMethod != null
                && productSubscriptionType != null
                && inventory != null
                && accountid != null
            )
            {
                return await prod.CreateProduct(
                    category:category,
                    name:name,
                    image:image,
                    price:(decimal)price,
                    description:description,
                    productstatus:(models.ProductStatus)productstatus,
                    size:(int)size,
                    color:color,
                    productType:(MODELS.ProductType)productType,
                    deliveryMethod:(MODELS.ProductDeliveryMethod)deliveryMethod,
                    productSubscriptionType:(MODELS.ProductSubscriptionType)productSubscriptionType,
                    inventory:(int)inventory,
                    accountid:new Guid(actions.v2_Decrypt(accountid))
                );
            }
            else
            {
                return false;
            }
        }
    }

    public interface IProductLogic { 
        Task<IEnumerable<models.Product>> GetProducts();
        Task<bool> CreateProduct(
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
        );
    }
}
