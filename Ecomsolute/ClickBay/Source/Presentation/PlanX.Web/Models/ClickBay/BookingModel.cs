﻿using FluentValidation.Attributes;
using PlanX.Web.Validators.ClickBay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanX.Web.Models.ClickBay
{
    [Validator(typeof(BookingValidator))]
    public class BookingModel
    {
      
        public string ContactName { get; set; }
        public string ContactGender { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAddress { get; set; }
        public int ContactCountryId { get; set; }
        public string ContactCityName { get; set; }
        public DateTime? ContactBirthDate { get; set; }
        
        public short Adult { get; set; }
        public short Child { get; set; }
        public short Infant { get; set; }

        public string CustomerNote { get; set; }
        
        public TicketModel TicketInfo { get; set; }
        public TicketModel TicketInfoReturn { get; set; }

        public List<BookingPasserModel> BookingPassers { get; set; }
        public int ContactPassengerType { get; set; }

        public List<SelectListItem> PassengerTypes { get; set; }

        public List<SelectListItem> Countries { get; set; }

      
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