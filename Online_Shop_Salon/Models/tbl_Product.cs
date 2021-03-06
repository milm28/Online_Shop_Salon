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

    public partial class tbl_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Product()
        {
            this.tbl_Invoice_Detail = new HashSet<tbl_Invoice_Detail>();
            this.tbl_Photo = new HashSet<tbl_Photo>();
        }

        public int Product_Id { get; set; }
        [Display(Name = "Ime Proizvda")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50,ErrorMessage ="Maksimalno moze 50 karaktera!")]
        //[RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Koristi samo slova")]
        public string Product_Name { get; set; }

        [Display(Name = "Zemlja Porekla")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50, ErrorMessage = "Maksimalno moze 50 karaktera!")]
        public string Made_In { get; set; }

        [Display(Name = "Godina Proizvodnje")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(4, ErrorMessage = "Unesi samo godinu yyyy!")]
        public string Made_Year { get; set; }

        [Display(Name = "Cena po Komadu")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [Range(0.0, 1000000, ErrorMessage = "Cena ne moze da bude preko 1000000.00!")]
        //[DataType(DataType.Currency,ErrorMessage ="Nije validna Cena ")]
        [RegularExpression(@"^\d+\.\d{0,2}$",ErrorMessage ="Broj mora biti decimalan")]
        public decimal Price_Per { get; set; }

        [Display(Name = "Kolicina u salonu")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [Range(0, 1000, ErrorMessage = "Maksimalno mozete da unesete 500 komada")]
       
        public int Quantity_Stock { get; set; }

        [Display(Name = "Izaberi Salon")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public int StoreId { get; set; }

        [Display(Name = "Izaberi Kategoriju")]
        [Required(ErrorMessage = "Polje je obavezno")]
        public int Category_Id { get; set; }
        [Display(Name = "Status")]
        public bool Status { get; set; }

        public virtual tbl_Category tbl_Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Invoice_Detail> tbl_Invoice_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Photo> tbl_Photo { get; set; }
        public virtual tbl_Store tbl_Store { get; set; }
    }
}
