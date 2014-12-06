using PlanX.Core.Domain.ClickBay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PlanX.Services.ClickBay
{
     public  interface IOnePayServices
    {
         void PostProcessPayment(Booking ticket);

         bool GetReturnPaymentSuccess(FormCollection form);
    }
}
