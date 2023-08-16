using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MODELS
{
    public class Product
    {
        public string id { get; set; }
        public string? category { get; set; }
        public string? name { get; set; }
        public string image { get; set; }
        public decimal price { get; set; }
        public string? description { get; set; }
        public ProductStatus productstatus { get; set; }
        public int size { get; set; }
        public string? color { get; set; }

        public ProductType productType { get; set; }
        public ProductDeliveryMethod deliveryMethod { get; set; }
        public ProductSubscriptionType productSubscriptionType { get; set; }
        public int inventory { get; set; }
        public DateTime added { get; set; }
        public DateTime updated { get; set; }

        public string accountid { get; set; }
    }
}
