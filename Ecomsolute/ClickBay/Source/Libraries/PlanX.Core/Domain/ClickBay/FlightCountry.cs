using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
   public class FlightCountry:BaseEntity
   {
       /*Id
Code
Name
EnglishName
JetStarCode
VietJetAirCode
*/
       public string Code { get; set; }
       public string Name { get; set; }
       public string EnglishName { get; set; }
       public string JetStarCode { get; set; }
       public string VietJetAirCode { get; set; }

    }
}
