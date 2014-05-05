using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhToanDiDong.Models.Payment;

namespace ThanhToanDiDong.Controllers
{
    public class PaymentController : Controller
    {
        //
        // GET: /Payment/
        public ActionResult Topup()
        {
            var model = new TopupModel();
            return View(model);
        }

        //[HttpGet]
        //public ActionResult GetBrand(string phone)
        //{
        //    if (phone.Length < 9)
        //        throw new Exception("");
            
        //}
        

    }
}
