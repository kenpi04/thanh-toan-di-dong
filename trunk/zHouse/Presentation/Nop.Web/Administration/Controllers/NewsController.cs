using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Models.News;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.News;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.News;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;
using Nop.Core.Domain.Stores;
using Nop.Services.Catalog;

namespace Nop.Admin.Controllers
{
	[AdminAuthorize]
    public partial class NewsController : BaseNopController
	{
		#region Fields

        private readonly INewsService _newsService;
        private readonly ILanguageService _languageService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IStoreService _storeService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryNewsService _categoryNewsService;
        private readonly ICateNewsService _cateNewsService;
        private readonly IPictureService _pictureService;
        private readonly AdminAreaSettings _adminAreaSettings;
        
		#endregion

		#region Constructors

        public NewsController(INewsService newsService, ICategoryService categoryService,
            ILanguageService languageService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            IStoreService storeService, 
            IStoreMappingService storeMappingService,
            ICategoryNewsService categoryNewsService,
            IPictureService pictureService,
            AdminAreaSettings adminAreaSettings, ICateNewsService cateNewsService
            )
        {
            this._categoryService = categoryService;
            this._newsService = newsService;
            this._languageService = languageService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._urlRecordService = urlRecordService;
            this._storeService = storeService;
            this._storeMappingService = storeMappingService;
            this._cateNewsService = cateNewsService;
            this._categoryNewsService = categoryNewsService;
            this._pictureService = pictureService;
            this._adminAreaSettings = adminAreaSettings;
		}

		#endregion 

        #region Utilities

        [NonAction]
        private void PrepareStoresMappingModel(NewsItemModel model, NewsItem newsItem, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableStores = _storeService
                .GetAllStores()
                .Select(s => s.ToModel())
                .ToList();
            if (!excludeProperties)
            {
                if (newsItem != null)
                {
                    model.SelectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(newsItem);
                }
                else
                {
                    model.SelectedStoreIds = new int[0];
                }
            }
        }

        [NonAction]
        protected void SaveStoreMappings(NewsItem newsItem, NewsItemModel model)
        {
            var existingStoreMappings = _storeMappingService.GetStoreMappings(newsItem);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds != null && model.SelectedStoreIds.Contains(store.Id))
                {
                    //new role
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(newsItem, store.Id);
                }
                else
                {
                    //removed role
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }


        #endregion

        #region News items

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //public ActionResult List()
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
        //        return AccessDeniedView();

        //    var model = new NewsItemListModel();
        //    //stores
        //    model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    foreach (var s in _storeService.GetAllStores())
        //        model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });

        //    return View(model);
        //}

        //[HttpPost, GridAction(EnableCustomBinding = true)]
        //public ActionResult List(GridCommand command, NewsItemListModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
        //        return AccessDeniedView();

        //    var news = _newsService.GetAllNews(0, model.SearchStoreId, command.Page - 1, command.PageSize, true);
        //    var gridModel = new GridModel<NewsItemModel>
        //    {
        //        Data = news.Select(x =>
        //        {
        //            var m = x.ToModel();
        //            if (x.StartDateUtc.HasValue)
        //                m.StartDate = _dateTimeHelper.ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
        //            if (x.EndDateUtc.HasValue)
        //                m.EndDate = _dateTimeHelper.ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
        //            m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
        //            m.LanguageName = x.Language.Name;
        //            m.Comments = x.CommentCount;
        //            return m;
        //        }),
        //        Total = news.TotalCount
        //    };
        //    return new JsonResult
        //    {
        //        Data = gridModel
        //    };
        //}

        //public ActionResult Create()
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
        //        return AccessDeniedView();

        //    ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
        //    var model = new NewsItemModel();
        //    //Stores
        //    PrepareStoresMappingModel(model, null, false);
        //    //default values
        //    model.Published = true;
        //    model.AllowComments = true;
        //    return View(model);
        //}

        //[HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        //public ActionResult Create(NewsItemModel model, bool continueEditing)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
        //        return AccessDeniedView();

