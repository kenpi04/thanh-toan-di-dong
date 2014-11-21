using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
    public class FlightDetailDto:BaseEntity
    {
         /* "FlightDuration": "2 giờ 5 phút",
                    "FlightNumber": "BL  790",
                    "From": "6:15, 15/6/2014 - Thành phố Hồ Chí Minh",
                    "Id": 1250,
                    "To": "8:20, 15/6/2014 - Hà Nội"*/
        public string FlightDuration { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
