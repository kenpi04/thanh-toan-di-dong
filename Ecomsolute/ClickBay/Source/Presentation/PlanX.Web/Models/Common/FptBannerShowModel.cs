using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlanX.Web.Framework.Mvc;
using PlanX.Web.Models.Media;

namespace PlanX.Web.Models.Common
{
    public class FptBannerShowModel : BaseNopEntityModel
    {
        public FptBannerShowModel()
        {

            Banners = new List<FptBannerItemModel>();
        }
        public int Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public IList<FptBannerItemModel> Banners { get; set; }

        public class FptBannerItemModel :BaseNopEntityModel
        {
                   
            public String Url { get; set; }
            public  String Name { get; set; }
            public String Description { get; set; }
            public int BannerType { get; set; }
            public String PictureUrl { get; set; }
            public String Target { get; set; }
        }
    }
}