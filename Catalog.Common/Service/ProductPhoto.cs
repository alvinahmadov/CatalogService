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
    
    public partial class ProductPhoto
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string FileName { get; set; }
        public byte[] ThumbNailPhoto { get; set; }
        public byte[] LargePhoto { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
