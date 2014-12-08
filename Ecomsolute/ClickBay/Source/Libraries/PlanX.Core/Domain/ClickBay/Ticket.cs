using System;
using System.Collections.Generic;

namespace PlanX.Core.Domain.ClickBay
{
  public  class Ticket:BaseEntity
    {
             /* Id	Int	Id của hành trình tìm được	1
        FlightNumber	String	Mã chuyến bay	VJ8662
        Description	String	Chưa sử dụng	
        Airline	String	Hãng	VietJetAir
        AirlineCode	String	Mã hãng	VietJetAir
        DepartTime	DateTime	Giờ cất cánh	2014-05-24T07:10:00
        LandingTime	DateTime	Giờ hạ cánh	2014-05-24T07:10:00
        FlightDuration	TimeSpan	Thời gian bay	PT2H5M
        FlightDuration	TimeSpan	Thời gian bay	PT2H5M
        FromAirport	String	Sân bay đi	Tân Sân Nhất
        FromAirportId	Int	Mã sân bay đi	1
        ToAirport	String	Sân bay đến	Nội Bài
        ToAirportId	Int	Mã sân bay đến	2
        FromPlace	String	Nơi đi	Hồ Chí Minh

        FromPlaceId	Int	Mã nơi đi	1
        ToPlace	String	Nơi đến	Hà Nội
        ToPlaceId	Int	Mã nơi đến	2
        Price	Decimal	Giá vé NET	1694000.00
        TotalPrice	Decimal	Tổng giá	1694000.00
        Stops	INT	Số chặng dừng	0 hay 1, 2 …
        Filter		Nối chăng abacus	
        FareBasis	String	Hạng ghế ng ồi	
        Details	FlightDetailDto (Navigation)	Danh sách các vé khác hạng	
        TicketPriceDetails	TicketPriceDetailDto (Navigation)	Chi tiết giá vé	
        TicketOptions	TicketOptionDto	Các giá vé khác	*/


      private ICollection<TicketPriceDetailDto> _ticketDetails;
      private ICollection<FlightDetailDto> _flightDetailDto;
      /// <summary>
        /// Mã chuyến bay	VJ8662
      /// </summary>
        public string FlightNumer { get; set; }
        public string Description { get; set; }
        public string Airline { get; set; }
        public string AirlineCode { get; set; }

        public  DateTime DepartTime { get; set; }
        public DateTime  LandingTime { get; set; }
        public String FlightDuration { get; set; }
        public string FromAirport { get; set; }
        public int FromAirportId { get; set; }
        public string ToAirport { get; set; }
        public int ToAirportId { get; set; }
        public string FromPlace { get; set; }
        public int FromPlaceId { get; set; }
        public string ToPlace { get; set; }
        public int ToPlaceId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int Stops { get; set; }
        public string Filter { get; set; }
        public string FareBasis { get; set; }
        public ICollection<TicketPriceDetailDto> TicketPriceDetailDto { 
            
            get{return _ticketDetails??(_ticketDetails= new List<TicketPriceDetailDto>());}
            protected set { _ticketDetails = value; }
        
        }
        public ICollection<FlightDetailDto> FlightDetailDto
        {
            get { return _flightDetailDto ?? (_flightDetailDto = new List<FlightDetailDto>()); }
            protected set { _flightDetailDto = value; }
        }




    }
}
