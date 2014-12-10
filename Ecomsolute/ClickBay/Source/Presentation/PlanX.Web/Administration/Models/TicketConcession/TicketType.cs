using PlanX.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PlanX.Admin.Validators.TicketConcession;
using PlanX.Web.Framework;
using FluentValidation.Attributes;

namespace PlanX.Admin.Models.TicketConcession
{
   
    public class TicketTypeAndPlceModel : BaseNopEntityModel
    {
        public string Name { get; set; }      
    }

}