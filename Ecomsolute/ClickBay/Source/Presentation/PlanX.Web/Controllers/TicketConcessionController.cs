using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanX.Core.Domain.ClickBay;
using PlanX.Services.ClickBay;
using PlanX.Web.Models.ClickBay;
using PlanX.Web.Framework.UI.Captcha;

namespace PlanX.Web.Controllers
{
    public class TicketConcessionController : BaseNopController
    {
        private readonly ITicketConcessionService _ticketConcessionService;
        public TicketConcessionController(ITicketConcessionService ticketConcessionService)
        {
            this._ticketConcessionService = ticketConcessionService;
        }


        #region TicketConcession

        public ActionResult TicketConcessionPost()
        {
            var model = new TicketConcessionPostModel();
            model.DepartDate = DateTime.Now.ToString("MM/dd/yyyy");
            model.ReturnDate = DateTime.Now.ToString("MM/dd/yyyy");
            model.TimeDepartDate = DateTime.Now.ToShortTimeString();
            model.TimeReturnDate = DateTime.Now.ToShortTimeString();
            model.listType = _ticketConcessionService.GetAllTicketType().ToList();
            model.listPlace = _ticketConcessionService.GetAllPlace().ToList();

            return View(model);
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult TicketConcessionPost(TicketConcessionPostModel model, bool captchaValid)
        {
            if (ModelState.IsValid)
            {
                var ticketConcession = new TicketConcession();
                ticketConcession.ContactEmail = model.ContactEmail;
                ticketConcession.ContactName = model.ContactName;
                ticketConcession.ContactPhone = model.ContactPhone;
                ticketConcession.CreatedOnUtc = DateTime.Now;
                ticketConcession.CurrencyCode = model.CurrencyCode;
                ticketConcession.Deleted = false;
                ticketConcession.DepartDate = Convert.ToDateTime(model.DepartDate + " " + model.TimeDepartDate);
                ticketConcession.Description = model.Description;
                ticketConcession.FromPlace = model.FromPlace;
                ticketConcession.IsHelper = model.IsHelper;
                ticketConcession.PassengerName = model.PassengerName;
                if (model.RoundTrip == true)
                    ticketConcession.ReturnDate = Convert.ToDateTime(model.ReturnDate + " " + model.TimeReturnDate);
                else
                    ticketConcession.ReturnDate = ticketConcession.DepartDate;
                ticketConcession.RoundTrip = model.RoundTrip;
                ticketConcession.TicketPrice = model.TicketPrice;
                ticketConcession.TicketType = model.TicketType;
                ticketConcession.ToPlace = model.ToPlace;
                ticketConcession.IsRename = model.IsRename;

                _ticketConcessionService.InsertTicketConcession(ticketConcession);
                SuccessNotification("Đăng tin thành công");
                return RedirectToAction("List");
            }

            return View(model);
        }

        //public ActionResult List()
        //{
        //    var model = new TicketConcessionListModel();
        //    var lst = _ticketConcessionService.GetAllTicketConcession(0, 20);
        //    model.Total = lst.TotalCount;
        //    model.Page = 1;
        //    model.PageSize = 20;
        //    model.listType = _ticketConcessionService.GetAllTicketType().ToList();
        //    model.listPlace = _ticketConcessionService.GetAllPlace().ToList();

        //    foreach (var item in lst)
        //    {
        //        model.listItem.Add(PrepareTicketConcessionModel(item));
        //    }
        //    return View(model);
        //}

        //[HttpPost]
        public ActionResult List(TicketConcessionListModel model)
        {
            if (model.PageSize == 0) model.PageSize = 10;
            if(model.Page<=0) model.Page=1;
            var listItem = _ticketConcessionService.SearchTicketConcession(model.Page - 1, model.PageSize, model.PassengerNameSearch, model.FromPlaceSearch, model.ToPlaceSearch, model.TicketTypeSearch, model.DepartDateSearch);

            model.listType = _ticketConcessionService.GetAllTicketType().ToList();
            model.listPlace = _ticketConcessionService.GetAllPlace().ToList();

            model.Total = listItem.TotalCount;

            foreach (var item in listItem)
            {
                model.listItem.Add(PrepareTicketConcessionModel(item));
            }
            if (Request.IsAjaxRequest())
                return View("_ListTicketConcession", model.listItem);
            return View(model);
        }

        public TicketConcessionModel PrepareTicketConcessionModel(TicketConcession item) 
        {
            var temp = new TicketConcessionModel();
            temp.Id = item.Id;
            temp.ContactEmail = item.ContactEmail;
            temp.ContactName = item.ContactName;
            temp.IsRename = item.IsRename;
            temp.ContactPhone = item.ContactPhone;
            temp.CreatedOnUtc = item.CreatedOnUtc;
            temp.DepartDate = item.DepartDate;
            temp.Description = item.Description;
            temp.FromPlace = item.FromPlace;
            temp.IsHelper = item.IsHelper;
            temp.PassengerName = item.PassengerName;
            temp.ReturnDate = item.ReturnDate;
            temp.RoundTrip = item.RoundTrip;
            temp.TicketType = item.TicketType;
          
            if (item.TicketPrice == 0)
            {
                temp.TicketPrice = "Thỏa thuận";
            }
            else
            {
                temp.TicketPrice = item.TicketPrice.ToString("0,#") +" "+ item.CurrencyCode;
            }
            temp.ToPlace = item.ToPlace;
            return temp;
        }
        #endregion



    }
}