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
    
    public partial class Service:BaseEntity
    {
        public Service()
        {
            this.Providers = new HashSet<Provider>();
        }
    
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public bool Published { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
    
        public virtual ICollection<Provider> Providers { get; set; }
    }
}
