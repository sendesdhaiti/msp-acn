using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleStore_Main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Order : ControllerBase
    {
        private readonly actions.Imsactions actions;
        private readonly logic.IAccountLogic accounts;
        private readonly logic.IProductLogic products;
        private readonly logic.IOrderLogic orders;

        public Order(
            actions.Imsactions a,
            logic.IAccountLogic acc,
            logic.IProductLogic p,
            logic.IOrderLogic o
        )
        {
            actions = a;
            accounts = acc;
            products = p;
            orders = o;
        }

        [HttpPost("make-order")]
        public async Task<IActionResult> MakeOrder(models.Order? o)
        {
            var request = Request.Headers["request"];
            if (!string.IsNullOrEmpty(request))
            {
                var order = this.actions.ConvertJsonObj<models.Order>(request);
                if (order != null)
                {
                    var t = order.type;
                    var c = this.actions.DecryptFromClient(order.category);
                    var amm = order.amount;
                    var d = this.actions.DecryptFromClient(order.description);
                    var os = order.orderstatus;
                    var pm = order.paymentmethod;
                    var accid = order.accountid;
                    var prodid = order.productid;
                    return Created("making order", await this.orders.CreateOrder(t, c,amm,d,os,pm,accid,prodid));
                }
                else
                {
                    return BadRequest(null);
                }
            }
            else
            {
                var t = o.type;
                var c = this.actions.DecryptFromClient(o.category);
                var amm = o.amount;
                var d = this.actions.DecryptFromClient(o.description);

                if(d.Length > 30){
                    d = d.Substring(0,30);
                }
                var os = o.orderstatus;
                var pm = o.paymentmethod;
                var accid = o.accountid;
                var prodid = o.productid;
                return Created("making order", await this.orders.CreateOrder(t, c,amm,d,os,pm,accid,prodid));
            }
        }
    }
}
