using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanX.Core.Domain.ClickBay;
using PlanX.Services.ClickBay;
using PlanX.Web.Models.ClickBay;
using PlanX.Core;
using PlanX.Web.Framework;
using PlanX.Core.Caching;
using PlanX.Services.Localization;
using System.Globalization;

namespace PlanX.Web.Controllers
{
    public class ClickBayController : Controller
    {
        #region Fields
        private readonly IClickBayService _clickBayService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private const string CACHE_SEARCH_MODEL="CACHE_SEARCH_MODEL";
        private const string SELECTED_TICKET_SESSION = "BOOKING_{0}";
        private const string SESSION_SEARCH_NAME = "{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}";
        private const string VIETNAM_CITY_CACHE = "VietNam_City_Cache";
        #endregion
        
        #region Ctor
        public ClickBayController(IClickBayService clickBayService,IWorkContext workContext,
        ILocalizationService localizationService
            ,ICacheManager cacheManager
            )
        {
            this._clickBayService = clickBayService;
            _workContext = workContext;
            _cacheManager = cacheManager;
            _localizationService=localizationService;
        }
        #endregion

        #region Method
        #region common
        [HttpGet]
        public ActionResult GetCountries()
        {
            var data = _clickBayService.GetListCountry();
            return new JsonResult
            {
                Data = data.Select(x => new { x.Code, x.Name, x.Id }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult Getcity(string term=null)
        {
            if(term!=null)
            {
                term = term.RemoveSign4VietnameseString();
            }
            var data = _clickBayService.GetListCity(0,term);           
            return new JsonResult
            {
                Data = data.Select(x => new { x.Code, x.Name, x.Id }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        #region Search
        public ActionResult SearchBox()
        {
            var model = new SearchModel();
            model.ListCitys =_cacheManager.Get(VIETNAM_CITY_CACHE,()=>{
             return   _clickBayService.GetListCity(1).Select(x => new SelectListItem
            {
                Value=x.Id.ToString(),
                Text=x.Name
            }).ToList();
            });
            return View(model);   
        }
        public ActionResult Search(SearchModel model)
        {
           if(!string.IsNullOrEmpty(model.FromId))
            model.FromName = _clickBayService.GetcityByCode(model.FromId).Name;
           if (!string.IsNullOrEmpty(model.ToId))
            model.ToName = _clickBayService.GetcityByCode(model.ToId).Name;

         
          
            return View(model);
            
        }

        public ActionResult TicketSearch(SearchModel model)
        {
            
            if (model.SessionId != null)
                model.Tickets = Session[model.SessionId] as List<TicketModel>;
            else
            {
                DateTime datePart = DateTime.ParseExact(model.DepartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime? returnDate = null;
                if (model.ReturnDate != null)
                    returnDate = DateTime.ParseExact(model.ReturnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var result = _clickBayService.SearchTicket(
                    model.FromId, model.ToId,
                    datePart, model.Adult,
                    model.Child, model.Flant,
                    returnDate: returnDate,
                    source: model.Source != null ? model.Source.Aggregate((a, b) => a + "," + b) : null,
                    expendDetails: true,
                    roundTrip:returnDate.HasValue                    
                    );
                var ticket = PrepairingTicketModel(result);
                DateTime dt = DateTime.Now;
                model.SessionId = string.Format(SELECTED_TICKET_SESSION, model.FromId,                     
                    model.ToId, datePart.ToString("ddMMyyyy"),
                    returnDate.HasValue ? returnDate.Value.ToString("ddMMyyyy") : "",
                    model.Adult, model.Child, model.Flant);
                
              Session[model.SessionId]=  model.Tickets = ticket.ToList();
            
            }
            return PartialView("_SearchTicketPartial", model);
        }
        [HttpGet]
       public ActionResult GetTicketDetail(string sessionId,int id)
        {
            if (Session[sessionId] == null)
                return Content("");
            var ticket = (Session[sessionId] as List<Ticket>).FirstOrDefault(x => x.Id == id);
            if (ticket == null)
                return Content("");
            return PartialView("_TicketDetail", ticket);
        }
        #endregion

        #region Booking
        public ActionResult Booking()
        {
            var selectedTicket=Session[string.Format(SELECTED_TICKET_SESSION, _workContext.CurrentCustomer.Id)]as TicketModel;
            if (selectedTicket == null)
                return RedirectToRoute("HomePage");
            var model = new BookingModel();
            
            selectedTicket.AirlinesConditions = _clickBayService.GetListAirlinesConditionByAirlineId(selectedTicket.AirlineId).Select(x => new TicketModel.AirlinesConditionModel { 
            ConditionDescription=x.ConditionDescription,
            ConditionName=x.ConditionName
            }).ToList();
            selectedTicket.ArilinesBaggageConditions = _clickBayService.GetListArilinesBaggageCondition(selectedTicket.AirlineId).Select(x => new TicketModel.ArilinesBaggageCondition { 
            Baggage=x.Baggage,
            BaggageFee=x.BaggageFee
            
            }).ToList();
            
            model.PassengerTypes = Enum.GetValues(typeof(PasserType)).Cast<PasserType>().Select(v => new SelectListItem
            {
                Text =_localizationService.GetResource("PasserType."+v.ToString()),
                Value = ((int)v).ToString()
            }).ToList();
            
            model.TicketInfo = selectedTicket;
            
            return View();
        }
        public ActionResult SelectedTicket(int id,string session)
        {
            if (Session[session] == null)
                return Json("NOTOK");
            var ticket = (Session[session] as List<TicketModel>).FirstOrDefault(x => x.Id == id);
            if (ticket == null)
                return Json("NOTOK");
          
            Session[string.Format(SELECTED_TICKET_SESSION,_workContext.CurrentCustomer.Id)] = ticket;
            return RedirectToAction("Booking");
        }
        #endregion
        
    /*  public string a()
        {
            var country = _clickBayService.GetCountry();
           country.ToList().ForEach(x => _clickBayService.InsertCountry(x));
                //var city = _clickBayService.GetCity();
         //  city.ToList().ForEach(x => _clickBayService.UpdateCity(x));
           // var airport = _clickBayService.GetAirport();
        //    airport.ToList().ForEach(x => _clickBayService.InsertAirport(x));
          // string a=_clickBayService.GetData();
            return "a";
        }*/
        #endregion

        #region Until
        private IEnumerable<TicketModel> PrepairingTicketModel(IEnumerable<Ticket> data)
        {
            if (data == null)
                return new List<TicketModel>();

            return data.Select(x =>
            {
                var item = new TicketModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    FlightNumber = x.FlightNumer,
                    DepartTime = x.DepartTime,
                    LandingTime = x.LandingTime,
                    FromAirport = x.FromAirport,
                    ToAirport = x.ToAirport,
                    FromAirportCode = x.FromAirportId.ToString(),
                    ToAirportCode = x.ToAirportId.ToString(),
                    FromPlace = x.FromPlace,

                    ToPlace = x.ToPlace,
                    BrandCode = x.Airline,
                    AirlineName = x.Airline,
                    HangVe = x.FareBasis,
                    AirlineId=x.AirlineCode,
                    



                };
               
                try
                {
                    item.FromCountry = _clickBayService.GetCityById(x.FromPlaceId).Country.Name;
                    item.ToCountry = _clickBayService.GetCityById(x.ToPlaceId).Country.Name;
                }
                catch { }
                return item;
            }

                );

        }

        #endregion



    }
   
}