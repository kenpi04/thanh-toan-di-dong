﻿using System.Web.Mvc;
using System.Web.Routing;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Cms
{
    public partial class WidgetModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.ContentManagement.Widgets.Fields.FriendlyName")]
        [AllowHtml]
        public string FriendlyName { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.Widgets.Fields.SystemName")]
        [AllowHtml]
        public string SystemName { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.Widgets.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.Widgets.Fields.IsActive")]
        public bool IsActive { get; set; }
        

        public string ConfigurationActionName { get; set; }
        public string ConfigurationControllerName { get; set; }
        public RouteValueDictionary ConfigurationRouteValues { get; set; }
    }
}