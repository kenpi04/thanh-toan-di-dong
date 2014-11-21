using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
  public  class BookingPassenger
    {
        /*"PassengerType": 0, 				// Loại hành khach - kiểu số
  "Gender": "1", 					// Giới tính - kiểu chuỗi 
  "Title": "MR", 					// Định danh - Kiểu chuỗi
  "FirstName":"DUC", 			
  "LastName":"NGUYEN", 
  "MiddleName":"VAN", 
  "MobileNumber": "0938215268",		// Số điện thoại - kiểu chuỗi
  "Baggage": 15,					// Hành lý lượt đi - kiểu số
  "ReturnBaggage": 0,				// Hành lý lượt về - kiểu số
  "BirthDay": "1988-01-01T00:00:00", 		// Ngày sinh - Kiểu dateTime
  "Email": "ticketing@phuongnamstar.vn" 	// Email hãng gửi về khi book thành công .
  "PassportNumber": "B548432543",		// Passport – kiêu chuỗi ( chỉ dùng với abacus)
  "PassportExpired": "2021-01-01T00:00:00"	// ngày hết hạn passport - kiểu datetime
  */
        public int PassengerType { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MobileNumber { get; set; }
        public int Baggage { get; set; }
        public int ReturnBaggage { get; set; }
        public string BirthDay { get; set; }

        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public string PassportExpired { get; set; }
    }
}
