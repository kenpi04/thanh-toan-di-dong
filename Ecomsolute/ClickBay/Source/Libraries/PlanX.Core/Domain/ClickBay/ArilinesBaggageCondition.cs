//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanX.Core.Domain.ClickBay
{
    using System;
    using System.Collections.Generic;
    
    public partial class ArilinesBaggageCondition:BaseEntity
    {
     
        public string AirlinesId { get; set; }
        public int Baggage { get; set; }
        public decimal BaggageFee { get; set; }
        public int DisplayOrder { get; set; }
    }
}
