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

        public ActionResult List()
        {
            var model = new TicketConcessionListModel();
            var lst = _ticketConcessionService.GetAllTicketConcession(0, int.MaxValue);
            model.listType = _ticketConcessionService.GetAllTicketType().ToList();
            model.listPlace = _ticketConcessionService.GetAllPlace().ToList();

            foreach (var item in lst)
            {
                var temp = new TicketConcessionModel();
                temp.Id = item.Id;
                temp.ContactEmail = item.ContactEmail;
                temp.ContactName = item.ContactName;
                temp.ContactPhone = item.ContactPhone;
                temp.CreatedOnUtc = DateTime.Now;
                temp.CurrencyCode = item.CurrencyCode;
                temp.Deleted = false;
                temp.DepartDate = item.DepartDate;
                temp.Description = item.Description;
                temp.FromPlace = item.FromPlace;
                temp.IsHelper = item.IsHelper;
                temp.PassengerName = item.PassengerName;
                temp.ReturnDate = item.ReturnDate;
                temp.RoundTrip = item.RoundTrip;
                temp.TicketPrice = item.TicketPrice;
                temp.TicketType = item.TicketType;
                temp.ToPlace = item.ToPlace;
                model.listItem.Add(temp);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult List( TicketConcessionListModel model)
        {
            var listItem = _ticketConcessionService.SearchTicketConcession(0, int.MaxValue, model.PassengerNameSearch, model.FromPlaceSearch, model.ToPlaceSearch, model.TicketTypeSearch, model.DepartDateSearch);
  
            var lst = _ticketConcessionService.GetAllTicketConcession(0, int.MaxValue);
            model.listType = _ticketConcessionService.GetAllTicketType().ToList();
            model.listPlace = _ticketConcessionService.GetAllPlace().ToList();

            foreach (var item in listItem)
            {
                var temp = new TicketConcessionModel();
                temp.Id = item.Id;
                temp.ContactEmail = item.ContactEmail;
                temp.ContactName = item.ContactName;
                temp.ContactPhone = item.ContactPhone;
                temp.CreatedOnUtc = DateTime.Now;
                temp.CurrencyCode = item.CurrencyCode;
                temp.Deleted = false;
                temp.DepartDate = item.DepartDate;
                temp.Description = item.Description;
                temp.FromPlace = item.FromPlace;
                temp.IsHelper = item.IsHelper;
                temp.PassengerName = item.PassengerName;
                temp.ReturnDate = item.ReturnDate;
                temp.RoundTrip = item.RoundTrip;
                temp.TicketPrice = item.TicketPrice;
                temp.TicketType = item.TicketType;
                temp.ToPlace = item.ToPlace;
                model.listItem.Add(temp);
            }
            return View(model);
        }

        #endregion



    }
}