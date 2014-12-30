using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlanX.Core;
using PlanX.Core.Domain.ClickBay;
namespace PlanX.Web.Models.ClickBay
{
    public class TicketModel
    {
        public TicketModel()
        {
            ArilinesBaggageConditions = new List<ArilinesBaggageCondition>();
            AirlinesConditions = new List<AirlinesConditionModel>();
            BookingFlightPriceModels = new List<BookingFlightPriceModel>();
            this.FlightInfoDetails = new List<FlightInfoDetail>();
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
        /// <summary>
        /// Dieu kien hanh ly
        /// </summary>
        public List<ArilinesBaggageCondition> ArilinesBaggageConditions { get; set; }
        /// <summary>
        /// dieu kien gia ve
        /// </summary>
        public List<AirlinesConditionModel> AirlinesConditions { get; set; }
        /// <summary>
        /// Chi tiet gia ve
        /// </summary>
        public List<BookingFlightPriceModel> BookingFlightPriceModels { get; set; }
        /// <summary>
        /// Chi tiet gia ve tong hop theo hanh khach: hanh khach - so luong - gia - thue&phi - giam gia - giamgia
        /// </summary>
        public List<TotalPriceShow> TotalPriceShows { get; set; }
        /// <summary>
        /// Chi tiet thong tin bay theo chang: neu co > 1 lan dung(Stops)
        /// </summary>
        public List<FlightInfoDetail> FlightInfoDetails { get; set; }

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

            public PassengerType? PassengerTypeEnum
            {
                get
                {
                    PassengerType pas;
                    if (Enum.TryParse(this.PassengerType, out pas))
                    {
                        return pas;
                    }
                    return null;
                }
            }
        }

        public Int16 Stops { get; set; }

        public string FlightDurationString { get; set; }

        public class TotalPriceShow
        {
            /// <summary>
            /// Loai hanh khach
            /// </summary>
            public string PassengerType { get; set; }
            /// <summary>
            /// Gia Net
            /// </summary>
            public decimal Price { get; set; }
            /// <summary>
            /// So luong hanh khach
            /// </summary>
            public Int16 Quantity { get; set; }
            /// <summary>
            /// Thue va Phi
            /// </summary>
            public decimal TaxAndFee { get; set; }
            /// <summary>
            /// Giam gia
            /// </summary>
            public decimal DiscountAmount { get; set; }
        }

        /*"Airline" : "",
        "AirlineCode" : "",
        "FlightDuration" : "2 giờ 0 phút",
        "FlightNumber" : "3K 558",
        "From" : "15:45, 15/1/2015 - Thành phố Hồ Chí Minh",
        "To" : "18:45, 15/1/2015 - Singapore",
        "DepartTime" : "",
        "LandingTime" : ""
         */
        public class FlightInfoDetail
        {
            public string Airline { get; set; }
            public string AirlineCode { get; set; }
            public string FlightDuration { get; set; }
            public string FlightNumber { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string DepartTime { get; set; }
            public string LandingTime { get; set; }
            public string FromPlace { get; set; }
            public string ToPlace { get; set; }
        }

    }
}