using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanX.Core.Domain.ClickBay;

namespace PlanX.Services.ClickBay
{
    public partial interface IClickBayService
    {
        #region API 
        /*
         Adult	Int	Người lớn
 Child	Int	Trẻ em
 Infant	Int	Trẻ sơ sinh
 RoundTrip	Bool ( true, false )	Khứ hồi
 DepartDate	DateTime	Ngày khởi hành
 ReturnDate	DateTime	Ngày trở về
 FromPlace	String	Mã nơi đi
 ToPlace	String	Mã nới đến
 CurrecyType	String	Loại tiền tệ (Mặc định VND)
 Source	String	

         */

        /// <summary>
        /// Tìm vé
        /// </summary>
        /// <param name="Adult">Người lớn</param>
        /// <param name="child">Trẻ em</param>
        /// <param name="fant">Trẻ sơ sinh</param>
        /// <param name="roundTrip">Khứ hồi</param>
        /// <param name="departDate">Ngày đi</param>
        /// <param name="returnDate">Ngày về</param>
        /// <param name="fromPlace">Nơi đi</param>
        /// <param name="toPlace">Nới đến</param>
        /// <param name="currentType">Loại tiền tệ</param>
        /// <param name="source"></param>
        /// <returns>Danh sách vé</returns>
        IEnumerable<Ticket> SearchTicket(string fromPlace, string toPlace, DateTime departDate,
    int Adult = 0, int child = 0, int Infant = 0,
    string FareBasis = null,
    bool roundTrip = false, DateTime? returnDate = null,
    string CasyncurrencyType = "VND", string source = null, bool expendDetails = false,
    bool expendTicketPriceDetails = false, bool expandOption = false
    );
        /// <summary>
        /// Lấy danh sách sân bay
        /// </summary>
        /// <returns></returns>
        IEnumerable<Airport> GetAirport();

        /// <summary>
        /// Get city
        /// </summary>
        /// <returns></returns>
        IEnumerable<FlightCity> GetCity();

        /// <summary>
        /// Get country
        /// </summary>
        /// <returns></returns>
        IEnumerable<FlightCountry> GetCountry();

        Booking BookTicket(Booking model);
        #endregion

        #region database Service

        IEnumerable<FlightCity> GetListCity(int countryId = 0,string name=null);

        IEnumerable<FlightCity> GetListCountry();

        IEnumerable<Airport> GetListAirport(int country = 0, int city = 0);
        FlightCity GetCityById(int id);
        FlightCity GetcityByCode(string code);
        FlightCountry GetCountryById(int id);

        Booking GetBookTicketById(int id);
        Ticket GetTicketById(int id);

        /*void InsertOrUpdateBooking(Booking book);

        void DeleteBooking(Booking book);*/

        void InsertCity(FlightCity city);
        void UpdateCity(FlightCity city);

        void InsertCountry(FlightCountry entity);
        void UpdateCountry(FlightCountry country);

        void InsertAirport(Airport airport);
        void UpdateAirport(Airport airport);
        //void InsertOrupdateAirline(Airline Airline);

        IList<AirlinesCondition> GetListAirlinesConditionByAirlineId(string airlineCode);
        IList<ArilinesBaggageCondition> GetListArilinesBaggageCondition(string airlineCode);

        string GetData();
            


        #endregion




    }
}

