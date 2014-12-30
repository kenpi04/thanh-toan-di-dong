using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
    /*"Airline" : "",
    "AirlineCode" : "",
    "FlightDuration" : "2 giờ 0 phút",
    "FlightNumber" : "3K 558",
    "From" : "15:45, 15/1/2015 - Thành phố Hồ Chí Minh",
    "To" : "18:45, 15/1/2015 - Singapore",
    "DepartTime" : "",
    "LandingTime" : ""
     */
    public class FlightDetailDto:BaseEntity
    {
        public string Airline { get; set; }
        public string AirlineCode { get; set; }        
        public string FlightDuration { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string DepartTime { get; set; }
        public string LandingTime { get; set; }
    }
}
