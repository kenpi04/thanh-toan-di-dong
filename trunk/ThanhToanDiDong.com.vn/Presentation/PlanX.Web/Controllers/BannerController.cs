using System.Web.Mvc;
using PlanX.Services.Common;
using PlanX.Web.Models.Common;
using PlanX.Services.Media;
using System.Linq;
using PlanX.Core.Caching;

namespace PlanX.Web.Controllers
{
    public class BannerController : BaseNopController
    {
        #region Fields
        private readonly IBannerService _bannerService;
        private readonly IPictureService _pictureService;
        //private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Constructors
        public BannerController(IBannerService bannerService, IPictureService pictureService, 
            //IWorkContext workContext, 
            ICacheManager cacheManager)
        {
            this._bannerService = bannerService;
            this._pictureService = pictureService;
            //this._workContext = workContext;
            this._cacheManager = cacheManager;
        }
        #endregion

        #region Methods

        public ActionResult BannerShow(int? position, int? categoryId, int? storeId, string viewName)
        {
            string keyCache = string.Format(PlanX.Web.Infrastructure.Cache.ModelCacheEventConsumer.BANNER_MODEL_KEY, position??0, categoryId??0, storeId??0, viewName);
            var modelCache = _cacheManager.Get(keyCache, () =>
            {
                var model = new ListBannerModel();                
                var banners = _bannerService.GetAllBanners(position:position??0);

                if (banners.Count() == 0)
                {
                    return null;
                }

                model.Banners = banners.Select(x =>
                {
                    var item = new BannerModel
                    {
                        Id = x.Id,
                        BannerType = x.Type,
                        Title = x.Title,
                    };
                    if (x.Type == 1)
                    {
                        item.Url = x.Url;
                        item.Target = x.Target;
                        item.PictureUrl = _pictureService.GetPictureUrl(x.PictureId, 0, true);
                    }
                    else
                    {
                        item.Description = x.Description;
                    }
                    return item;

                }).ToList();
                model.Position = banners.FirstOrDefault().Position;
                return model;
            });
            if (modelCache == null) return Content("");
            if (!string.IsNullOrWhiteSpace(viewName))
                return View(viewName, modelCache);
            return View(modelCache);
        }
        #endregion
    }
}
