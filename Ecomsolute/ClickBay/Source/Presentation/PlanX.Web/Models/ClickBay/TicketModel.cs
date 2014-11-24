using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlanX.Core;
namespace PlanX.Web.Models.ClickBay
{
    public class TicketModel:BaseEntity
    {
        public string FromId { get; set; }
        public string ToId { get; set; }
        public DateTime DateBook { get; set; }
        public decimal Price { get; set; }

        public string ImgBrand { get; set; }
        public string BrandCode { get; set; }
       
    }
}