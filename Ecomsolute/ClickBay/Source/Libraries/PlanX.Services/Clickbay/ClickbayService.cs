using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanX.Core.Domain.ClickBay;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using PlanX.Core.Data;
using PlanX.Core;
using System.IO;
using System.Net.Http.Headers;
using PlanX.Data;

namespace PlanX.Services.ClickBay
{
    public partial class ClickBayService : IClickBayService
    {
        private readonly IRepository<Booking> _BookingRepository;
        private readonly IRepository<FlightCountry> _flightCountryRepository;
        private readonly IRepository<FlightCity> _flightCityRepository;
        private readonly IRepository<Airport> _airportRepository;
        private readonly IRepository<AirlinesCondition> _airlinesConditionRepository;
        private readonly IRepository<ArilinesBaggageCondition> _airlineBaggageConditionRepository;
        private readonly IWebHelper _webHelper;
        //private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<BookingInfoFlight> _bookingInfoFlightRepository;
        private readonly IRepository<BookingBaggage> _bookingBaggageRepository;
        private readonly IRepository<BookingPassenger> _bookingPassengerRepository;
        private readonly IRepository<BookingPriceDetail> _bookingPriceDetailRepository;
        private readonly IRepository<BookingInfoCondition> _bookingInfoConditionRepository;
        private readonly IRepository<BookTicketNote> _bookTicketNoteRepository;
        private readonly IRepository<Airline> _airlineRepository;
        private readonly IDbContext _dbContext;

        public ClickBayService(IRepository<Booking> BookingRepository,
            IRepository<FlightCountry> flightCountryRepository,
            IRepository<FlightCity> flightCityRepository,
            IRepository<Airport> _airportRepository,
            IRepository<AirlinesCondition> airlinesConditionRepository,
            IRepository<ArilinesBaggageCondition> airlineBaggageConditionRepository,
            IWebHelper webHelper,            
            IRepository<BookingBaggage> bookingBaggageRepository,
            IRepository<BookingPassenger> bookingPassengerRepository,
            IRepository<BookingPriceDetail> bookingPriceDetailRepository,
            IRepository<BookingInfoCondition> bookingInfoConditionRepository,
            IRepository<BookTicketNote> bookTicketNoteRepository,
            IRepository<Airline> airlineRepository,
            IRepository<BookingInfoFlight> bookingInfoFlightRepository,
            IDbContext dbContext
            )
        {
            this._BookingRepository = BookingRepository;
            this._flightCityRepository = flightCityRepository;
            this._flightCountryRepository = flightCountryRepository;
            this._airportRepository = _airportRepository;
            this._airlineBaggageConditionRepository = airlineBaggageConditionRepository;
            this._airlinesConditionRepository = airlinesConditionRepository;
            this._webHelper = webHelper;
            this._bookingBaggageRepository = bookingBaggageRepository;
            this._bookingPassengerRepository = bookingPassengerRepository;
            this._bookingPriceDetailRepository = bookingPriceDetailRepository;
            this._bookingInfoConditionRepository = bookingInfoConditionRepository;
            this._bookTicketNoteRepository = bookTicketNoteRepository;
            this._airlineRepository = airlineRepository;
            this._bookingInfoFlightRepository = bookingInfoFlightRepository;
            this._dbContext = dbContext;
        }

        #region API
        public IEnumerable<Ticket> SearchTicket(string fromPlace, string toPlace, DateTime departDate,
            int Adult = 0, int child = 0, int Infant = 0,
            string FareBasis = null,
            bool roundTrip = false, DateTime? returnDate = null,
            string CurrencyType = "VND", string source = null, bool expendDetails = false,
            bool expendTicketPriceDetails = false, bool expandOption = false, bool priceSummaries = false
            )
        {
            string url = ClickBayContant.URL_SEARCH;
            if (expendDetails || expendTicketPriceDetails || expandOption || priceSummaries)
            {
                List<string> querys = new List<string>();
                if (expendDetails)
                    querys.Add("Details");
                if (expendTicketPriceDetails)
                    querys.Add("TicketPriceDetails");
                if (expandOption)
                    querys.Add("TicketOptions");
                if (priceSummaries)
                    querys.Add("PriceSummaries");
                url += "?$expand=" + querys.Aggregate((a, b) => a + "," + b);

            }

            var searchModel = new TicketSearch();

            searchModel.Adult = Adult;
            searchModel.Child = child;
            searchModel.Infant = Infant;
            searchModel.RoundTrip = false;
            searchModel.DepartDate = departDate.ToString("yyyy-MM-ddT00:00:00");//2015-01-15T00:00:00.000
            searchModel.ReturnDate = departDate.ToString("yyyy-MM-ddT00:00:00"); ;//returnDate.HasValue? returnDate.Value.ToString("yyyy-MM-ddT00:00:00.000"):null;
            searchModel.FromPlace = fromPlace;
            searchModel.ToPlace = toPlace;
            searchModel.CurrencyType = "VND";
            searchModel.Sources = string.IsNullOrEmpty(source) ? "VietJetAir,VietnamAirlines,JetStar" : source;

            string data = JsonConvert.SerializeObject(searchModel);

            string result = GetData(url, false, null, data);
            //string result = readFile("data_detail.txt");
            if (string.IsNullOrEmpty(result))
                return new List<Ticket>();
            return JsonConvert.DeserializeObject<IEnumerable<Ticket>>(result);
        }

