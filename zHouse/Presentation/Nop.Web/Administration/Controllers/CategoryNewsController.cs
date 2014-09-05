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
                    categoryModel.Breadcrumb = x.GetCategoryBreadCrumb(_categoryService);
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
                if (_workContext.CurrentCustomer.Username == "" || _workContext.CurrentCustomer.Username == null)
                {
                    model.CreatedBy = _workContext.CurrentCustomer.Email;
                }
                else
                {
                    model.CreatedBy = _workContext.CurrentCustomer.Username;
                }
                model.UpdatedBy = "NON_UPDATE";
                var category = model.ToEntity();
                category.CreatedOnUtc = DateTime.UtcNow;
                category.UpdatedOnUtc = DateTime.UtcNow;
                string SeName = category.ValidateSeName("", category.Name.RemoveSign4VietnameseString(), true);
                _urlRecordService.SaveSlug(category,SeName,0);
                _categoryService.InsertCategory(category);

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
                var email = _workContext.CurrentCustomer.Email;
                if (email == null)
                    email = "";
                model.UpdatedBy = email;
                category = model.ToEntity(category);
                category.UpdatedOnUtc = DateTime.UtcNow;
                string SeName = category.ValidateSeName(category.GetSeName(), Nop.Web.Framework.Extensions.RemoveSign4VietnameseString(category.Name), true);
                _urlRecordService.SaveSlug(category, SeName, 0);
                _categoryService.UpdateCategory(category);

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
            _categoryService.DeleteCategory(category);

            //activity log
            _customerActivityService.InsertActivity("DeleteCategory", _localizationService.GetResource("ActivityLog.DeleteCategory"), category.Name);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.Category.Deleted"));
            return RedirectToAction("List");
        }

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