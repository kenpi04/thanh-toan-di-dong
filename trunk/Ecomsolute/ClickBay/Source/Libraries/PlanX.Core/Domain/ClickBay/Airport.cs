using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanX.Core.Domain.ClickBay
{
  public  class Airport:BaseEntity
    {
        /*
         Id	Int	
  Code	String	Mã sân bay
  Name	String	Tên sân bay, thành phố
  Order	Int	Sắp xếp

         */
        public string Code { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string CountryCode { get; set; }
        public int Order { get; set; }

    }
}
