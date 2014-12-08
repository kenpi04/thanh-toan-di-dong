using PlanX.Core.Data;
using PlanX.Core.Domain.ClickBay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanX.Services.Clickbay
{
    public partial class ClickBayService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<BookingInfoFlight> _bookingInfoFlightRepository;
        private readonly IRepository<BookingBaggage> _bookingBaggageRepository;
        private readonly IRepository<BookingPassenger> _bookingPassengerRepository;
        private readonly IRepository<BookingPriceDetail> _bookingPriceDetailRepository;
        private readonly IRepository<BookingInfoCondition> _bookingInfoConditionRepository;
        private readonly IRepository<BookTicketNote> _bookTicketNoteRepository;
        //private readonly IWebHelper _webHelper;

        #region Booking
        public virtual void InsertBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException("Booking is null");

            _bookingRepository.Insert(booking);
        }
        public virtual void UpdateBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException("Booking is null");

            _bookingRepository.Update(booking);
        }
        public virtual void DeletedBooking(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException("Booking is null");

            booking.Deleted = true;
            _bookingRepository.Update(booking);
        }
        public virtual Booking GetBookingById(int bookingId)
        {
            if (bookingId <= 0)
                return null;
            return _bookingRepository.GetById(bookingId);
        }
        public virtual IList<Booking> GetAllBooking(DateTime? fromDate, DateTime? toDate, int? bookingStatusId, int? paymentStatusId, int? contactStatusId, int customerId=0, string contactNameOrPhone="")
        {
            var query = _bookingRepository.Table;
            query = query.Where(x=>!x.Deleted);
            if(fromDate.HasValue)
                query=query.Where(x=>x.CreatedOn>fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(x=>x.CreatedOn<toDate.Value);
            if (bookingStatusId.HasValue)
                query = query.Where(x=>x.BookingStatusId==bookingStatusId);
            if (paymentStatusId.HasValue)
                query = query.Where(x => x.PaymentStatusId == paymentStatusId);
            if (contactStatusId.HasValue)
                query = query.Where(x => x.ContactStatusId == contactStatusId);
            if (!String.IsNullOrEmpty(contactNameOrPhone))
                query = query.Where(x => x.ContactName.Contains(contactNameOrPhone) || x.ContactPhone.Contains(contactNameOrPhone));
            if (customerId != 0)
                query = query.Where(x => x.CustomerId == customerId);

            return query.ToList();
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
        #endregion
    }
}
