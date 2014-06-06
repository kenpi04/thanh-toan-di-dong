using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Models.Catalog;
using Nop.Admin.Models.News;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.News;
using Nop.Services.Catalog;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.News;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using Nop.Services.Seo;
using Nop.Web.Framework;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public class CategoryNewsController : BaseNopController
    {
        #region Fields
        private readonly ICategoryNewsService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IManufacturerTemplateService _manufacturerTemplateService;
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IExportManager _exportManager;
        private readonly IWorkContext _workContext;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly INewsService _newsService;
        private readonly IUrlRecordService _urlRecordService;

        #endregion

        #region Contructor
        public CategoryNewsController(ICategoryNewsService categoryService, IManufacturerService manufacturerService,
            IManufacturerTemplateService manufacturerTemplateService, IProductService productService,
            IPictureService pictureService, ILanguageService languageService,
            ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
            IExportManager exportManager, IWorkContext workContext,
            ICustomerActivityService customerActivityService, IPermissionService permissionService,
            AdminAreaSettings adminAreaSettings, CatalogSettings catalogSettings, INewsService newsService,IUrlRecordService urlRecordService)
        {
            this._urlRecordService = urlRecordService;
            this._categoryService = categoryService;
            this._manufacturerTemplateService = manufacturerTemplateService;
            this._manufacturerService = manufacturerService;
            this._productService = productService;
            this._pictureService = pictureService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._exportManager = exportManager;
            this._workContext = workContext;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._adminAreaSettings = adminAreaSettings;
            this._catalogSettings = catalogSettings;
            this._newsService = newsService;
        }
        #endregion

        #region Utilities

        [NonAction]
        protected void UpdateLocales(CategoryNews category, CategoryNewsModel model)
        {
            //foreach (var localized in model.Locales)
            //{
            //    _localizedEntityService.SaveLocalizedValue(category,
            //                                                   x => x.Name,
            //                                                   localized.Name,
            //                                                   localized.LanguageId);

            //    _localizedEntityService.SaveLocalizedValue(category,
            //                                               x => x.MetaTitle,
            //                                               localized.MetaTitle,
            //                                               localized.LanguageId);
            //}
        }

        #endregion

        #region List
      
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            //Permission();
            var model = new CategoryNewsListModel();
            var category = _categoryService.GetAllCategories(null, 0, _adminAreaSettings.GridPageSize, true);
            model.Categories = new GridModel<CategoryNewsModel>
            {
                Data = category.Select(x =>
                {
                    var categoryModel = x.ToModel();
                    categoryModel.Breadcrumb = x.GetCategoryBreadCrumb(_categoryService);
                    return categoryModel;
                }),
                Total = category.TotalCount
            };
            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, CategoryNewsListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var categories = _categoryService.GetAllCategories(model.SearchCategoryName,
                command.Page - 1, command.PageSize, true);
            var gridModel = new GridModel<CategoryNewsModel>
            {
                Data = categories.Select(x =>
                {
                    var categoryModel = x.ToModel();
                  //  categoryModel.Breadcrumb = x.GetCategoryBreadCrumb(_categoryService);
                    return categoryModel;
                }),
                Total = categories.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        //ajax
        public ActionResult AllCategories(string text, int selectedId)
        {
            var categories = _categoryService.GetAllCategories(true);
            categories.Insert(0, new CategoryNews { Name = "[None]", Id = 0 });
            var selectList = new List<SelectListItem>();
            foreach (var c in categories)
                selectList.Add(new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == selectedId
                });

            //var selectList = new SelectList(categories, "Id", "Name", selectedId);
            return new JsonResult { Data = selectList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Tree()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var rootCategories = _categoryService.GetAllCategoriesByParentCategoryId(0, true);
            return View(rootCategories);
        }

        //ajax
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TreeLoadChildren(TreeViewItem node)
        {
            var parentId = !string.IsNullOrEmpty(node.Value) ? Convert.ToInt32(node.Value) : 0;

            var children = _categoryService.GetAllCategoriesByParentCategoryId(parentId, true).Select(x =>
                new TreeViewItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    LoadOnDemand = _categoryService.GetAllCategoriesByParentCategoryId(x.Id, true).Count > 0,
                    Enabled = true,
                    ImageUrl = Url.Content("~/Administration/Content/images/ico-content.png")
                });

            return new JsonResult { Data = children };
        }

        //ajax
        public ActionResult TreeDrop(int item, int destinationitem, string position)
        {
            var categoryItem = _categoryService.GetCategoryById(item);
            var categoryDestinationItem = _categoryService.GetCategoryById(destinationitem);

            switch (position)
            {
                case "over":
                    categoryItem.ParentCategoryNewsId = categoryDestinationItem.Id;
                    break;
                case "before":
                    categoryItem.ParentCategoryNewsId = categoryDestinationItem.ParentCategoryNewsId;
                    categoryItem.DisplayOrder = categoryDestinationItem.DisplayOrder - 1;
                    break;
                case "after":
                    categoryItem.ParentCategoryNewsId = categoryDestinationItem.ParentCategoryNewsId;
                    categoryItem.DisplayOrder = categoryDestinationItem.DisplayOrder + 1;
                    break;
            }

            _categoryService.UpdateCategory(categoryItem);

            return Json(new { success = true });
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            Permission();
            var model = new CategoryNewsModel();

            //parent categories
            model.ParentCategories = new List<DropDownItem> { new DropDownItem { Text = "[None]", Value = "0" } };
            //locales
            AddLocales(_languageService, model.Locales);
            //templates

            model.PageSize = 4;
            model.Published = true;

            model.AllowCustomersToSelectPageSize = true;
            model.PageSizeOptions = _catalogSettings.DefaultManufacturerPageSizeOptions;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Create(CategoryNewsModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                //2012-09-13 HUNGLAI ADD STT Set User Create this category
                if (_workContext.CurrentCustomer.Username == "" || _workContext.CurrentCustomer.Username == null)
                {
                    model.CreatedBy = _workContext.CurrentCustomer.Email;
                }
                else
                {
                    model.CreatedBy = _workContext.CurrentCustomer.Username;
                }
                model.UpdatedBy = "NON_UPDATE";
                //2012-09-13 HUNGLAI ADD END

                var category = model.ToEntity();
                category.CreatedOnUtc = DateTime.UtcNow;
                category.UpdatedOnUtc = DateTime.UtcNow;
                string SeName = category.ValidateSeName("", category.Name.RemoveSign4VietnameseString(), true);
                _urlRecordService.SaveSlug(category,SeName,0);
                _categoryService.InsertCategory(category);

                //locales
                UpdateLocales(category, model);

                _categoryService.UpdateCategory(category);

                //activity log
                _customerActivityService.InsertActivity("AddNewCategory", _localizationService.GetResource("ActivityLog.AddNewNewsCategory"), category.Name);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.Category.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = category.Id }) : RedirectToAction("List");
            }

            model.ParentCategories = new List<DropDownItem> { new DropDownItem { Text = "[None]", Value = "0" } };
            if (model.ParentCategoryNewsId > 0)
            {
                var parentCategory = _categoryService.GetCategoryById(model.ParentCategoryNewsId);
                if (parentCategory != null && !parentCategory.Deleted)
                    model.ParentCategories.Add(
                        new DropDownItem { 
                            Text = parentCategory.GetCategoryBreadCrumb(_categoryService),
                            Value = parentCategory.Id.ToString() });
                else
                    model.ParentCategoryNewsId = 0;
            }

            return View(model);
        }

        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            Permission();
            var category = _categoryService.GetCategoryById(id);

            if (category == null || category.Deleted)
                //No manufacturer found with the specified id
                return RedirectToAction("List");

            var model = category.ToModel();
            //parent categories
            model.ParentCategories = new List<DropDownItem> { new DropDownItem { Text = "[None]", Value = "0" } };
            if (model.ParentCategoryNewsId > 0)
            {
                var parentCategory = _categoryService.GetCategoryById(model.ParentCategoryNewsId);
                if (parentCategory != null && !parentCategory.Deleted)
                    model.ParentCategories.Add(new DropDownItem { 
                       // Text = parentCategory.GetCategoryBreadCrumb(_categoryService), 
                        Value = parentCategory.Id.ToString() });
                else
                    model.ParentCategoryNewsId = 0;
            }
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
               // locale.Name = category.GetLocalized(x => x.Name, languageId, false, false);
                //locale.MetaTitle = category.GetLocalized(x => x.MetaTitle, languageId, false, false);
            });
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Edit(CategoryNewsModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var category = _categoryService.GetCategoryById(model.Id);
            if (category == null || category.Deleted)
                //No manufacturer found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //2012-09-13 HUNGLAI ADD STT Update last User Update this category
              //  model.CreatedBy = category.CreatedOnUtc;
                // 03.12.2012 Chuyển sang dùng email thay vì user cập nhật cuối cùng trong hàm Edit()
                var email = _workContext.CurrentCustomer.Email;
                if (email == null)
                    email = "";
                model.UpdatedBy = email;
                ///2012-09-13 HUNGLAI ADD END
                category = model.ToEntity(category);
                category.UpdatedOnUtc = DateTime.UtcNow;
                string SeName = category.ValidateSeName(category.GetSeName(), Nop.Web.Framework.Extensions.RemoveSign4VietnameseString(category.Name), true);
                _urlRecordService.SaveSlug(category, SeName, 0);
                _categoryService.UpdateCategory(category);
                //locales
                UpdateLocales(category, model);

                //activity log
                _customerActivityService.InsertActivity("EditCategory", _localizationService.GetResource("ActivityLog.EditCategory1"), category.Name);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.Category.Updated"));
                return continueEditing ? RedirectToAction("Edit", category.Id) : RedirectToAction("List");
            }
            return View(model);
        }

        #endregion

        #region Deleted
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            Permission();
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                //No manufacturer found with the specified id
                return RedirectToAction("List");
            //2012-09-12 HUNGLAI ADD STT - Update User Delete
            //category.UpdatedBy = _workContext.CurrentCustomer.Email;
            //2012-09-12 HUNGLAI ADD END
            _categoryService.DeleteCategory(category);


            //activity log
            _customerActivityService.InsertActivity("DeleteCategory", _localizationService.GetResource("ActivityLog.DeleteCategory"), category.Name);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.Category.Deleted"));
            return RedirectToAction("List");
        }

        #endregion

        #region News
        /*
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult NewsList(GridCommand command, int category1Id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var newscategory = _categoryService.GetNewsCategoriesByCategoryId(category1Id,
                command.Page - 1, command.PageSize, true);

            var model = new GridModel<CategoryNewsModel.CategoryNewsModel>
            {
                Data = newscategory
                .Select(x =>
                {
                    return new CategoryNewsModel.CategoryNewsModel()
                    {
                        Id = x.Id,
                        CategoryNewsId = x.CategoryNewsId,
                        NewsId = x.NewsId,
                        NewsTitle = _newsService.GetNewsById(x.NewsId).Title,                       
                    };
                }),
                Total = newscategory.TotalCount
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsUpdate(GridCommand command, CategoryNewsModel.CategoryNewsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var newscategory = _categoryService.GetNewsCategoryById(model.Id);
            if (newscategory == null)
                throw new ArgumentException("No news category mapping found with the specified id");
            
            _categoryService.UpdateNewsCategory(newscategory);

            return NewsList(command, newscategory.CategoryNewsId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsDelete(int id, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var newscategory = _categoryService.GetNewsCategoryById(id);
            if (newscategory == null)
                throw new ArgumentException("No news category mapping found with the specified id");

            var category1Id = newscategory.CategoryNewsId;
            _categoryService.DeleteNewsCategory(newscategory);

            return NewsList(command, category1Id);
        }
        /*
        public ActionResult NewsAddPopup(int category1Id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var model = new Category1Model.AddCategoryNewsModel();
            model.NewsItem = new GridModel<NewsItemModel>
            {
                Data = products.Select(x => x.ToModel()),
                Total = products.TotalCount
            };
            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var c in _categoryService.GetAllCategories(true))
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetCategoryNameWithPrefix(_categoryService), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductAddPopupList(GridCommand command, ManufacturerModel.AddManufacturerProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var gridModel = new GridModel();
            IList<int> filterableSpecificationAttributeOptionIds = null;
            var products = _productService.SearchProducts(model.SearchCategoryId,
                model.SearchManufacturerId, null, null, null, 0, model.SearchProductName, false, false,
                _workContext.WorkingLanguage.Id, new List<int>(),
                ProductSortingEnum.Position, command.Page - 1, command.PageSize,
                false, out filterableSpecificationAttributeOptionIds, true);
            gridModel.Data = products.Select(x => x.ToModel());
            gridModel.Total = products.TotalCount;
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult ProductAddPopup(string btnId, string formId, ManufacturerModel.AddManufacturerProductModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            if (model.SelectedProductIds != null)
            {
                foreach (int id in model.SelectedProductIds)
                {
                    var product = _productService.GetProductById(id);
                    if (product != null)
                    {
                        var existingProductmanufacturers = _manufacturerService.GetProductManufacturersByManufacturerId(model.ManufacturerId, 0, int.MaxValue, true);
                        if (existingProductmanufacturers.FindProductManufacturer(id, model.ManufacturerId) == null)
                        {
                            _manufacturerService.InsertProductManufacturer(
                                new ProductManufacturer()
                                {
                                    ManufacturerId = model.ManufacturerId,
                                    ProductId = id,
                                    IsFeaturedProduct = false,
                                    DisplayOrder = 1
                                });
                        }
                    }
                }
            }

            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            model.Products = new GridModel<ProductModel>();
            return View(model);
        }
        */
        #endregion

        #region Permission

        public void Permission()
        {
            ViewBag.ManageCategoryNewsCreate = _permissionService.Authorize(StandardPermissionProvider.ManageNews);
            ViewBag.ManageCategoryNewsEdit = _permissionService.Authorize(StandardPermissionProvider.ManageNews);
            ViewBag.ManageCategoryNewsDelete = _permissionService.Authorize(StandardPermissionProvider.ManageNews);
            ViewBag.ManageCategoryNewsDetail = _permissionService.Authorize(StandardPermissionProvider.ManageNews);
        }

        #endregion
    }
}