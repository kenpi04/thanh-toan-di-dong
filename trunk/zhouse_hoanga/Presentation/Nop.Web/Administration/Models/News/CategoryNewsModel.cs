using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Models.News;
using Nop.Admin.Validators.News;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;

namespace Nop.Admin.Models.News
{
    [Validator(typeof(CategoryNewsValidator))]
    public partial class CategoryNewsModel : BaseNopEntityModel
    {
        public CategoryNewsModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }
        }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.CreatedBy")]
        public string CreatedBy { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.UpdatedBy")]
        public string UpdatedBy { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.Parent")]
        public int ParentCategoryNewsId { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.Deleted")]
        public bool Deleted { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.Categories.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public string Breadcrumb { get; set; }

        public IList<DropDownItem> ParentCategories { get; set; }

        #region Nested classes

        public partial class CategoryNewsMapModel : BaseNopEntityModel
        {
            public int CategoryNewsId { get; set; }

            public int NewsId { get; set; }

            [NopResourceDisplayName("Admin.ContentManagement.News.Mapping.Title")]
            public string NewsTitle { get; set; }

        }

        public partial class AddCategoryNewsModel : BaseNopModel
        {
            public AddCategoryNewsModel()
            {
                AvailableCategories = new List<SelectListItem>();
            }
            public GridModel<NewsItemModel> NewsItem { get; set; }

            [NopResourceDisplayName("Admin.ContentManagement.News.List.SearchNewsTitle")]
            [AllowHtml]
            public string SearchNewsTitle { get; set; }
            [NopResourceDisplayName("Admin.ContentManagement.News.List.SearchCategory")]
            public int SearchCategoryId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }

            public int CategoryId { get; set; }

            public int[] SelectedNewsIds { get; set; }
        }

        #endregion

    }
}