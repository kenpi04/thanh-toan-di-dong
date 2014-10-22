using Domain.Entity;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhToanDiDong.Admin.Models;

namespace ThanhToanDiDong.Admin.Controllers
{
    public class PromotionEventController : Controller
    {
        PromotionEventService _promotionEventService = new PromotionEventService();

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        #region Promotion Event
        public ActionResult List()
        {
            var model = new FilterModel();
            model.PagingModel = new PagingInfo
            {
                CurrentPage = 1,
                ItemsPerPage = 15
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Paging(FilterModel page)
        {
            DateTime? startDateValue = (page.StartDate == null) ? null
                            : (DateTime?)page.StartDate;

            DateTime? endDateValue = (page.EndDate == null) ? null
                            : (DateTime?)page.EndDate;

            var model = _promotionEventService.GetPage(startDate: startDateValue, endDate: endDateValue, pageIndex: page.PagingModel.CurrentPage, pageSize: page.PagingModel.ItemsPerPage);
            return PartialView("_PromotionEventList", model);
        }

        public ActionResult Create()
        {
            var model = new PromotionEventModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PromotionEventModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var promotionEvent = new PromotionEvent()
                    {
                        Description = model.Description,
                        CreatedOn = DateTime.Now,
                        Published = true,
                        Deleted = false
                    };
                    _promotionEventService.InsertOrUpdate(promotionEvent);
                    return Json("1");
                }
                catch { }
            }

            return Json("0");
        }
        public ActionResult Edit(int id)
        {
            if (id <= 0)
                return RedirectToAction("List");

            var promotionEvent = _promotionEventService.GetById(id);
            if (promotionEvent == null)
                return RedirectToAction("List");

            var model = new PromotionEventModel()
            {
                Id = promotionEvent.Id,
                Description = promotionEvent.Description,
                Published = promotionEvent.Published,
                CreatedOn = promotionEvent.CreatedOn
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(PromotionEventModel model)
        {
            var promotionEvent = _promotionEventService.GetById(model.Id);
            if (promotionEvent == null)
                return View("");

            if (ModelState.IsValid)
            {
                promotionEvent.Description = model.Description;
                promotionEvent.Published = model.Published;
                _promotionEventService.InsertOrUpdate(promotionEvent);
                return Json("1");
            }

            return Json("0");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var promotionEvent = _promotionEventService.GetById(id);
            if (promotionEvent == null)
                return Json("0");

            promotionEvent.Deleted = true;
            _promotionEventService.InsertOrUpdate(promotionEvent);
            return Json("1");
        }


        #endregion

    }
}
