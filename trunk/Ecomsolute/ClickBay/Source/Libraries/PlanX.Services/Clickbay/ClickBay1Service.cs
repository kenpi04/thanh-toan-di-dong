using PlanX.Core;
using PlanX.Core.Data;
using PlanX.Core.Domain.ClickBay;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PlanX.Services.ClickBay
{
    public partial class ClickBayService
    {
        #region Booking
        public virtual void InsertBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException("Booking is null");

            _BookingRepository.Insert(booking);
        }
        public virtual void UpdateBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException("Booking is null");

            _BookingRepository.Update(booking);
        }
        public virtual void DeletedBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException("Booking is null");

            booking.Deleted = true;
            _BookingRepository.Update(booking);
        }
        public virtual Booking GetBookingById(int bookingId)
        {
            if (bookingId <= 0)
                return null;
            return _BookingRepository.GetById(bookingId);
        }
        public virtual Booking GetBookingByTickedId(string ticketId)
        {
            if (string.IsNullOrEmpty(ticketId))
                return null;
            var query = from b in _BookingRepository.Table
                        where b.TicketId == ticketId
                        select b;

            return query.FirstOrDefault();
        }
        public virtual IPagedList<Booking> GetAllBooking(DateTime? fromDate, DateTime? toDate, int? bookingStatusId, int? paymentStatusId, int? contactStatusId, int customerId = 0, string contactNameOrPhone = "", int pageIndex = 0, int pageSize = int.MaxValue, int id=0)
        {
            var query = _BookingRepository.Table;
            
            query = query.Where(x=>!x.Deleted);
            if (id > 0)
                query = query.Where(x => x.Id == id);
            else
            {
                if (fromDate.HasValue)
                    query = query.Where(x => x.CreatedOn > fromDate.Value);
                if (toDate.HasValue)
                    query = query.Where(x => x.CreatedOn < toDate.Value);
                if (bookingStatusId.HasValue && bookingStatusId.Value > 0)
                    query = query.Where(x => x.BookingStatusId == bookingStatusId);
                if (paymentStatusId.HasValue)
                    query = query.Where(x => x.PaymentStatusId == paymentStatusId);
                if (contactStatusId.HasValue)
                    query = query.Where(x => x.ContactStatusId == contactStatusId);
                if (!String.IsNullOrEmpty(contactNameOrPhone))
                    query = query.Where(x => x.ContactName.Contains(contactNameOrPhone) || x.ContactPhone.Contains(contactNameOrPhone));
                if (customerId != 0)
                    query = query.Where(x => x.CustomerId == customerId);
                query = query.OrderBy(x => x.ContactStatusId).ThenBy(x => x.CreatedOn);
            }
            return new PagedList<Booking>(query, pageIndex, pageSize);
        }

        public virtual string GetTicketId(int bookingId)
        {
            if (bookingId == 0)
                return "";

            string ticketId = "";
            SqlParameter pTicketId = new SqlParameter("ticketId", System.Data.SqlDbType.NVarChar, 20);
            pTicketId.Direction = System.Data.ParameterDirection.Output;

            var excute = _dbContext.ExecuteSqlCommand("execute [GetTickedId] @bookingId,  @ticketId output", false, null,
                    new SqlParameter("bookingId", bookingId),
                    pTicketId);

            return ticketId = pTicketId.Value == DBNull.Value ? "" : pTicketId.Value.ToString(); ;
        }

        #endregion

        #region Booking Info Flight
        public virtual void InsertBookingInfoFlight(BookingInfoFlight bookingInfoFlight)
        {
            if (bookingInfoFlight == null)
                throw new ArgumentNullException("BookingInfoFlight is null");

            _bookingInfoFlightRepository.Insert(bookingInfoFlight);
        }
        public virtual void UpdateBookingInfoFlight(BookingInfoFlight bookingInfoFlight)
        {
            if (bookingInfoFlight == null)
                throw new ArgumentNullException("BookingInfoFlight is null");

            _bookingInfoFlightRepository.Update(bookingInfoFlight);
        }

        public virtual BookingInfoFlight GetBookingInfoFightById(int infoFlightId)
        {
            return _bookingInfoFlightRepository.GetById(infoFlightId);
        }
        #endregion

        #region Booking Baggage
        public virtual void InsertBookingBaggage(BookingBaggage bookingBaggage)
        {
            if (bookingBaggage == null)
                throw new ArgumentNullException("BookingBaggage is null");

            _bookingBaggageRepository.Insert(bookingBaggage);
        }

        public virtual BookingBaggage GetBookingBaggageById(int baggageId)
        {
            return _bookingBaggageRepository.GetById(baggageId);
        }
        #endregion

        #region Booing Info Condition
        public virtual void InsertBookingInfoCondition(BookingInfoCondition bookingInfoCondition)
        {
            if (bookingInfoCondition == null)
                throw new ArgumentNullException("BookingInfoCondition is null");

            _bookingInfoConditionRepository.Insert(bookingInfoCondition);
        }
        #endregion

        #region Booing Passenger
        public virtual void InsertBookingPassenger(BookingPassenger bookingPassenger)
        {
            if (bookingPassenger == null)
                throw new ArgumentNullException("BookingPassenger is null");

            _bookingPassengerRepository.Insert(bookingPassenger);
        }
        public virtual void UpdateBookingPassenger(BookingPassenger bookingPassenger)
        {
            if (bookingPassenger == null)
                throw new ArgumentNullException("BookingPassenger is null");

            _bookingPassengerRepository.Update(bookingPassenger);
        }
        public virtual BookingPassenger GetAllBookingPassengerById(int id)
        {
            if (id == 0)
                return null;
            return  _bookingPassengerRepository.GetById(id);
            
        }
        public virtual IList<BookingPassenger> GetAllBookingPassengerByBookingId(int bookingId)
        {
            if (bookingId == 0)
                return null;

            var query = from pas in _bookingPassengerRepository.Table
                        where pas.BookingId == bookingId
                        select pas;
            
            if (query == null)
                return null;
            return query.ToList();
        }

        #endregion

        #region Booing Price detail
        public virtual void InsertBookingPriceDetail(BookingPriceDetail bookingPriceDetail)
        {
            if (bookingPriceDetail == null)
                throw new ArgumentNullException("BookingPriceDetail is null");

            _bookingPriceDetailRepository.Insert(bookingPriceDetail);
        }
        #endregion

        #region Booking Ticket Note
        public virtual void InsertBookTicketNote(BookTicketNote ticketNote)
        {
            if (ticketNote == null)
                throw new ArgumentNullException("TicketNote is null");

            _bookTicketNoteRepository.Insert(ticketNote);
        }
        
        public virtual BookTicketNote GetBookTicketNoteById(int id)
        {
            return _bookTicketNoteRepository.GetById(id);
        }
        public virtual IList<BookTicketNote> GetAllBookTicketNoteByBokingId(int bookingId)
        {
            var query = from bn in _bookTicketNoteRepository.Table
                        where bn.BookTicketId == bookingId
                        select bn;

            if (query == null)
                return null;
            return query.ToList();
        }
        public virtual void DeletedBookTicketNote(BookTicketNote bookTicketNote)
        {
            if (bookTicketNote == null)
                throw new ArgumentNullException("bookTicketNote is null");
            _bookTicketNoteRepository.Delete(bookTicketNote);
        }

        #endregion
    }
}
