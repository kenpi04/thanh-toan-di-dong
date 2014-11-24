using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanX.Core.Domain.ClickBay;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace PlanX.Services.Clickbay
{
  public  class ClickbayService:IClickbayService
    {
     
      public ClickbayService ()
	    {
         
         
	    }
      public async Task<IEnumerable<Ticket>> SearchTicket(string fromPlace, string toPlace, DateTime departDate ,
          int Adult = 0, int child = 0, int Infant = 0,
          string FareBasis=null,
          bool roundTrip = false,  DateTime? returnDate = null,          
          string CurrencyType = "VND", string source = null,bool expendDetails=false,
          bool expendTicketPriceDetails=false,bool expandOption=false
          )
        {
            string url = ClickbayContant.URL_SEARCH;
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
          
            string result = await GetData(url,false, param);
            if (result == null)
                return null;
            return await  Task.Factory.StartNew(()=>  JsonConvert.DeserializeObject<Dictionary<string, string>>(result))
                .ContinueWith<IEnumerable<Ticket>>(t=>{
              return   JsonConvert.DeserializeObject<IEnumerable<Ticket>>(t.Result["values"]);
            
            });
            
            
           
        }

        public async Task<IEnumerable<Airport>> GetAirport()
        {
            string result = await GetData(ClickbayContant.URL_GET_AIRPLACES);
            if (result == null)
                return null;
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Dictionary<string, string>>(result))
               .ContinueWith<IEnumerable<Airport>>(t =>
               {
                   return JsonConvert.DeserializeObject<IEnumerable<Airport>>(t.Result["values"]);

               });
            
        }

        public async Task<IEnumerable<FlightCity>> GetCity()
        {
            string result = await GetData(ClickbayContant.URL_GET_CITYS);
            if (result == null)
                return null;
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Dictionary<string, string>>(result))
              .ContinueWith<IEnumerable<FlightCity>>(t =>
              {
                  return JsonConvert.DeserializeObject<IEnumerable<FlightCity>>(t.Result["values"]);

              });
        }

        public async Task<IEnumerable<FlightCountry>> GetCountry()
        {
            string result = await GetData(ClickbayContant.URL_GET_COUNTRIES);
            if (result == null)
                return null;
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Dictionary<string, string>>(result))
              .ContinueWith<IEnumerable<FlightCountry>>(t =>
              {
                  return JsonConvert.DeserializeObject<IEnumerable<FlightCountry>>(t.Result["values"]);

              });
        }

        public async Task<BookTicket> BookTicket(BookTicket model)
        {
            var data = model.ToDictionary().Select(x=>new KeyValuePair<string,string>(x.Key,x.Value.ToString()));
            string resultJson = await GetData(ClickbayContant.URL_BOOK,false,data);

            return await Task.Factory.StartNew<BookTicket>(() => JsonConvert.DeserializeObject<BookTicket>(resultJson));

           
        }
     
      private async Task<string>GetData(string url,bool isGET=true,IEnumerable<KeyValuePair<string,string>> para=null)
       {
         
          
           string resultJson=string.Empty;
         
          HttpClientHandler handler=new HttpClientHandler();
          handler.Credentials=new NetworkCredential(ClickbayContant.USERNAME,ClickbayContant.PASSWORD);
         HttpClient client=new HttpClient(handler);
          HttpRequestMessage request=new HttpRequestMessage{
          Method=isGET?HttpMethod.Get:HttpMethod.Post,
          RequestUri=new Uri(url),
       
         
          };
          request.Headers.Add("Content-Type","application/json");
          if(para!=null)
          {
              var data = new FormUrlEncodedContent(para);
              request.Content = data;
          }
          var response= await client.SendAsync(request);

          if (response.IsSuccessStatusCode)
          {
              response.EnsureSuccessStatusCode();
              return await response.Content.ReadAsStringAsync();


          }
          else
              return null;
          

  
          

      }




      public FlightCity GetCityById(int id)
      {
          throw new NotImplementedException();
      }

      public FlightCountry GetCountryById(int id)
      {
          throw new NotImplementedException();
      }

      public BookTicket GetBookTicketById(int id)
      {
          throw new NotImplementedException();
      }

      public Ticket GetTicketById(int id)
      {
          throw new NotImplementedException();
      }
    }
}
