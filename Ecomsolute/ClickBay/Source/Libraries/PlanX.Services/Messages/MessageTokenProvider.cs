using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using PlanX.Core;
//using PlanX.Core.Domain.Blogs;
//using PlanX.Core.Domain.Catalog;
using PlanX.Core.Domain.Customers;
//using PlanX.Core.Domain.Forums;
using PlanX.Core.Domain.Messages;
//using PlanX.Core.Domain.News;
//using PlanX.Core.Domain.Orders;
//using PlanX.Core.Domain.Shipping;
using PlanX.Core.Domain.Stores;
//using PlanX.Core.Domain.Tax;
using PlanX.Core.Html;
//using PlanX.Services.Catalog;
using PlanX.Services.Common;
using PlanX.Services.Customers;
using PlanX.Services.Directory;
using PlanX.Services.Events;
//using PlanX.Services.Forums;
using PlanX.Services.Helpers;
using PlanX.Services.Localization;
using PlanX.Services.Media;
//using PlanX.Services.Orders;
//using PlanX.Services.Payments;
using PlanX.Services.Seo;
using PlanX.Core.Domain.News;
using PlanX.Core.Domain.ClickBay;
using PlanX.Services.ClickBay;

namespace PlanX.Services.Messages
{
    public partial class MessageTokenProvider : IMessageTokenProvider
    {
        #region Fields

        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEmailAccountService _emailAccountService;
        //private readonly IPriceFormatter _priceFormatter;
        //private readonly ICurrencyService _currencyService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        //private readonly IDownloadService _downloadService;
        //private readonly IOrderService _orderService;
        //private readonly IPaymentService _paymentService;
        //private readonly IProductAttributeParser _productAttributeParser;

        private readonly MessageTemplatesSettings _templatesSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        //private readonly CatalogSettings _catalogSettings;
        //private readonly TaxSettings _taxSettings;

        private readonly IEventPublisher _eventPublisher;
        private readonly IClickBayService _clickBayService;

        #endregion

        #region Ctor

        public MessageTokenProvider(ILanguageService languageService,
            ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEmailAccountService emailAccountService,
            //IPriceFormatter priceFormatter, ICurrencyService currencyService,
            IWebHelper webHelper,
            IWorkContext workContext, //IDownloadService downloadService,
            //IOrderService orderService, IPaymentService paymentService,
            //IProductAttributeParser productAttributeParser,
            MessageTemplatesSettings templatesSettings,
            EmailAccountSettings emailAccountSettings,
            //CatalogSettings catalogSettings,
            //TaxSettings taxSettings, 
            IEventPublisher eventPublisher,
            IClickBayService clickBayService)
        {
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._emailAccountService = emailAccountService;
            //this._priceFormatter = priceFormatter;
            //this._currencyService = currencyService;
            this._webHelper = webHelper;
            this._workContext = workContext;
            //this._downloadService = downloadService;
            //this._orderService = orderService;
            //this._paymentService = paymentService;
            //this._productAttributeParser = productAttributeParser;

            this._templatesSettings = templatesSettings;
            this._emailAccountSettings = emailAccountSettings;
            //this._catalogSettings = catalogSettings;
            //this._taxSettings = taxSettings;
            this._eventPublisher = eventPublisher;
            this._clickBayService = clickBayService;
        }

        #endregion

        #region Utilities
        private string GetCountryName(string cityCode)
        {
            try
            {
                var city = _clickBayService.GetcityByCode(cityCode);
                var c = _clickBayService.GetCountryById(city.CountryId);
                if (c != null)
                    return c.Name;
                return "";
            }
            catch { return ""; }
        }
        private string GetFlightDuration(decimal number)
        {
            int h = (int)(number / 60);
            if (h == 0)
                return string.Format("{0} phút", Convert.ToInt32(number));
            decimal m = number % 60;
            return string.Format("{0} giờ {1} phút", h, m);
        }
        /// <summary>
        /// Convert a collection to a HTML table
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>HTML table of products</returns>

