using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PlanX.Admin.Models.News;
using PlanX.Core.Domain.Customers;
using PlanX.Core.Domain.News;
using PlanX.Services.Helpers;
using PlanX.Services.Localization;
using PlanX.Services.News;
using PlanX.Services.Security;
using PlanX.Services.Seo;
using PlanX.Services.Stores;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Controllers;
using PlanX.Web.Framework.Mvc;
using System.Web;
using PlanX.Services.ClickBay;
using PlanX.Admin.Models.TicketConcession;
using Telerik.Web.Mvc;
using PlanX.Core.Domain.ClickBay;

namespace PlanX.Admin.Controllers
{
    [AdminAuthorize]
    public class TicketConcessionController : BaseNopController
    {
	    #region Fields

        private readonly ITicketConcessionService _ticketConcessionService;
        private readonly ILanguageService _languageService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IStoreService _storeService;
        private readonly IStoreMappingService _storeMappingService;
        
		#endregion

		#region Constructors

        public TicketConcessionController(ITicketConcessionService ticketConcessionService, 
            ILanguageService languageService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            IStoreService storeService, 
            IStoreMappingService storeMappingService)
        {
            this._ticketConcessionService = ticketConcessionService;
            this._languageService = languageService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._urlRecordService = urlRecordService;
            this._storeService = storeService;
            this._storeMappingService = storeMappingService;
		}

		#endregion 

        #region Utilities

        [NonAction]
        protected virtual void PrepareStoresMappingModel(NewsItemModel model, NewsItem newsItem, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableStores = _storeService
                .GetAllStores()
                .Select(s => s.ToModel())
                .ToList();
            if (!excludeProperties)
            {
                if (newsItem != null)
                {
                    model.SelectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(newsItem);
                }
                else
                {
                    model.SelectedStoreIds = new int[0];
                }
            }
        }

        [NonAction]
        protected virtual void SaveStoreMappings(NewsItem newsItem, NewsItemModel model)
        {
            var existingStoreMappings = _storeMappingService.GetStoreMappings(newsItem);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds != null && model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(newsItem, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }


        #endregion

        #region TicketConcession
        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var model = new TicketConcessionListModel();
            //stores
            model.listType = _ticketConcessionService.GetAllTicketType().ToList();
            model.listPlace = _ticketConcessionService.GetAllPlace().ToList();

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command,TicketConcessionListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            var time = model.DepartDateSearch.ToShortDateString();
            if (model.DepartDateSearch.Year < 2001)
                time = "";
            var listItem = _ticketConcessionService.SearchTicketConcession(command.Page-1,command.PageSize, model.PassengerNameSearch, model.FromPlaceSearch, model.ToPlaceSearch, model.TicketTypeSearch, time);
            var gridModel = new GridModel<TicketConcessionModel>
            {
                Data = listItem.Select(item =>
                {
                    var temp = new TicketConcessionModel();
                    temp.Id = item.Id;
                    temp.ContactEmail = item.ContactEmail;
                    temp.ContactName = item.ContactName;
                    if (item.IsRename == true)
                        temp.ContactName = temp.ContactName + "( Có thể đổi )";
                    temp.ContactPhone = item.ContactPhone;
                    temp.CreatedOnUtc = DateTime.Now;
                    temp.Deleted = false;
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
                        temp.CurrencyCode = "Thỏa thuận";
                    }
                    else
                    {
                        temp.CurrencyCode = item.TicketPrice.ToString("0,#") + " " + item.CurrencyCode;
                    }
                    temp.ToPlace = item.ToPlace;
                    return temp;

                }),
                Total = listItem.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Edit(int Id)
        {
             if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
             var model = PrepareTicketConcessionModel(_ticketConcessionService.GetTicketConcessionById(Id));
            if (model != null)
                return View(model);
            else
                return RedirectToAction("List");
        }

        public TicketConcessionModel PrepareTicketConcessionModel(TicketConcession item)
        {
            if (item == null)
                return null;
            var temp = new TicketConcessionModel();
            temp.Id = item.Id;
            temp.ContactEmail = item.ContactEmail;
            temp.ContactName = item.ContactName;
            temp.IsHelper = item.IsHelper;
            temp.IsRename = item.IsRename;
            temp.ContactPhone = item.ContactPhone;
            temp.CreatedOnUtc = item.CreatedOnUtc;
            temp.Deleted = item.Deleted;
            temp.DepartDate = item.DepartDate;
            temp.Description = item.Description;
            temp.FromPlace = item.FromPlace;
            temp.IsHelper = item.IsHelper;
            temp.PassengerName = item.PassengerName;
            temp.ReturnDate = item.ReturnDate;
            temp.RoundTrip = item.RoundTrip;
            temp.TicketType = item.TicketType;
            temp.TicketPrice = item.TicketPrice;
            temp.CurrencyCode = item.CurrencyCode;
            temp.ToPlace = item.ToPlace;

            return temp;
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(TicketConcessionModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var ticketConcession = _ticketConcessionService.GetTicketConcessionById(model.Id);
                ticketConcession.ContactEmail = model.ContactEmail;
                ticketConcession.ContactName = model.ContactName;
                ticketConcession.ContactPhone = model.ContactPhone;
                ticketConcession.CurrencyCode = model.CurrencyCode;
                ticketConcession.DepartDate = Convert.ToDateTime(model.DepartDate);
                ticketConcession.Description = model.Description;
                ticketConcession.FromPlace = model.FromPlace;
                ticketConcession.IsHelper = model.IsHelper;
                ticketConcession.PassengerName = model.PassengerName;
                ticketConcession.IsRename = model.IsRename;
                if (model.RoundTrip == true)
                    ticketConcession.ReturnDate = Convert.ToDateTime(model.ReturnDate );
                else
                    ticketConcession.ReturnDate = ticketConcession.DepartDate;
                ticketConcession.RoundTrip = model.RoundTrip;
                ticketConcession.TicketPrice = model.TicketPrice;
                ticketConcession.TicketType = model.TicketType;
                ticketConcession.ToPlace = model.ToPlace;
                ticketConcession.IsRename = model.IsRename;

                _ticketConcessionService.UpdateTicketConcession(ticketConcession);
                SuccessNotification("Sửa tin thành công");
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = ticketConcession.Id });
                }
                else
                {
                    return RedirectToAction("List");
                }
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var model = _ticketConcessionService.GetTicketConcessionById(id);
            
