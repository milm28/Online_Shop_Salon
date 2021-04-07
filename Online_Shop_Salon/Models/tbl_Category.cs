

namespace Online_Shop_Salon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class tbl_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Category()
        {
            this.tbl_Category1 = new HashSet<tbl_Category>();
            this.tbl_Product = new HashSet<tbl_Product>();
        }
    
        public int Category_Id { get; set; }
        [Display(Name = "Ime Kategorije")]
        [Required(ErrorMessage = "Polje je obavezno")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Koristi samo slova")]
        public string Category_Name { get; set; }

        [StringLength(550,ErrorMessage ="Maksimalno je 550 karaktera!")]
        public string Description { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Slika Kategorije")]
        public string Category_Image { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Category> tbl_Category1 { get; set; }
        public virtual tbl_Category tbl_Category2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Product> tbl_Product { get; set; }

        /// <summary>
        /// koristimo a upload slike na ceate new catergory
        /// </summary>
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