        protected virtual string BookingListPassengerToHtmlTable(List<BookingPassenger> passengers)
        {
            var result = "";

            var sb = new StringBuilder();
            sb.AppendLine("<table style=\"width:100%;border-collapse:collapse;\"><tbody>");

            #region Passenger
            //hearder
            sb.AppendLine("<tr style=\"text-decoration:underline\"><td style=\"padding:5px 0px 5px 10px;text-decoration:underline;\">Quý danh</td><td>Họ và tên</td><td>Ngày sinh</td></tr>");
            //details
            var table = passengers;
            foreach (var i in passengers)
            {
                sb.AppendLine(string.Format("<tr style=\"border-bottom:1px dashed #ccc;\"><td style=\"padding:5px 10px;\">{0}</td><td style=\"text-transform: uppercase;\">{1} {2} {3}</td><td>{4}</td></tr>", _localizationService.GetResource("passertype." + i.PasserType.ToString()), i.FirstName, i.MiddleName, i.LastName, i.BirthDay.HasValue ? i.BirthDay.Value.ToString("dd/MM/yyyy") : ""));
            }
            #endregion

            sb.AppendLine("</tbody></table>");
            result = sb.ToString();
            return result;
        }
        protected virtual string BookingListPriceToHtmlTable(Booking booking)
        {
            var result = "";
            decimal totalBaggage = 0;
            decimal totalPrice = 0;

            var listPrice = booking.BookingInfoFlight.BookingPriceDetails.ToList();
            totalBaggage = booking.BookingInfoFlight.BookingBaggages.Where(x => !x.IsFree).Sum(b => b.BaggageFee);

            if (booking.BookingInfoFlightReturn != null)
            {
                listPrice.AddRange(booking.BookingInfoFlightReturn.BookingPriceDetails.ToList());
                totalBaggage += booking.BookingInfoFlightReturn.BookingBaggages.Where(x => !x.IsFree).Sum(b => b.BaggageFee);
            }

            var sb = new StringBuilder();
            sb.AppendLine("<table style=\"width:100%;border-collapse:collapse;text-align:right;\"><tbody>");
            //hearder
            sb.AppendLine("<tr style=\"text-decoration:underline;\"><td style=\"padding:10px;text-align:left;\">Hành khách</td><td>Số lượng</td><td>Giá vé/1 KH</td><td>Thuế và phí/1 HK</td><td>Giảm giá/1 HK</td><td style=\"padding-right:10px;\">Tổng(VND)</td></tr>");

            if (booking.Adult > 0)
            {
                decimal[] arr = new decimal[4];
                var pr = listPrice.Where(x => x.PassengerType == PassengerType.ADT.ToString()).ToList();
                arr[0] = pr.Where(p => p.CodeFee == "NET").Sum(p => p.TotalPrice);
                arr[1] = pr.Where(p => p.CodeFee != "NET" && p.CodeFee != "DIS").Sum(p => p.TotalPrice);
                arr[2] = pr.Where(p => p.CodeFee == "DIS").Sum(p => p.TotalPrice);                
                arr[3] = (arr[0] + arr[1] - arr[2]) * booking.Adult;
                sb.AppendLine(string.Format("<tr style=\"border-bottom:1px dashed #ccc;\"><td style=\"padding:5px 10px;text-align:left;\">{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td style=\"padding-right:10px;\">{5}</td></tr>", _localizationService.GetResource("passertype." + PassengerType.ADT.ToString(), _workContext.WorkingLanguage.Id), booking.Adult.ToString("#,0"), arr[0].ToString("#,0"), arr[1].ToString("#,0"), arr[2].ToString("#,0"), arr[3].ToString("#,0")));
                totalPrice += arr[3];
            }
            if (booking.Child > 0)
            {
                decimal[] arr = new decimal[4];
                var pr = listPrice.Where(x => x.PassengerType == PassengerType.CHD.ToString()).ToList();
                arr[0] = pr.Where(p => p.CodeFee == "NET").Sum(p => p.TotalPrice);
                arr[1] = pr.Where(p => p.CodeFee != "NET" && p.CodeFee != "DIS").Sum(p => p.TotalPrice);
                arr[2] = pr.Where(p => p.CodeFee == "DIS").Sum(p => p.TotalPrice);                
                arr[3] = (arr[0] + arr[1] - arr[2]) * booking.Child;
                sb.AppendLine(string.Format("<tr style=\"border-bottom:1px dashed #ccc;\"><td style=\"padding:5px 10px;text-align:left;\">{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td style=\"padding-right:10px;\">{5}</td></tr>", _localizationService.GetResource("passertype." + PassengerType.CHD.ToString(), _workContext.WorkingLanguage.Id), booking.Adult.ToString("#,0"), arr[0].ToString("#,0"), arr[1].ToString("#,0"), arr[2].ToString("#,0"), arr[3].ToString("#,0")));
                totalPrice += arr[3];
            }
            if (booking.Infant > 0)
            {
                decimal[] arr = new decimal[4];
                var pr = listPrice.Where(x => x.PassengerType == PassengerType.INF.ToString()).ToList();
                arr[0] = pr.Where(p => p.CodeFee == "NET").Sum(p => p.TotalPrice);
                arr[1] = pr.Where(p => p.CodeFee != "NET" && p.CodeFee != "DIS").Sum(p => p.TotalPrice);
                arr[2] = pr.Where(p => p.CodeFee == "DIS").Sum(p => p.TotalPrice);                
                arr[3] = (arr[0] + arr[1] - arr[2]) * booking.Infant;
                sb.AppendLine(string.Format("<tr style=\"border-bottom:1px dashed #ccc;\"><td style=\"padding:5px 10px;text-align:left;\">{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td style=\"padding-right:10px;\">{5}</td></tr>", _localizationService.GetResource("passertype." + PassengerType.INF.ToString(), _workContext.WorkingLanguage.Id), booking.Adult.ToString("#,0"), arr[0].ToString("#,0"), arr[1].ToString("#,0"), arr[2].ToString("#,0"), arr[3].ToString("#,0")));
                totalPrice += arr[3];
            }
            sb.AppendLine(string.Format("<tr><td colspan=\"5\" style=\"padding:5px 10px;\">Tổng cộng:</td><td style=\"font-size:14px;font-weight:600;padding-right:10px;\">{0}</td></tr>", totalPrice.ToString("#,0")));
            //Total Baggage
            sb.AppendLine(string.Format("<tr><td colspan=\"5\" style=\"padding:5px 10px;\">Tổng phí hành lý:</td><td style=\"font-size:14px;font-weight:600;padding-right:10px;\">{0}</td></tr>", totalBaggage.ToString("#,0")));
            //Total final
            sb.AppendLine(string.Format("<tr><td colspan=\"5\" style=\"padding:5px 10px;\">Tổng cộng:</td><td style=\"font-size:14px;font-weight:600;padding-right:10px;\">{0}</td></tr>", (totalPrice + totalBaggage).ToString("#,0")));

            sb.AppendLine("</tbody></table>");
            result = sb.ToString();
            return result;
        }
        protected virtual string BookingListConditionToHtmlTable(Booking booking)
        {
            var result = "";

            var sb = new StringBuilder();
            sb.AppendLine("<table style=\"width:100%;border-collapse:collapse;\"><tbody>");

            #region Conditions
            if (booking.BookingInfoFlightReturn != null && booking.RoundTrip)
            {
                //neu khac Airline hoac TicketType
                if (booking.BookingInfoFlight.AirlinesId != booking.BookingInfoFlightReturn.AirlinesId || booking.BookingInfoFlight.TicketType != booking.BookingInfoFlightReturn.TicketType)
                {
                    sb.AppendLine("<tr><td colspan=\"2\" style=\"padding:5px 10px;font-weight:600;\">Điều kiện chiều đi</td></tr>");
                    foreach (var con in booking.BookingInfoFlight.BookingInfoConditions)
                    {
                        sb.AppendLine(string.Format("<tr><td style=\"padding:5px 20px;width:140px\">{0}</td><td style=\"padding:5px 0px;border-bottom:1px dashed #ccc;\">{1}</td></tr>", con.ConditionType, con.ConditionDescription));
                    }
                    sb.AppendLine("<tr><td colspan=\"2\" style=\"padding:5px 10px;font-weight:600;\">Điều kiện chiều về</td></tr>");
                    foreach (var con in booking.BookingInfoFlightReturn.BookingInfoConditions)
                    {
                        sb.AppendLine(string.Format("<tr><td style=\"padding:5px 20px;width:140px\">{0}</td><td style=\"padding:5px 0px;border-bottom:1px dashed #ccc;\">{1}</td></tr>", con.ConditionType, con.ConditionDescription));
                    }
                }
                else
                {
                    foreach (var con in booking.BookingInfoFlight.BookingInfoConditions)
                    {
                        sb.AppendLine(string.Format("<tr><td style=\"padding:5px 20px;width:140px\">{0}</td><td style=\"padding:5px 0px;border-bottom:1px dashed #ccc;\">{1}</td></tr>", con.ConditionType, con.ConditionDescription));
                    }
                }
            }
            else
            {
                foreach (var con in booking.BookingInfoFlight.BookingInfoConditions)
                {
                    sb.AppendLine(string.Format("<tr><td style=\"padding:5px 20px;width:140px\">{0}</td><td style=\"padding:5px 0px;border-bottom:1px dashed #ccc;\">{1}</td></tr>", con.ConditionType, con.ConditionDescription));
                }
            }

            #endregion

            sb.AppendLine("</tbody></table>");
            result = sb.ToString();
            return result;
        }
        protected virtual string BookingListBaggageToHtmlTable(Booking booking)
        {
            var result = "";

            var sb = new StringBuilder();
            sb.AppendLine("<table style=\"width:100%;border-collapse:collapse;\"><tbody>");

            #region Conditions
            int hasHandLuggage = 1;
            if (booking.BookingInfoFlightReturn != null && booking.RoundTrip)
            {
                sb.AppendLine("<tr><td colspan=\"2\" style=\"padding:5px 10px;font-weight:600;\">Hành lý chiều đi</td></tr>");
                foreach (var con in booking.BookingInfoFlight.BookingBaggages.Where(x => x.IsHandLuggage && x.IsFree))
                {

                    sb.AppendLine(string.Format("<tr><td style=\"padding:5px 10px\">Hành lý xách tay</td><td>{0}</td></tr>", con.Description));
                }
                sb.AppendLine("<tr><td style=\"padding:5px 10px\">Hành lý ký gửi</td><td>");
                foreach (var con in booking.BookingInfoFlight.BookingBaggages.Where(x => !x.IsHandLuggage).OrderByDescending(x=> x.IsFree))
                {
                    if (!con.IsFree)
                    {
                        sb.AppendLine(string.Format("<p>Hành khách {0}: {1} kg</p>", hasHandLuggage, con.Baggage));
                        hasHandLuggage++;
                    }
                    else sb.AppendLine(string.Format("<p>{0}</p>", con.Description));
                    
                }
                sb.AppendLine("</td></tr>");
                //return
                hasHandLuggage = 1;
                sb.AppendLine("<tr><td colspan=\"2\" style=\"padding:5px 10px;font-weight:600;\">Hành lý chiều về</td></tr>");
                foreach (var con in booking.BookingInfoFlightReturn.BookingBaggages.Where(x => x.IsHandLuggage && x.IsFree))
                {
                    sb.AppendLine(string.Format("<tr><td style=\"padding:5px 10px\">Hành lý xách tay</td><td>{0}</td></tr>", con.Description));
                }
                sb.AppendLine("<tr><td style=\"padding:5px 10px\">Hành lý ký gửi</td><td>");
                foreach (var con in booking.BookingInfoFlightReturn.BookingBaggages.Where(x => !x.IsHandLuggage).OrderByDescending(x=> x.IsFree))
                {
                    if (!con.IsFree)
                    {
                        sb.AppendLine(string.Format("<p>Hành khách {0}: {1} kg</p>", hasHandLuggage, con.Baggage));
                        hasHandLuggage++;
                    }
                    else sb.AppendLine(string.Format("<p>{0}</p>", con.Description));
                }
                sb.AppendLine("</td></tr>");
            }
            else
            {
                foreach (var con in booking.BookingInfoFlight.BookingBaggages.Where(x => x.IsHandLuggage))
                {
                    sb.AppendLine(string.Format("<tr><td style=\"padding:5px 10px\">Hành lý xách tay</td><td>{0}</td></tr>", con.Description));
                }
                sb.AppendLine("<tr><td style=\"padding:5px 10px\">Hành lý ký gửi</td><td>");
                foreach (var con in booking.BookingInfoFlight.BookingBaggages.Where(x => !x.IsHandLuggage).OrderByDescending(x=> x.IsFree))
                {
                    if (!con.IsFree)
                    {
                        sb.AppendLine(string.Format("<p>Hành khách {0}: {1} kg</p>", hasHandLuggage, con.Baggage));
                        hasHandLuggage++;
                    }
                    else sb.AppendLine(string.Format("<p>{0}</p>", con.Description));
                }
                sb.AppendLine("</td></tr>");
            }

            #endregion

            sb.AppendLine("</tbody></table>");
            result = sb.ToString();
            return result;
        }
        protected virtual string BookingListInfoFlightToHtmlTable(Booking booking)
        {
            var result = "";
            var fromCountryName = GetCountryName(booking.BookingInfoFlight.FromPlaceCode);
            var sb = new StringBuilder();
            sb.AppendLine("<table style=\"width:100%;border-collapse:collapse;\"><tbody>");

            #region info            
            if (booking.BookingInfoFlight.Stops == 0)
            {
                sb.AppendLine(string.Format("<tr style=\"background:#f2f2f2;\"><td><img src=\"http://clickbay.com.vn/Themes/ClickBay/Content/images/{0}.gif\" /></td><td>Khởi hành từ <strong>{1}, {2}</strong></td><td></td>" +
                                                        "<td>Thời gian bay: <strong>{3}</strong></td></tr><tr><td></td><td style=\"padding:5px 0\">Từ: <strong>{4} ({5})</strong></td>" +
                                                        "<td>tới: <strong>{6} ({7})</strong></td><td>{8} ({9})</td></tr><tr><td></td>" +
                                                        "<td style=\"padding:5px 0\"><strong>{10}</strong></td><td><strong>{11}</strong></td><td>Loại vé: {12}</td></tr>",
                                                        booking.BookingInfoFlight.Brand,
                                                        booking.BookingInfoFlight.FromPlaceName,
                                                        fromCountryName,
                                                        GetFlightDuration((decimal)booking.BookingInfoFlight.FlightDuration),
                                                        booking.BookingInfoFlight.FromPlaceName,
                                                        booking.BookingInfoFlight.FromPlaceCode,
                                                        booking.BookingInfoFlight.ToPlaceName,
                                                        booking.BookingInfoFlight.ToPlaceCode,
                                                        booking.BookingInfoFlight.BrandName, booking.BookingInfoFlight.FlightNumber,
                                                        booking.BookingInfoFlight.DepartDateTime.HasValue ? booking.BookingInfoFlight.DepartDateTime.Value.ToString("HH'h':mm dd-MM-yyyy") : "",
                                                        booking.BookingInfoFlight.ArrivalDateTime.HasValue ? booking.BookingInfoFlight.ArrivalDateTime.Value.ToString("HH'h':mm dd-MM-yyyy") : "",
                                                        booking.BookingInfoFlight.TicketType));
            }else
            {
                sb.AppendLine(string.Format("<tr style=\"background:#f2f2f2;\"><td><img src=\"http://clickbay.com.vn/Themes/ClickBay/Content/images/{0}.gif\" /></td>"+
                                                                                                    "<td>Khởi hành từ <strong>{1}, {2}</strong></td>"+
                                                                                                    "<td>Số điểm dừng: <strong>{3}</strong></td>"+
                                                                                                    "<td>Tổng thời gian: <strong>{4}</strong></td>"+
                                                                                                    "</tr>", 
                                                                                                    booking.BookingInfoFlight.Brand,
                                                                                                    booking.BookingInfoFlight.FromPlaceName,
                                                                                                    fromCountryName,
                                                                                                    booking.BookingInfoFlight.Stops,
                                                                                                    GetFlightDuration((decimal)booking.BookingInfoFlight.FlightDuration)));
                foreach(var item in booking.BookingInfoFlight.BookingInfoFlightDetails)
                {
                    sb.AppendLine(string.Format("<tr><td></td><td style=\"padding:5px 0\">Từ: <strong>{0}</strong></td>"+
                                                        "<td>tới: <strong>{1}</strong></td>"+
                                                        "<td>Thời gian bay: <strong>{2}</strong></td></tr>"+
                                                        "<tr><td></td>"+
                                                        "<td style=\"padding:5px 0\"><strong>{3}</strong></td>"+
                                                        "<td><strong>{4}</strong></td>"+
                                                        "<td>{5} (<strong>{6}</strong>)</td></tr>",
                                                        item.FromPlace,
                                                        item.ToPlace,
                                                        item.FlightDuration,
                                                        item.DepartTime,
                                                        item.LandingTime,
                                                        item.Airline,item.AirlineCode));
                }
            }
            if(booking.BookingInfoFlightReturn!= null)
            {
                var returnCountryName = GetCountryName(booking.BookingInfoFlightReturn.FromPlaceCode);
                if (booking.BookingInfoFlightReturn.Stops == 0)
                {
                    sb.AppendLine(string.Format("<tr style=\"background:#f2f2f2;\"><td><img src=\"http://clickbay.com.vn/Themes/ClickBay/Content/images/{0}.gif\" /></td><td>Khởi hành từ <strong>{1}, {2}</strong></td><td></td>" +
                                                        "<td>Thời gian bay: <strong>{3}</strong></td></tr><tr><td></td><td style=\"padding:5px 0\">Từ: <strong>{4} ({5})</strong></td>" +
                                                        "<td>tới: <strong>{6} ({7})</strong></td><td>{8} ({9})</td></tr><tr><td></td>" +
                                                        "<td style=\"padding:5px 0\"><strong>{10}</strong></td><td><strong>{11}</strong></td><td>Loại vé: {12}</td></tr>",
                                                        booking.BookingInfoFlightReturn.Brand,
                                                        booking.BookingInfoFlightReturn.FromPlaceName,
                                                        returnCountryName,
                                                        GetFlightDuration((decimal)booking.BookingInfoFlight.FlightDuration),
                                                        booking.BookingInfoFlightReturn.FromPlaceName,
                                                        booking.BookingInfoFlightReturn.FromPlaceCode,
                                                        booking.BookingInfoFlightReturn.ToPlaceName,
                                                        booking.BookingInfoFlightReturn.ToPlaceCode,
                                                        booking.BookingInfoFlightReturn.BrandName, booking.BookingInfoFlight.FlightNumber,
                                                        booking.BookingInfoFlightReturn.DepartDateTime.HasValue ? booking.BookingInfoFlightReturn.DepartDateTime.Value.ToString("HH'h':mm dd-MM-yyyy") : "",
                                                        booking.BookingInfoFlightReturn.ArrivalDateTime.HasValue ? booking.BookingInfoFlightReturn.ArrivalDateTime.Value.ToString("HH'h':mm dd-MM-yyyy") : "",
                                                        booking.BookingInfoFlightReturn.TicketType));
                }
                else
                {
                    sb.AppendLine(string.Format("<tr style=\"background:#f2f2f2;\"><td><img src=\"http://clickbay.com.vn/Themes/ClickBay/Content/images/{0}.gif\" /></td>" +
                                                                                                    "<td>Khởi hành từ <strong>{1}, {2}</strong></td>" +
                                                                                                    "<td>Số điểm dừng: <strong>{3}</strong></td>" +
                                                                                                    "<td>Tổng thời gian: <strong>{4}</strong></td>" +
                                                                                                    "</tr>",
                                                                                                    booking.BookingInfoFlightReturn.Brand,
                                                                                                    booking.BookingInfoFlightReturn.FromPlaceName,
                                                                                                    fromCountryName,
                                                                                                    booking.BookingInfoFlightReturn.Stops,
                                                                                                    GetFlightDuration((decimal)booking.BookingInfoFlightReturn.FlightDuration)));
                    foreach (var item in booking.BookingInfoFlightReturn.BookingInfoFlightDetails)
                    {
                        sb.AppendLine(string.Format("<tr><td></td><td style=\"padding:5px 0\">Từ: <strong>{0}</strong></td>" +
                                                            "<td>tới: <strong>{1}</strong></td>" +
                                                            "<td>Thời gian bay: <strong>{2}</strong></td></tr>" +
                                                            "<tr><td></td>" +
                                                            "<td style=\"padding:5px 0\"><strong>{3}</strong></td>" +
                                                            "<td><strong>{4}</strong></td>" +
                                                            "<td>{5} (<strong>{6}</strong>)</td></tr>",
                                                            item.FromPlace,
                                                            item.ToPlace,
                                                            item.FlightDuration,
                                                            item.DepartTime,
                                                            item.LandingTime,
                                                            item.Airline, item.AirlineCode));
                    }
                }
            }
            #endregion

            sb.AppendLine("</tbody></table>");
            result = sb.ToString();
            return result;
        }
        #endregion

