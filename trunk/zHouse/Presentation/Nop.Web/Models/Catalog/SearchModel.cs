using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;

namespace Nop.Web.Models.Catalog
{
    public partial class SearchModel : BaseNopModel
    {
        public SearchModel()
        {
            PagingFilteringContext = new SearchPagingFilteringModel();
            Products = new List<ProductOverviewModel>();
            this.AvailableCategories = new List<SelectListItem>();
            this.AvailableCategoriesRent = new List<SelectListItem>();
            this.AvailableManufacturers = new List<SelectListItem>();
            PriceRange = Nop.Web.Framework.Extensions.GetPrice();
            PriceRangeRent = Nop.Web.Framework.Extensions.GetPrice(true);
            SelectedOptionIds = new List<int>();
            DistrictIds = new List<int>();
            Districts = new List<SelectListItem>();
            Wards = new List<SelectListItem>();
            Areas = Nop.Web.Framework.Extensions.GetArea();
            Cids = new List<int>();
            Mids = new List<int>();
            BedRooms = new List<SelectListItem>();
            BathRooms = new List<SelectListItem>();
            Directories = new List<SelectListItem>();
        }

        public string Warning { get; set; }

        public bool NoResults { get; set; }

        /// <summary>
        /// Query string
        /// </summary>
        [NopResourceDisplayName("Search.SearchTerm")]
        [AllowHtml]
        public string Q { get; set; }
        /// <summary>
        /// Category ID
        /// </summary>
        [NopResourceDisplayName("Search.Category")]
        public int Cid { get; set; }
        [NopResourceDisplayName("Search.IncludeSubCategories")]
        public bool Isc { get; set; }
        /// <summary>
        /// Manufacturer ID
        /// </summary>
        [NopResourceDisplayName("Search.Manufacturer")]
        public int Mid { get; set; }
        /// <summary>
        /// Price - From 
        /// </summary>
        [AllowHtml]
        public string Pf { get; set; }
        /// <summary>
        /// Price - To
        /// </summary>
        [AllowHtml]
        public string Pt { get; set; }
        /// <summary>
        /// A value indicating whether to search in descriptions
        /// </summary>
        [NopResourceDisplayName("Search.SearchInDescriptions")]
        public bool Sid { get; set; }
        /// <summary>
        /// A value indicating whether to search in descriptions
        /// </summary>
        [NopResourceDisplayName("Search.AdvancedSearch")]
        public bool As { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvaiilableCategoriesProject { get; set; }
        public IList<SelectListItem> AvailableCategoriesRent { get; set; }
        public IList<SelectListItem> AvailableManufacturers { get; set; }
        public SearchPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<ProductOverviewModel> Products { get; set; }
        public IList<SelectListItem> Districts { get; set; }
        public IList<SelectListItem> Wards { get; set; }
        public IList<SelectListItem> PriceRange { get; set;}
        public IList<SelectListItem> PriceRangeRent { get; set; }
        public IList<SelectListItem> Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int DistrictId { get; set; }
        public int StatusId { get; set; }
        public string PriceString { get; set; }
        public bool OnlyCustomer { get; set; }
        public IList<int> Cids { get; set; }
        public IList<int> DistrictIds { get; set; }
        public IList<int> Mids { get; set; }
        public IList<int> SelectedOptionIds { get; set; }
        public IList<SelectListItem> BathRooms { get; set; }
        public IList<SelectListItem> BedRooms { get; set; }
        public IList<SelectListItem> Directories { get; set; }
        public IList<SelectListItem> Areas { get; set; }
        public bool IsOrderPage { get; set; }
        public bool IsProject { get; set; }
        public string MetaTitle { get; set; }
    }
}