        //    if (ModelState.IsValid)
        //    {
        //        var newsItem = model.ToEntity();
        //        newsItem.StartDateUtc = model.StartDate;
        //        newsItem.EndDateUtc = model.EndDate;
        //        newsItem.CreatedOnUtc = DateTime.UtcNow;
        //        _newsService.InsertNews(newsItem);
                
        //        //search engine name
        //        var seName = newsItem.ValidateSeName(model.SeName, model.Title, true);
        //        _urlRecordService.SaveSlug(newsItem, seName, newsItem.LanguageId);

        //        //Stores
        //        SaveStoreMappings(newsItem, model);

        //        SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Added"));
        //        return continueEditing ? RedirectToAction("Edit", new { id = newsItem.Id }) : RedirectToAction("List");
        //    }

        //    //If we got this far, something failed, redisplay form
        //    ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
        //    //Stores
        //    PrepareStoresMappingModel(model, null, true);
        //    return View(model);
        //}

        //public ActionResult Edit(int id)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
        //        return AccessDeniedView();

        //    var newsItem = _newsService.GetNewsById(id);
        //    if (newsItem == null)
        //        //No news item found with the specified id
        //        return RedirectToAction("List");

        //    ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
        //    var model = newsItem.ToModel();
        //    model.StartDate = newsItem.StartDateUtc;
        //    model.EndDate = newsItem.EndDateUtc;
        //    //Store
        //    PrepareStoresMappingModel(model, newsItem, false);
        //    return View(model);
        //}

        //[HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        //public ActionResult Edit(NewsItemModel model, bool continueEditing)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
        //        return AccessDeniedView();

        //    var newsItem = _newsService.GetNewsById(model.Id);
        //    if (newsItem == null)
        //        //No news item found with the specified id
        //        return RedirectToAction("List");

        //    if (ModelState.IsValid)
        //    {
        //        newsItem = model.ToEntity(newsItem);
        //        newsItem.StartDateUtc = model.StartDate;
        //        newsItem.EndDateUtc = model.EndDate;
        //        _newsService.UpdateNews(newsItem);

        //        //search engine name
        //        var seName = newsItem.ValidateSeName(model.SeName, model.Title, true);
        //        _urlRecordService.SaveSlug(newsItem, seName, newsItem.LanguageId);

        //        //Stores
        //        SaveStoreMappings(newsItem, model);

        //        SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));
        //        return continueEditing ? RedirectToAction("Edit", new { id = newsItem.Id }) : RedirectToAction("List");
        //    }

        //    //If we got this far, something failed, redisplay form
        //    ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
        //    //Store
        //    PrepareStoresMappingModel(model, newsItem, true);
        //    return View(model);
        //}

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var newsItem = _newsService.GetNewsById(id);
            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");

            _newsService.DeleteNews(newsItem);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Deleted"));
            return RedirectToAction("List");
        }

        #endregion

        #region Comments