        #region Methods

        public virtual void AddBookingTokens(IList<Token> tokens, Booking booking)
        {
            tokens.Add(new Token("Booking.TicketId", booking.TicketId));
            tokens.Add(new Token("Booking.CreatedOn", booking.CreatedOn.ToString("dd-MM-yyyy")));
            tokens.Add(new Token("Booking.RoundTrip", booking.RoundTrip ? "Khứ hồi" : "Một chiều"));

            string quantityPassenger = "";
            if (booking.Adult > 0)
                quantityPassenger += booking.Adult.ToString() + " người lớn";
            if (booking.Child > 0)
                quantityPassenger += (!string.IsNullOrEmpty(quantityPassenger) ? ", " : "") + booking.Child.ToString() + " trẻ em";
            if (booking.Infant > 0)
                quantityPassenger += (!string.IsNullOrEmpty(quantityPassenger) ? ", " : "") + booking.Infant.ToString() + " em bé";
            tokens.Add(new Token("Booking.QuantityPassenger", quantityPassenger));
            tokens.Add(new Token("Booking.DepartDateTime", booking.BookingInfoFlight.DepartDateTime.HasValue ? booking.BookingInfoFlight.DepartDateTime.Value.ToString("dd-MM-yyyy") : ""));
            string returnDate = "";
            if (booking.BookingInfoFlightReturn != null)
            {
                returnDate = booking.BookingInfoFlightReturn.DepartDateTime.HasValue ? booking.BookingInfoFlightReturn.DepartDateTime.Value.ToString("dd-MM-yyyy") : "";
            }

            tokens.Add(new Token("Booking.ContactRequestMore", booking.ContactRequestMore));

            tokens.Add(new Token("Booking.ReturnDateTime", returnDate));
            //info
            tokens.Add(new Token("Booking.InfoFlight", BookingListInfoFlightToHtmlTable(booking),true));
            //price
            tokens.Add(new Token("Booking.Prices", BookingListPriceToHtmlTable(booking),true));
            //passenger
            tokens.Add(new Token("Booking.Passengers", BookingListPassengerToHtmlTable(booking.BookingPassengers.ToList()),true));
            //condition
            tokens.Add(new Token("Booking.Conditions", BookingListConditionToHtmlTable(booking),true));
            //baggage
            tokens.Add(new Token("Booking.Baggages", BookingListBaggageToHtmlTable(booking),true));
            //payment
            string paymentmethod = string.Format(_localizationService.GetResource("booking.paymentmethod." + booking.PaymentMethodId.ToString(), _workContext.WorkingLanguage.Id), booking.ContactAddress + "," + booking.ContactCityName, booking.ContactPhone, booking.ContactName);
            tokens.Add(new Token("Booking.PaymentMethod", paymentmethod));
            //contact
            tokens.Add(new Token("Booking.ContactAddress", booking.ContactAddress));
            tokens.Add(new Token("Booking.ContactName", booking.ContactName));
            tokens.Add(new Token("Booking.ContactPassengerType", _localizationService.GetResource("passertype." + booking.PasserType.ToString())));
            tokens.Add(new Token("Booking.ContactPhone", booking.ContactPhone));
            tokens.Add(new Token("Booking.ContactEmail", booking.ContactEmail));
            tokens.Add(new Token("Booking.ContactBirthDate", booking.ContactBirthDate.HasValue ? booking.ContactBirthDate.Value.ToString("dd/MM/yyyy") : ""));
            tokens.Add(new Token("Booking.ContactCityName", booking.ContactCityName));
            var contactCountry = _clickBayService.GetCountryById(booking.ContactCountryId);

            tokens.Add(new Token("Booking.GetCountryById", contactCountry!= null?contactCountry.Name:""));
            //event notification
            _eventPublisher.EntityTokensAdded(booking, tokens);
        }

