using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.News;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.News;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.UI.Captcha;
using Nop.Web.Infrastructure.Cache;
using Nop.Web.Models.Media;
using Nop.Web.Models.News;
using System.Threading.Tasks;

namespace Nop.Web.Controllers
{
    [NopHttpsRequirement(SslRequirement.No)]
    public partial class NewsController : BaseNopController
    {
        #region Fields

        private readonly INewsService _newsService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IWebHelper _webHelper;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ICategoryNewsService _cateNewsService;

        private readonly MediaSettings _mediaSettings;
        private readonly NewsSettings _newsSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly CaptchaSettings _captchaSettings;

        #endregion

        #region Constructors

        public NewsController(INewsService newsService,
            IWorkContext workContext, IStoreContext storeContext,
            IPictureService pictureService, ILocalizationService localizationService,
            IDateTimeHelper dateTimeHelper,
            IWorkflowMessageService workflowMessageService, IWebHelper webHelper,
            ICacheManager cacheManager, ICustomerActivityService customerActivityService,
            IStoreMappingService storeMappingService,
            MediaSettings mediaSettings, NewsSettings newsSettings,
            LocalizationSettings localizationSettings, CustomerSettings customerSettings,
            CaptchaSettings captchaSettings, ICategoryNewsService cateNewsService)
        {
            this._cateNewsService = cateNewsService;
            this._newsService = newsService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._workflowMessageService = workflowMessageService;
            this._webHelper = webHelper;
            this._cacheManager = cacheManager;
            this._customerActivityService = customerActivityService;
            this._storeMappingService = storeMappingService;

            this._mediaSettings = mediaSettings;
            this._newsSettings = newsSettings;
            this._localizationSettings = localizationSettings;
            this._customerSettings = customerSettings;
            this._captchaSettings = captchaSettings;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected NewsItemModel PrepareNewsItemModel(NewsItem newsItem, bool prepareComments, bool preparePicture = false)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsItem");

            //if (model == null)
            //    throw new ArgumentNullException("model");
            string keyCache = string.Format("PrepareNewsItemModel-{0}-{1}-{2}", newsItem.Id, prepareComments, preparePicture);
            return _cacheManager.Get(keyCache, 10, () =>
            {
                var modelNews = new NewsItemModel();
                modelNews.Id = newsItem.Id;
                modelNews.MetaTitle = newsItem.MetaTitle;
                modelNews.MetaDescription = newsItem.MetaDescription;
                modelNews.MetaKeywords = newsItem.MetaKeywords;
                modelNews.SeName = newsItem.GetSeName(newsItem.LanguageId, ensureTwoPublishedLanguages: false);
                modelNews.Title = newsItem.Title;
                modelNews.Short = newsItem.Short;
                modelNews.Full = newsItem.Full;
                modelNews.AllowComments = newsItem.AllowComments;
                modelNews.CreatedOn = _dateTimeHelper.ConvertToUserTime(newsItem.CreatedOnUtc, DateTimeKind.Utc);
                modelNews.NumberOfComments = newsItem.CommentCount;
                var cateNews = newsItem.NewsCategoriesMap.FirstOrDefault();
                if (cateNews != null)
                {
                    var categoryNews = cateNews.CategoryNews;
                    modelNews.CateName = categoryNews.Name;
                    modelNews.CategorySename = categoryNews.GetSeName();
                }
            //  model.AddNewComment.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage;
            if (preparePicture && newsItem.PictureId.HasValue)
            {
                #region Prepare product picture

                //If a size has been set in the view, we use it in priority

                //prepare picture model
                var defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.NEWS_PICTURE_KEY, newsItem.Id);
                modelNews.DefaultPictureModel = _cacheManager.Get(defaultProductPictureCacheKey, () =>
                {
                    var picture = _pictureService.GetPictureById((int)newsItem.PictureId);
                    var pictureModel = new PictureModel()
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture, 254, true),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                        Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat"), newsItem.Title),
                        AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat"), modelNews.Title)
                    };
                    return pictureModel;
                });

                #endregion
            }
            //if (prepareComments)
            //{
            //    var newsComments = newsItem.NewsComments.OrderBy(pr => pr.CreatedOnUtc);
            //    foreach (var nc in newsComments)
            //    {
            //        var commentModel = new NewsCommentModel()
            //        {
            //            Id = nc.Id,
            //            CustomerId = nc.CustomerId,
            //           // CustomerName = nc.Customer.FormatUserName(),
            //            CommentTitle = nc.CommentTitle,
            //            CommentText = nc.CommentText,
            //            CreatedOn = _dateTimeHelper.ConvertToUserTime(nc.CreatedOnUtc, DateTimeKind.Utc),
            //           // AllowViewingProfiles = _customerSettings.AllowViewingProfiles && nc.Customer != null && !nc.Customer.IsGuest(),
            //        };
            //        if (_customerSettings.AllowCustomersToUploadAvatars)
            //        {
            //            //commentModel.CustomerAvatarUrl = _pictureService.GetPictureUrl(
            //            //    nc.Customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId), 
            //            //    _mediaSettings.AvatarPictureSize, 
            //            //    _customerSettings.DefaultAvatarEnabled,
            //            //    defaultPictureType:PictureType.Avatar);
            //        }
            //        //model.Comments.Add(commentModel);
            //    }
            //}
            return modelNews;
            });
        }

        #endregion

        #region Methods
        public ActionResult NewsCateNavigation()
        {
            var model = new CategoryNewsListModel();
            var catenews = Task.Run(async()=> await _cateNewsService.GetAllCategoriesByParentCategoryIdAsync(0)).Result;
            model.CategoryNews = catenews.ToList();
            return View(model);
        }
        public ActionResult HomePageNews(int? pageSize)
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowNewsOnMainPage)
                return Content("");

            var cacheKey = string.Format(ModelCacheEventConsumer.HOMEPAGE_NEWSMODEL_KEY, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var newsItems = Task.Run(async () => await _newsService.GetAllNewsAsync(_workContext.WorkingLanguage.Id, 0, 0, 0, pageSize.HasValue ? pageSize.Value : _newsSettings.MainPageNewsCount)).Result;
                return new HomePageNewsItemsModel()
                {
                    WorkingLanguageId = _workContext.WorkingLanguage.Id,
                    NewsItems = newsItems.Select(x =>
                                                    {
                                                        var newsModel = new NewsItemModel();
                                                        newsModel = PrepareNewsItemModel(x, false, true);
                                                        return newsModel;
                                                    }).ToList()
                };
            });

            var model = (HomePageNewsItemsModel)cachedModel.Clone();
            return PartialView(model);
        }

        public ActionResult HomePageNewsCate()
        {
            var model = new List<FNewsItemListModel>();
            var newsCate = Task.Run(async()=> await _cateNewsService.GetAllCategoriesByParentCategoryIdAsync(0)).Result.Take(2);
            foreach (var cn in newsCate)
            {
                var row = new FNewsItemListModel
                {
                    CateName = cn.Name,
                    CateId = cn.Id,
                    NewsItems = Task.Run(async()=> await _newsService.GetAllNewsAsync(0, 0, cn.Id, 0, 4)).Result.Select(x =>
                    {
                        var item = new NewsItemModel();
                        item = PrepareNewsItemModel(x, false);
                        return item;
                    }).ToList()

                };
                model.Add(row);
            }
            return View(model);

        }

        public ActionResult List(NewsPagingFilteringModel command)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var model = new NewsItemListModel();
            // model.WorkingLanguageId = _workContext.WorkingLanguage.Id;

            if (command.PageSize <= 0) command.PageSize = _newsSettings.NewsArchivePageSize;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            string keyCache = string.Format("GetAllNews-{0}-{1}-{2}-{3}-{4}", _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id, command.CateId, command.PageNumber, command.PageSize);
            var newsItems = _cacheManager.Get(keyCache, 30, () =>
            {
                return _newsService.GetAllNews(_workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id, command.CateId,
                command.PageNumber - 1, command.PageSize);
            });
            model.PagingFilteringContext.LoadPagedList(newsItems);

            model.NewsItems = newsItems
                .Select(x =>
                {
                    var newsModel = new NewsItemModel();
                    newsModel = PrepareNewsItemModel(x, false, true);
                    return newsModel;
                })
                .ToList();
            var cate = _cateNewsService.GetCategoryById(command.CateId);
            if (cate == null)
                return RedirectToRoute("HomePage");

            model.CateName = cate.Name;
            model.MetaTitle = cate.MetaTitle;
            model.MetaKeywords = cate.MetaKeywords;
            model.MetaDescription = cate.MetaDescription;

            return View(model);
        }

        public ActionResult ListRss(int languageId)
        {
            var feed = new SyndicationFeed(
                                    string.Format("{0}: News", _storeContext.CurrentStore.GetLocalized(x => x.Name)),
                                    "News",
                                    new Uri(_webHelper.GetStoreLocation(false)),
                                    "NewsRSS",
                                    DateTime.UtcNow);

            if (!_newsSettings.Enabled)
                return new RssActionResult() { Feed = feed };

            var items = new List<SyndicationItem>();
            var newsItems = _newsService.GetAllNews(languageId, _storeContext.CurrentStore.Id, 0, 0, int.MaxValue);
            foreach (var n in newsItems)
            {
                string newsUrl = Url.RouteUrl("NewsItem", new { SeName = n.GetSeName(n.LanguageId, ensureTwoPublishedLanguages: false) }, "http");
                items.Add(new SyndicationItem(n.Title, n.Short, new Uri(newsUrl), String.Format("Blog:{0}", n.Id), n.CreatedOnUtc));
            }
            feed.Items = items;
            return new RssActionResult() { Feed = feed };
        }

        public ActionResult NewsItem(int newsItemId)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var newsItem = _newsService.GetNewsById(newsItemId);
            if (newsItem == null ||
                !newsItem.Published ||
                (newsItem.StartDateUtc.HasValue && newsItem.StartDateUtc.Value >= DateTime.UtcNow) ||
                (newsItem.EndDateUtc.HasValue && newsItem.EndDateUtc.Value <= DateTime.UtcNow) ||
                //Store mapping
                !_storeMappingService.Authorize(newsItem))
                return RedirectToRoute("HomePage");

            var model = new NewsItemModel();
            model = PrepareNewsItemModel(newsItem, true);

            return View(model);
        }

        [HttpPost, ActionName("NewsItem")]
        [FormValueRequired("add-comment")]
        [CaptchaValidator]
        public ActionResult NewsCommentAdd(int newsItemId, NewsItemModel model, bool captchaValid)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var newsItem = _newsService.GetNewsById(newsItemId);
            if (newsItem == null || !newsItem.Published || !newsItem.AllowComments)
                return RedirectToRoute("HomePage");

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptcha"));
            }

            if (_workContext.CurrentCustomer.IsGuest() && !_newsSettings.AllowNotRegisteredUsersToLeaveComments)
            {
                ModelState.AddModelError("", _localizationService.GetResource("News.Comments.OnlyRegisteredUsersLeaveComments"));
            }

            if (ModelState.IsValid)
            {
                var comment = new NewsComment()
                {
                    NewsItemId = newsItem.Id,
                    CustomerId = _workContext.CurrentCustomer.Id,
                    //CommentTitle = model.AddNewComment.CommentTitle,
                    // CommentText = model.AddNewComment.CommentText,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                newsItem.NewsComments.Add(comment);
                //update totals
                newsItem.CommentCount = newsItem.NewsComments.Count;
                _newsService.UpdateNews(newsItem);


                //notify a store owner;
                if (_newsSettings.NotifyAboutNewNewsComments)
                    _workflowMessageService.SendNewsCommentNotificationMessage(comment, _localizationSettings.DefaultAdminLanguageId);

                //activity log
                _customerActivityService.InsertActivity("PublicStore.AddNewsComment", _localizationService.GetResource("ActivityLog.PublicStore.AddNewsComment"));

                //The text boxes should be cleared after a comment has been posted
                //That' why we reload the page
                TempData["nop.news.addcomment.result"] = _localizationService.GetResource("News.Comments.SuccessfullyAdded");
                return RedirectToRoute("NewsItem", new { SeName = newsItem.GetSeName(newsItem.LanguageId, ensureTwoPublishedLanguages: false) });
            }


            //If we got this far, something failed, redisplay form
            model = PrepareNewsItemModel(newsItem, true);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult RssHeaderLink()
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowHeaderRssUrl)
                return Content("");

            string link = string.Format("<link href=\"{0}\" rel=\"alternate\" type=\"application/rss+xml\" title=\"{1}: News\" />",
                Url.RouteUrl("NewsRSS", new { languageId = _workContext.WorkingLanguage.Id }, _webHelper.IsCurrentConnectionSecured() ? "https" : "http"), _storeContext.CurrentStore.GetLocalized(x => x.Name));

            return Content(link);
        }

        #endregion
    }
}
