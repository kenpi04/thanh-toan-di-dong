﻿using System;
using System.Collections.Generic;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Web.Models.News
{
    public partial class HomePageNewsItemsModel : BaseNopModel, ICloneable
    {
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }

        public int WorkingLanguageId { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return this.MemberwiseClone();
        }
    }
}