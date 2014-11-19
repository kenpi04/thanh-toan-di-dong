﻿
using System.Web.Routing;
using PlanX.Core.Domain.Seo;

namespace PlanX.Web.Framework.Seo
{
    /// <summary>
    /// Event to handle unknow URL record entity names
    /// </summary>
    public class CustomUrlRecordEntityNameRequested
    {
        public CustomUrlRecordEntityNameRequested(RouteData routeData, UrlRecord urlRecord)
        {
            this.RouteData = routeData;
            this.UrlRecord = urlRecord;
        }

        public RouteData RouteData { get; private set; }
        public UrlRecord UrlRecord { get; private set; }
    }
}
