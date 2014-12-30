using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PlanX.Admin.Models.News;
using PlanX.Core.Domain.Customers;
using PlanX.Core.Domain.News;
using PlanX.Services.Helpers;
using PlanX.Services.Localization;
using PlanX.Services.News;
using PlanX.Services.Security;
using PlanX.Services.Seo;
using PlanX.Services.Stores;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Controllers;
using Telerik.Web.Mvc;
using PlanX.Services.Logging;

namespace PlanX.Admin.Controllers
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
        private readonly ICategoryNewsService _cateNewsService;
        private readonly ITagService _tagService;
        private readonly ICustomerActivityService _customerActivityService;
        #endregion

        #region Constructors

        public NewsController(INewsService newsService,
            ILanguageService languageService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            IStoreService storeService,
            IStoreMappingService storeMappingService,
            ICategoryNewsService cateNewsService,
            ITagService tagService,
            ICustomerActivityService customerActivityService
            )
        {
            this._newsService = newsService;
            this._languageService = languageService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._urlRecordService = urlRecordService;
            this._storeService = storeService;
            this._storeMappingService = storeMappingService;
            this._cateNewsService = cateNewsService;
            this._tagService = tagService;
            this._customerActivityService = customerActivityService;
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
            model.NumberOfAvailableCategories = _cateNewsService.GetAllCategories().Count();
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

        [NonAction]
        private string[] ParseNewsTags(string tagsString)
        {
            var result = new List<string>();
            if (!String.IsNullOrWhiteSpace(tagsString))
            {
                string[] values = tagsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string val1 in values)
                    if (!String.IsNullOrEmpty(val1.Trim()))
                        result.Add(val1.Trim());
            }
            return result.ToArray();
        }
        [NonAction]
        private void SaveNewsTags(NewsItem newsItem, string[] newsTags)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsitem");

            //news tags
            var existingNewsTags = newsItem.Tags.ToList();
            var newsTagsToRemove = new List<Tag>();
            foreach (var existingNewsTag in existingNewsTags)
            {
                bool found = false;
                foreach (string newNewsTag in newsTags)
                {
                    if (existingNewsTag.Name.Equals(newNewsTag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    newsTagsToRemove.Add(existingNewsTag);
                }
            }
            foreach (var tag in newsTagsToRemove)
            {
                newsItem.Tags.Remove(tag);
                _newsService.UpdateNews(newsItem);
            }
            foreach (string tagName in newsTags)
            {
                Tag tag = null;
                var tag2 = _tagService.GetTagByName(tagName);
                if (tag2 == null)
                {
                    //add new tag
                    tag = new Tag()
                    {
                        Name = tagName,
                        EnglishName = tagName.RemoveSign4VietnameseString()
                    };
                    _tagService.InsertTag(tag);
                }
                else
                {
                    tag = tag2;
                }
                if (!newsItem.TagExists(tag.Id))
                {
                    newsItem.Tags.Add(tag);
                    _newsService.UpdateNews(newsItem);
                }
            }
        }

        [NonAction]
        private NewsItemModel PrepareNewsItemModel(NewsItem newsItem)
        {
            var model = new NewsItemModel();

            if (newsItem != null)
            {
                model = newsItem.ToModel();
                model.StartDate = newsItem.StartDateUtc;
                model.EndDate = newsItem.EndDateUtc;
                model.MetaDescription = newsItem.MetaDescription;
                model.MetaKeywords = newsItem.MetaKeywords;
                model.MetaTitle = model.MetaTitle;
                if (string.IsNullOrEmpty(model.MetaDescription))
                    newsItem.MetaDescription = model.Title;
                if (string.IsNullOrEmpty(newsItem.MetaTitle))
                    newsItem.MetaTitle = model.Title;

                //tags
                var result = new System.Text.StringBuilder();
                for (int i = 0; i < newsItem.Tags.Count; i++)
                {
                    var pt = newsItem.Tags.ToList()[i];
                    result.Append(pt.Name);
                    if (i != newsItem.Tags.Count - 1)
                        result.Append(", ");
                }
                model.TagsString = result.ToString();
            }
            return model;
        }

        #endregion

        #region News items

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var model = new NewsItemListModel();
            //stores
            model.AvailableStores.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString() });
            var cates = _cateNewsService.GetAllCategories(false).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.ListCates = cates;
            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, NewsItemListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var news = _newsService.GetAllNews(0, model.SearchStoreId, command.Page - 1, command.PageSize, true);
            var gridModel = new GridModel<NewsItemModel>
            {
                Data = news.Select(x =>
                {
                    var m = x.ToModel();
                    if (x.StartDateUtc.HasValue)
                        m.StartDate = x.StartDateUtc.Value;
                    if (x.EndDateUtc.HasValue)
                        m.EndDate = x.EndDateUtc.Value;
                    m.CreatedOn = x.CreatedOnUtc;
                    m.LanguageName = x.Language.Name;
                    m.Comments = x.CommentCount;
                    return m;
                }),
                Total = news.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = new NewsItemModel();
            //Stores
            PrepareStoresMappingModel(model, null, false);
            //default values
            model.Published = true;
            model.AllowComments = true;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(NewsItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var newsItem = model.ToEntity();
                newsItem.StartDateUtc = model.StartDate;
                newsItem.EndDateUtc = model.EndDate;
                newsItem.CreatedOnUtc = DateTime.Now;
                newsItem.MetaDescription = model.MetaDescription;
                newsItem.MetaKeywords = model.MetaKeywords;
                newsItem.MetaTitle = model.MetaTitle;
                if (string.IsNullOrEmpty(model.MetaDescription))
                    newsItem.MetaDescription = model.Title;
                if (string.IsNullOrEmpty(newsItem.MetaTitle))
                    newsItem.MetaTitle = model.Title;
                _newsService.InsertNews(newsItem);

                //search engine name
                var seName = newsItem.ValidateSeName(model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(newsItem, seName, 0);

                //Stores
                SaveStoreMappings(newsItem, model);
                //save tags
                SaveNewsTags(newsItem, ParseNewsTags(model.TagsString));

                _customerActivityService.InsertActivity("AddNewNewsItem", _localizationService.GetResource("ActivityLog.AddNewNewsItem"), newsItem.Id.ToString() + ":" + newsItem.Title);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = newsItem.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            model = PrepareNewsItemModel(null);
            //Stores
            PrepareStoresMappingModel(model, null, true);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var newsItem = _newsService.GetNewsById(id);
            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");

            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = PrepareNewsItemModel(newsItem);
            //Store
            PrepareStoresMappingModel(model, newsItem, false);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(NewsItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var newsItem = _newsService.GetNewsById(model.Id);
            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                newsItem = model.ToEntity(newsItem);
                newsItem.StartDateUtc = model.StartDate;
                newsItem.EndDateUtc = model.EndDate;
                newsItem.MetaDescription = model.MetaDescription;
                newsItem.MetaKeywords = model.MetaKeywords;
                newsItem.MetaTitle = model.MetaTitle;
                _newsService.UpdateNews(newsItem);

                //search engine name
                var seName = newsItem.ValidateSeName(model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(newsItem, seName, 0);

                //Stores
                SaveStoreMappings(newsItem, model);
                //save tags
                SaveNewsTags(newsItem, ParseNewsTags(model.TagsString));

                _customerActivityService.InsertActivity("EditNewsItem", _localizationService.GetResource("ActivityLog.EditNewsItem"), newsItem.Id.ToString() + ":" + newsItem.Title);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();
                    return RedirectToAction("Edit", new { id = newsItem.Id });
                }
                else
                {
                    return RedirectToAction("List");
                }
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            model = PrepareNewsItemModel(newsItem);
            //Store
            PrepareStoresMappingModel(model, newsItem, true);
            return View(model);
        }

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

            _customerActivityService.InsertActivity("DeletedNewsItem", _localizationService.GetResource("ActivityLog.DeletedNewsItem"), newsItem.Id.ToString() + ":" + newsItem.Title);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Deleted"));
            return RedirectToAction("List");
        }

        #endregion

        #region News Category

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult NewsCateList(GridCommand command, int newsId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var newsCategories = _cateNewsService.GetNewsCategoriesByNewsId(newsId, false);
            var newsCategoriesModel = newsCategories
                .Select(x =>
                {
                    return new NewsItemModel.CateNewsMapModel()
                    {
                        Id = x.Id,
                        NewsCate = _cateNewsService.GetCategoryById(x.CategoryNewsId).GetCategoryBreadCrumb(_cateNewsService),
                        NewsId = x.NewsId,
                        CategoryId = x.CategoryNewsId,
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

            var newsCategory = new NewsCategoryNews()
            {
                NewsId = model.NewsId,
                CategoryNewsId = Int32.Parse(model.NewsCate), //use Category property (not CategoryId) because appropriate property is stored in it
            };

            _cateNewsService.InsertNewsCategory(newsCategory);
            var category = _cateNewsService.GetCategoryById(newsCategory.CategoryNewsId);
            if (category != null)
            {
                category.NewsCount++;
                _cateNewsService.UpdateCategory(category);
            }

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

            var categoryOld = _cateNewsService.GetCategoryById(newsCategory.CategoryNewsId);

            //use Category property (not CategoryId) because appropriate property is stored in it
            newsCategory.CategoryNewsId = Int32.Parse(model.NewsCate);

            _cateNewsService.UpdateNewsCategory(newsCategory);
            
            if (categoryOld != null)
            {
                categoryOld.NewsCount--;
                _cateNewsService.UpdateCategory(categoryOld);
            }

            var categoryNew = _cateNewsService.GetCategoryById(Int32.Parse(model.NewsCate));
            if (categoryNew != null)
            {
                categoryNew.NewsCount++;
                _cateNewsService.UpdateCategory(categoryNew);
            }

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
            var category = _cateNewsService.GetCategoryById(newsCategory.CategoryNewsId);
            if (category != null)
            {
                category.NewsCount--;
                _cateNewsService.UpdateCategory(category);
            }

            return NewsCateList(command, newsId);
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


        #endregion

        #region Tags

        public ActionResult Tags()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            return View();
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Tags(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var tags = _tagService.GetAllTags()
                .OrderByDescending(x => _tagService.GetNewsCount(x.Id, 0))
                .Select(x =>
                {
                    return new TagModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        EnglishName = x.EnglishName,
                        IsShowHomePage = x.IsShowHomePage,
                        NewsItemCount = _tagService.GetNewsCount(x.Id, 0)
                    };
                })
                .ForCommand(command);

            var model = new GridModel<TagModel>
            {
                Data = tags.PagedForCommand(command),
                Total = tags.Count()
            };
            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult TagDelete(int id, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var tag = _tagService.GetTagById(id);
            if (tag == null)
                throw new ArgumentException("No tag found with the specified id");
            _tagService.DeleteTag(tag);

            return Tags(command);
        }
        //edit
        public ActionResult EditTag(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var tag = _tagService.GetTagById(id);
            if (tag == null)
                //No tag found with the specified id
                return RedirectToAction("List");

            var model = new TagModel()
            {
                Id = tag.Id,
                Name = tag.Name,
                EnglishName = tag.EnglishName,
                IsShowHomePage = tag.IsShowHomePage,
                NewsItemCount = _tagService.GetNewsCount(tag.Id, 0)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditTag(string btnId, string formId, TagModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            var tag = _tagService.GetTagById(model.Id);
            if (tag == null)
                //No tag found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                tag.Name = model.Name;
                tag.EnglishName = model.EnglishName;
                tag.IsShowHomePage = model.IsShowHomePage;
                _tagService.UpdateTag(tag);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }


        #endregion
    }
}
