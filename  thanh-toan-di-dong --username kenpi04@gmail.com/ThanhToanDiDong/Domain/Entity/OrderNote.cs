//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderNote:BaseEntity
    {
        public int OrderId { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string FunctionName { get; set; }
        public Nullable<int> FunctionReturnCode { get; set; }
        public string FunctionMessage { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
