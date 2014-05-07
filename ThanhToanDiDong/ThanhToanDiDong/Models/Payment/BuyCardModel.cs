using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThanhToanDiDong.Models.Payment
{
    public class BuyCardModel
    {
        public IList<CateCard> CateCards { get; set; }
        public int CardId { get; set; }

        public class CateCard {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
        }
      
    }
}