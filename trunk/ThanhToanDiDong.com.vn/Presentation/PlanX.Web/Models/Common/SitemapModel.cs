using System.Collections.Generic;
using PlanX.Web.Framework.Mvc;
//using PlanX.Web.Models.Catalog;
using PlanX.Web.Models.Topics;

namespace PlanX.Web.Models.Common
{
    public partial class SitemapModel : BaseNopModel
    {
        public SitemapModel()
        {
            //Products = new List<ProductOverviewModel>();
            //Categories = new List<CategoryModel>();
            //Manufacturers = new List<ManufacturerModel>();
            Topics = new List<TopicModel>();
        }
        //public IList<ProductOverviewModel> Products { get; set; }
        //public IList<CategoryModel> Categories { get; set; }
        //public IList<ManufacturerModel> Manufacturers { get; set; }
        public IList<TopicModel> Topics { get; set; }
    }
}