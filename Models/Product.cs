//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ALLINSHOP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public System.Guid PO_ID { get; set; }
        public string PO_Name { get; set; }
        public decimal PO_Price { get; set; }
        public string PO_Pic { get; set; }
        public System.Guid Type_ID { get; set; }
        public System.Guid Hero_ID { get; set; }
    
        public virtual Hero Hero { get; set; }
        public virtual Type Type { get; set; }
    }
}
