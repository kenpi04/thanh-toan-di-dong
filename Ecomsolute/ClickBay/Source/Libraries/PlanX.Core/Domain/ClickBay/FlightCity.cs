using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PlanX.Core.Domain.ClickBay
{
  public  class FlightCity:BaseEntity
    {
        /*Id	Int
  Code	String
  Name	String
  CountryId	Int
  JetStarCode	String
  VietJetAirCode	String
  */
        public string Code { get; set; }
        public string Name { get; set; }

        public string EnglishName { get; set; }
        public int CountryId { get; set; }
        public string JetStarCode { get; set; }
        public string VietJetAirCode { get; set; }

        public virtual FlightCountry Country { get; set; }

    }
}
