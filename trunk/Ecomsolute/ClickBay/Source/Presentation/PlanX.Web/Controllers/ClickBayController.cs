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
        #region
        private const string CACHE_SEARCH_MODEL = "CACHE_SEARCH_MODEL";
        private const string SELECTED_TICKET_SESSION = "BOOKING_{0}";
        private const string SUCCESS_BOOKING_SESSION = "SUCCESS_{0}";
        private const string SESSION_SEARCH_NAME = "{0}-{1}-{2}-{3}-{4}-{5}-{6}";
        private const string VIETNAM_CITY_CACHE = "VietNam_City_Cache";
        private const string COUNTRIES_CACHE = "COUNTRIES_CACHE";
        #endregion
        #region Fields
        private readonly IClickBayService _clickBayService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly INewsLetterSubscriptionService _NewsLetterSubscriptionService;

        #endregion

        #region Ctor
        public ClickBayController(IClickBayService clickBayService, IWorkContext workContext,
        ILocalizationService localizationService
            , ICacheManager cacheManager,
            INewsLetterSubscriptionService NewsLetterSubscriptionService
            )
        {
            this._NewsLetterSubscriptionService = NewsLetterSubscriptionService;
            this._clickBayService = clickBayService;
            _workContext = workContext;
            _cacheManager = cacheManager;
            _localizationService = localizationService;
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


            return View(model);

        }

        public ActionResult TicketSearch(SearchModel model)
        {
            DateTime datePart = DateTime.ParseExact(model.DepartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            model.SearchDate = datePart;
            DateTime? returnDate = null;
            if (model.ReturnDate != null)
                returnDate = DateTime.ParseExact(model.ReturnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            model.Tickets = new List<TicketModel>();
            var sources = new List<string>();
            if(!string.IsNullOrWhiteSpace(model.Source))
            {
                sources = model.Source.Split(',').ToList();
            }
            if (model.SessionId != null)
                model.Tickets = Session[model.SessionId] as List<TicketModel>;

            if (model.Tickets != null && model.Tickets.Count > 0)
            {
                if (sources.Count > 0)
                    model.Tickets = model.Tickets.Where(x => sources.Contains(x.AirlineName)).ToList();
            }
            else
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
                var ticket = PrepairingTicketModel(result, true);

                DateTime dt = DateTime.Now;
                model.SessionId = string.Format(SESSION_SEARCH_NAME, model.FromId,
                    model.ToId, datePart.ToString("ddMMyyyy"),
                    returnDate.HasValue ? returnDate.Value.ToString("ddMMyyyy") : "",
                    model.Adult, model.Child, model.Flant);

                Session[model.SessionId] = model.Tickets = ticket.ToList();

            }
            if (model.Sort == (int)Sort.Price)
            {
                model.Tickets = model.Tickets.OrderBy(x => x.Price).ToList();
            }
            else
                if (model.Sort == (int)Sort.Date)
                {
                    model.Tickets = model.Tickets.OrderByDescending(x => x.DepartTime).ToList();
                }
                else
                {
                    model.Tickets = model.Tickets.OrderByDescending(x => x.AirlineName).ToList();
                }
            return PartialView("_SearchTicketPartial", model);
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

            //bo sung: get Price & Condition
            return PartialView("_TicketDetail", ticket);
        }
        #endregion

        private TicketModel PrepareSelectedTicket(TicketModel selectedTicket)
        {
            if (selectedTicket == null)
                return null;

            var ticket = selectedTicket;
          
            //ticket.AirlinesConditions = _clickBayService.GetListAirlinesConditionByAirlineId(ticket.AirlineId).Select(x => new TicketModel.AirlinesConditionModel
            //{
            //    ConditionDescription = x.ConditionDescription,
            //    ConditionName = x.ConditionName
            //}).ToList();
            
            //ticket.ArilinesBaggageConditions = _clickBayService.GetListArilinesBaggageCondition(ticket.AirlineId).Select(x => new TicketModel.ArilinesBaggageCondition
            //{
            //    Baggage = x.Baggage,
            //    BaggageFee = x.BaggageFee
            //}).ToList();
            //phi cong them cua he thong - tinh theo moi luot di://chua chuyen sang setting
            decimal PricePlus = 0;
            if (decimal.TryParse(_localizationService.GetResource("booking.price.clickbay.amount"), out PricePlus))
            {
                if (PricePlus > 0)
                {
                    var fee = ticket.BookingFlightPriceModels.Where(x => x.Code == "CLICKBAY").FirstOrDefault();
                    if (fee != null)
                    {
                        fee.Price = PricePlus;
                        fee.Total = PricePlus;
                    }
                    else
                    {
                        ticket.BookingFlightPriceModels.Add(new TicketModel.BookingFlightPriceModel()
                        {
                            Code = "CLICKBAY",
                            Description = _localizationService.GetResource("booking.price.clickbay.description"),
                            Price = PricePlus,
                            Quantity = 1,
                            Total = PricePlus,
                            PassengerType = PassengerType.ADT.ToString()
                        });
                    }
                }
            }
            //giam gia cua he thong - tin theo moi luot di://chua chuyen sang setting
            decimal DiscountAmount = 0;
            if (decimal.TryParse(_localizationService.GetResource("booking.price.discount.amount"), out DiscountAmount))
            {
                if (DiscountAmount > 0)
                {
                    var discount = ticket.BookingFlightPriceModels.Where(x => x.Code == "DIS").FirstOrDefault();
                    if (discount != null)
                    {
                        discount.Price = DiscountAmount;
                        discount.Total = DiscountAmount;
                    }
                    else
                    {
                        ticket.BookingFlightPriceModels.Add(new TicketModel.BookingFlightPriceModel()
                        {
                            Code = "DIS",
                            Description = _localizationService.GetResource("booking.price.discount.description"),
                            Price = DiscountAmount,
                            Quantity = 1,
                            Total = DiscountAmount,
                            PassengerType = PassengerType.ADT.ToString()
                        });
                    }
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

            var listPrice = new List<TicketModel.BookingFlightPriceModel>();

            #region Ticket
            model.TicketInfo = PrepareSelectedTicket(selectedTicket.Ticket);
            listPrice.AddRange(selectedTicket.Ticket.BookingFlightPriceModels);
            #endregion

            #region ticket return
            if (selectedTicket.TicketReturn != null)
            {
                model.TicketInfoReturn = PrepareSelectedTicket(selectedTicket.TicketReturn);
                listPrice.InsertRange(0,selectedTicket.TicketReturn.BookingFlightPriceModels);
            }
            #endregion

            #region totalPrice

            model.TotalPriceForPassengerTypes = new List<BookingModel.TotalPriceForPassengerType>();
            //net price
            var l1 = (from pr in listPrice
                      where pr.Code == "NET"
                      group pr by new { pr.PassengerType, pr.Quantity } into gr
                      select new
                      {
                          gr = gr.Key,
                          Total = gr.Sum(x=> x.Total)
                      }).ToList();
             foreach(var i in l1)
             {
                 model.TotalPriceForPassengerTypes.Add(new BookingModel.TotalPriceForPassengerType() { Code = "NET", PassengerType = i.gr.PassengerType, Quantity = i.gr.Quantity, Price = i.Total });
             }
            //fee
             var l2 = (from pr in listPrice
                       where pr.Code != "NET" && pr.Code != "DIS"
                       group pr by new { pr.PassengerType, pr.Quantity } into gr
                       select new
                       {
                           gr = gr.Key,
                           Total = gr.Sum(x => x.Total)
                       }).ToList();
             foreach (var i in l2)
             {
                 model.TotalPriceForPassengerTypes.Add(new BookingModel.TotalPriceForPassengerType() { Code = "FEE", PassengerType = i.gr.PassengerType, Quantity = i.gr.Quantity, Price = i.Total });
             }
            //discount
             model.DiscountAmount = listPrice.Where(x => x.Code == "DIS").Sum(x => x.Total);

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

            //bookingInfo.TotalBaggageFee = selectedTicket.ArilinesBaggageConditions.Sum(x => x.BaggageFee);
            bookingInfo.TotalPriceNet = selectedTicket.BookingFlightPriceModels.Where(x => x.Code == "NET").Sum(x => x.Total);
            bookingInfo.TotalFee = selectedTicket.BookingFlightPriceModels.Where(x => x.Code != "NET" && x.Code!="DIS").Sum(x => x.Total);
            bookingInfo.DiscountAmount = selectedTicket.BookingFlightPriceModels.Where(x => x.Code == "DIS").Sum(x => x.Total);
            bookingInfo.TotalPrice = bookingInfo.TotalPriceNet;
            _clickBayService.InsertBookingInfoFlight(bookingInfo);
            
            //price
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

            //baggages: gom: free + user chon hanh ly mang ky gui them;
            selectedTicket.ArilinesBaggageConditions.Where(x=> x.IsFree).ToList().ForEach(x =>
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
            if(isFlightTo)
            {
                if (!string.IsNullOrEmpty(bookingModel.TicketInfoBaggages))
                {
                    baggageItems = bookingModel.TicketInfoBaggages.Split(',').Where(x=> !string.IsNullOrWhiteSpace(x)).Select(x=> int.TryParse(x,out j)?j:0).ToList();
                }
            }
            else {
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
                                 Description = string.Format(item.Description, item.Baggage, item.BaggageFee),
                                 IsFree = item.IsFree,
                                 IsHandLuggage = item.IsHandLuggage,
                                 BookingInfoFlightId = bookingInfo.Id
                             };
                        _clickBayService.InsertBookingBaggage(entityBag);
                    }
                }
            }

            //conditions
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
            var bookingModel = new Booking
            {
                Adult = selectedTicket.Ticket.Adult,
                Child = selectedTicket.Ticket.Child,
                Infant = selectedTicket.Ticket.Infant,
                BookingStatusId = (int)BookingStatus.ChuaXyLy,
                PaymentStatusId = (int)PaymentStatus.ChuaThanhToan,
                IsInvoid = model.IsInvoid,
                CustomerId = 0,
                RoundTrip = bookingFlightReturn != null,
                // TicketId = selectedTicket.Ticket.Id,
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
                bookingModel.ContactBirthDate = DateTime.ParseExact(model.ContactBirthDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);

            //Amount:
            //tong gia
            bookingModel.TotalAmount += bookingFlight.TotalPrice;
            //tong phi+ thue
            bookingModel.TotalFeeAmount += bookingFlight.TotalFee;
            //tong giam gia
            bookingModel.TotalDiscountAmount += bookingFlight.DiscountAmount;
            //tong phi hanh ly
            bookingModel.TotalBaggageFeeAmount += bookingFlight.TotalBaggageFee;

            if (bookingFlightReturn != null)
            {
                bookingModel.BookingInfoFlightReturnId = bookingFlightReturn.Id;
                //tong gia
                bookingModel.TotalAmount += bookingFlightReturn.TotalPrice;
                //tong phi+ thue
                bookingModel.TotalFeeAmount += bookingFlightReturn.TotalFee;
                //tong giam gia
                bookingModel.TotalDiscountAmount += bookingFlightReturn.DiscountAmount;
                //tong phi hanh ly
                bookingModel.TotalBaggageFeeAmount += bookingFlightReturn.TotalBaggageFee;
            }
            _clickBayService.InsertBooking(bookingModel);

            //passenger
            model.BookingPassers.ForEach(x =>
            {
                var bp = new BookingPassenger
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    PassengerType = (short)x.PassserType,
                    BookingId = bookingModel.Id
                };
                if (x.BirthDay != null)
                    bp.BirthDay = DateTime.ParseExact(x.BirthDay, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                _clickBayService.InsertBookingPassenger(bp);
            });

            //newsletter
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


            Session[string.Format(SUCCESS_BOOKING_SESSION, _workContext.CurrentCustomer.Id)] = bookingModel.Id;
            #endregion

            return new JsonResult
            {
                Data = new
                {
                    url = Url.Action("BookingSuccess"),
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

        public ActionResult BookingSuccess()
        {
            var bookingId = Session[string.Format(SUCCESS_BOOKING_SESSION, _workContext.CurrentCustomer.Id)];
            
            if (bookingId == null)
                return RedirectToAction("HomePage");

            var booking = _clickBayService.GetBookingById((int)bookingId);
            if (booking == null)
                throw new NopException("Booking is null id (" + bookingId + ")");

            Session.Remove(string.Format(SUCCESS_BOOKING_SESSION, _workContext.CurrentCustomer.Id));
            Session.Remove(string.Format(SELECTED_TICKET_SESSION,_workContext.CurrentCustomer.Id));

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
                CustomerNote = booking.ContactRequestMore
            };
            //counttry
            model.ContactCountryName = GetCountryName(model.ContactCountryId);
            
            var listPrice = new List<TicketModel.BookingFlightPriceModel>();       
            
            model.TicketInfo = PrepareTicketModel(booking.BookingInfoFlight);
            listPrice.AddRange(model.TicketInfo.BookingFlightPriceModels);
            if (booking.BookingInfoFlightReturnId.HasValue || booking.BookingInfoFlightReturnId != 0)
            {
                model.TicketInfoReturn = PrepareTicketModel(booking.BookingInfoFlightReturn);
                listPrice.AddRange(model.TicketInfo.BookingFlightPriceModels);
            }
            //total price
            #region
            model.TotalPriceForPassengerTypes = new List<BookingModel.TotalPriceForPassengerType>();
            //net price
            var l1 = (from pr in listPrice
                      where pr.Code == "NET"
                      group pr by new { pr.PassengerType, pr.Quantity } into gr
                      select new
                      {
                          gr = gr.Key,
                          Total = gr.Sum(x => x.Total)
                      }).ToList();
            foreach (var i in l1)
            {
                model.TotalPriceForPassengerTypes.Add(new BookingModel.TotalPriceForPassengerType() { Code = "NET", PassengerType = i.gr.PassengerType, Quantity = i.gr.Quantity, Price = i.Total });
            }
            //fee
            var l2 = (from pr in listPrice
                      where pr.Code != "NET" && pr.Code != "DIS"
                      group pr by new { pr.PassengerType, pr.Quantity } into gr
                      select new
                      {
                          gr = gr.Key,
                          Total = gr.Sum(x => x.Total)
                      }).ToList();
            foreach (var i in l2)
            {
                model.TotalPriceForPassengerTypes.Add(new BookingModel.TotalPriceForPassengerType() { Code = "FEE", PassengerType = i.gr.PassengerType, Quantity = i.gr.Quantity, Price = i.Total });
            }
            //discount
            model.DiscountAmount = listPrice.Where(x => x.Code == "DIS").Sum(x => x.Total);
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
                BirthDay = x.BirthDay.HasValue? x.BirthDay.Value.ToString("dd/MM/yyyy") : "",
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
            //var airline = _clickBayService.GetAirlineById(ticket.AirlineId);
            //dieu kien ve: phu thuoc vao hang hang khong & loai ve
            ///where ticketType
            //ticket.AirlinesConditions = airline.AirlinesConditions.Where(x => !x.Deleted /*&& x.TicketType==ticket.TicketType*/).Select(x => new TicketModel.AirlinesConditionModel
            //{
            //    ConditionDescription = x.ConditionDescription,
            //    ConditionName = x.ConditionName
            //}).ToList();
            //baggage
            //ticket.ArilinesBaggageConditions = airline.ArilinesBaggageConditions.Where(x => !x.Deleted).Select(x => new TicketModel.ArilinesBaggageCondition
            //{
            //    Id = x.Id,
            //    Baggage = x.Baggage,
            //    BaggageFee = x.BaggageFee,
            //    IsFree = x.IsFree,
            //    IsHandLuggage = x.IsHandLuggage,
            //    DisplayOrder = x.DisplayOrder,
            //    Description = x.Description
            //}).ToList();
            return ticket;
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
        #region Until
        private string getFlightDuration(decimal number)
        {
            int h = (int)(number % 60);
            decimal m = number % 60;
            //decimal h = Math.Round(number, 0);
            //decimal m = Math.Round(number, 2) - h;
            return string.Format("{0} giờ {1} phút", h, m);
        }
        private IEnumerable<TicketModel> PrepairingTicketModel(IEnumerable<Ticket> data, bool preparingPrice = false)
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
                    source: model.Source.Count > 0 ? model.Source.Aggregate((a, b) => a + "," + b) : null,
                    expendDetails: true,
                    expendTicketPriceDetails: false,
                    priceSummaries: true,
                    roundTrip: false
                    );

            return Content(result);
        }

    }

}