using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Olx.Models
{
    public class ViewDetail_Product
    {

        public int product_ID { get; set; }
        public string product_NAME { get; set; }
        public string product_IMAGE { get; set; }
        public string product_PRICE { get; set; }
        public string product_DESCRIPTION { get; set; }
        public Nullable<int> category_ID_foreign_key { get; set; }
        public Nullable<int> userr_ID_foreign_key { get; set; }


        public int category_ID { get; set; }
        public string category_NAME { get; set; }


        public int userr_ID { get; set; }
        public string userr_NAME { get; set; }
        public string userr_CONTACT { get; set; }
        public string userr_IMAGE { get; set; }
    }
}