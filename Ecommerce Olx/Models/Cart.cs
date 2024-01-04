using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Olx.Models
{
    public class Cart
    {
        public int product_ID { get; set; }
        public string product_NAME { get; set; }
        public string product_IMAGE { get; set; }
        public string product_PRICE { get; set; }



        public int order_quantity { get; set; }
        public Nullable<int> order_Sub_Total { get; set; }

    }
}