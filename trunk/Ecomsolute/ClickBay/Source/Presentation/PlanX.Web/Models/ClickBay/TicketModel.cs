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

        public string FlightNumber { get; set; }

        public DateTime DepartTime { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime LandingTime { get; set; }

        public string FlightDuration { get; set; }

        public string FromAirport { get; set; }

        public string ToAirport { get; set; }

        public string  Currency { get; set; }

        public string FromAirportCode { get; set; }

        public string FromCountry { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public string ToCountry { get; set; }

        public string ToAirportCode { get; set; }

        public string HangVe { get; set; }

        public string AirlineName { get; set; }

       

        public string AirlineId { get; set; }

        public string TicketType { get; set; }
        public List<ArilinesBaggageCondition> ArilinesBaggageConditions { get; set; }
        public List<AirlinesConditionModel> AirlinesConditions { get; set; }
        public class ArilinesBaggageCondition
        {
            public int Baggage { get; set; }
            public decimal BaggageFee { get; set; }
        }
        public class AirlinesConditionModel
        {
            public string ConditionName { get; set; }
            public string ConditionDescription { get; set; }
        }
    }
}