        public ActionResult Comments(int? filterByNewsItemId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            ViewBag.FilterByNewsItemId = filterByNewsItemId;
            return View();
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Comments(int? filterByNewsItemId, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            IList<NewsComment> comments;
            if (filterByNewsItemId.HasValue)
            {
                //filter comments by news item
                var newsItem = _newsService.GetNewsById(filterByNewsItemId.Value);
                comments = newsItem.NewsComments.OrderBy(bc => bc.CreatedOnUtc).ToList();
            }
            else
            {
                //load all news comments
                comments = _newsService.GetAllComments(0);
            }

            var gridModel = new GridModel<NewsCommentModel>
            {
                Data = comments.PagedForCommand(command).Select(newsComment =>
                {
                    var commentModel = new NewsCommentModel();
                    commentModel.Id = newsComment.Id;
                    commentModel.NewsItemId = newsComment.NewsItemId;
                    commentModel.NewsItemTitle = newsComment.NewsItem.Title;
                    commentModel.CustomerId = newsComment.CustomerId;
                    commentModel.IsApproved = newsComment.IsApproved;
                    commentModel.AppName = newsComment.AppName;
                    var customer = newsComment.Customer;
                    commentModel.CustomerInfo = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest");
                    commentModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(newsComment.CreatedOnUtc, DateTimeKind.Utc);
                    commentModel.CommentTitle = newsComment.CommentTitle;
                    commentModel.CommentText = Core.Html.HtmlHelper.FormatText(newsComment.CommentText, false, true, false, false, false, false);
                    return commentModel;
                }),
                Total = comments.Count,
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult CommentDelete(int? filterByNewsItemId, int id, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var comment = _newsService.GetNewsCommentById(id);
            if (comment == null)
                throw new ArgumentException("No comment found with the specified id");

            var newsItem = comment.NewsItem;
            _newsService.DeleteNewsComment(comment);
            //update totals
            newsItem.CommentCount = newsItem.NewsComments.Count;
            _newsService.UpdateNews(newsItem);

            return Comments(filterByNewsItemId, command);
        }

        [HttpPost]
        public ActionResult ApproveSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                foreach (var id in selectedIds)
                {
                    var comment = _newsService.GetNewsCommentById(id);
                    var newsItem = comment.NewsItem;
                    if (comment != null)
                    {
                        comment.IsApproved = true;
                        _newsService.UpdateNews(newsItem);
                    }
                }
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult DisapproveSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                foreach (var id in selectedIds)
                {
                    var comment = _newsService.GetNewsCommentById(id);
                    var newsItem = comment.NewsItem;
                    if (comment != null)
                    {
                        comment.IsApproved = false;
                        _newsService.UpdateNews(newsItem);
                    }
                }
            }

            return Json(new { Result = true });
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Update NewsPicture
        /// 26/09/2012  - TrucNHT@fpt.com.vn                 
        /// </summary>

        [NonAction]
        protected void UpdatePictureSeoNames(NewsItem newsItem )
        {
            var picture = _pictureService.GetPictureById(Convert.ToInt32( newsItem.PictureId));
            if (picture != null)
                _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(newsItem.Title));
        }

        #endregion

        #region News Category

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCategoryList(GridCommand command, int newsId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var newsCategories = _categoryNewsService.GetNewsCategoriesByNewsId(newsId, true);
            var newsCategoriesModel = newsCategories
                .Select(x =>
                {
                    return new NewsItemModel.CategoryNewsMapModel()
                    {
                        Id = x.Id,
                        Category = _categoryNewsService.GetCategoryById(x.CategoryNewsId).GetCategoryBreadCrumb(_categoryNewsService),
                        NewsId = x.NewsId,
                        CategoryNewsId = x.CategoryNewsId,
                    };
                })
                .ToList();

            var model = new GridModel<NewsItemModel.CategoryNewsMapModel>
            {
                Data = newsCategoriesModel,
                Total = newsCategoriesModel.Count
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCategoryInsert(GridCommand command, NewsItemModel.CategoryNewsMapModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            
            var newsCategory = new NewsCategoryNews()
            {
                NewsId = model.NewsId,
                CategoryNewsId = Int32.Parse(model.Category), //use Category property (not CategoryId) because appropriate property is stored in it

            };

            _categoryNewsService.InsertNewsCategory(newsCategory);

            return NewsCategoryList(command, model.NewsId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCategoryUpdate(GridCommand command, NewsItemModel.CategoryNewsMapModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            
            var newsCategory = _categoryNewsService.GetNewsCategoryById(model.Id);
            if (newsCategory == null)
                throw new ArgumentException("No news category mapping found with the specified id");

            //use Category property (not CategoryId) because appropriate property is stored in it
            newsCategory.CategoryNewsId = Int32.Parse(model.Category);

            _categoryNewsService.UpdateNewsCategory(newsCategory);

            return NewsCategoryList(command, newsCategory.NewsId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCategoryDelete(int id, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
          
            var newsCategory = _categoryNewsService.GetNewsCategoryById(id);
            if (newsCategory == null)
                throw new ArgumentException("No news category mapping found with the specified id");

            var newsId = newsCategory.NewsId;
            _categoryNewsService.DeleteNewsCategory(newsCategory);

            return NewsCategoryList(command, newsId);
        }

        #endregion

        #region News items

        public void PrepaireNumberOfAvailableCategories(NewsItemModel model)
        {
            model.NumberOfAvailableCategories = _categoryNewsService.GetAllCategories(false).Count;
        }
        

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = new NewsItemModel();
            //default values
            model.Published = true;
            model.AllowComments = true;
            // 2012-09-10 Hung lai STT
            model.NumberOfAvailableCategories = _categoryNewsService.GetAllCategories(false).Count;
            // 2012-09-10 Hung lai END            
            return View(model);
        }

        //[HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        //public ActionResult Create(NewsItemModel model, bool continueEditing)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
        //        return AccessDeniedView();

        //    if (ModelState.IsValid)
        //    {

        //        var newsItem = model.ToEntity();
        //        newsItem.StartDateUtc = model.StartDate;
        //        newsItem.EndDateUtc = model.EndDate;
        //        newsItem.CreatedOnUtc = DateTime.UtcNow;
        //        _newsService.InsertNews(newsItem);

        //        SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Added"));
        //        return continueEditing ? RedirectToAction("Edit", new { id = newsItem.Id }) : RedirectToAction("List");
        //    }
        //    // 2012-09-10 Hung lai STT
        //    model.NumberOfAvailableCategories = _categoryService.GetAllCategories(false).Count;
        //    // 2012-09-10 Hung lai END            
        //    //If we got this far, something failed, redisplay form
        //    ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
        //    return View(model);
        //}

       

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var newsItem = _newsService.GetNewsById(id);
            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");

            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = newsItem.ToModel();
            
            //vudh nop280 sename
            // model.SeName = newsItem.GetSeName();
            model.SeName = newsItem.GetSeName(newsItem.LanguageId, ensureTwoPublishedLanguages: false);
            model.LimitedToStores = newsItem.LimitedToStores;
            model.StartDate = newsItem.StartDateUtc;
            model.EndDate = newsItem.EndDateUtc;
            // 2012-09-10 Hung lai STT
            PrepaireNumberOfAvailableCategories(model);
            // 2012-09-10 Hung lai END
            return View(model);
        }

        // <summary>
        /// Update NewsPicture
        /// 26/09/2012  - TrucNHT@fpt.com.vn      -- Lấy thêm giá trị ProductId           
        /// </summary>
        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Edit(NewsItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var newsItem = _newsService.GetNewsById(model.Id);
            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");

                int prevPictureId =Convert.ToInt32( newsItem.PictureId);
                newsItem = model.ToEntity(newsItem);
                newsItem.StartDateUtc = model.StartDate;
                newsItem.EndDateUtc = model.EndDate;
                newsItem.IsMainNews = model.IsMainNews;

                 if (!newsItem.LimitedToStores)
                {
                    newsItem.LimitedToStores = true;
                    var store = _storeService.GetAllStores().FirstOrDefault();
                    _storeMappingService.InsertStoreMapping(new StoreMapping { EntityId = newsItem.Id, StoreId = store.Id, EntityName = "NewsItem" });
                }
                _newsService.UpdateNews(newsItem);
                
                //delete old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != newsItem.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }
                //update picture seo file name
                UpdatePictureSeoNames(newsItem);

                //search engine name
                var seName = newsItem.ValidateSeName(model.SeName,Nop.Web.Framework.Extensions.RemoveSign4VietnameseString(model.Title), true);
                _urlRecordService.SaveSlug(newsItem, seName, newsItem.LanguageId);
               
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = newsItem.Id }) : RedirectToAction("List");

        }

        #endregion

        #region News CategoryPr

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCateList(GridCommand command, int newsId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
           
            var newsCategories = _cateNewsService.GetNewsCategoriesByNewsId(newsId, true);
            var newsCategoriesModel = newsCategories
                .Select(x =>
                {
                    return new NewsItemModel.CateNewsMapModel()
                    {
                        Id = x.Id,
                        NewsCate = _categoryService.GetCategoryById(x.CategoryId).GetFormattedBreadCrumb(_categoryService),
                        NewsId = x.NewsId,
                        CategoryId = x.CategoryId,
                    };
                })
                .ToList();

            var model = new GridModel<NewsItemModel.CateNewsMapModel>
            {
                Data = newsCategoriesModel,
                Total = newsCategoriesModel.Count
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCateInsert(GridCommand command, NewsItemModel.CateNewsMapModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
           
            var newsCategory = new NewsCategory()
            {
                NewsId = model.NewsId,
                CategoryId = Int32.Parse(model.NewsCate), //use Category property (not CategoryId) because appropriate property is stored in it

            };

            _cateNewsService.InsertNewsCategory(newsCategory);

            return NewsCateList(command, model.NewsId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCateUpdate(GridCommand command, NewsItemModel.CateNewsMapModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
         
            var newsCategory = _cateNewsService.GetNewsCategoryById(model.Id);
            if (newsCategory == null)
                throw new ArgumentException("No news category mapping found with the specified id");

            //use Category property (not CategoryId) because appropriate property is stored in it
            newsCategory.CategoryId = Int32.Parse(model.NewsCate);

            _cateNewsService.UpdateNewsCategory(newsCategory);

            return NewsCateList(command, newsCategory.NewsId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCateDelete(int id, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
           
            var newsCategory = _cateNewsService.GetNewsCategoryById(id);
            if (newsCategory == null)
                throw new ArgumentException("No news category mapping found with the specified id");

            var newsId = newsCategory.NewsId;
            _cateNewsService.DeleteNewsCategory(newsCategory);

            return NewsCateList(command, newsId);
        }

        #endregion

        //vudh nop280 sename
        //lưu news, và lưu sename vào tbl urlrecord
        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Create(NewsItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var newsItem = model.ToEntity();
                newsItem.StartDateUtc = model.StartDate;
                newsItem.EndDateUtc = model.EndDate;
                newsItem.CreatedOnUtc = DateTime.UtcNow;
                newsItem.CommentCount = 0;
                newsItem.LimitedToStores = true;
                newsItem.IsMainNews = model.IsMainNews;
                _newsService.InsertNews(newsItem);


                //search engine name
                var seName = newsItem.ValidateSeName(model.SeName,Nop.Web.Framework.Extensions.RemoveSign4VietnameseString(model.Title), true);
                _urlRecordService.SaveSlug(newsItem, seName, newsItem.LanguageId);

                //var store = _storeService.GetAllStores().FirstOrDefault();
                //_storeMappingService.InsertStoreMapping(new StoreMapping { EntityId = newsItem.Id, StoreId=store.Id, EntityName = "NewsItem" });

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = newsItem.Id }) : RedirectToAction("List");
            }
            // 2012-09-10 Hung lai STT
            PrepaireNumberOfAvailableCategories(model);
            // 2012-09-10 Hung lai END
            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            return View(model);
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
          
            var store = _storeService.GetAllStores().FirstOrDefault();
            var news = _newsService.GetAllNews(0, store.Id, 0, _adminAreaSettings.GridPageSize, true);
            var gridModel = new GridModel<NewsItemModel>
            {
                Data = news.Select(x =>
                {
                    var m = x.ToModel();
                    if (x.StartDateUtc.HasValue)
                        m.StartDate = _dateTimeHelper.ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
                    if (x.EndDateUtc.HasValue)
                        m.EndDate = _dateTimeHelper.ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
                    m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                    m.LanguageName = x.Language.Name;
                    m.Comments = x.ApprovedCommentCount + x.NotApprovedCommentCount;
                    return m;
                }),
                Total = news.TotalCount
            };
            var mode = new NewsItemListModel();
            mode.List_NewsItemModel = gridModel;

            var mylist = new List<SelectListItem>();

            var NameCategories = _categoryNewsService.GetAll();

            mylist.Add(new SelectListItem { Text = "Tất cả", Value = "0", Selected = true });

            foreach (var Names in NameCategories)
            {
                mylist.Add(new SelectListItem { Text = Names.Name, Value = Names.Id.ToString() });
            }
            ViewBag.ListNames = mylist;

            return View(mode);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, NewsItemListModel searchModel)
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
        
            var store = _storeService.GetAllStores().FirstOrDefault();
            var news = _newsService.GetAllNews(0, 0, command.Page - 1, command.PageSize, true);
            var gridModel = new GridModel<NewsItemModel>();

            if (searchModel.SearchTileName == null && searchModel.SearchCategoryName != 0)
            {
                var news1 = _newsService.GetpageGetNewsSeachByTile(0, command.Page - 1, command.PageSize, searchModel.SearchCategoryName);
                var gridModel1 = new GridModel<NewsItemModel>
                {
                    Data = news1.Select(x =>
                    {
                        var m = x.ToModel();
                        if (x.StartDateUtc.HasValue)
                            m.StartDate = _dateTimeHelper.ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
                        if (x.EndDateUtc.HasValue)
                            m.EndDate = _dateTimeHelper.ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
                        m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                        m.LanguageName = x.Language.Name;
                        m.Comments = x.ApprovedCommentCount + x.NotApprovedCommentCount;
                        return m;
                    }),
                    Total = news1.TotalCount
                };
                gridModel = gridModel1;
            }
            else
            {
                if (searchModel.SearchTileName != null)
                {
                    if (searchModel.SearchCategoryName == 0)
                    {
                        var news1 = _newsService.GetpageGetNewsSeachByKey(0, command.Page - 1, command.PageSize, searchModel.SearchTileName);
                        var gridModel1 = new GridModel<NewsItemModel>
                        {
                            Data = news1.Select(x =>
                            {
                                var m = x.ToModel();
                                if (x.StartDateUtc.HasValue)
                                    m.StartDate = _dateTimeHelper.ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
                                if (x.EndDateUtc.HasValue)
                                    m.EndDate = _dateTimeHelper.ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
                                m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                                m.LanguageName = x.Language.Name;
                                m.Comments = x.ApprovedCommentCount + x.NotApprovedCommentCount;
                                return m;
                            }),
                            Total = news1.TotalCount
                        };
                        gridModel = gridModel1;
                    }

                    else
                    {
                        var news2 = _newsService.GetpageGetNewsSeachByKeyandTile(0, command.Page - 1, command.PageSize, searchModel.SearchTileName, searchModel.SearchCategoryName);
                        var gridModel2 = new GridModel<NewsItemModel>
                        {
                            Data = news2.Select(x =>
                            {
                                var m = x.ToModel();
                                if (x.StartDateUtc.HasValue)
                                    m.StartDate = _dateTimeHelper.ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
                                if (x.EndDateUtc.HasValue)
                                    m.EndDate = _dateTimeHelper.ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
                                m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                                m.LanguageName = x.Language.Name;
                                m.Comments = x.ApprovedCommentCount + x.NotApprovedCommentCount;
                                return m;
                            }),

                            Total = news2.TotalCount
                        };
                        gridModel = gridModel2;
                    }
                }

                else
                {
                    var gridModel3 = new GridModel<NewsItemModel>
                    {
                        Data = news.Select(x =>
                        {
                            var m = x.ToModel();
                            if (x.StartDateUtc.HasValue)
                                m.StartDate = _dateTimeHelper.ConvertToUserTime(x.StartDateUtc.Value, DateTimeKind.Utc);
                            if (x.EndDateUtc.HasValue)
                                m.EndDate = _dateTimeHelper.ConvertToUserTime(x.EndDateUtc.Value, DateTimeKind.Utc);
                            m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                            m.LanguageName = x.Language.Name;
                            m.Comments = x.ApprovedCommentCount + x.NotApprovedCommentCount;
                            return m;
                        }),
                        Total = news.TotalCount
                    };
                    gridModel = gridModel3;

                }
            }

            return new JsonResult
            {
                Data = gridModel
            };
        }
     }
}
