using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using PlanX.Core;
using PlanX.Core.Caching;
using PlanX.Core.Domain.Customers;
using PlanX.Core.Domain.Localization;
using PlanX.Core.Domain.Media;
using PlanX.Core.Domain.News;
using PlanX.Services.Common;
using PlanX.Services.Customers;
using PlanX.Services.Helpers;
using PlanX.Services.Localization;
using PlanX.Services.Logging;
using PlanX.Services.Media;
using PlanX.Services.Messages;
using PlanX.Services.News;
using PlanX.Services.Seo;
using PlanX.Services.Stores;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Controllers;
using PlanX.Web.Framework.Security;
using PlanX.Web.Framework.UI.Captcha;
using PlanX.Web.Infrastructure.Cache;
using PlanX.Web.Models.Media;
using PlanX.Web.Models.News;
using System.Threading.Tasks;
using PlanX.Services.Directory;
using System.Web;

namespace PlanX.Web.Controllers
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
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IWebHelper _webHelper;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ICategoryNewsService _cateNewsService;
        private readonly ITagService _tagService;
        private readonly IStateProvinceService _stateProvinceService;
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
            IWorkflowMessageService workflowMessageService, IWebHelper webHelper,
            ICacheManager cacheManager, ICustomerActivityService customerActivityService,
            IStoreMappingService storeMappingService,
            MediaSettings mediaSettings, NewsSettings newsSettings,
            LocalizationSettings localizationSettings, CustomerSettings customerSettings,
            CaptchaSettings captchaSettings, ICategoryNewsService cateNewsService,
            IStateProvinceService stateProvinceService,
            ITagService tagService)
        {
            this._cateNewsService = cateNewsService;
            this._newsService = newsService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._workflowMessageService = workflowMessageService;
            this._webHelper = webHelper;
            this._cacheManager = cacheManager;
            this._customerActivityService = customerActivityService;
            this._storeMappingService = storeMappingService;
            this._stateProvinceService = stateProvinceService;

            this._mediaSettings = mediaSettings;
            this._newsSettings = newsSettings;
            this._localizationSettings = localizationSettings;
            this._customerSettings = customerSettings;
            this._captchaSettings = captchaSettings;
            this._tagService = tagService;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected NewsItemModel PrepareNewsItemModel(NewsItem newsItem, bool prepareComments, bool preparePicture = false, int? thumImageSize = 0)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsItem");

            //if (model == null)
            //    throw new ArgumentNullException("model");
            string keyCache = string.Format("PrepareNewsItemModel-{0}-{1}-{2}", newsItem.Id, prepareComments, preparePicture);
            return _cacheManager.Get(keyCache, 10, () =>
            {
                var model = new NewsItemModel();
                model.Id = newsItem.Id;
                model.MetaTitle = newsItem.MetaTitle;
                model.MetaDescription = newsItem.MetaDescription;
                model.MetaKeywords = newsItem.MetaKeywords;
                model.SeName = newsItem.GetSeName();
                model.Title = newsItem.Title;
                model.Short = newsItem.Short;
                model.Full = newsItem.Full;
                model.AllowComments = newsItem.AllowComments;
                model.CreatedOn = newsItem.CreatedOnUtc;
                model.NumberOfComments = newsItem.CommentCount;
                var cateNews = newsItem.NewsCategoryNews.FirstOrDefault();
                if (cateNews != null)
                {
                    var categoryNews = cateNews.CategoryNews;
                    model.CategoryNewsModel = new CategoryNewsModel()
                    {
                        Id = categoryNews.Id,
                        Name = categoryNews.Name,
                    };

                }
                //  model.AddNewComment.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage;
                if (preparePicture && newsItem.PictureId.HasValue)
                {
                    #region Prepare product picture

                    //If a size has been set in the view, we use it in priority

                    //prepare picture model
                    var imageSize = thumImageSize ?? 0;
                    if (imageSize == 0) imageSize = 365;
                    var defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.NEWS_PICTURE_KEY, newsItem.Id);
                    model.DefaultPictureModel = _cacheManager.Get(defaultProductPictureCacheKey, () =>
                    {
                        var picture = _pictureService.GetPictureById((int)newsItem.PictureId);
                        var pictureModel = new PictureModel()
                        {
                            ImageUrl = _pictureService.GetPictureUrl(picture, imageSize, true),
                            FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                            Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat"), newsItem.Title),
                            AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat"), model.Title)
                        };
                        return pictureModel;
                    });

                    #endregion
                }
                if (prepareComments)
                {
                    var newsComments = newsItem.NewsComments.OrderBy(pr => pr.CreatedOnUtc);
                    foreach (var nc in newsComments)
                    {
                        var commentModel = new NewsCommentModel()
                        {
                            Id = nc.Id,
                            CustomerId = nc.CustomerId,
                            CustomerName = nc.Customer.FormatUserName(),
                            CommentTitle = nc.CommentTitle,
                            CommentText = nc.CommentText,
                            CreatedOn = nc.CreatedOnUtc,
                            // AllowViewingProfiles = _customerSettings.AllowViewingProfiles && nc.Customer != null && !nc.Customer.IsGuest(),
                        };
                        if (_customerSettings.AllowCustomersToUploadAvatars)
                        {
                            //commentModel.CustomerAvatarUrl = _pictureService.GetPictureUrl(
                            //    nc.Customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId), 
                            //    _mediaSettings.AvatarPictureSize, 
                            //    _customerSettings.DefaultAvatarEnabled,
                            //    defaultPictureType:PictureType.Avatar);
                        }
                        model.Comments.Add(commentModel);
                    }
                }
                return model;
            });
        }

        [NonAction]
        protected List<CategoryNewsModel> GetChildCategoryNews(int parentId, bool isShowNewsCount = false)
        {
            if (parentId >= 0)
            {
                var catenews = _cateNewsService.GetAllCategoriesByParentCategoryId(parentId);
                return catenews.Select(x =>
                {
                    return new CategoryNewsModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SeName = x.GetSeName(),
                        CategoryNewsChild = GetChildCategoryNews(x.Id),
                        NewsCount = x.NewsCount,
                    };
                }).ToList();
            }
            return null;
        }

        [NonAction]
        protected void GetAllChildCategoryNewsIds(int parentId, List<int> result)
        {            
            if (parentId >= 0)
            {
                var catenews = _cateNewsService.GetAllCategoriesByParentCategoryId(parentId);
                if(catenews != null && catenews.Count()>0)
                { 
                    result.AddRange(catenews.Select(x => { return x.Id; }).ToList());
                    foreach (var cate in catenews)
                        GetAllChildCategoryNewsIds(cate.Id, result);
                }
            }
        }       

        #endregion

        #region Methods
        [ChildActionOnly]
        public ActionResult NewsCateNavigation(int? categoryId = 0, int currentCategoryId = 0, string viewReturn = "", bool isShowNewsCount = false)
        {
            categoryId = categoryId ?? 0;
            var model = new NavigativeCategory();
            model.CurrentCategoryId = currentCategoryId;

            if (categoryId > 0)
            {
                var category = _cateNewsService.GetCategoryById(categoryId.Value);
                if (category != null)
                    model.CategoryNewsChild.Add(new CategoryNewsModel()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        SeName = category.GetSeName(),
                        CategoryNewsChild = GetChildCategoryNews(category.Id, isShowNewsCount),
                        NewsCount = isShowNewsCount ? category.NewsCount : 0,
                    });
            }
            else
            {
                model.CategoryNewsChild = GetChildCategoryNews(categoryId.Value, isShowNewsCount);
            }
            if (!String.IsNullOrEmpty(viewReturn))
            {
                return View(viewReturn, model);
            }
            return View(model);
        }
        public ActionResult HomePageNews( int? pageSize, int? thumsImageSize,int pageIndex=1,int cate=0)
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowNewsOnMainPage)
                return Content("");
            int pagesize = pageSize ?? _newsSettings.MainPageNewsCount;
            //get all child category
            List<int> cateIds = new List<int>();
            if (cate > 0)
            {
                cateIds.Add(cate);
                GetAllChildCategoryNewsIds(cate, cateIds);
            }
            var cacheKey = string.Format(ModelCacheEventConsumer.HOMEPAGE_NEWSMODEL_KEY, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id, pageIndex, pagesize,cate);
            var cachedModel = _cacheManager.Get(cacheKey, 5, () =>
            {
                var newsItems = Task.Run(async () => await _newsService.GetAllNewsAsync(_workContext.WorkingLanguage.Id, 0, pageIndex-1, pagesize,categoryNewsIds:cateIds)).Result;
                return new HomePageNewsItemsModel()
                {
                    WorkingLanguageId = _workContext.WorkingLanguage.Id,
                    NewsItems = newsItems.Select(x =>
                    {
                        var newsModel = new NewsItemModel();
                        newsModel = PrepareNewsItemModel(x, false, true, thumsImageSize);
                        return newsModel;
                    }).ToList(),
                    TotalPage = newsItems.TotalPages
                };
            });


            var model = (HomePageNewsItemsModel)cachedModel.Clone();
            model.PageIndex = pageIndex;
            return PartialView(model);
        }

        public ActionResult HomePageNewsCate()
        {
            var model = new List<NewsItemListModel>();
            var newsCate = Task.Run(async () => await _cateNewsService.GetAllCategoriesByParentCategoryIdAsync(0)).Result.Take(2);
            foreach (var cn in newsCate)
            {
                var row = new NewsItemListModel
                {
                    CategoryName = cn.Name,
                    CategoryId = cn.Id,
                    NewsItems = Task.Run(async () => await _newsService.GetAllNewsAsync(0, 0, 0, 4, false, null)).Result.Select(x =>
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

        public async Task<ActionResult> List(NewsPagingFilteringModel command)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var model = new NewsItemListModel();
            // model.WorkingLanguageId = _workContext.WorkingLanguage.Id;
            if (command.PageSize <= 0) command.PageSize = _newsSettings.NewsArchivePageSize;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            //get all child category
            List<int> cateIds = new List<int>();
            if(command.CategoryId>0)
            {
                cateIds.Add(command.CategoryId);
                GetAllChildCategoryNewsIds(command.CategoryId, cateIds);                
            }

            string keyCache = string.Format("GetAllNews-{0}-{1}-{2}-{3}-{4}", 0, _storeContext.CurrentStore.Id, command.CategoryId, command.PageNumber, command.PageSize);
            var newsItems = await _cacheManager.Get(keyCache, 30, () =>
            {
                return _newsService.GetAllNewsAsync(0, _storeContext.CurrentStore.Id, categoryNewsIds: cateIds,
                    pageIndex: command.PageNumber - 1, pageSize: command.PageSize);
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
            if(Request.IsAjaxRequest())
            {
                var lsModel = new HomePageNewsItemsModel
                {
                    NewsItems=model.NewsItems,
                    PageIndex=command.PageNumber,
                    TotalPage=command.TotalPages
                };
                return PartialView("HomePageNews", lsModel);

            }
            var cate = await _cateNewsService.GetCategoryByIdAsync(command.CategoryId);
            if (cate == null)
                return RedirectToRoute("HomePage");

            model.CategoryName = cate.Name;
            model.CategoryId = cate.Id;
            model.MetaTitle = cate.MetaTitle;
            model.MetaKeywords = cate.MetaKeywords;
            model.MetaDescription = cate.MetaDescription;
            model.CategoryId = command.CategoryId;
           
           
            return View(model);
        }

        public async Task<ActionResult> HotView(int? pageSize, int categoryId = 0)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var model = new List<NewsItemModel>();
            var cateIds = new List<int>();
            GetAllChildCategoryNewsIds(categoryId, cateIds);

            var list = await _newsService.GetAllNewsAsync(0, 0, 0, pageSize: pageSize ?? _newsSettings.NewsArchivePageSize, categoryNewsIds: cateIds, isHostView: true);
            foreach (var news in list)
            {
                var newsItem = new NewsItemModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    SeName = news.GetSeName()
                };
                model.Add(newsItem);
            }

            return View(model);
        }

        public async Task<ActionResult> MostView(int? pageSize, int categoryId = 0)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var model = new List<NewsItemModel>();
            var cateIds = new List<int>();
            GetAllChildCategoryNewsIds(categoryId, cateIds);

            var list = await _newsService.GetAllNewsAsync(0, 0, 0, pageSize: pageSize ?? _newsSettings.NewsArchivePageSize, categoryNewsIds: cateIds, isMostView: true);
            foreach (var news in list)
            {
                var newsItem = new NewsItemModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    SeName = news.GetSeName()
                };
                model.Add(newsItem);
            }

            return View(model);
        }

        public ActionResult Related(int? pageSize, int newsId = 0)
        {
            if (newsId == 0)
                return Content("");

            var newsCurrent = _newsService.GetNewsById(newsId);
            int? categoryId = 0;
            if (newsCurrent != null && newsCurrent.NewsCategoryNews.FirstOrDefault() != null)
                categoryId = newsCurrent.NewsCategoryNews.FirstOrDefault().CategoryNewsId;
            
            var model = new List<NewsItemModel>();            
            var cateIds = new List<int>();
            GetAllChildCategoryNewsIds(categoryId.HasValue ? categoryId.Value : 0, cateIds);
            
            var list = _newsService.GetAllNews(0, 0, 0, pageSize: pageSize ?? _newsSettings.NewsArchivePageSize, categoryNewsIds: cateIds);
            foreach (var news in list)
            {
                var newsItem = new NewsItemModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    SeName = news.GetSeName()
                };
                model.Add(newsItem);
            }

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult NewsBreadcrumb(int newsId, int categoryId = 0)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.NEWS_BREADCRUMB_MODEL_KEY, newsId, categoryId, "", 0);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                var model = new NewsItemModel.NewsBreadcrumbModel();
                CategoryNews category = null;
                if (newsId > 0)
                {
                    var news = _newsService.GetNewsById(newsId);
                    model.NewsId = news.Id;
                    model.NewsName = news.Title;
                    model.NewsSeName = news.GetSeName();
                    var newsCategories = news.NewsCategoryNews.ToList();
                    if (newsCategories.Count > 0)
                    {
                        category = newsCategories[0].CategoryNews;
                    }
                }
                else
                {
                    category = _cateNewsService.GetCategoryById(categoryId);
                }

                if (category != null)
                {
                    foreach (var catBr in category.GetCategoryBreadCrumb(_cateNewsService, false))
                    {
                        model.CategoryBreadcrumb.Add(new CategoryNewsModel()
                        {
                            Id = catBr.Id,
                            Name = catBr.Name,
                            SeName = catBr.GetSeName(),
                            CategoryNewsChild = catBr.Id != 0 ? _cateNewsService.GetAllCategoriesByParentCategoryId(catBr.Id)
                                                .Select(x =>
                                                {
                                                    return new CategoryNewsModel()
                                                    {
                                                        Id = x.Id,
                                                        Name = x.Name,
                                                        SeName = x.GetSeName()
                                                    };
                                                }).ToList() : new List<CategoryNewsModel>(),
                        });
                    }
                }

                return model;
            });

            return PartialView(cacheModel);
        }

        /*
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
        */

        public async Task<ActionResult> NewsItem(int newsItemId)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var newsItem = await _newsService.GetNewsByIdAsync(newsItemId);
            if (newsItem == null ||
                !newsItem.Published ||
                (newsItem.StartDateUtc.HasValue && newsItem.StartDateUtc.Value >= DateTime.Now) ||
                (newsItem.EndDateUtc.HasValue && newsItem.EndDateUtc.Value <= DateTime.Now))
                //|| Store mapping 
                //!_storeMappingService.Authorize(newsItem))
                return RedirectToRoute("HomePage");

            var model = new NewsItemModel();
            model = PrepareNewsItemModel(newsItem, true);
          
            //number view ++;
            newsItem.ViewCount++;
            _newsService.UpdateNews(newsItem);

            return View(model);
        }


        [HttpPost]       
        [CaptchaValidator]
        public async Task<ActionResult> NewsCommentAdd(int newsItemId, AddNewsCommentModel model, bool captchaValid)      
        {
            if (!_newsSettings.Enabled)
                return Json("UNENABLE");

            var newsItem = await _newsService.GetNewsByIdAsync(newsItemId);
            if (newsItem == null || !newsItem.Published || !newsItem.AllowComments)
                return Json("UNENABLE");

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage && !captchaValid)
            {
                return Json( _localizationService.GetResource("Common.WrongCaptcha"));
            }

            if (_workContext.CurrentCustomer.IsGuest() && !_newsSettings.AllowNotRegisteredUsersToLeaveComments)
            {
                return Json( _localizationService.GetResource("News.Comments.OnlyRegisteredUsersLeaveComments"));
            }

            if (ModelState.IsValid)
            {
                var comment = new NewsComment()
                {
                    NewsItemId = newsItem.Id,
                    CustomerId = _workContext.CurrentCustomer.Id,
                    CommentTitle = model.CommentTitle,
                    CommentText = model.CommentText,
                    CustomerPlace=model.Place,
                    CustomerName=model.UserName,
                    CustomerEmail=_workContext.CurrentCustomer.Email,
                    IsApproved=_workContext.CurrentCustomer.IsAdmin(),
                    ParentId=model.ParentId,
                    CreatedOnUtc = DateTime.Now,
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
                return Json( _localizationService.GetResource("News.Comments.SuccessfullyAdded"));
               // return RedirectToRoute("NewsItem", new { SeName = newsItem.GetSeName(newsItem.LanguageId, ensureTwoPublishedLanguages: false) });
            }


            //If we got this far, something failed, redisplay form
            return Json(_localizationService.GetResource("News.Comments.NotSuccessfullyAdded"));
        }

        /*
        [ChildActionOnly]
        public ActionResult RssHeaderLink()
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowHeaderRssUrl)
                return Content("");

            string link = string.Format("<link href=\"{0}\" rel=\"alternate\" type=\"application/rss+xml\" title=\"{1}: News\" />",
                Url.RouteUrl("NewsRSS", new { languageId = _workContext.WorkingLanguage.Id }, _webHelper.IsCurrentConnectionSecured() ? "https" : "http"), _storeContext.CurrentStore.GetLocalized(x => x.Name));

            return Content(link);
        }
        */
        
        public ActionResult BannerSlideShow(int pictureThumbSize,int?numitem)
        {
            string cachekey=string.Format("BannerSlideShow-{0}-{1}",numitem,pictureThumbSize);
            var banners = _cacheManager.Get(cachekey, () =>
            {
                var news= _newsService.GetAllNewShowBanner(_workContext.WorkingLanguage.Id,numitem ?? 5).Result;
                return news.Select(x=>PrepareNewsItemModel(x,false,true,pictureThumbSize)).ToList();
            });
            return View(banners);
        }

        public ActionResult NewsComment(int newsId)
        {
            var model = new AddNewsCommentModel();
            model.CustomProperties.Add("newsId", newsId);
            return View(model);
        }
        [HttpGet]
        public ActionResult CommentList(int pageIndex,int pageSize,int newsId,int parentId=0)
        {
            var data = _newsService.GetAllComments(0, newsId,0,true, pageIndex-1, pageSize);
            var comments = PreparingCommentModel(data);
            var model = new NewsCommentListModel { 
            PageIndex=pageIndex,
            TotalPage=data.TotalPages,
            Comments=comments,
            NewsId=newsId
            };
            return View(model);
        }
        public IList<NewsCommentModel> PreparingCommentModel(IEnumerable<NewsComment> ls,int parentId=0)
        {
            var result=new List<NewsCommentModel>();
          var parentList=  ls.Where(x=>x.ParentId==parentId);
            foreach(var e in parentList)
            {
            result.Add( new NewsCommentModel
                    {
                        Id=e.Id,
                        CommentText=e.CommentText,
                        CommentTitle=e.CommentTitle,
                        CreatedOn=e.CreatedOnUtc,
                        CustomerAvatarUrl = _pictureService.GetPictureUrl(
                        e.Customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId),
                        _mediaSettings.AvatarPictureSize,
                        true,null,PictureType.Avatar),
                        CustomerId=e.Customer.Id,
                        CustomerName=e.CustomerName,
                        Place=e.CustomerPlace,
                        TotalLike=e.LikeTotal,
                        SubComments =parentId==0? PreparingCommentModel(ls,e.Id):new List<NewsCommentModel>()
                
                    }
            );
            }
            return result;
            
        }
        public ActionResult GetCity()
        {
            
           var data= _stateProvinceService.GetStateProvincesByCountryId(230).Select(x => new {name=x.Name,id=x.Id}).ToList();
           return Json(data, JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public ActionResult CommentLike(int id)
        {
            string cookieName="nlcids";
            var listIds = new List<string>();
             var cookieNewsIds = Request.Cookies[cookieName];
            if(cookieNewsIds!=null)
                 listIds=cookieNewsIds.Value.Split(',').ToList();
                if (!listIds.Contains(id.ToString()))
                {
                    var comment = _newsService.GetNewsCommentById(id);
                    if (comment == null)
                        return Json("NOT OK");
           
                   
                        comment.LikeTotal++;
                        listIds.Add(id.ToString());
                        var cookie = new HttpCookie(cookieName);
                        cookie.Expires = DateTime.Now.AddDays(365);
                        cookie.Value = listIds.Aggregate((a, b) => a + "," + b);
                        Response.Cookies.Add(cookie);
                        _newsService.UpdateNewsComment(comment);

                        return Json(comment.LikeTotal);
                    
                }
                return Json("NOT OK");
           

        }
        #endregion

        #region tags

        //get tags in detail news
        public async Task<ActionResult> NewsItemTags(int newsId, string viewName)
        {
            var news = await _newsService.GetNewsByIdAsync(newsId);
            if (news == null)
                throw new ArgumentException("No news found with the specified id");

            var model = news.Tags.Select(x =>
            {
                return new NewsTagModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    SeName = x.GetSeName(),
                };
            }).ToList();

            if (!string.IsNullOrEmpty(viewName))
                return PartialView(viewName, model);
            return PartialView(model);
        }

        //get tag show home page
        public async Task<ActionResult> HomePageTags()
        {
            var tags = await _tagService.GetAllTagsAsync(isShowHomePage: true);

            var model = new List<NewsTagModel>();

            if (tags.Count > 0)
            {
                model = tags.Select(x =>
                {
                    return new NewsTagModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SeName = x.GetSeName(),
                    };
                }).ToList();
            }
            return PartialView(model);
        }

        //get tag show catalog page
        public ActionResult ListTags(int categoryId, string viewName)
        {
            var tags = _tagService.GetAllTagsByCategoryNewsId(categoryId, 10);

            var model = new List<NewsTagModel>();

            if (tags.Count > 0)
            {
                model = tags.Select(x =>
                {
                    return new NewsTagModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        SeName = x.GetSeName(),
                    };
                }).ToList();
            }
            if (!string.IsNullOrEmpty(viewName))
                return PartialView(viewName, model);
            return PartialView(model);
        }

        //page news by tags
        public async Task<ActionResult> NewsItemsByTag(int newsTagId, NewsPagingFilteringModel command)
        {
            var newsTag = _tagService.GetTagById(newsTagId);
            if (newsTag == null)
                return InvokeHttp404();

            var model = new NewsByTagModel()
            {
                Id = newsTagId,
                TagName = newsTag.Name,
            };

            if (command.PageSize <= 0) command.PageSize = _newsSettings.NewsArchivePageSize;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            
            string keyCache = string.Format("getallnews.by.tags-{0}-{1}-{2}-{3}-{4}", 0, _storeContext.CurrentStore.Id, newsTagId, command.PageNumber, command.PageSize);
            var newsItems = await _cacheManager.Get(keyCache, 30, () =>
            {
                return _newsService.GetAllNewsAsync(languageId: 0, storeId: _storeContext.CurrentStore.Id, categoryNewsIds: null,
                    pageIndex: command.PageNumber - 1, pageSize: command.PageSize, showHidden: false, newsTagId: newsTagId);
            });
            model.PagingFilteringContext.LoadPagedList(newsItems);

            model.NewsItems = newsItems.Select(x =>
                                        {
                                            var newsModel = new NewsItemModel();
                                            newsModel = PrepareNewsItemModel(x, false, true);
                                            return newsModel;
                                        }).ToList();

            return View(model);
        }

        #endregion
    }
}