        public string SearchTicketJson(string fromPlace, string toPlace, DateTime departDate,
            int Adult = 0, int child = 0, int Infant = 0,
            string FareBasis = null,
            bool roundTrip = false, DateTime? returnDate = null,
            string CurrencyType = "VND", string source = null, bool expendDetails = false,
            bool expendTicketPriceDetails = false, bool expandOption = false, bool priceSummaries = false
            )
        {
            string url = ClickBayContant.URL_SEARCH;
            if (expendDetails || expendTicketPriceDetails || expandOption || priceSummaries)
            {
                List<string> querys = new List<string>();
                if (expendDetails)
                    querys.Add("Details");
                if (expendTicketPriceDetails)
                    querys.Add("TicketPriceDetails");
                if (expandOption)
                    querys.Add("TicketOptions");
                if (priceSummaries)
                    querys.Add("PriceSummaries");
                url += "?$expand=" + querys.Aggregate((a, b) => a + "," + b);

            }

            var searchModel = new TicketSearch();

            searchModel.Adult = Adult;
            searchModel.Child = child;
            searchModel.Infant = Infant;
            searchModel.RoundTrip = false;
            searchModel.DepartDate = departDate.ToString("yyyy-MM-ddT00:00:00");//2015-01-15T00:00:00.000
            searchModel.ReturnDate = departDate.ToString("yyyy-MM-ddT00:00:00");
            searchModel.FromPlace = fromPlace;
            searchModel.ToPlace = toPlace;
            searchModel.CurrencyType = "VND";//CurrencyType;
            searchModel.Sources = string.IsNullOrEmpty(source) ? "VietJetAir,JetStar,VietnamAirlines" : source;
            string data = JsonConvert.SerializeObject(searchModel);

            string result = GetData1(url, false, null, data);
            return result;
        }

        public IEnumerable<Airport> GetAirport()
        {
            string result = GetData(ClickBayContant.URL_GET_AIRPLACES);
            if (result == null)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<Airport>>(result);

        }

        public IEnumerable<FlightCity> GetCity()
        {
            string result = GetData(ClickBayContant.URL_GET_CITYS);
            if (result == null)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<FlightCity>>(result);
        }

        public IEnumerable<FlightCountry> GetCountry()
        {
            string result = GetData(ClickBayContant.URL_GET_COUNTRIES);
            if (result == null)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<FlightCountry>>(result);
        }

