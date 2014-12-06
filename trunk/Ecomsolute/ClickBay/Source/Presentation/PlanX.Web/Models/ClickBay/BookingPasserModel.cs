using FluentValidation.Attributes;
using PlanX.Web.Validators.ClickBay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanX.Web.Models.ClickBay
{
    
    public class BookingPasserModel
    {
        public string Gender { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public int PersonType { get; set; }

        public int PassserType { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}