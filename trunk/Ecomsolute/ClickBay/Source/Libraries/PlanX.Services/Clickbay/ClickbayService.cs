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

namespace PlanX.Services.ClickBay
{
  public  class ClickbayService:IClickBayService
    {
      private readonly IRepository<Booking> _BookingRepository;
      private readonly IRepository<FlightCountry> _flightCountryRepository;
      private readonly IRepository<FlightCity> _flightCityRepository;
      private readonly IRepository<Airport> _airportRepository;
      private readonly IRepository<AirlinesCondition> _airlinesConditionRepository;
      private readonly IRepository<ArilinesBaggageCondition> _airlineBaggageConditionRepository;
      private readonly IWebHelper _webHelper;
    
      public ClickbayService (IRepository<Booking>BookingRepository,
          IRepository<FlightCountry> flightCountryRepository,
          IRepository<FlightCity> flightCityRepository,
          IRepository<Airport>_airportRepository,
          IRepository<AirlinesCondition> airlinesConditionRepository,
          IRepository<ArilinesBaggageCondition> airlineBaggageConditionRepository,
          IWebHelper webHelper
          )
	    {
            this._BookingRepository = BookingRepository;
            this._flightCityRepository = flightCityRepository;
            this._flightCountryRepository = flightCountryRepository;
            this._airportRepository = _airportRepository;
            this._airlineBaggageConditionRepository = airlineBaggageConditionRepository;
            this._airlinesConditionRepository = airlinesConditionRepository;
            _webHelper = webHelper;
	    }

      #region API
      public IEnumerable<Ticket> SearchTicket(string fromPlace, string toPlace, DateTime departDate ,
          int Adult = 0, int child = 0, int Infant = 0,
          string FareBasis=null,
          bool roundTrip = false,  DateTime? returnDate = null,          
          string CurrencyType = "VND", string source = null,bool expendDetails=false,
          bool expendTicketPriceDetails=false,bool expandOption=false
          )
        {
            string url = ClickBayContant.URL_SEARCH;
          if(expendDetails||expendTicketPriceDetails||expandOption)
          {
             List<string> querys=new List<string>();
              if (expendDetails)
                  querys.Add("Details");
              if (expendTicketPriceDetails)
                  querys.Add("TicketPriceDetails");
              if (expandOption)
                  querys.Add("TicketOptions");
              url += "?$expand=" + querys.Aggregate((a, b) => a + "," + b);

          }
            var param = new List<KeyValuePair<string, string>> { 
            
                new KeyValuePair<string,string>("Adult",Adult.ToString()),
                new KeyValuePair<string,string>("Child",child.ToString()),
                new KeyValuePair<string,string>("Infant",Infant.ToString()),
                 new KeyValuePair<string,string>("FareBasis",FareBasis),
                new KeyValuePair<string,string>("roundTrip",roundTrip.ToString()),
                new KeyValuePair<string,string>("DepartDate",departDate.ToString("T00:00:00.000")),
                new KeyValuePair<string,string>("ReturnDate",returnDate.HasValue?returnDate.Value.ToString("T00:00:00.000"):null),
                new KeyValuePair<string,string>("FromPlace",fromPlace),
               new KeyValuePair<string,string>("ToPlace",toPlace),
                new KeyValuePair<string,string>("CurrencyType",CurrencyType),
                new KeyValuePair<string,string>("Sources",source),
                
            };

            string result = GetData(url,false, param);
            if (result == null)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<Ticket>>(result);
            
            
           
        }

        public  IEnumerable<Airport> GetAirport()
        {
            string result =  GetData(ClickBayContant.URL_GET_AIRPLACES);
            if (result == null)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<Airport>>(result);
            
        }

        public IEnumerable<FlightCity> GetCity()
        {
            string result =  GetData(ClickBayContant.URL_GET_CITYS);
            if (result == null)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<FlightCity>>(result);
        }

        public IEnumerable<FlightCountry> GetCountry()
        {
            string result =  GetData(ClickBayContant.URL_GET_COUNTRIES);
            if (result == null)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<FlightCountry>>(result);
        }

        public Booking BookTicket(Booking model)
        {
            var data = model.ToDictionary().Select(x=>new KeyValuePair<string,string>(x.Key,x.Value.ToString()));
            string result = GetData(ClickBayContant.URL_BOOK, false, data);
            return JsonConvert.DeserializeObject<Booking>(result);

           
        }
     
      private string GetData(string url,bool isGET=true,IEnumerable<KeyValuePair<string,string>> para=null)
       {


           string resultJson = string.Empty;
           HttpClientHandler handler = new HttpClientHandler();
           //handler.Credentials = new NetworkCredential(ClickBayContant.USERNAME, ClickBayContant.PASSWORD);
           HttpClient client = new HttpClient(handler);
           String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ClickBayContant.USERNAME + ":" + ClickBayContant.PASSWORD));
           client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);
         


           var response = client.GetAsync(new Uri(ClickBayContant.URL_GET_AIRPLACES)).Result;

           if (response.IsSuccessStatusCode)
           {
               response.EnsureSuccessStatusCode();
               string result= response.Content.ReadAsStringAsync().Result;
               if (result != null)
               {
                   var convertModel = JsonConvert.DeserializeObject<ConvertDataModel>(result);
                   if (convertModel.Value != null)
                       return convertModel.Value.ToString();
               }


           }          
           return null;
          

  
          

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
      public IEnumerable<FlightCity> GetListCity(int countryId = 0)
      {
          var q = _flightCityRepository.Table;
          if (countryId > 0)
              q = q.Where(x => x.CountryId == countryId);
          return q.AsEnumerable();
      }

      public IEnumerable<FlightCity> GetListCountry()
      {
          return _flightCityRepository.Table;
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

      class ConvertDataModel
      {
          public object Value { get; set; }
      }


      /*
      public void InsertOrUpdateBooking(Booking book)
      {
          if (book == null)
              throw new ArgumentNullException("Book is Null");
          if (book.Id > 0)
              _bookTicketRepository.Update(book);
          else
              _bookTicketRepository.Insert(book);

      }

      public void DeleteBooking(BookTicket book)
      {
          _bookTicketRepository.Delete(book);
      }*/

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
         if(airport==null)
             throw new ArgumentNullException("Airport is null");
         
             _airportRepository.Insert(airport);
      }


      public IList<AirlinesCondition> GetListAirlinesConditionByAirlineId(string airlineCode)
      {
          return _airlinesConditionRepository.Table.Where(x => x.AirlinesId == airlineCode).OrderBy(x => x.DisplayOrder).ToList();
      }

      public IList<ArilinesBaggageCondition> GetListArilinesBaggageCondition(string airlineCode)
      {
          return _airlineBaggageConditionRepository.Table.Where(x => x.AirlinesId == airlineCode).OrderBy(x=>x.DisplayOrder).ToList();
      }



      public  string GetData()
      {
          string resultJson = string.Empty;

          HttpClientHandler handler = new HttpClientHandler();
          //handler.Credentials = new NetworkCredential(ClickBayContant.USERNAME, ClickBayContant.PASSWORD);
        
          HttpClient client = new HttpClient(handler);
         
          String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ClickBayContant.USERNAME + ":" + ClickBayContant.PASSWORD));
          client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",encoded);
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
    }
}
