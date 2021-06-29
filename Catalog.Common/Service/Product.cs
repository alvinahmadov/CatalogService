//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Catalog.Common.Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Description = "\"\"";
            this.SpecialOfferProducts = new HashSet<SpecialOfferProduct>();
        }
    
        public int ID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> ProductSubcategoryID { get; set; }
        public string ArticleNumber { get; set; }
        public string Code { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public System.Guid rowguid { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        public virtual ProductSubcategory ProductSubcategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialOfferProduct> SpecialOfferProducts { get; set; }
        public virtual ProductPhoto ProductPhoto { get; set; }
        public virtual ProductInventory ProductInventories { get; set; }
        public virtual ShoppingCartItem ShoppingCartItem { get; set; }
    }
}
