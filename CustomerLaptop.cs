//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaptopsLLC
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustomerLaptop
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int LaptopId { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Laptop Laptop { get; set; }
    }
}
