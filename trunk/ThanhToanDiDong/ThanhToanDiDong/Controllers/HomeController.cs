using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.DAO;
using Domain.Entity;
using Domain.Services;

namespace ThanhToanDiDong.Controllers
{
    public class HomeController : Controller
    {
        PromotionEventService _promotionService = new PromotionEventService();
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PromotionNews(int? numberItems=3)
        {
            var model = _promotionService.GetAll().Where(x=> x.Published & (!x.StartDate.HasValue || x.StartDate.Value < DateTime.Now) & (!x.EndDate.HasValue || x.EndDate > DateTime.Now));
            return View(model.ToList());
        }

    }
}
