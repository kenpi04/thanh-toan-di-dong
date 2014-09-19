using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Catalog
{
    public partial class ProductListModel : BaseNopModel
    {
        public ProductListModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableWards = new List<SelectListItem>();
            AvailableDistricts = new List<SelectListItem>();
            AvailableStateProvinces = new List<SelectListItem>();
            AvaiableCustomers = new List<SelectListItem>();
            AvailableStatus = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
        [AllowHtml]
        public string SearchProductName { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
        public int SearchCategoryId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchIncludeSubCategories")]
        public bool SearchIncludeSubCategories { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
        public int SearchManufacturerId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
        public int SearchStoreId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchStateProvice")]
        public int SearchStateProviceId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchDistrict")]
        public int SearchDistrictId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchWard")]
        public int SearchWardId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCustomer")]
        public int SearchCustomerId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchStatus")]
        public int SearchStatus { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.GoDirectlyToSku")]
        [AllowHtml]
        public string GoDirectlyToSku { get; set; }

        public bool DisplayProductPictures { get; set; }

        public bool IsLoggedInAsVendor { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableManufacturers { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableDistricts { get; set; }
        public IList<SelectListItem> AvailableStateProvinces { get; set; }
        public IList<SelectListItem> AvailableWards { get; set; }
        public IList<SelectListItem> AvaiableCustomers { get; set; }
        public IList<SelectListItem> AvailableStatus { get; set; }
    }
}