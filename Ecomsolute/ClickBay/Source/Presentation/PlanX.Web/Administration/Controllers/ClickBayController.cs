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
using PlanX.Core;
using PlanX.Web.Framework.Controllers;
using PlanX.Services.Directory;
using PlanX.Services.Customers;

namespace PlanX.Admin.Controllers
{
    public class ClickBayController : BaseNopController
    {
        #region Fields
        private readonly IClickBayService _clickBayService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ICountryService _countryService;
        private readonly ICustomerService _customerService;
        #endregion


        #region Contruct

        public ClickBayController(IClickBayService clickBayService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            ICountryService countryService,
            ICustomerService customerService)
        {
            this._clickBayService = clickBayService;
            this._permissionService = permissionService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._countryService = countryService;
            this._customerService = customerService;
        }

        #endregion

        #region Booking
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

            var bookings = _clickBayService.GetAllBooking(fromDate: model.CreateDate,
                toDate: model.CreateDate, bookingStatusId: model.BookingStatusId, paymentStatusId: null,
                contactStatusId: null, customerId: model.CustomerId,
                contactNameOrPhone: model.CustomerNameOrPhone, pageIndex: command.Page - 1, pageSize: command.PageSize, id: model.BookingId, pRNCode: model.PRNCode, ticketId: model.TicketId);

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
                        BookingStatus = x.BookingStatus.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                        PaymentStatus = x.PaymentStatus.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                        BookingStatusId = x.BookingStatusId,
                        TotalAmount = x.TotalAmount,
                        TicketId = x.TicketId
                    };
                }),
                Total = bookings.TotalCount
            };


            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var booking = _clickBayService.GetBookingById(id);
            if (booking == null || booking.Deleted)
                return RedirectToAction("List");

            var model = new BookingModel();
            model = PrepareBookingDetailsModel(booking);

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("btnSaveBookingStatus")]
        public ActionResult ChangeBookingStatus(int id, BookingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var booking = _clickBayService.GetBookingById(id);
            if (booking == null)
                return RedirectToAction("List");

            try
            {
                booking.BookingStatusId = model.BookingStatusId;
                _clickBayService.UpdateBooking(booking);
                model = new BookingModel();
                model = PrepareBookingDetailsModel(booking);
                return View(model);
            }
            catch (Exception exc)
            {
                //error
                model = model = new BookingModel();
                model = PrepareBookingDetailsModel(booking);
                ErrorNotification(exc, false);
                return View(model);
            }
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("btnSaveContactStatus")]
        public ActionResult ChangeContactStatus(int id, BookingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var booking = _clickBayService.GetBookingById(id);
            if (booking == null)
                return RedirectToAction("List");

            try
            {
                booking.ContactStatusId = model.ContactStatusId;
                _clickBayService.UpdateBooking(booking);
                model = new BookingModel();
                model = PrepareBookingDetailsModel(booking);
                return View(model);
            }
            catch (Exception exc)
            {
                //error
                model = model = new BookingModel();
                model = PrepareBookingDetailsModel(booking);
                ErrorNotification(exc, false);
                return View(model);
            }
        }


        [NonAction]
        private BookingModel PrepareBookingDetailsModel(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException("order");

            var model = new BookingModel();

            model = booking.ToModel();
            model.BookingStatus = booking.BookingStatus.GetLocalizedEnum(_localizationService, _workContext);
            var contractCountry = _clickBayService.GetCountryById(booking.ContactCountryId);
            model.ContactCountryId = booking.ContactCountryId;
            model.ContactCountryName = contractCountry != null ? contractCountry.Name : "";
            model.ContactStatus = booking.ContactStatus.GetLocalizedEnum(_localizationService, _workContext);
            model.PaymentMethod = ((PaymentMethod)(booking.PaymentMethodId)).GetLocalizedEnum(_localizationService, _workContext);
            model.PaymentStatus = booking.PaymentStatus.GetLocalizedEnum(_localizationService, _workContext);
            //info flight
            model.BookingInfoFlightModel = PrepareBookingInfoFlight(model.BookingInfoFlightToId ?? 0);
            model.BookingInfoFlightReturnModel = PrepareBookingInfoFlight(model.BookingInfoFlightReturnId ?? 0);
            if (model.BookingInfoFlightModel != null)
            {
                model.FromPlace = model.BookingInfoFlightModel.FromPlaceName;
                model.ToPlace = model.BookingInfoFlightModel.ToPlaceName;
                model.ToDate = model.BookingInfoFlightModel.DepartDateTime;
            }

            //passenger
            var passengers = _clickBayService.GetAllBookingPassengerByBookingId(booking.Id);
            if (passengers != null)
                model.BookingPassengerModel = passengers.Select(x => { return x.ToModel(); }).ToList();
            //note
            var notes = _clickBayService.GetAllBookTicketNoteByBokingId(booking.Id);
            if (notes != null)
                model.BookTicketNotesModel = notes.Select(x => { return x.ToModel(); }).ToList();


            //customer
            if (booking.CustomerId != 0)
            {
                var customer = _customerService.GetCustomerById(booking.CustomerId);
                if (customer != null)
                {
                    model.CustomerName = customer.GetFullName();
                }
            }

            return model;
        }

        [NonAction]
        private BookingInfoFlightModel PrepareBookingInfoFlight(int idInfoFlight)
        {
            if (idInfoFlight <= 0)
                return null;

            var infoFlight = _clickBayService.GetBookingInfoFightById(idInfoFlight);
            if (infoFlight != null)
            {
                var bookingInfoFlightModel = infoFlight.ToModel();
                //price
                if (infoFlight.BookingPriceDetails.FirstOrDefault() != null)
                {
                    bookingInfoFlightModel.BookingPriceDetailsModel = infoFlight.BookingPriceDetails.Select(x =>
                    {
                        var price = x.ToModel();
                        price.PassengerTypeName = price.PassengerTypeName = x.PassengerTypes.GetLocalizedEnum(_localizationService, _workContext);
                        return price;
                    }).ToList();
                }
                //condition
                if (infoFlight.BookingInfoConditions.FirstOrDefault() != null)
                {
                    bookingInfoFlightModel.BookingInfoConditionsModel = infoFlight.BookingInfoConditions.Select(x =>
                    {
                        return x.ToModel();
                    }).ToList();
                }
                //baggage
                if (infoFlight.BookingBaggages.FirstOrDefault() != null)
                {
                    bookingInfoFlightModel.BookingBaggagesModel = infoFlight.BookingBaggages.Select(x =>
                    {
                        var bag = x.ToModel();
                        bag.PassengerTypeName = x.PassengerTypeEnum.GetLocalizedEnum(_localizationService, _workContext);
                        return bag;
                    }).ToList();
                }
                return bookingInfoFlightModel;
            }

            return null;
        }

        #endregion

        #region Notes
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult BookingNotesSelect(int bookingId, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var booking = _clickBayService.GetBookingById(bookingId);
            if (booking == null)
                throw new ArgumentException("No booking found with the specified id");

            //booking notes
            var bookingNoteModels = booking.BookTicketNotes.Select(x => { return x.ToModel(); }).ToList();


            var model = new GridModel<BookTicketNoteModel>
            {
                Data = bookingNoteModels,
                Total = bookingNoteModels.Count
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [ValidateInput(false)]
        public ActionResult BookingNoteAdd(int bookingId, string message)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var booking = _clickBayService.GetBookingById(bookingId);
            if (booking == null)
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);


            var bookingNote = new BookTicketNote
            {
                BookTicketId = booking.Id,
                Description = message,
                CreateDate = DateTime.Now
            };
            _clickBayService.InsertBookTicketNote(bookingNote);

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult BookingNoteDelete(int bookingId, int bookingNoteId, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var booking = _clickBayService.GetBookingById(bookingId);
            if (booking == null)
                throw new ArgumentException("No booking found with the specified id");

            var bookingNote = booking.BookTicketNotes.FirstOrDefault(on => on.Id == bookingNoteId);
            if (bookingNote == null)
                throw new ArgumentException("No booking note found with the specified id");
            _clickBayService.DeletedBookTicketNote(bookingNote);

            return BookingNotesSelect(bookingNoteId, command);
        }
        #endregion

        #region Sumary
        [NonAction]
        protected virtual IList<BookingAverageReportLineSummaryModel> GetBookingAverageReportModel()
        {
            var report = new List<BookingAverageReportLineSummary>();
            report.Add(_clickBayService.BookingAverageReport(BookingStatus.ChuaXyLy));
            report.Add(_clickBayService.BookingAverageReport(BookingStatus.GiuCho));
            report.Add(_clickBayService.BookingAverageReport(BookingStatus.HoanThanh));
            report.Add(_clickBayService.BookingAverageReport(BookingStatus.HoanVe));
            report.Add(_clickBayService.BookingAverageReport(BookingStatus.DaHuy));
            var model = report.Select(x =>
            {
                return new BookingAverageReportLineSummaryModel()
                {
                    BookingStatus = x.BookingStatus.GetLocalizedEnum(_localizationService, _workContext),
                    SumTodayBookings = x.SumTodayBookings.ToString("#,0"),
                    SumThisWeekBookings = x.SumThisWeekBookings.ToString("#,0"),
                    SumThisMonthBookings = x.SumThisMonthBookings.ToString("#,0"),
                    SumThisYearBookings = x.SumThisYearBookings.ToString("#,0"),
                    SumAllTimeBookings = x.SumAllTimeBookings.ToString("#,0"),
                };
            }).ToList();

            return model;
        }

        [ChildActionOnly]
        public ActionResult BookingAverageReport()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return Content("");

            var model = GetBookingAverageReportModel();

            return PartialView(model);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult BookingrAverageReportList(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return Content("");

            var model = GetBookingAverageReportModel();
            var gridModel = new GridModel<BookingAverageReportLineSummaryModel>
            {
                Data = model,
                Total = model.Count
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [ChildActionOnly]
        public ActionResult ListReport()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return Content("");
            return PartialView();
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult BookingListReport(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return Content("");

            var bookings = _clickBayService.GetAllBooking(fromDate: null,
                toDate: null, bookingStatusId: null, paymentStatusId: null,
                contactStatusId: null, customerId: 0,
                contactNameOrPhone: null, pageIndex: command.Page - 1, pageSize: command.PageSize, id: 0);

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
                        BookingStatus = x.BookingStatus.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                        PaymentStatus = x.PaymentStatus.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                        BookingStatusId = x.BookingStatusId,
                        TotalAmount = x.TotalAmount
                    };
                }),
                Total = bookings.TotalCount
            };


            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion

        #region Airline Baggage Condition

        public ActionResult AirlineBaggageConditionList()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var model = new SearchConditionsModel();

            model.Airlines = _clickBayService.GetListAirline().Select(x => new SelectListItem { Text = x.AirlinesName, Value = x.Id.ToString() }).ToList();
            model.Airlines.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View(model);
        }

        [GridAction(EnableCustomBinding = true)]
        [HttpPost]
        public ActionResult AirlineBaggageConditionList1(GridCommand command, SearchConditionsModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var conditions = _clickBayService.GetListArilinesBaggageCondition(searchModel.AirlineId);

            var gridModel = new GridModel<ArilinesBaggageConditionModel>
            {
                Data = conditions.Select(x =>
                {
                    return new ArilinesBaggageConditionModel()
                    {
                        Id = x.Id,
                        AirlinesId = x.AirlinesId,
                        Baggage = x.Baggage,
                        BaggageFee = x.BaggageFee,
                        Description = x.Description,
                        DisplayOrder = x.DisplayOrder,
                        IsFree = x.IsFree,
                        IsHandLuggage = x.IsHandLuggage,
                    };
                }),
                Total = conditions.Count()
            };


            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult AirlineBaggageConditionCreate()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var model = new ArilinesBaggageConditionModel();

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult AirlineBaggageConditionCreate(ArilinesBaggageConditionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                _clickBayService.InsertAirlinesBaggageCondition(entity);
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("AirlineBaggageConditionEdit", new { id = entity.Id });
                }
                else
                {
                    return RedirectToAction("AirlineBaggageConditionList");
                }
            }

            return View(model);
        }

        public ActionResult AirlineBaggageConditionEdit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var baggage = _clickBayService.ArilinesBaggageConditionById(id);
            if (baggage == null || baggage.Deleted)
                return RedirectToAction("AirlineBaggageConditionList");

            var model = baggage.ToModel();

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult AirlineBaggageConditionEdit(ArilinesBaggageConditionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var baggage = _clickBayService.ArilinesBaggageConditionById(model.Id);
            if (baggage == null || baggage.Deleted)
                return RedirectToAction("AirlineBaggageConditionList");
            if (ModelState.IsValid)
            {
                baggage.AirlinesId = model.AirlinesId;
                baggage.Baggage = model.Baggage;
                baggage.BaggageFee = model.BaggageFee;
                baggage.Description = model.Description;
                baggage.DisplayOrder = model.DisplayOrder;
                baggage.IsFree = model.IsFree;
                baggage.IsHandLuggage = model.IsHandLuggage;

                _clickBayService.UpdateAirlinesBaggageCondition(baggage);
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("AirlineBaggageConditionEdit", new { id = model.Id });
                }
                else
                {
                    return RedirectToAction("AirlineBaggageConditionList");
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AirlineBaggageConditionDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var baggage = _clickBayService.ArilinesBaggageConditionById(id);
            if (baggage == null || baggage.Deleted)
                return RedirectToAction("AirlineBaggageConditionList");

            _clickBayService.DeleteAirlinesBaggageCondition(baggage);
            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));
            
            return RedirectToAction("AirlineBaggageConditionList");

        }
        #endregion


        #region Airline Condition

        public ActionResult AirlineConditionList()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var model = new SearchConditionsModel();

            model.Airlines = _clickBayService.GetListAirline().Select(x => new SelectListItem { Text = x.AirlinesName, Value = x.Id.ToString() }).ToList();
            model.Airlines.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View(model);
        }

        [GridAction(EnableCustomBinding = true)]
        [HttpPost]
        public ActionResult AirlineConditionList1(GridCommand command, SearchConditionsModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var conditions = _clickBayService.GetListAirlinesConditionByAirlineId(searchModel.AirlineId);

            var gridModel = new GridModel<AirlinesConditionModel>
            {
                Data = conditions.Select(x =>
                {
                    return new AirlinesConditionModel()
                    {
                        Id = x.Id,
                        AirlinesId = x.AirlinesId,
                        ConditionName = x.ConditionName,
                        ConditionDescription = x.ConditionDescription,
                        DisplayOrder = x.DisplayOrder,
                        TicketType = x.TicketType,
                    };
                }),
                Total = conditions.Count()
            };


            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult AirlineConditionCreate()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var model = new AirlinesConditionModel();

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult AirlineConditionCreate(AirlinesConditionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                _clickBayService.InsertAirlinesCondition(entity);
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("AirlineConditionEdit", new { id = entity.Id });
                }
                else
                {
                    return RedirectToAction("AirlineConditionList");
                }
            }

            return View(model);
        }

        public ActionResult AirlineConditionEdit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var condition = _clickBayService.AirlinesConditionById(id);
            if (condition == null || condition.Deleted)
                return RedirectToAction("AirlineConditionList");

            var model = condition.ToModel();

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult AirlineConditionEdit(AirlinesConditionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var condition = _clickBayService.AirlinesConditionById(model.Id);
            if (condition == null || condition.Deleted)
                return RedirectToAction("AirlineConditionList");
            if (ModelState.IsValid)
            {
                condition.ConditionName = model.ConditionName;
                condition.ConditionDescription = model.ConditionDescription;
                condition.DisplayOrder = model.DisplayOrder;
                condition.AirlinesId = model.AirlinesId;
                condition.TicketType = model.TicketType;

                _clickBayService.UpdateAirlinesCondition(condition);
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("AirlineConditionEdit", new { id = model.Id });
                }
                else
                {
                    return RedirectToAction("AirlineConditionList");
                }
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult AirlineConditionDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            var condition = _clickBayService.AirlinesConditionById(id);
            if (condition == null || condition.Deleted)
                return RedirectToAction("AirlineConditionList");

            _clickBayService.DeleteAirlinesCondition(condition);
            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));

            return RedirectToAction("AirlineConditionList");

        }
        #endregion
    }
}