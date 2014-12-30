using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using PlanX.Admin.Models.Catalog;
using PlanX.Admin.Models.News;
using PlanX.Core;
//using PlanX.Core.Domain.Catalog;
using PlanX.Core.Domain.Common;
using PlanX.Core.Domain.News;
//using PlanX.Services.Catalog;
//using PlanX.Services.ExportImport;
using PlanX.Services.Localization;
using PlanX.Services.Logging;
using PlanX.Services.Media;
using PlanX.Services.News;
using PlanX.Services.Security;
using PlanX.Web.Framework.Controllers;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using PlanX.Services.Seo;
using PlanX.Web.Framework;

namespace PlanX.Admin.Controllers
{
    [AdminAuthorize]
    public class CategoryNewsController : BaseNopController
    {
        #region Fields
        private readonly ICategoryNewsService _categoryService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly INewsService _newsService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly NewsSettings _newsSetting;

        #endregion

        #region Contructor
        public CategoryNewsController(ICategoryNewsService categoryService,
            IPictureService pictureService, ILanguageService languageService,
            ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
            IWorkContext workContext,
            ICustomerActivityService customerActivityService, IPermissionService permissionService,
            AdminAreaSettings adminAreaSettings, INewsService newsService, IUrlRecordService urlRecordService,
            NewsSettings newsSetting)
        {
            this._urlRecordService = urlRecordService;
            this._categoryService = categoryService;
            this._pictureService = pictureService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._workContext = workContext;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._adminAreaSettings = adminAreaSettings;
            this._newsService = newsService;
            this._newsSetting = newsSetting;
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
            var model = new CategoryNewsModel();

            //parent categories
            model.ParentCategories = new List<DropDownItem> { new DropDownItem { Text = "[None]", Value = "0" } };

            model.PageSize = _newsSetting.MainPageNewsCount;
            model.Published = true;

            model.AllowCustomersToSelectPageSize = true;
            model.PageSizeOptions = _newsSetting.MainPageNewsCount.ToString();

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
                category.CreatedOnUtc = DateTime.Now;
                category.UpdatedOnUtc = DateTime.Now;
                category.NewsCount = 0;
                string SeName = category.ValidateSeName("", category.Name, true);
                _categoryService.InsertCategory(category);
                _urlRecordService.SaveSlug(category, SeName, 0);
                _categoryService.UpdateCategory(category);

                //activity log
                _customerActivityService.InsertActivity("AddCategoryNews", _localizationService.GetResource("ActivityLog.AddNewCategoryNews"), string.Format("{0}-{1}",category.Id,category.Name));

                SuccessNotification(_localizationService.GetResource("Admin.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = category.Id }) : RedirectToAction("List");
            }

            model.ParentCategories = new List<DropDownItem> { new DropDownItem { Text = "[None]", Value = "0" } };
            if (model.ParentCategoryNewsId > 0)
            {
                var parentCategory = _categoryService.GetCategoryById(model.ParentCategoryNewsId);
                if (parentCategory != null && !parentCategory.Deleted)
                    model.ParentCategories.Add(
                        new DropDownItem
                        {
                            Text = parentCategory.GetCategoryBreadCrumb(_categoryService),
                            Value = parentCategory.Id.ToString()
                        });
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
                    model.ParentCategories.Add(new DropDownItem
                    {
                        // Text = parentCategory.GetCategoryBreadCrumb(_categoryService), 
                        Value = parentCategory.Id.ToString()
                    });
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
                category.UpdatedOnUtc = DateTime.Now;
                string SeName = category.ValidateSeName(category.GetSeName(), category.Name, true);
                _urlRecordService.SaveSlug(category, SeName, 0);
                _categoryService.UpdateCategory(category);

                //activity log
                _customerActivityService.InsertActivity("EditCategoryNews", _localizationService.GetResource("ActivityLog.EditCategoryNew"), string.Format("{0}-{1}", category.Id, category.Name));

                SuccessNotification(_localizationService.GetResource("Admin.Updated"));
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
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                //No manufacturer found with the specified id
                return RedirectToAction("List");
            _categoryService.DeleteCategory(category);

            //activity log
            _customerActivityService.InsertActivity("DeleteCategoryNews", _localizationService.GetResource("ActivityLog.DeleteCategoryNews"), string.Format("{0}-{1}",category.Id, category.Name));

            SuccessNotification(_localizationService.GetResource("Admin.Deleted"));
            return RedirectToAction("List");
        }

        #endregion
    }
}