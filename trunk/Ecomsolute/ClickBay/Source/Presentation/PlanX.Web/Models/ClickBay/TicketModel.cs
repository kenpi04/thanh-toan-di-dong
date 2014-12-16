using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlanX.Core;
namespace PlanX.Web.Models.ClickBay
{
    public class TicketModel
    {
        public TicketModel()
        {
            ArilinesBaggageConditions = new List<ArilinesBaggageCondition>();
            AirlinesConditions = new List<AirlinesConditionModel>();
            BookingFlightPriceModels = new List<BookingFlightPriceModel>();
        }
        public string Id { get; set; }

        public int Index { get; set; }
        public string FromCode { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string ToCode { get; set; }
        public DateTime DateBook { get; set; }
        public decimal Price { get; set; }

        public string ImgBrand { get; set; }
        public string BrandCode { get; set; }

        public string FlightNumber { get; set; }

        public DateTime DepartTime { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime LandingTime { get; set; }

        public double FlightDuration { get; set; }

        public string FromAirport { get; set; }

        public string ToAirport { get; set; }

        public string Currency { get; set; }

        public string FromAirportCode { get; set; }

        public string FromCountry { get; set; }

        public string FromPlace { get; set; }

        public string ToPlace { get; set; }

        public string ToCountry { get; set; }

        public string ToAirportCode { get; set; }

        public string FareBasis { get; set; }

        public string AirlineName { get; set; }



        public string AirlineCode { get; set; }

        public string TicketType { get; set; }

        public int AirlineId { get; set; }
        public Int16 Adult { get; set; }
        public Int16 Child { get; set; }
        public Int16 Infant { get; set; }        
        public List<ArilinesBaggageCondition> ArilinesBaggageConditions { get; set; }
        public List<AirlinesConditionModel> AirlinesConditions { get; set; }

        public List<BookingFlightPriceModel> BookingFlightPriceModels { get; set; }
        public class ArilinesBaggageCondition
        {
            public int Id { get; set; }
            public int Baggage { get; set; }
            public decimal BaggageFee { get; set; }
            public int DisplayOrder { get; set; }
            public bool IsHandLuggage { get; set; }
            public bool IsFree { get; set; }
            public string Description { get; set; }
        }
        public class AirlinesConditionModel
        {
            public string ConditionName { get; set; }
            public string ConditionDescription { get; set; }
        }
        public class BookingFlightPriceModel
        {
            /*Code:    "PassengerType" : "ADT",
                   "Description" : "Giá vé người lớn",
                   "Code" : "NET",
                   "Quantity" : 1,
                   "Price" : 895000.0000,
                   "Total" : 895000.0000
            */

            public string PassengerType { get; set; }
            public string Code { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public Int16 Quantity { get; set; }
            public decimal Total { get; set; }

        }

        public Int16 Stops { get; set; }

        public string FlightDurationString { get; set; }
    }
}