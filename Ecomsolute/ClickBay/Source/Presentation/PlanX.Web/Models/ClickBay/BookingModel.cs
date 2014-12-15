using FluentValidation.Attributes;
using PlanX.Core;
using PlanX.Web.Framework.Mvc;
using PlanX.Web.Validators.ClickBay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanX.Web.Models.ClickBay
{
    [Validator(typeof(BookingValidator))]
    public class BookingModel : BaseEntity
    {
        public BookingModel()
        {
            TicketInfo = new TicketModel();
            TicketInfoReturn = new TicketModel();
            BookingPassers = new List<BookingPasserModel>();
            PassengerTypes = new List<SelectListItem>();
            Countries = new List<SelectListItem>();
            BookingInfoConditions = new List<BookingInfoConditionModel>();
           
        }
        public string ContactName { get; set; }
        public string ContactGender { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public int ContactCountryId { get; set; }
        public string ContactCityName { get; set; }
        public string ContactBirthDate { get; set; }
        public bool NewLetterAccept { get; set; }
        public int PaymentMethodId { get; set; }

        public bool IsInvoid { get; set; }    
        public Int16 Adult { get; set; }
        public Int16 Child { get; set; }
        public Int16 Infant { get; set; }

        public string CustomerNote { get; set; }

        public decimal DiscountAmount { get; set; }
        
        public TicketModel TicketInfo { get; set; }
        public TicketModel TicketInfoReturn { get; set; }

        public List<BookingPasserModel> BookingPassers { get; set; }
        public Int16 ContactPassengerType { get; set; }

        public List<SelectListItem> PassengerTypes { get; set; }

        public List<SelectListItem> Countries { get; set; }
        public int CountryId { get; set; }
      
        public List<BookingInfoConditionModel> BookingInfoConditions { get; set; }

        #region Nestest class
    
       public class BookingInfoConditionModel
       {
           public string ConditionType { get; set; }
           public string  ConditionDescription { get; set; }
       }
        #endregion
       
      
        
       
       
    }
}