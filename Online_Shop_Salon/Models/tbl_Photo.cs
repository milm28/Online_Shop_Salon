//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Online_Shop_Salon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_Photo
    {
        public int Image_Id { get; set; }

        [Display(Name = "Slika")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public string Image_Name { get; set; }
        public bool Status { get; set; }
        public int Product_Id { get; set; }
        public bool Main_Image { get; set; }
    
        public virtual tbl_Product tbl_Product { get; set; }
    }
}