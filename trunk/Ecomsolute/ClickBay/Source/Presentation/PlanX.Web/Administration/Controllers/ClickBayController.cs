using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanX.Core.Domain.ClickBay;
using PlanX.Admin.Models.ClickBay;
using PlanX.Services.ClickBay;
using PlanX.Services.Security;
using Telerik.Web.Mvc;
using PlanX.Web.Framework;
using PlanX.Services.Localization;

namespace PlanX.Admin.Controllers
{
    public class ClickBayController : BaseNopController
    {
        #region Fields
        private readonly IClickBayService _clickBayService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        #endregion


        #region Contruct

        public ClickBayController(IClickBayService clickBayService,
            IPermissionService permissionService,
            ILocalizationService localizationService)
        {
            this._clickBayService = clickBayService;
            this._permissionService = permissionService;
            this._localizationService = localizationService;
        }

        #endregion
        // GET: ClickBay
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var model = new BookingListModel();
            model.CreateDate = DateTime.Now;
            model.AvailableBookingStatuses = BookingStatus.ChuaXyLy.ToSelectList(false).ToList();
            model.AvailableBookingStatuses.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View(model);
        }

        [GridAction(EnableCustomBinding = true)]
        [HttpPost]
        public ActionResult BookingList(GridCommand command, BookingListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            BookingStatus? bookingStatus = model.BookingStatusId > 0 ? (BookingStatus?)(model.BookingStatusId) : null;

            var bookings = _clickBayService.GetAllBooking(fromDate:model.CreateDate,
                toDate:model.CreateDate,bookingStatusId:model.BookingStatusId, paymentStatusId:null,
                contactStatusId:null,customerId:model.CustomerId,
                contactNameOrPhone:model.CustomerNameOrPhone, pageIndex: command.Page-1, pageSize:command.PageSize);

            var gridModel = new GridModel<BookingModel>
            {
                Data = bookings.Select(x =>
                {
                    return new BookingModel()
                    {
                        Id = x.Id,
                        ContactName = x.ContactName,
                        ContactPhone = x.ContactPhone,
                        ContactEmail = x.ContactEmail,
                        CreatedOn = x.CreatedOn,
                        BookingStatus = x.BookingStatus.GetLocalizedEnum(_localizationService,0),
                        PaymentStatus = x.PaymentStatus.GetLocalizedEnum(_localizationService,0),
                        TotalAmount = x.TotalAmount
                    };
                }),
                Total = bookings.Count()
            };


            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Edit()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            return View();
        }
        
    }
}