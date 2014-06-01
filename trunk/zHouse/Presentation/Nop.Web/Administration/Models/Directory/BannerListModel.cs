using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace Nop.Admin.Models.Directory
{
    public partial class BannerListModel : BaseNopModel
    {
        public GridModel<BannerModel> banners { get; set; }
    }
}