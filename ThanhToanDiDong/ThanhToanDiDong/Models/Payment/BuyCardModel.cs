using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThanhToanDiDong.Models.Payment
{
    public class BuyCardModel
    {
        public IList<SelectListItem> CateCard { get; set; }
        public int CardId { get; set; }
      
    }
}