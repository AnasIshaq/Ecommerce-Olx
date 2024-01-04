using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce_Olx.Models
{
    public class DisplayCategoryRecordsAdmin
    {





        public int category_ID { get; set; }
        public string category_NAME { get; set; }
        public string category_IMAGE { get; set; }
        public Nullable<int> category_STATUS { get; set; }


        public int admin_ID { get; set; }
        public string admin_NAME { get; set; }





    }
}