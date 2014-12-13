using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
  public  class TicketPriceDetailDto:BaseEntity
    {
        /*"Code": "NET",
                      "Description": "Giá vé",
                      "Id": 3971,
                      "Price": "1130000.00",
                      "Quantity": 1,
                      "Total": "1130000.00"
        */
        public string Code { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }

    }
}