        public virtual void AddStoreTokens(IList<Token> tokens, Store store)
        {
            tokens.Add(new Token("Store.Name", store.GetLocalized(x => x.Name)));
            tokens.Add(new Token("Store.URL", store.Url, true));
            var defaultEmailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (defaultEmailAccount == null)
                defaultEmailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            tokens.Add(new Token("Store.Email", defaultEmailAccount.Email));

            //event notification
            _eventPublisher.EntityTokensAdded(store, tokens);
        }
        /*
        public virtual void AddOrderTokens(IList<Token> tokens, Order order, int languageId)
        {
            tokens.Add(new Token("Order.OrderNumber", order.Id.ToString()));

            tokens.Add(new Token("Order.CustomerFullName", string.Format("{0} {1}", order.BillingAddress.FirstName, order.BillingAddress.LastName)));
            tokens.Add(new Token("Order.CustomerEmail", order.BillingAddress.Email));


            tokens.Add(new Token("Order.BillingFirstName", order.BillingAddress.FirstName));
            tokens.Add(new Token("Order.BillingLastName", order.BillingAddress.LastName));
            tokens.Add(new Token("Order.BillingPhoneNumber", order.BillingAddress.PhoneNumber));
            tokens.Add(new Token("Order.BillingEmail", order.BillingAddress.Email));
            tokens.Add(new Token("Order.BillingFaxNumber", order.BillingAddress.FaxNumber));
            tokens.Add(new Token("Order.BillingCompany", order.BillingAddress.Company));
            tokens.Add(new Token("Order.BillingAddress1", order.BillingAddress.Address1));
            tokens.Add(new Token("Order.BillingAddress2", order.BillingAddress.Address2));
            tokens.Add(new Token("Order.BillingCity", order.BillingAddress.City));
            tokens.Add(new Token("Order.BillingStateProvince", order.BillingAddress.StateProvince != null ? order.BillingAddress.StateProvince.GetLocalized(x => x.Name) : ""));
            tokens.Add(new Token("Order.BillingZipPostalCode", order.BillingAddress.ZipPostalCode));
            tokens.Add(new Token("Order.BillingCountry", order.BillingAddress.Country != null ? order.BillingAddress.Country.GetLocalized(x => x.Name) : ""));

            tokens.Add(new Token("Order.ShippingMethod", order.ShippingMethod));
            tokens.Add(new Token("Order.ShippingFirstName", order.ShippingAddress != null ? order.ShippingAddress.FirstName : ""));
            tokens.Add(new Token("Order.ShippingLastName", order.ShippingAddress != null ? order.ShippingAddress.LastName : ""));
            tokens.Add(new Token("Order.ShippingPhoneNumber", order.ShippingAddress != null ? order.ShippingAddress.PhoneNumber : ""));
            tokens.Add(new Token("Order.ShippingEmail", order.ShippingAddress != null ? order.ShippingAddress.Email : ""));
            tokens.Add(new Token("Order.ShippingFaxNumber", order.ShippingAddress != null ? order.ShippingAddress.FaxNumber : ""));
            tokens.Add(new Token("Order.ShippingCompany", order.ShippingAddress != null ? order.ShippingAddress.Company : ""));
            tokens.Add(new Token("Order.ShippingAddress1", order.ShippingAddress != null ? order.ShippingAddress.Address1 : ""));
            tokens.Add(new Token("Order.ShippingAddress2", order.ShippingAddress != null ? order.ShippingAddress.Address2 : ""));
            tokens.Add(new Token("Order.ShippingCity", order.ShippingAddress != null ? order.ShippingAddress.City : ""));
            tokens.Add(new Token("Order.ShippingStateProvince", order.ShippingAddress != null && order.ShippingAddress.StateProvince != null ? order.ShippingAddress.StateProvince.GetLocalized(x => x.Name) : ""));
            tokens.Add(new Token("Order.ShippingZipPostalCode", order.ShippingAddress != null ? order.ShippingAddress.ZipPostalCode : ""));
            tokens.Add(new Token("Order.ShippingCountry", order.ShippingAddress != null && order.ShippingAddress.Country != null ? order.ShippingAddress.Country.GetLocalized(x => x.Name) : ""));

            var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(order.PaymentMethodSystemName);
            var paymentMethodName = paymentMethod != null ? paymentMethod.GetLocalizedFriendlyName(_localizationService, _workContext.WorkingLanguage.Id) : order.PaymentMethodSystemName;
            tokens.Add(new Token("Order.PaymentMethod", paymentMethodName));
            tokens.Add(new Token("Order.VatNumber", order.VatNumber));

            tokens.Add(new Token("Order.Product(s)", ProductListToHtmlTable(order, languageId), true));

            var language = _languageService.GetLanguageById(languageId);
            if (language != null && !String.IsNullOrEmpty(language.LanguageCulture))
            {
                DateTime createdOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, TimeZoneInfo.Utc, _dateTimeHelper.GetCustomerTimeZone(order.Customer));
                tokens.Add(new Token("Order.CreatedOn", createdOn.ToString("D", new CultureInfo(language.LanguageCulture))));
            }
            else
            {
                tokens.Add(new Token("Order.CreatedOn", order.CreatedOnUtc.ToString("D")));
            }

            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            tokens.Add(new Token("Order.OrderURLForCustomer", string.Format("{0}orderdetails/{1}", _webHelper.GetStoreLocation(false), order.Id), true));

            //event notification
            _eventPublisher.EntityTokensAdded(order, tokens);
        }
        
        public virtual void AddShipmentTokens(IList<Token> tokens, Shipment shipment, int languageId)
        {
            tokens.Add(new Token("Shipment.ShipmentNumber", shipment.Id.ToString()));
            tokens.Add(new Token("Shipment.TrackingNumber", shipment.TrackingNumber));
            tokens.Add(new Token("Shipment.Product(s)", ProductListToHtmlTable(shipment, languageId), true));
            tokens.Add(new Token("Shipment.URLForCustomer", string.Format("{0}orderdetails/shipment/{1}", _webHelper.GetStoreLocation(false), shipment.Id), true));

            //event notification
            _eventPublisher.EntityTokensAdded(shipment, tokens);
        }

        public virtual void AddOrderNoteTokens(IList<Token> tokens, OrderNote orderNote)
        {
            tokens.Add(new Token("Order.NewNoteText", orderNote.FormatOrderNoteText(), true));

            //event notification
            _eventPublisher.EntityTokensAdded(orderNote, tokens);
        }

        public virtual void AddRecurringPaymentTokens(IList<Token> tokens, RecurringPayment recurringPayment)
        {
            tokens.Add(new Token("RecurringPayment.ID", recurringPayment.Id.ToString()));

            //event notification
            _eventPublisher.EntityTokensAdded(recurringPayment, tokens);
        }

        public virtual void AddReturnRequestTokens(IList<Token> tokens, ReturnRequest returnRequest, OrderItem orderItem)
        {
            tokens.Add(new Token("ReturnRequest.ID", returnRequest.Id.ToString()));
            tokens.Add(new Token("ReturnRequest.Product.Quantity", returnRequest.Quantity.ToString()));
            tokens.Add(new Token("ReturnRequest.Product.Name", orderItem.Product.Name));
            tokens.Add(new Token("ReturnRequest.Reason", returnRequest.ReasonForReturn));
            tokens.Add(new Token("ReturnRequest.RequestedAction", returnRequest.RequestedAction));
            tokens.Add(new Token("ReturnRequest.CustomerComment", HtmlHelper.FormatText(returnRequest.CustomerComments, false, true, false, false, false, false), true));
            tokens.Add(new Token("ReturnRequest.StaffNotes", HtmlHelper.FormatText(returnRequest.StaffNotes, false, true, false, false, false, false), true));
            tokens.Add(new Token("ReturnRequest.Status", returnRequest.ReturnRequestStatus.GetLocalizedEnum(_localizationService, _workContext)));

            //event notification
            _eventPublisher.EntityTokensAdded(returnRequest, tokens);
        }

        public virtual void AddGiftCardTokens(IList<Token> tokens, GiftCard giftCard)
        {
            tokens.Add(new Token("GiftCard.SenderName", giftCard.SenderName));
            tokens.Add(new Token("GiftCard.SenderEmail",giftCard.SenderEmail));
            tokens.Add(new Token("GiftCard.RecipientName", giftCard.RecipientName));
            tokens.Add(new Token("GiftCard.RecipientEmail", giftCard.RecipientEmail));
            tokens.Add(new Token("GiftCard.Amount", _priceFormatter.FormatPrice(giftCard.Amount, true, false)));
            tokens.Add(new Token("GiftCard.CouponCode", giftCard.GiftCardCouponCode));

            var giftCardMesage = !String.IsNullOrWhiteSpace(giftCard.Message) ? 
                HtmlHelper.FormatText(giftCard.Message, false, true, false, false, false, false) : "";

            tokens.Add(new Token("GiftCard.Message", giftCardMesage, true));

            //event notification
            _eventPublisher.EntityTokensAdded(giftCard, tokens);
        }
        
        public virtual void AddCustomerTokens(IList<Token> tokens, Customer customer)
        {
            tokens.Add(new Token("Customer.Email", customer.Email));
            tokens.Add(new Token("Customer.Username", customer.Username));
            tokens.Add(new Token("Customer.FullName", customer.GetFullName()));
            tokens.Add(new Token("Customer.VatNumber", customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber)));
            tokens.Add(new Token("Customer.VatNumberStatus", ((VatNumberStatus)customer.GetAttribute<int>(SystemCustomerAttributeNames.VatNumberStatusId)).ToString()));



            //note: we do not use SEO friendly URLS because we can get errors caused by having .(dot) in the URL (from the email address)
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            string passwordRecoveryUrl = string.Format("{0}passwordrecovery/confirm?token={1}&email={2}", _webHelper.GetStoreLocation(false), customer.GetAttribute<string>(SystemCustomerAttributeNames.PasswordRecoveryToken), HttpUtility.UrlEncode(customer.Email));
            string accountActivationUrl = string.Format("{0}customer/activation?token={1}&email={2}", _webHelper.GetStoreLocation(false), customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountActivationToken), HttpUtility.UrlEncode(customer.Email));
            var wishlistUrl = string.Format("{0}wishlist/{1}", _webHelper.GetStoreLocation(false), customer.CustomerGuid);
            tokens.Add(new Token("Customer.PasswordRecoveryURL", passwordRecoveryUrl, true));
            tokens.Add(new Token("Customer.AccountActivationURL", accountActivationUrl, true));
            tokens.Add(new Token("Wishlist.URLForCustomer", wishlistUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(customer, tokens);
        }*/

