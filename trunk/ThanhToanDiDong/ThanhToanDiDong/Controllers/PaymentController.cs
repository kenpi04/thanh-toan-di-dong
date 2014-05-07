using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhToanDiDong.Models.Payment;
using Domain.Services;
using System.Text.RegularExpressions;

namespace ThanhToanDiDong.Controllers
{
    public class PaymentController : Controller
    {
        CardMobileService _cardMobileService = new CardMobileService();
        CategoryCardMobileService _cateService = new CategoryCardMobileService();
      
        //
        // GET: /Payment/
        public ActionResult Topup()
        {
            var model = new TopupModel();          
            return View(model);
        }

        [HttpGet]
        public ActionResult GetCategory(string phone)
        {
            int lengt = 3;
            if (phone.Length > 10)
                lengt = 4;

            Regex rg = new Regex(phone.Substring(0, lengt));
            var cate = _cateService.GetAll(x =>rg.IsMatch(x.DauSo)).FirstOrDefault();
            if (cate != null)
            {

                var listCardType =cate.CardMobile.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                return Json(listCardType, JsonRequestBehavior.AllowGet);
            }
            return Json("",JsonRequestBehavior.AllowGet);
        }
        

    }
}
