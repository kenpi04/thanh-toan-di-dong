using System;
using System.Collections.Generic;
using PlanX.Core.Domain.ClickBay;
using PlanX.Core;

namespace PlanX.Services.ClickBay
{
    public partial interface IClickBayService
    {
        void InsertBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeletedBooking(Booking booking);
        Booking GetBookingById(int bookingId);
        Booking GetBookingByTickedId(string ticketId);
        IPagedList<Booking> GetAllBooking(DateTime? fromDate, DateTime? toDate, int? bookingStatusId, int? paymentStatusId, int? contactStatusId, int customerId = 0, string contactNameOrPhone = "", int pageIndex = 0, int pageSize = int.MaxValue, int id = 0, string pRNCode = "", string ticketId = "");
        string GetTicketId(int bookingId);
        void InsertBookingInfoFlight(BookingInfoFlight bookingInfoFlight);
        void UpdateBookingInfoFlight(BookingInfoFlight bookingInfoFlight);
        BookingInfoFlight GetBookingInfoFightById(int infoFlightId);
        void InsertBookingInfoFlightDetail(BookingInfoFlightDetail bookingInfoFlightDetail);
        void UpdateBookingInfoFlightDetail(BookingInfoFlightDetail bookingInfoFlightDetail);
        BookingInfoFlightDetail GetBookingInfoFightDetailById(int infoFlightDetailId);
        void InsertBookingBaggage(BookingBaggage bookingBaggage);
        void UpdateBookingPassenger(BookingPassenger bookingPassenger);
        BookingPassenger GetAllBookingPassengerById(int id);
        IList<BookingPassenger> GetAllBookingPassengerByBookingId(int bookingId);
        BookingBaggage GetBookingBaggageById(int baggageId);
        void InsertBookingInfoCondition(BookingInfoCondition bookingInfoCondition);
        void InsertBookingPassenger(BookingPassenger bookingPassenger);
        void InsertBookingPriceDetail(BookingPriceDetail bookingPriceDetail);
        void InsertBookTicketNote(BookTicketNote ticketNote);
        BookTicketNote GetBookTicketNoteById(int id);
        IList<BookTicketNote> GetAllBookTicketNoteByBokingId(int bookingId);
        void DeletedBookTicketNote(BookTicketNote bookTicketNote);

        #region report

        BookingAverageReportLine GetBookingAverageReportLine(BookingStatus? os,
            PaymentStatus? ps, DateTime? startTimeUtc, DateTime? endTimeUtc);

        BookingAverageReportLineSummary BookingAverageReport(BookingStatus os);
        #endregion
    }
}

