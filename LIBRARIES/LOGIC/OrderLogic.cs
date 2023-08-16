using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC
{
    public class OrderLogic : BASELOGICCLASS, IOrderLogic
    {
        public OrderLogic(
            actions.Imsactions a,
            repo.IAccount_REPO acc,
            repo.IProduct_REPO p,
            repo.IOrder_REPO o
        ) : base(a, acc, p, o) { }

        public async Task<bool> CreateOrder(
            models.ProductType? type,
            string? category,
            decimal? ammount,
            string? desc,
            models.OrderStatus? orderstatus,
            string? paymentmethod,
            string? accid,
            string? prodid
        )
        {
            if (
                type != null
                && category != null
                && ammount != null
                && desc != null
                && orderstatus != null
                && paymentmethod != null
                && accid != null
                && prodid != null
            )
            {
                return await this.order.CreateOrder(
                    (models.ProductType)type,
                    (string)category,
                    (decimal)ammount,
                    (string)desc,
                    (models.OrderStatus)orderstatus,
                    (string)paymentmethod,
                    new Guid(this.actions.v2_Decrypt(accid)),
                    new Guid(this.actions.v2_Decrypt(prodid))
                );
            }
            else
            {
                return false;
            }
        }
    }

    public interface IOrderLogic
    {
        Task<bool> CreateOrder(
            models.ProductType? type,
            string? category,
            decimal? ammount,
            string? desc,
            models.OrderStatus? orderstatus,
            string? paymentmethod,
            string? accid,
            string? prodid
        );
    }
}