            if (model == null)
                //No news item found with the specified id
                return RedirectToAction("List");

            _ticketConcessionService.DeleteTicketConcession(model);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Deleted"));
            return RedirectToAction("List");
        }
        #endregion

        #region Type
        public ActionResult IndexType()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMeasures))
                return AccessDeniedView();

            return View();
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ListType(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var lst = _ticketConcessionService.GetAllType();
            var lstModel = new List<TicketTypeAndPlceModel>();
            foreach(var x in lst)
            {
                var temp = new TicketTypeAndPlceModel();
                temp.Id = x.Id;
                temp.Name = x.TicketTypeName;
                lstModel.Add(temp);
            }
            
            var model = new GridModel<TicketTypeAndPlceModel>
            {
                Data = lstModel,
                Total = lstModel.Count
            };

		    return new JsonResult
			{
				Data = model
			};
		}

        [GridAction(EnableCustomBinding=true)]
        public ActionResult TicketTypeUpdate(TicketTypeAndPlceModel model, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            
            if (!ModelState.IsValid)
            {
                //display the first model error
                var modelStateErrors = this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return Content(modelStateErrors.FirstOrDefault());
            }

            var temp = _ticketConcessionService.GetTicketTypeById(model.Id);
            temp.TicketTypeName = model.Name;
            _ticketConcessionService.UpdateTicketType(temp);

            return ListType(command);
        }
        
        [GridAction(EnableCustomBinding = true)]
        public ActionResult TicketTypeAdd([Bind(Exclude = "Id")] TicketTypeAndPlceModel model, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMeasures))
                return AccessDeniedView();

            if (!ModelState.IsValid)
            {
                //display the first model error
                var modelStateErrors = this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return Content(modelStateErrors.FirstOrDefault());
            }

            var temp = new TicketType();
            temp.TicketTypeName = model.Name;
            _ticketConcessionService.InsertTicketType(temp);

            return ListType(command);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult TicketTypeDelete(int id,  GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMeasures))
                return AccessDeniedView();

            var temp = _ticketConcessionService.GetTicketTypeById(id);
            if (temp == null)
                throw new ArgumentException("No Ticket Type found with the specified id");

           
            _ticketConcessionService.DeleteTicketType(temp);


            return ListType(command);
        }

        #endregion

        #region Place
        public ActionResult IndexPlace()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMeasures))
                return AccessDeniedView();

            return View();
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ListPlace(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var lst = _ticketConcessionService.GetAllPlaceToAdmin();
            var lstModel = new List<TicketTypeAndPlceModel>();
            foreach (var x in lst)
            {
                var temp = new TicketTypeAndPlceModel();
                temp.Id = x.Id;
                temp.Name = x.PlaceName;
                lstModel.Add(temp);
            }

            var model = new GridModel<TicketTypeAndPlceModel>
            {
                Data = lstModel,
                Total = lstModel.Count
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult PlaceUpdate(TicketTypeAndPlceModel model, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (!ModelState.IsValid)
            {
                //display the first model error
                var modelStateErrors = this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return Content(modelStateErrors.FirstOrDefault());
            }

            var temp = _ticketConcessionService.GetPlaceById(model.Id);
            temp.PlaceName = model.Name;
            _ticketConcessionService.UpdatePlace(temp);

            return ListPlace(command);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult PlaceAdd([Bind(Exclude = "Id")] TicketTypeAndPlceModel model, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (!ModelState.IsValid)
            {
                //display the first model error
                var modelStateErrors = this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return Content(modelStateErrors.FirstOrDefault());
            }

            var temp = new Place();
            temp.PlaceName = model.Name;
            _ticketConcessionService.InsertPlace(temp);

            return ListPlace(command);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult PlaceDelete(int id, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var temp = _ticketConcessionService.GetPlaceById(id);
            if (temp == null)
                throw new ArgumentException("No Ticket Type found with the specified id");


            _ticketConcessionService.DeletePlace(temp);


            return ListPlace(command);
        }

        #endregion
    }
}