        public Booking BookTicket(Booking model)
        {
            var data = model.ToDictionary().Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));
            string result = GetData(ClickBayContant.URL_BOOK, false, data);
            return JsonConvert.DeserializeObject<Booking>(result);
        }

        private string GetData(string url, bool isGET = true, IEnumerable<KeyValuePair<string, string>> para = null, string dataString = null)
        {
            string resultJson = string.Empty;
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.Credentials = new NetworkCredential(ClickBayContant.USERNAME, ClickBayContant.PASSWORD);
            HttpClient client = new HttpClient();
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ClickBayContant.USERNAME + ":" + ClickBayContant.PASSWORD));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);
            HttpResponseMessage response;
            if (isGET)
                response = client.GetAsync(new Uri(url)).Result;
            else
            {
                HttpContent data;
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    data = new StringContent(dataString, Encoding.UTF8, "application/json");
                }
                else { data = new FormUrlEncodedContent(para); }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = client.PostAsync(url, data).Result;
            }
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                if (result != null)
                {                    
                    //convert dang bi sai?
                    var convertModel = JsonConvert.DeserializeObject<ConvertDataModel>(result);
                    if (convertModel.Value != null)
                    return convertModel.Value.ToString();
                }
            }
            return null;
        }

        private string GetData1(string url, bool isGET = true, IEnumerable<KeyValuePair<string, string>> para = null, string dataString = null)
        {
            bool error= false;
            var result = GetResponseString(out error, url, isGET, para, dataString);
            if (!error && !string.IsNullOrEmpty(result))
            {
                try
                {
                    var convertModel = JsonConvert.DeserializeObject<ConvertDataModel>(result);
                    return result;
                }
                catch (Exception ex) { return ex.ToString(); }
            }
            else return result;
        }

        private string GetResponseString(out bool error, string url, bool isGET = true, IEnumerable<KeyValuePair<string, string>> para = null, string dataString = null)
        {
            error = false;
            try
            {
                string resultJson = string.Empty;
                //HttpClientHandler handler = new HttpClientHandler();
                //handler.Credentials = new NetworkCredential(ClickBayContant.USERNAME, ClickBayContant.PASSWORD);
                HttpClient client = new HttpClient();
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ClickBayContant.USERNAME + ":" + ClickBayContant.PASSWORD));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);
                HttpResponseMessage response;
                if (isGET)
                    response = client.GetAsync(new Uri(url)).Result;
                else
                {
                    HttpContent data;
                    if (!string.IsNullOrWhiteSpace(dataString))
                    {
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        data = new StringContent(dataString, Encoding.UTF8, "application/json");
                    }
                    else { data = new FormUrlEncodedContent(para); }

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    response = client.PostAsync(url, data).Result;
                }
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    if (result != null)
                    {
                        //convert dang bi sai?
                        //var convertModel = JsonConvert.DeserializeObject<ConvertDataModel>(result);
                        //if (convertModel.Value != null)
                        //    return convertModel.Value.ToString();
                        return result;
                    }
                }
                return null;
            }
            catch (Exception ex) { error = true; return ex.ToString(); }
        }
        #endregion

        #region DataBase Service

        public FlightCity GetCityById(int id)
        {
            return _flightCityRepository.GetById(id);
        }

        public FlightCountry GetCountryById(int id)
        {
            return _flightCountryRepository.GetById(id);
        }

        public Booking GetBookTicketById(int id)
        {
            return _BookingRepository.GetById(id);
        }

        public Ticket GetTicketById(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<FlightCity> GetListCity(int countryId = 0, string name = null)
        {
            var q = _flightCityRepository.Table;
            if (countryId > 0)
                q = q.Where(x => x.CountryId == countryId);
            if (!string.IsNullOrEmpty(name))
            {
                q = q.Where(x => x.EnglishName.Contains(name.ToLower()));
            }
            return q.AsEnumerable();
        }

        public IEnumerable<FlightCountry> GetListCountry()
        {
            return _flightCountryRepository.Table;
        }

        public IEnumerable<Airport> GetListAirport(int country = 0, int city = 0)
        {
            return _airportRepository.Table;

        }
        #endregion


        public FlightCity GetcityByCode(string code)
        {
            return _flightCityRepository.Table.FirstOrDefault(x => x.Code.Equals(code));
        }
        private string readFile(string fileName)
        {
            string filePath = Path.Combine(_webHelper.MapPath("~/content/"), fileName);
            return File.ReadAllText(filePath, Encoding.UTF8).Trim();
        }

        public void InsertCity(FlightCity city)
        {
            if (city == null)
                throw new ArgumentNullException("City is null");
            _flightCityRepository.Insert(city);
        }

        public void InsertCountry(FlightCountry entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Country is null");

            _flightCountryRepository.Insert(entity);
        }

        public void InsertAirport(Airport airport)
        {
            if (airport == null)
                throw new ArgumentNullException("Airport is null");

            _airportRepository.Insert(airport);
        }


        public IList<AirlinesCondition> GetListAirlinesConditionByAirlineId(int airlineId)
        {
            return _airlinesConditionRepository.Table.Where(x => x.AirlinesId == airlineId).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IList<ArilinesBaggageCondition> GetListArilinesBaggageCondition(int airlineId)
        {
            return _airlineBaggageConditionRepository.Table.Where(x => x.AirlinesId == airlineId).OrderBy(x => x.DisplayOrder).ToList();


        }
        public IList<Airline>GetListAirline()
        {
            return _airlineRepository.Table.ToList();
        }

        public string GetData()
        {
            string resultJson = string.Empty;

            HttpClientHandler handler = new HttpClientHandler();
            //handler.Credentials = new NetworkCredential(ClickBayContant.USERNAME, ClickBayContant.PASSWORD);

            HttpClient client = new HttpClient(handler);

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ClickBayContant.USERNAME + ":" + ClickBayContant.PASSWORD));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);
            //  request.Headers.Add("Authorization", "Basic " + encoded);


            var response = client.GetAsync(new Uri(ClickBayContant.URL_GET_AIRPLACES)).Result;

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;


            }
            else
                return null;
        }

        public void UpdateCity(FlightCity city)
        {
            if (city == null)
                throw new ArgumentNullException("City is null");
            _flightCityRepository.Update(city);
        }

        public void UpdateCountry(FlightCountry entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Country is null");

            _flightCountryRepository.Update(entity);
        }

        public void UpdateAirport(Airport airport)
        {
            if (airport == null)
                throw new ArgumentNullException("Airport is null");

            _airportRepository.Update(airport);
        }
        public   Airline GetAirlineById(int id)
        {
            return _airlineRepository.GetById(id);
        }
        
        class ConvertDataModel
        {
            public object Value { get; set; }
        }
        protected class TicketSearch
        {
            public TicketSearch()
            { }
            public bool RoundTrip { get; set; }
            public int Adult { get; set; }
            public int Child { get; set; }

            public int Infant { get; set; }

            //public string FareBasis { get; set; }


            public string DepartDate { get; set; }

            public string ReturnDate { get; set; }

            public string FromPlace { get; set; }

            public string ToPlace { get; set; }

            public string CurrencyType { get; set; }

            public string Sources { get; set; }


        }
    }
}
