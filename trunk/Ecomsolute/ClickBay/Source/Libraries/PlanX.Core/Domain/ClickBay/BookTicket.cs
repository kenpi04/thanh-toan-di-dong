using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
 public   class BookTicket
    {
        /*
          "Brand": "JetStar",			//  kiểu chuỗi – hãng hàng không 
         "Adult": 1, 				// Kiểu số 
        "Child": 1, 				// Kiểu số
        "Infant": 1, 				// Kiểu số
        "RoundTrip": false, 			// Kiểu boolean ( true, false )
        "DepartDate": "2014-10-21T00:00:00", 		//Ngày đi - Kiểu datetime
        "ReturnDate": "2014-10-21T00:00:00", 		//Ngày về - Kiểu datetime
        "FlightNumber": "BL 790", 				// Mã chuyến bay - Kiểu chuỗi
        "TicketPrice": "690000", 				// Giá - Kiểu chuỗi
        "TicketType": "Starter",				// Loại vé - Kiểu chuỗi
        "FromPlaceId": 1, 				// Mã nơi đi - Kiểu số ( không phải VNA, abacus)
        "ToPlaceId": 2,				// Mã nơi đén - Kiểu số ( không phải VNA, abacus)
        "FromPlaceCode": "SGN",			// Mã nơi đi - Kiểu chuỗi (VNA, abacus)
        "ToPlaceCode": "HAN",			// Mã nơi đén - Kiểu chuỗi ( VNA, abacus)
        "FareBasis":null,				// Hạng ghế lượt đi - kiểu chuỗi (đối với VNA)
        "ReturnFareBasis":null 			// Hạng ghế lượt về - kiểu chuỗi (đối với VNA)
        "CurrencyType": "VND", 				// Loại tiền - Kiểu chuỗi
        "CallBackUrl" : "url", 				// URL mà server API gọi trở lại web mình*/

      private ICollection<BookingPassenger>_bookingPassenger;
      public string Brand { get; set; }
      public int Adult { get; set; }
      public int Child { get; set; }
      public int Infant { get; set; }
      public bool RoundTrip { get; set; }
      public DateTime DepartDate { get; set; }
      public DateTime ReturnDate { get; set; }
      public string FlightNumber { get; set; }
      public string TicketPrice { get; set; }
      public string TicketType { get; set; }
      public string FromPlace { get; set; }
      public int FromPlaceId { get; set; }
      public string ToPlace { get; set; }
      public int ToPlaceId { get; set; }
      public string FareBasis { get; set; }
      public string ReturnFareBasis { get; set; }
      public string CurrencyType { get; set; }
      public string CallBackUrl { get; set; }

      public virtual ICollection<BookingPassenger> BookingPasssenger
      {
          get { return _bookingPassenger ?? (_bookingPassenger = new List<BookingPassenger>()); }
          protected set { _bookingPassenger = value; }
      }

        
      


    }
}
