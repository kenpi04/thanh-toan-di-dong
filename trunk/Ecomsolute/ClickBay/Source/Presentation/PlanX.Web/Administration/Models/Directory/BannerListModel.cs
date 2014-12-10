using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace PlanX.Admin.Models.Directory
{
    public partial class BannerListModel : BaseNopModel
    {
        public GridModel<BannerModel> banners { get; set; }
    }
}