        public virtual void AddNewsLetterSubscriptionTokens(IList<Token> tokens, NewsLetterSubscription subscription)
        {
            tokens.Add(new Token("NewsLetterSubscription.Email", subscription.Email));


            const string urlFormat = "{0}newsletter/subscriptionactivation/{1}/{2}";

            var activationUrl = String.Format(urlFormat, _webHelper.GetStoreLocation(false), subscription.NewsLetterSubscriptionGuid, "true");
            tokens.Add(new Token("NewsLetterSubscription.ActivationUrl", activationUrl, true));

            var deActivationUrl = String.Format(urlFormat, _webHelper.GetStoreLocation(false), subscription.NewsLetterSubscriptionGuid, "false");
            tokens.Add(new Token("NewsLetterSubscription.DeactivationUrl", deActivationUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(subscription, tokens);
        }
        /*
        public virtual void AddProductReviewTokens(IList<Token> tokens, ProductReview productReview)
        {
            tokens.Add(new Token("ProductReview.ProductName", productReview.Product.Name));

            //event notification
            _eventPublisher.EntityTokensAdded(productReview, tokens);
        }

        public virtual void AddBlogCommentTokens(IList<Token> tokens, BlogComment blogComment)
        {
            tokens.Add(new Token("BlogComment.BlogPostTitle", blogComment.BlogPost.Title));

            //event notification
            _eventPublisher.EntityTokensAdded(blogComment, tokens);
        }

       

        public virtual void AddProductTokens(IList<Token> tokens, Product product, int languageId)
        {
            tokens.Add(new Token("Product.ID", product.Id.ToString()));
            tokens.Add(new Token("Product.Name", product.GetLocalized(x => x.Name, languageId)));
            tokens.Add(new Token("Product.ShortDescription", product.GetLocalized(x => x.ShortDescription, languageId), true));
            tokens.Add(new Token("Product.StockQuantity", product.StockQuantity.ToString()));

            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            var productUrl = string.Format("{0}{1}", _webHelper.GetStoreLocation(false), product.GetSeName());
            tokens.Add(new Token("Product.ProductURLForCustomer", productUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(product, tokens);
        }*/
        /*
        public virtual void AddForumTopicTokens(IList<Token> tokens, ForumTopic forumTopic, 
            int? friendlyForumTopicPageIndex = null, int? appendedPostIdentifierAnchor = null)
        {
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            string topicUrl = null;
            if (friendlyForumTopicPageIndex.HasValue && friendlyForumTopicPageIndex.Value > 1)
                topicUrl = string.Format("{0}boards/topic/{1}/{2}/page/{3}", _webHelper.GetStoreLocation(false), forumTopic.Id, forumTopic.GetSeName(), friendlyForumTopicPageIndex.Value);
            else
                topicUrl = string.Format("{0}boards/topic/{1}/{2}", _webHelper.GetStoreLocation(false), forumTopic.Id, forumTopic.GetSeName());
            if (appendedPostIdentifierAnchor.HasValue && appendedPostIdentifierAnchor.Value > 0)
                topicUrl = string.Format("{0}#{1}", topicUrl, appendedPostIdentifierAnchor.Value);
            tokens.Add(new Token("Forums.TopicURL", topicUrl, true));
            tokens.Add(new Token("Forums.TopicName", forumTopic.Subject));

            //event notification
            _eventPublisher.EntityTokensAdded(forumTopic, tokens);
        }*/
        /*
        public virtual void AddForumPostTokens(IList<Token> tokens, ForumPost forumPost)
        {
            tokens.Add(new Token("Forums.PostAuthor", forumPost.Customer.FormatUserName()));
            tokens.Add(new Token("Forums.PostBody", forumPost.FormatPostText(), true));

            //event notification
            _eventPublisher.EntityTokensAdded(forumPost, tokens);
        }*/
        /*
        public virtual void AddForumTokens(IList<Token> tokens, Forum forum)
        {
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            var forumUrl = string.Format("{0}boards/forum/{1}/{2}", _webHelper.GetStoreLocation(false), forum.Id, forum.GetSeName());
            tokens.Add(new Token("Forums.ForumURL", forumUrl, true));
            tokens.Add(new Token("Forums.ForumName", forum.Name));

            //event notification
            _eventPublisher.EntityTokensAdded(forum, tokens);
        }*/
        /*
        public virtual void AddPrivateMessageTokens(IList<Token> tokens, PrivateMessage privateMessage)
        {
            tokens.Add(new Token("PrivateMessage.Subject", privateMessage.Subject));
            tokens.Add(new Token("PrivateMessage.Text",  privateMessage.FormatPrivateMessageText(), true));

            //event notification
            _eventPublisher.EntityTokensAdded(privateMessage, tokens);
        }*/
        /*
        public virtual void AddBackInStockTokens(IList<Token> tokens, BackInStockSubscription subscription)
        {
            tokens.Add(new Token("BackInStockSubscription.ProductName", subscription.Product.Name));
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            var productUrl = string.Format("{0}{1}", _webHelper.GetStoreLocation(false), subscription.Product.GetSeName());
            tokens.Add(new Token("BackInStockSubscription.ProductUrl", productUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(subscription, tokens);
        }
        */

        public virtual void AddNewsCommentTokens(IList<Token> tokens, NewsComment newsComment)
        {
            tokens.Add(new Token("NewsComment.NewsTitle", newsComment.NewsItem.Title));

            //event notification
            _eventPublisher.EntityTokensAdded(newsComment, tokens);
        }

        /// <summary>
        /// Gets list of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>List of allowed (supported) message tokens for campaigns</returns>
        public virtual string[] GetListOfCampaignAllowedTokens()
        {
            var allowedTokens = new List<string>()
            {
                "%Store.Name%",
                "%Store.URL%",
                "%Store.Email%",
                "%NewsLetterSubscription.Email%",
                "%NewsLetterSubscription.ActivationUrl%",
                "%NewsLetterSubscription.DeactivationUrl%"
            };
            return allowedTokens.ToArray();
        }

        public virtual string[] GetListOfAllowedTokens()
        {
            var allowedTokens = new List<string>()
            {
                "%Store.Name%",
                "%Store.URL%",
                "%Store.Email%",
                "%Order.OrderNumber%",
                "%Order.CustomerFullName%",
                "%Order.CustomerEmail%",
                "%Order.BillingFirstName%",
                "%Order.BillingLastName%",
                "%Order.BillingPhoneNumber%",
                "%Order.BillingEmail%",
                "%Order.BillingFaxNumber%",
                "%Order.BillingCompany%",
                "%Order.BillingAddress1%",
                "%Order.BillingAddress2%",
                "%Order.BillingCity%",
                "%Order.BillingStateProvince%",
                "%Order.BillingZipPostalCode%",
                "%Order.BillingCountry%",
                "%Order.ShippingMethod%",
                "%Order.ShippingFirstName%",
                "%Order.ShippingLastName%",
                "%Order.ShippingPhoneNumber%",
                "%Order.ShippingEmail%",
                "%Order.ShippingFaxNumber%",
                "%Order.ShippingCompany%",
                "%Order.ShippingAddress1%",
                "%Order.ShippingAddress2%",
                "%Order.ShippingCity%",
                "%Order.ShippingStateProvince%",
                "%Order.ShippingZipPostalCode%", 
                "%Order.ShippingCountry%",
                "%Order.PaymentMethod%",
                "%Order.VatNumber%", 
                "%Order.Product(s)%",
                "%Order.CreatedOn%",
                "%Order.OrderURLForCustomer%",
                "%Order.NewNoteText%",
                "%RecurringPayment.ID%",
                "%Shipment.ShipmentNumber%",
                "%Shipment.TrackingNumber%",
                "%Shipment.Product(s)%",
                "%Shipment.URLForCustomer%",
                "%ReturnRequest.ID%", 
                "%ReturnRequest.Product.Quantity%",
                "%ReturnRequest.Product.Name%", 
                "%ReturnRequest.Reason%", 
                "%ReturnRequest.RequestedAction%", 
                "%ReturnRequest.CustomerComment%", 
                "%ReturnRequest.StaffNotes%",
                "%ReturnRequest.Status%",
                "%GiftCard.SenderName%", 
                "%GiftCard.SenderEmail%",
                "%GiftCard.RecipientName%", 
                "%GiftCard.RecipientEmail%", 
                "%GiftCard.Amount%", 
                "%GiftCard.CouponCode%",
                "%GiftCard.Message%",
                "%Customer.Email%", 
                "%Customer.Username%", 
                "%Customer.FullName%", 
                "%Customer.VatNumber%",
                "%Customer.VatNumberStatus%", 
                "%Customer.PasswordRecoveryURL%", 
                "%Customer.AccountActivationURL%", 
                "%Wishlist.URLForCustomer%", 
                "%NewsLetterSubscription.Email%", 
                "%NewsLetterSubscription.ActivationUrl%",
                "%NewsLetterSubscription.DeactivationUrl%", 
                "%ProductReview.ProductName%", 
                "%BlogComment.BlogPostTitle%", 
                "%NewsComment.NewsTitle%",
                "%Product.ID%", 
                "%Product.Name%",
                "%Product.ShortDescription%", 
                "%Product.ProductURLForCustomer%",
                "%Product.StockQuantity%", 
                "%Forums.TopicURL%",
                "%Forums.TopicName%", 
                "%Forums.PostAuthor%",
                "%Forums.PostBody%",
                "%Forums.ForumURL%", 
                "%Forums.ForumName%", 
                "%PrivateMessage.Subject%", 
                "%PrivateMessage.Text%",
                "%BackInStockSubscription.ProductName%",
                "%BackInStockSubscription.ProductUrl%",
            };
            return allowedTokens.ToArray();
        }

        #endregion
    }
}
