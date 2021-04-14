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

    public partial class tbl_Store
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Store()
        {
            this.tbl_Product = new HashSet<tbl_Product>();
            this.tbl_Invoice_Detail = new HashSet<tbl_Invoice_Detail>();
        }
    
        public int Store_Id { get; set; }
        [Display(Name = "Ime Salona")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50)]
        public string Store_Name { get; set; }
        [Display(Name = "Ime Vlasnika")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50)]
        public string Owner_Name { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "Grad")]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "Postanski Broj")]
        [StringLength(5, ErrorMessage = "Pib sadrzi 5 cifara!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Morate uneti brojeve")]
        public string Zip_Code { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "Adresa i broj")]
        [StringLength(50, MinimumLength = 3)]
        public string Address { get; set; }
        [Required(ErrorMessage = "polje je obavezno")]
        [Display(Name = "Broj telefona")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Broj nije validan,10 brojeva sa pozivnim")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "polje je obavezno")]
        [StringLength(250, MinimumLength = 3)]
        public string Web_Site { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "PIB")]
        [StringLength(8, ErrorMessage = "Pib sadrzi 8 cifara!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "UPRN must be numeric")]
        public string PIB { get; set; }

        [Required(ErrorMessage = "Polje je obavezno")]
        [Display(Name = "Bankovni Racun")]
        [StringLength(18, ErrorMessage = "bankovni racun sadrzi 18 cifara!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Morate uneti brojeve")]
        public string Bank_Account { get; set; }
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Product> tbl_Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Invoice_Detail> tbl_Invoice_Detail { get; set; }
    }
}
