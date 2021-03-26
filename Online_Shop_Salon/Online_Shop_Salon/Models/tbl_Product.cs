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
    
    public partial class tbl_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Product()
        {
            this.tbl_Invoice_Detail = new HashSet<tbl_Invoice_Detail>();
            this.tbl_Photo = new HashSet<tbl_Photo>();
        }
    
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Made_In { get; set; }
        public string Made_Year { get; set; }
        public decimal Price_Per { get; set; }
        public int Quantity_Stock { get; set; }
        public int StoreId { get; set; }
        public int Category_Id { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual tbl_Category tbl_Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Invoice_Detail> tbl_Invoice_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Photo> tbl_Photo { get; set; }
        public virtual tbl_Store tbl_Store { get; set; }
    }
}
