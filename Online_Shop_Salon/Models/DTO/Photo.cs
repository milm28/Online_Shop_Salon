
namespace Online_Shop_Salon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class tbl_Photo
    {

       
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
