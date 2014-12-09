using PlanX.Web.Framework.Mvc;
using System;
using PlanX.Core.Domain.ClickBay;

namespace PlanX.Admin.Models.ClickBay
{
    public class BookingPassengerModel : BaseNopEntityModel
    {
        public int BookingId { get; set; }
        public short PassengerType { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MobileNumber { get; set; }
        public int Baggage { get; set; }
        public int ReturnBaggage { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public Nullable<System.DateTime> PassportExpired { get; set; }
        public decimal BaggageFee { get; set; }
        public decimal ReturnBaggageFee { get; set; }

        public virtual PasserType PasserType
        {
            get { return (PasserType)this.PassengerType; }
            set { this.PassengerType = (short)value; }
        }
        //public virtual BookingModel BookingModel { get; set; }
    }
}