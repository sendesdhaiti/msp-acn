using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MODELS
{
    public class Order
    {
        public string id { get; set; }
        public ProductType type { get; set; }
        public string category { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
        public OrderStatus orderstatus { get; set; }
        public string paymentmethod { get; set; }
        public DateTime added { get; set; }
        public DateTime updated { get; set; }
        public string accountid { get; set; }
        public string productid { get; set; }
    }

    


}
