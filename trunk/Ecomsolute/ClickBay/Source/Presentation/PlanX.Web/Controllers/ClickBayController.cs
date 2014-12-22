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
using PlanX.Services.Messages;

namespace PlanX.Web.Controllers
{
    public class ClickBayController : Controller
    {
        #region const
        private const string CACHE_SEARCH_MODEL = "CACHE_SEARCH_MODEL";
        private const string SELECTED_TICKET_SESSION = "BOOKING_{0}";
        private const string SUCCESS_BOOKING_SESSION = "SUCCESS_{0}";
        private const string SESSION_SEARCH_NAME = "{0}-{1}-{2}-{3}-{4}-{5}-{6}";
        private const string VIETNAM_CITY_CACHE = "VietNam_City_Cache";
        private const string COUNTRIES_CACHE = "COUNTRIES_CACHE";
        private const string CODE_FEE_SYSTEM = "CLY";
        private const string CODE_DIS_SYSTEM = "DIS";
        #endregion

        #region Fields
        private readonly IClickBayService _clickBayService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly INewsLetterSubscriptionService _NewsLetterSubscriptionService;
        private readonly IWorkflowMessageService _workflowMessageService;
        #endregion

        #region Ctor
        public ClickBayController(IClickBayService clickBayService, IWorkContext workContext,
            ILocalizationService localizationService,
            ICacheManager cacheManager,
            INewsLetterSubscriptionService NewsLetterSubscriptionService,
            IWorkflowMessageService workflowMessageService
            )
        {
            this._NewsLetterSubscriptionService = NewsLetterSubscriptionService;
            this._clickBayService = clickBayService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
            this._workflowMessageService = workflowMessageService;
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
        public ActionResult Getcity(string term = null)
        {
            if (term != null)
            {
                term = term.RemoveSign4VietnameseString();
            }
            var data = _clickBayService.GetListCity(0, term);
            return new JsonResult
            {
                Data = data.Select(x => new { x.Code, x.Name, x.Id }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private string GetCountryName(int id)
        {
            var c = _clickBayService.GetCountryById(id);
            if (c != null)
                return c.Name;
            return "";
        }
        
        #endregion

        #region Search
        public ActionResult SearchBox(bool isSmall = false)
        {
            var model = new SearchModel();
            model.ListCitys = _cacheManager.Get(VIETNAM_CITY_CACHE, () =>
            {
                return _clickBayService.GetListCity(1).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
            });
            if (isSmall)
                return View("_SearchBoxSmall", model);
            return View(model);
        }

        public ActionResult Search(SearchModel model)
        {
            if (!string.IsNullOrEmpty(model.FromId))
                model.FromName = _clickBayService.GetcityByCode(model.FromId).Name;
            if (!string.IsNullOrEmpty(model.ToId))
                model.ToName = _clickBayService.GetcityByCode(model.ToId).Name;

            if (model.Adult < 1 && model.Child < 1 && model.Flant < 1)
                return RedirectToRoute("HomePage");

            DateTime datePart;
            DateTime returnDate;
            if (!DateTime.TryParseExact(model.DepartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out datePart))
            {
                return RedirectToRoute("HomePage");
            }
            else { model.SearchDate = datePart; }

            if (model.Return)
            {
                if (string.IsNullOrEmpty(model.ReturnDate))
                {
                    return RedirectToRoute("HomePage");
                }
                if (!DateTime.TryParseExact(model.ReturnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out returnDate))
                {
                    return RedirectToRoute("HomePage");
                }
                if (datePart > returnDate)
                    return RedirectToRoute("HomePage");
            }

            
            if(string.IsNullOrEmpty(model.FromId) || string.IsNullOrEmpty(model.ToId) || model.FromId == model.ToId)
                return RedirectToRoute("HomePage");

            return View(model);

        }

        
        public ActionResult TicketSearchGo(SearchModel model)
        {
            model = TicketSearch(model);
            return PartialView("_SearchTicketPartial", model);
        }

        
        public ActionResult TicketSearchReturn(SearchModel model)
        {
            model = TicketSearch(model);
            return PartialView("_SearchTicketPartial", model);
        }


        private SearchModel TicketSearch(SearchModel model)
        {
            //checked parameter is valid
            DateTime datePart = DateTime.ParseExact(model.DepartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            model.SearchDate = datePart;

            DateTime? returnDate = null;
            if (model.Return&&!string.IsNullOrWhiteSpace(model.ReturnDate))
            {
                returnDate = DateTime.ParseExact(model.ReturnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (datePart < DateTime.Now.Date)
                throw new ArgumentException("Depart date is isvalid");
            if (model.Return && (returnDate < DateTime.Now.Date || datePart > returnDate))
                throw new ArgumentException("Return date is isvalid");

            if (model.Adult < 1 && model.Child < 1 && model.Flant < 1)
                throw new ArgumentException("Adult or child or infant is isvalid");

            model.Tickets = new List<TicketModel>();

            //create sessionId
            model.SessionId = string.Format(SESSION_SEARCH_NAME, model.FromId,
                    model.ToId, datePart.ToString("ddMMyyyy"),
                    model.Return ? returnDate.Value.ToString("ddMMyyyy") : "",
                    model.Adult, model.Child, model.Flant);


            //find session has value
            if (Session[model.SessionId] != null)
                model.Tickets = Session[model.SessionId] as List<TicketModel>;

            var sources = new List<string>();
            if (!string.IsNullOrWhiteSpace(model.Source))
            {
                sources = model.Source.Split(',').ToList();
            }

            if (model.Tickets != null && model.Tickets.Count > 0)
            {
                if (sources.Count > 0)
                    model.Tickets = model.Tickets.Where(x => sources.Contains(x.AirlineName)).OrderBy(x => x.Price).ToList();
            }
            else//create new session if not found
            {
                var result = _clickBayService.SearchTicket(
                    model.FromId, model.ToId,
                    datePart, model.Adult,
                    model.Child, model.Flant,
                    returnDate: null,
                    source: sources.Count > 0 ? sources.Aggregate((a, b) => a + "," + b) : null,
                    expendDetails: true,
                    expendTicketPriceDetails: false,
                    priceSummaries: true,
                    roundTrip: false
                    );
                var ticket = PrepairingTicketModel(result, true, model.Adult, model.Child, model.Flant);

                //DateTime dt = DateTime.Now;
                //model.SessionId = string.Format(SESSION_SEARCH_NAME, model.FromId,
                //    model.ToId, datePart.ToString("ddMMyyyy"),
                //    returnDate.HasValue ? returnDate.Value.ToString("ddMMyyyy") : "",
                //    model.Adult, model.Child, model.Flant);

                Session[model.SessionId] = model.Tickets = ticket.OrderBy(x => x.Price).ToList();

            }
            if (model.Sort == (int)Sort.Price)
            {
                model.Tickets = model.Tickets.OrderBy(x => x.Price).ToList();
            }
            else
                if (model.Sort == (int)Sort.Date)
                {
                    model.Tickets = model.Tickets.OrderBy(x => x.DepartTime).ToList();
                }
                else
                {
                    model.Tickets = model.Tickets.OrderByDescending(x => x.AirlineName).ToList();
                }
            return  model;
        }
        [HttpGet]
        public ActionResult GetTicketDetail(string sessionId, int index)
        {
            if (Session[sessionId] == null)
                return Content("");
            var ticket = (Session[sessionId] as List<TicketModel>).FirstOrDefault(x => x.Index == index);
            if (ticket == null)
                return Content("");
            ticket = PrepareSelectedTicket(ticket);
            ticket.TotalPriceShows = GetTotalPriceShow(ticket);//get price only for show detail
            return PartialView("_TicketDetail", ticket);
        }
        private List<TicketModel.TotalPriceShow> GetTotalPriceShow(TicketModel ticket)
        {
            if (ticket == null)
                return null;
            if (ticket.BookingFlightPriceModels != null && ticket.BookingFlightPriceModels.Count > 0)
            {
                var TotalPriceShows = new List<TicketModel.TotalPriceShow>();
                if (ticket.Adult > 0)
                {
                    var adt = ticket.BookingFlightPriceModels.Where(x => x.PassengerType == PassengerType.ADT.ToString()).ToList();
                    TotalPriceShows.Add(new TicketModel.TotalPriceShow
                    {
                        PassengerType = PassengerType.ADT.ToString(),
                        Price = adt.Where(a => a.Code == "NET").Sum(b => b.Price),
                        Quantity = ticket.Adult,
                        TaxAndFee = adt.Where(a => a.Code != "NET" && a.Code != "DIS").Sum(b => b.Price),
                        DiscountAmount = adt.Where(a => a.Code == "DIS").Sum(b => b.Price),
                    });
                }
                if (ticket.Child > 0)
                {
                    var adt = ticket.BookingFlightPriceModels.Where(x => x.PassengerType == PassengerType.CHD.ToString()).ToList();
                    TotalPriceShows.Add(new TicketModel.TotalPriceShow
                    {
                        PassengerType = PassengerType.CHD.ToString(),
                        Price = adt.Where(a => a.Code == "NET").Sum(b => b.Price),
                        Quantity = ticket.Child,
                        TaxAndFee = adt.Where(a => a.Code != "NET" && a.Code != "DIS").Sum(b => b.Price),
                        DiscountAmount = adt.Where(a => a.Code == "DIS").Sum(b => b.Price),
                    });
                }
                if (ticket.Infant > 0)
                {
                    var adt = ticket.BookingFlightPriceModels.Where(x => x.PassengerType == PassengerType.INF.ToString()).ToList();
                    TotalPriceShows.Add(new TicketModel.TotalPriceShow
                    {
                        PassengerType = PassengerType.INF.ToString(),
                        Price = adt.Where(a => a.Code == "NET").Sum(b => b.Price),
                        Quantity = ticket.Infant,
                        TaxAndFee = adt.Where(a => a.Code != "NET" && a.Code != "DIS").Sum(b => b.Price),
                        DiscountAmount = adt.Where(a => a.Code == "DIS").Sum(b => b.Price),
                    });
                }
                return TotalPriceShows;
            }
            return null;
        }
        #endregion

        [NonAction]
        private List<TicketModel.BookingFlightPriceModel> GetBookingPricePlus(short adult, short child, short infant, decimal price, string codeFee, string description)
        {
            if((adult==0&&child==0&&infant==0)||price==0)
                return null;

            var list = new List<TicketModel.BookingFlightPriceModel>();
            int i=0;
            if (adult > 0) i++;
            if (child>0) i++;
            if (infant>0) i++;

            short quantity = 0;
            string passengerString="";

            for (var j = 0; j < i;j++)
            {
                if(j==0)
                {
                    quantity = adult;
                    passengerString = PassengerType.ADT.ToString();
                }else if(j==1)
                {
                    quantity = child;
                    passengerString = PassengerType.CHD.ToString();
                }
                else if(j==2)
                {
                    quantity = infant;
                    passengerString = PassengerType.INF.ToString();
                }

                list.Add(new TicketModel.BookingFlightPriceModel()
                {
                    Code = codeFee,
                    Description = description,
                    Price = price,
                    Quantity = quantity,
                    Total = price * quantity,
                    PassengerType = passengerString
                });
            }
            return list;
                
        }

        [NonAction]
        private TicketModel PrepareSelectedTicket(TicketModel selectedTicket)
        {
            if (selectedTicket == null)
                return null;

            var ticket = selectedTicket;

            //phi cong them cua he thong - tinh theo moi luot di://chua chuyen sang setting
            decimal PricePlusFerPassenger = 0;
            if (decimal.TryParse(_localizationService.GetResource("booking.price.clickbay.amount"), out PricePlusFerPassenger))
            {
                if (PricePlusFerPassenger > 0)
                {
                    //remove all fee old
                    var fee = ticket.BookingFlightPriceModels.Where(x => x.Code == CODE_FEE_SYSTEM).FirstOrDefault();
                    if (fee != null)
                    {
                        ticket.BookingFlightPriceModels.RemoveAll(x=> x.Code==CODE_FEE_SYSTEM);
                    }
                    //add new fee
                    var feeNew = GetBookingPricePlus(ticket.Adult, ticket.Child, ticket.Infant, PricePlusFerPassenger, CODE_FEE_SYSTEM, _localizationService.GetResource("booking.price.clickbay.description"));
                    ticket.BookingFlightPriceModels.AddRange(feeNew);
                }
            }
            //giam gia cua he thong - cho moi hanh khach//chua chuyen sang setting
            decimal discountAmount = 0;
            if (decimal.TryParse(_localizationService.GetResource("booking.price.discount.amount"), out discountAmount))
            {
                if (discountAmount > 0)
                {
                    //remove all discount old
                    var dis = ticket.BookingFlightPriceModels.Where(x => x.Code == CODE_DIS_SYSTEM).FirstOrDefault();
                    if (dis != null)
                    {
                        ticket.BookingFlightPriceModels.RemoveAll(x => x.Code == CODE_DIS_SYSTEM);
                    }
                    //add new discount
                    var discountNews = GetBookingPricePlus(ticket.Adult, ticket.Child, ticket.Infant, discountAmount, CODE_DIS_SYSTEM, _localizationService.GetResource("booking.price.discount.description"));
                    ticket.BookingFlightPriceModels.AddRange(discountNews);
                }
            }

            //dieu kien ve
            ticket.AirlinesConditions = _clickBayService.GetListAirlinesConditionByAirlineId(ticket.AirlineId).Where(x => !x.Deleted).OrderBy(x => x.DisplayOrder).Select(x => new TicketModel.AirlinesConditionModel()
            {
                ConditionName = x.ConditionName,
                ConditionDescription = x.ConditionDescription
            }).ToList();
            //dieu kien hanh ly
            ticket.ArilinesBaggageConditions = _clickBayService.GetListArilinesBaggageCondition(ticket.AirlineId).Where(x => !x.Deleted).Select(x => new TicketModel.ArilinesBaggageCondition
            {
                Id = x.Id,
                Description = x.Description,
                IsHandLuggage = x.IsHandLuggage,
                IsFree = x.IsFree,
                Baggage = x.Baggage,
                BaggageFee = x.BaggageFee,
                DisplayOrder = x.DisplayOrder
            }).ToList();

            return ticket;
        }

        #region Booking
        public ActionResult Booking()
        {
            var selectedTicket = Session[string.Format(SELECTED_TICKET_SESSION, _workContext.CurrentCustomer.Id)] as TicketSessionModel;

            if (selectedTicket == null)
                return RedirectToRoute("HomePage");

            var model = new BookingModel
            {
                Child = selectedTicket.Ticket.Child,
                Adult = selectedTicket.Ticket.Adult,
                Infant = selectedTicket.Ticket.Infant
            };

            model.PassengerTypes = Enum.GetValues(typeof(PasserType)).Cast<PasserType>().Select(v => new SelectListItem
            {
                Text = _localizationService.GetResource("PasserType." + v.ToString()),
                Value = ((int)v).ToString()
            }).ToList();

            var listPrice = new List<TicketModel.TotalPriceShow>();
            #region Ticket
            model.TicketInfo = PrepareSelectedTicket(selectedTicket.Ticket);
            listPrice.AddRange(GetTotalPriceShow(model.TicketInfo));
            #endregion

            #region ticket return
            if (selectedTicket.TicketReturn != null)
            {
                model.TicketInfoReturn = PrepareSelectedTicket(selectedTicket.TicketReturn);
                listPrice.AddRange(GetTotalPriceShow(model.TicketInfoReturn));
            }
            #endregion

            #region totalPrice
            model.TotalPriceShows = new List<TicketModel.TotalPriceShow>();
            var l1 = (from pr in listPrice
                      group pr by new { pr.PassengerType, pr.Quantity } into gr
                      select new
                      {
                          gr = gr.Key,
                          Price = gr.Sum(x => x.Price),
                          TaxAndFee = gr.Sum(x => x.TaxAndFee),
                          DiscountAmount = gr.Sum(x => x.DiscountAmount)
                      }).ToList();
            foreach (var i in l1)
            {
                model.TotalPriceShows.Add(new TicketModel.TotalPriceShow { PassengerType = i.gr.PassengerType, Quantity = i.gr.Quantity, Price = i.Price, TaxAndFee = i.TaxAndFee, DiscountAmount = i.DiscountAmount });
            }            
            #endregion

            model.Countries = _cacheManager.Get(COUNTRIES_CACHE, () =>
            {
                return _clickBayService.GetListCountry().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            });

            var fromPlace = _clickBayService.GetcityByCode(selectedTicket.Ticket.FromCode);

            for (int i = 0; i < selectedTicket.Ticket.Adult; i++)
            {
                var item = new BookingPasserModel
                {
                    PassserType = 1
                };
                model.BookingPassers.Add(item);
            }
            for (int i = 0; i < selectedTicket.Ticket.Child; i++)
            {
                var item = new BookingPasserModel
                {
                    PassserType = 2
                };
                model.BookingPassers.Add(item);
            }
            for (int i = 0; i < selectedTicket.Ticket.Infant; i++)
            {
                var item = new BookingPasserModel
                {
                    PassserType = 3
                };
                model.BookingPassers.Add(item);
            }
            return View(model);
        }

        public ActionResult BookingInfoPriceDetail()
        {
            var selectedTicket = Session[string.Format(SELECTED_TICKET_SESSION, _workContext.CurrentCustomer.Id)] as TicketModel;
            if (selectedTicket == null)
                return Content("");
            return View(selectedTicket);
        }

        private BookingInfoFlight PreparingBookingInfoFlight(TicketModel selectedTicket, BookingModel bookingModel, bool isFlightTo)
        {
            //booking info
            #region
            var bookingInfo = new BookingInfoFlight
            {
                Adult = selectedTicket.Adult,
                Child = selectedTicket.Child,
                Infant = selectedTicket.Infant,
                AirlinesId = selectedTicket.AirlineId,
                Brand = selectedTicket.BrandCode,
                FlightDuration = selectedTicket.FlightDuration,

                FlightNumber = selectedTicket.FlightNumber,
                DepartDateTime = selectedTicket.DepartTime,
                ArrivalDateTime = selectedTicket.LandingTime,

                Stops = selectedTicket.Stops,
                TicketType = selectedTicket.TicketType,
                FromPlaceCode = selectedTicket.FromCode,
                ToPlaceCode = selectedTicket.ToCode,
                ToPlaceName = selectedTicket.ToPlace,
                FromPlaceName = selectedTicket.FromPlace,
                FromPlaceId = selectedTicket.FromId,
                ToPlaceId = selectedTicket.ToId,
                FareBasis = selectedTicket.FareBasis,

            };
            var airline = _clickBayService.GetAirlineById(selectedTicket.AirlineId);
            if (airline != null)
            {
                selectedTicket.AirlineName = airline.AirlinesName;
                bookingInfo.BrandName = airline.AirlinesName;
            }            
            _clickBayService.InsertBookingInfoFlight(bookingInfo);
            #endregion
            //price
            #region
            selectedTicket.BookingFlightPriceModels.ForEach(x =>
            {
                var bpd = new BookingPriceDetail
                    {
                        CodeFee = x.Code,
                        Description = x.Description,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        TotalPrice = x.Price * x.Quantity,
                        TicketType = selectedTicket.TicketType,
                        BookingInfoFlightId = bookingInfo.Id,
                        PassengerType = x.PassengerType,
                    };
                _clickBayService.InsertBookingPriceDetail(bpd);

            });
            #endregion
            //baggages: 
            #region
            //gom: free + user chon hanh ly mang ky gui them;
            selectedTicket.ArilinesBaggageConditions.Where(x => x.IsFree).ToList().ForEach(x =>
            {
                var bb = new BookingBaggage
                {
                    Baggage = x.Baggage,
                    BaggageFee = x.BaggageFee,
                    IsFree = x.IsFree,
                    Description = x.Description,
                    IsHandLuggage = x.IsHandLuggage,
                    BookingInfoFlightId = bookingInfo.Id,
                    PassengerType = 0
                };
                _clickBayService.InsertBookingBaggage(bb);
            });
            //baggage fee
            List<int> baggageItems = new List<int>();
            int j = 0;
            if (isFlightTo)
            {
                if (!string.IsNullOrEmpty(bookingModel.TicketInfoBaggages))
                {
                    baggageItems = bookingModel.TicketInfoBaggages.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.TryParse(x, out j) ? j : 0).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(bookingModel.TicketInfoReturnBaggages))
                {
                    baggageItems = bookingModel.TicketInfoReturnBaggages.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.TryParse(x, out j) ? j : 0).ToList();
                }
            }
            if (baggageItems.Count > 0)
            {
                foreach (var i in baggageItems)
                {
                    var item = selectedTicket.ArilinesBaggageConditions.Where(x => x.Id == i && !x.IsFree && !x.IsHandLuggage).FirstOrDefault();
                    if (item != null)
                    {
                        var entityBag = new BookingBaggage
                             {
                                 Baggage = item.Baggage,
                                 BaggageFee = item.BaggageFee,
                                 Description = string.Format(item.Description, item.Baggage.ToString("0"), item.BaggageFee.ToString("0")),
                                 IsFree = item.IsFree,
                                 IsHandLuggage = item.IsHandLuggage,
                                 BookingInfoFlightId = bookingInfo.Id
                             };
                        _clickBayService.InsertBookingBaggage(entityBag);
                    }
                }
            }
            #endregion
            //conditions
            #region
            selectedTicket.AirlinesConditions.ForEach(x =>
            {
                var bic = new BookingInfoCondition
                {
                    ConditionDescription = x.ConditionDescription,
                    ConditionType = x.ConditionName,
                    BookingInfoFlightId = bookingInfo.Id
                };
                _clickBayService.InsertBookingInfoCondition(bic);
            });
            #endregion
            return bookingInfo;
        }

        [HttpPost]
        public ActionResult InsertBooking(BookingModel model)
        {

            var selectedTicket = Session[string.Format(SELECTED_TICKET_SESSION, _workContext.CurrentCustomer.Id)] as TicketSessionModel;

            if (selectedTicket == null)
                return RedirectToRoute("HomePage");

            #region Insert Ticket

            //Flight To
            BookingInfoFlight bookingFlight = PreparingBookingInfoFlight(selectedTicket.Ticket, model, true);
            //Flight Return
            BookingInfoFlight bookingFlightReturn = null;
            if (selectedTicket.TicketReturn != null)
            {
                bookingFlightReturn = PreparingBookingInfoFlight(selectedTicket.TicketReturn, model, false);
            }

            //booking
            var booking = new Booking
            {
                Adult = selectedTicket.Ticket.Adult,
                Child = selectedTicket.Ticket.Child,
                Infant = selectedTicket.Ticket.Infant,
                BookingStatusId = (int)BookingStatus.ChuaXyLy,
                PaymentStatusId = (int)PaymentStatus.ChuaThanhToan,
                IsInvoid = model.IsInvoid,
                CustomerId = 0,
                RoundTrip = bookingFlightReturn != null,
                ContactPassengerType = model.ContactPassengerType,
                ContactAddress = model.ContactAddress,
                ContactCityName = model.ContactCityName,
                ContactCountryId = model.ContactCountryId,
                ContactEmail = model.ContactEmail,
                ContactGender = ((PassengerType)model.ContactPassengerType).ToString(),
                ContactPhone = model.ContactPhone,
                ContactName = model.ContactName,
                ContactStatusId = (int)ContactStatus.ChuaLienLac,
                ContactRequestMore = model.CustomerNote,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                BookingInfoFlightToId = bookingFlight.Id,
                PaymentMethodId = (short)model.PaymentMethodId,
                CurrencyCode = selectedTicket.Ticket.Currency,
                CurrencyRate = 1,
            };
            if (model.ContactBirthDate != null)
                booking.ContactBirthDate = DateTime.ParseExact(model.ContactBirthDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            //Update Total:
            #region
            bookingFlight.TotalPriceNet = bookingFlight.BookingPriceDetails.Where(x => x.CodeFee == "NET").Sum(x => x.TotalPrice);
            bookingFlight.TotalFee = bookingFlight.BookingPriceDetails.Where(x => x.CodeFee != "NET" && x.CodeFee != CODE_DIS_SYSTEM).Sum(x => x.TotalPrice);
            bookingFlight.DiscountAmount = bookingFlight.BookingPriceDetails.Where(x => x.CodeFee == CODE_DIS_SYSTEM).Sum(x => x.TotalPrice);
            bookingFlight.TotalPrice = bookingFlight.TotalPriceNet;
            bookingFlight.TotalBaggageFee = bookingFlight.BookingBaggages.Where(x => !x.IsFree).Sum(x=>x.BaggageFee);
            if(bookingFlightReturn != null)
            {
                bookingFlightReturn.TotalPriceNet = bookingFlightReturn.BookingPriceDetails.Where(x => x.CodeFee == "NET").Sum(x => x.TotalPrice);
                bookingFlightReturn.TotalFee = bookingFlightReturn.BookingPriceDetails.Where(x => x.CodeFee != "NET" && x.CodeFee != CODE_DIS_SYSTEM).Sum(x => x.TotalPrice);
                bookingFlightReturn.DiscountAmount = bookingFlightReturn.BookingPriceDetails.Where(x => x.CodeFee == CODE_DIS_SYSTEM).Sum(x => x.TotalPrice);
                bookingFlightReturn.TotalPrice = bookingFlightReturn.TotalPriceNet;
                bookingFlightReturn.TotalBaggageFee = bookingFlightReturn.BookingBaggages.Where(x => !x.IsFree).Sum(x => x.BaggageFee);
            }


            //tong gia of booking
            booking.TotalAmount += bookingFlight.TotalPrice;
            //tong phi+ thue
            booking.TotalFeeAmount += bookingFlight.TotalFee;
            //tong giam gia
            booking.TotalDiscountAmount += bookingFlight.DiscountAmount;
            //tong phi hanh ly
            booking.TotalBaggageFeeAmount += bookingFlight.TotalBaggageFee;

            if (bookingFlightReturn != null)
            {
                booking.BookingInfoFlightReturnId = bookingFlightReturn.Id;
                //tong gia
                booking.TotalAmount += bookingFlightReturn.TotalPrice;
                //tong phi+ thue
                booking.TotalFeeAmount += bookingFlightReturn.TotalFee;
                //tong giam gia
                booking.TotalDiscountAmount += bookingFlightReturn.DiscountAmount;
                //tong phi hanh ly
                booking.TotalBaggageFeeAmount += bookingFlightReturn.TotalBaggageFee;
            }
            #endregion
            _clickBayService.InsertBooking(booking);
            //update total fight
            _clickBayService.UpdateBookingInfoFlight(bookingFlight);
            if(bookingFlightReturn!=null)
            {
                _clickBayService.UpdateBookingInfoFlight(bookingFlightReturn);
            }


            //passenger
            #region
            model.BookingPassers.ForEach(x =>
            {
                var bp = new BookingPassenger
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    PassengerType = (short)x.PassserType,
                    BookingId = booking.Id
                };
                if (x.BirthDay != null)
                    bp.BirthDay = DateTime.ParseExact(x.BirthDay, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                _clickBayService.InsertBookingPassenger(bp);
            });
            #endregion
            //newsletter
            #region
            if (model.NewLetterAccept)
            {
                _NewsLetterSubscriptionService.InsertNewsLetterSubscription(new Core.Domain.Messages.NewsLetterSubscription
                {
                    Active = true,
                    CreatedOnUtc = DateTime.Now,
                    Email = model.ContactEmail,
                    NewsLetterSubscriptionGuid = new Guid()
                });
            }
            #endregion
            //send mail for customer & admin
            #region
            _workflowMessageService.SendCustomerBookingSuccessfullMessage(booking);
            #endregion

            Session[string.Format(SUCCESS_BOOKING_SESSION, _workContext.CurrentCustomer.Id)] = booking.Id;
            //update TicketId
            _clickBayService.GetTicketId(booking.Id);
            #endregion

            return new JsonResult
            {
                Data = new
                {
                    url = Url.Action("BookingSuccess", new { bookingId=booking.Id }),
                    error = "",
                }
            };
        }

        private TicketModel PrepareTicketModel(BookingInfoFlight infoFlight)
        {
            if (infoFlight == null)
                return null;

            var model = new TicketModel
            {
                Adult = infoFlight.Adult,
                Child = infoFlight.Child,
                Infant = infoFlight.Infant,
                AirlineId = infoFlight.AirlinesId,
                FromPlace = infoFlight.FromPlaceName,
                FromCode = infoFlight.FromPlaceCode,
                ToPlace = infoFlight.ToPlaceName,
                ToCode = infoFlight.ToPlaceCode,
                FlightDurationString = getFlightDuration((decimal)infoFlight.FlightDuration),
                BrandCode = infoFlight.Brand,
                FlightNumber = infoFlight.FlightNumber,
                DepartTime = infoFlight.DepartDateTime.Value,
                LandingTime = infoFlight.ArrivalDateTime.Value,
                TicketType = infoFlight.TicketType,
                Price = infoFlight.TotalPrice,
                AirlineName = _clickBayService.GetAirlineById(infoFlight.AirlinesId).AirlinesName,
                //Prices
                BookingFlightPriceModels = infoFlight.BookingPriceDetails.Select(x => new TicketModel.BookingFlightPriceModel
                {
                    Price = x.Price,
                    Code = x.CodeFee,
                    Description = x.Description,
                    Quantity = x.Quantity,
                    Total = x.TotalPrice,
                    PassengerType = x.PassengerType
                }).ToList(),
                //conditions
                AirlinesConditions = infoFlight.BookingInfoConditions.Select(x => new TicketModel.AirlinesConditionModel
                {
                    ConditionDescription = x.ConditionDescription,
                    ConditionName = x.ConditionType
                }).ToList(),

                //baggages
                ArilinesBaggageConditions = infoFlight.BookingBaggages.Select(x => new TicketModel.ArilinesBaggageCondition
                {
                    Baggage = x.Baggage,
                    BaggageFee = x.BaggageFee,
                    Description = x.Description,
                    IsHandLuggage = x.IsHandLuggage,
                    IsFree = x.IsFree
                }).ToList()

            };
            var cityFrom = _clickBayService.GetcityByCode(infoFlight.FromPlaceCode);
            model.FromCountry = GetCountryName(cityFrom.CountryId);
            var cityTo = _clickBayService.GetcityByCode(infoFlight.ToPlaceCode);
            model.ToCountry = GetCountryName(cityTo.CountryId);


            return model;
        }

        public ActionResult BookingSuccess(int bookingId)
        {
            //var bookingId = Session[string.Format(SUCCESS_BOOKING_SESSION, _workContext.CurrentCustomer.Id)];

            //if (bookingId == null)
            //    return RedirectToAction("HomePage");

            var booking = _clickBayService.GetBookingById((int)bookingId);
            if (booking == null)
                throw new NopException("Booking is null id (" + bookingId + ")");

            Session.Remove(string.Format(SUCCESS_BOOKING_SESSION, _workContext.CurrentCustomer.Id));
            Session.Remove(string.Format(SELECTED_TICKET_SESSION, _workContext.CurrentCustomer.Id));

            var model = new BookingModel
            {
                Adult = booking.Adult,
                Child = booking.Child,
                Infant = booking.Infant,
                ContactAddress = booking.ContactAddress,
                ContactBirthDate = booking.ContactBirthDate.HasValue ? booking.ContactBirthDate.Value.ToString("dd/MM/yyyy") : null,
                ContactCityName = booking.ContactCityName,
                ContactCountryId = booking.ContactCountryId,
                ContactName = booking.ContactName,
                ContactEmail = booking.ContactEmail,
                ContactGender = booking.ContactGender,
                ContactPassengerType = booking.ContactPassengerType,
                ContactPassengerName = ((PasserType)booking.ContactPassengerType).GetLocalizedEnum(_localizationService, _workContext),
                ContactPhone = booking.ContactPhone,
                CustomerNote = booking.ContactRequestMore,
                TicketId = booking.TicketId,
            };
            //counttry
            model.ContactCountryName = GetCountryName(model.ContactCountryId);

            var listPrice = new List<TicketModel.TotalPriceShow>();

            model.TicketInfo = PrepareTicketModel(booking.BookingInfoFlight);
            listPrice.AddRange(GetTotalPriceShow(model.TicketInfo));
            
            if (booking.BookingInfoFlightReturnId.HasValue && booking.BookingInfoFlightReturnId.HasValue && booking.BookingInfoFlightReturnId.Value != 0)
            {
                model.TicketInfoReturn = PrepareTicketModel(booking.BookingInfoFlightReturn);
                listPrice.AddRange(GetTotalPriceShow(model.TicketInfoReturn));
            }
            //total price
            #region
            model.TotalPriceShows = new List<TicketModel.TotalPriceShow>();
            var l1 = (from pr in listPrice
                      group pr by new { pr.PassengerType, pr.Quantity } into gr
                      select new
                      {
                          gr = gr.Key,
                          Price = gr.Sum(x => x.Price),
                          TaxAndFee = gr.Sum(x => x.TaxAndFee),
                          DiscountAmount = gr.Sum(x => x.DiscountAmount)
                      }).ToList();
            foreach (var i in l1)
            {
                model.TotalPriceShows.Add(new TicketModel.TotalPriceShow { PassengerType = i.gr.PassengerType, Quantity = i.gr.Quantity, Price = i.Price, TaxAndFee = i.TaxAndFee, DiscountAmount = i.DiscountAmount });
            }          
            #endregion

            //passenger
            model.BookingPassers = booking.BookingPassengers.Select(x => new BookingPasserModel
            {
                PassserType = x.PassengerType,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                Title = x.Title,
                Gender = x.Gender,
                BirthDay = x.BirthDay.HasValue ? x.BirthDay.Value.ToString("dd/MM/yyyy") : "",
            }).ToList();

            return View(model);

        }

        public ActionResult SelectedTicket(int index, string session,
            short adult, short child, short iflant, int indexReturn = 0, string sessionReturn = null)
        {
            var ticketSelectedModel = new TicketSessionModel();
            var ticket = GetSelectedTicket(index, session);
            if (ticket == null)
                return Json("NOTOK");
            ticket.Adult = adult;
            ticket.Child = child;
            ticket.Infant = iflant;
            ticketSelectedModel.Ticket = ticket;
            if (indexReturn > 0)
            {
                var ticketReturn = GetSelectedTicket(indexReturn, sessionReturn);
                if (ticketReturn == null)
                    return Json("NOTOK");
                ticketReturn.Adult = adult;
                ticketReturn.Child = child;
                ticketReturn.Infant = iflant;

                ticketSelectedModel.TicketReturn = ticketReturn;
            }
            Session[string.Format(SELECTED_TICKET_SESSION, _workContext.CurrentCustomer.Id)] = ticketSelectedModel;
            return Json("OK");
        }
        private TicketModel GetSelectedTicket(int index, string session)
        {
            if (Session[session] == null)
                return null;
            var ticket = (Session[session] as List<TicketModel>).FirstOrDefault(x => x.Index == index);
            return ticket;
        }


        #endregion

        #region Until
        private string getFlightDuration(decimal number)
        {
            int h = (int)(number / 60);
            if (h == 0)
                return string.Format("{0} phút", Convert.ToInt32(number));
            decimal m = number % 60;
            return string.Format("{0} giờ {1} phút", h, m);
        }
        private IEnumerable<TicketModel> PrepairingTicketModel(IEnumerable<Ticket> data, bool preparingPrice = false, int adult = 0, int child = 0, int infant = 0)
        {
            if (data == null)
                return new List<TicketModel>();
            int i = 1;


            return data.Select(x =>
            {
                var item = new TicketModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    Adult = (short)adult,
                    Child = (short)child,
                    Infant = (short)infant,
                    FlightNumber = x.FlightNumber,
                    DepartTime = x.DepartTime,
                    LandingTime = x.LandingTime,
                    FromAirport = x.FromAirport,
                    ToAirport = x.ToAirport,
                    //  FromAirportCode = x.FromAirportId.ToString(),
                    //ToAirportCode = x.ToAirportId.ToString(),
                    FromPlace = x.FromPlace,
                    FromId = x.FromPlaceId,
                    ToId = x.ToPlaceId,
                    ToPlace = x.ToPlace,
                    BrandCode = x.AirlineCode,
                    AirlineName = x.Airline,
                    FareBasis = x.FareBasis,
                    AirlineCode = x.AirlineCode,
                    Index = i,
                    Stops = x.Stops,
                    FromCode = _clickBayService.GetCityById(x.FromPlaceId).Code,
                    ToCode = _clickBayService.GetCityById(x.ToPlaceId).Code,
                    FlightDuration = x.FlightDurationTime.TotalMinutes,
                    TicketType = x.TicketType,
                };
                item.FlightDurationString = getFlightDuration((decimal)item.FlightDuration);
                i++;
                if (preparingPrice)
                {
                    item.BookingFlightPriceModels = x.TicketPriceDetailDto.Select(pr => new TicketModel.BookingFlightPriceModel
                    {
                        Code = pr.Code,
                        Price = pr.Price,
                        Quantity = pr.Quantity,
                        Description = pr.Description,
                        PassengerType = pr.PassengerType,
                        Total = pr.Total
                    }).ToList();
                }
                try
                {

                    var fromCity = _clickBayService.GetCityById(x.FromPlaceId);
                    item.FromAirportCode = fromCity.Code;
                    item.FromCountry = fromCity.Country.Name;
                    var toCity = _clickBayService.GetCityById(x.ToPlaceId);
                    item.ToAirportCode = toCity.Code;
                    item.ToCountry = toCity.Country.Name;

                }
                catch { }
                item.AirlineId = 1;
                if (item.AirlineCode == "JetStar")
                    item.AirlineId = 2;


                if (item.AirlineCode == "VietJetAir")
                    item.AirlineId = 3;

                return item;
            }

                );

        }

        #endregion

        //booking moi dat o HomePage
        public ActionResult BookingRecent(int? numberBooking)
        {
            if (!numberBooking.HasValue || numberBooking.Value == 0)
                numberBooking = 5;
            var model = new List<BookingRecentModel>();
            var listBooking = _clickBayService.GetAllBooking(fromDate: null, toDate: null, bookingStatusId: null, paymentStatusId: null, contactStatusId: null, pageIndex: 1, pageSize: numberBooking.Value);
            foreach (var bo in listBooking)
            {
                var bookingRecent = new BookingRecentModel()
                {
                    Id = bo.Id,
                    CreateDate = bo.CreatedOn,
                    CreateDateString = PlanX.Web.Framework.Extensions.RelativeFormatVietNam(bo.CreatedOn, ""),
                    PriceNet = bo.TotalAmount,
                    TotalPrice = bo.TotalAmount,
                    CurrencyCode = bo.CurrencyCode,
                    Adult = bo.Adult,
                    Child = bo.Child,
                    Infant = bo.Infant
                };
                var infoFlightTo = bo.BookingInfoFlight;
                if (infoFlightTo != null)
                {
                    bookingRecent.FromPlace = infoFlightTo.FromPlaceName;
                    bookingRecent.FromPlaceCode = infoFlightTo.FromPlaceCode;
                    bookingRecent.ToPlace = infoFlightTo.ToPlaceName;
                    bookingRecent.ToPlaceCode = infoFlightTo.ToPlaceCode;
                    bookingRecent.BrandName = infoFlightTo.Brand;
                }
                if (bo.RoundTrip && bo.BookingInfoFlightReturnId.HasValue && bo.BookingInfoFlightReturnId.Value != 0)
                {
                    var infoFlightRe = bo.BookingInfoFlightReturn;
                    if (infoFlightRe != null)
                    {
                        bookingRecent.FromPlace = infoFlightRe.FromPlaceName;
                        bookingRecent.FromPlaceCode = infoFlightRe.FromPlaceCode;
                        bookingRecent.ToPlace = infoFlightRe.ToPlaceName;
                        bookingRecent.ToPlaceCode = infoFlightRe.ToPlaceCode;
                        bookingRecent.BrandName = infoFlightRe.Brand;
                    }
                }
                model.Add(bookingRecent);
            }
            return View(model);
        }
        #endregion


        public ActionResult JsonResult(SearchModel model, string u, string p)
        {
            if (u != "vnsai123" || p != "zxcasd")
                return RedirectToRoute("HomePage");

            model.Tickets = new List<TicketModel>();
            DateTime datePart = DateTime.ParseExact(model.DepartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            model.SearchDate = datePart;
            DateTime? returnDate = null;
            if (model.ReturnDate != null)
                returnDate = DateTime.ParseExact(model.ReturnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var result = _clickBayService.SearchTicketJson(
                model.FromId, model.ToId,
                datePart, model.Adult,
                model.Child, model.Flant,
                returnDate: null,
                //source: Source.Count > 0 ? model.Source.Aggregate((a, b) => a + "," + b) : null,
                expendDetails: true,
                expendTicketPriceDetails: false,
                priceSummaries: true,
                roundTrip: false
                );

            return Content(result);
        }
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
        public ActionResult Map()
        {
            return View();
        }

    }

}