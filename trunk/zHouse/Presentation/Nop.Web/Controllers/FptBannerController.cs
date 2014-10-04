using System.Web.Mvc;
using Nop.Services.Directory;
using Nop.Web.Models.Common;
using Nop.Services.Media;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;

namespace Nop.Web.Controllers
{
    public partial class FptBannerController : BaseNopController
    {
        #region Fields
        private readonly IBannerService _bannerService;
        private readonly IPictureService _pictureService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Constructors
        public FptBannerController(IBannerService bannerService, IPictureService pictureService, IWorkContext workContext, ICacheManager cacheManager)
        {
            this._bannerService = bannerService;
            this._pictureService = pictureService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
        }
        #endregion

        #region Methods
       
        [ChildActionOnly]
        public ActionResult BannerShow(int?Position,int?StateId,int?StoreId,string viewName)
        {
            string keyCache = string.Format(Nop.Web.Infrastructure.Cache.ModelCacheEventConsumer.BANNER_MODEL_KEY, Position, StateId, StoreId, viewName);            
            var model1 = _cacheManager.Get(keyCache, () =>
            {
                var model = new FptBannerShowModel();
                int _stateId = 0, position = 0;
                if (Position.HasValue)
                    position = Position.Value;
                if (StateId.HasValue)
                    _stateId = StateId.Value;
                else
                {
                    string stateidString = Request.QueryString["stat"];
                    int.TryParse(stateidString, out _stateId);
                }
                var banners = System.Threading.Tasks.Task.Run(async () => await _bannerService.GetAllBannersAsync(position)).Result;

                if (banners.Count() == 0)
                {
                    banners = System.Threading.Tasks.Task.Run(async () => await _bannerService.GetAllBannersAsync(position)).Result;
                    if (banners.Count() == 0)
                        return null;//Content("");
                }

                model.Banners = banners.Select(x =>
                {
                    var item = new FptBannerShowModel.FptBannerItemModel
                    {
                        Id = x.Id,
                        BannerType = x.Type,
                        Name = x.Name,
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
            if (model1 == null) return Content("");
            if (!string.IsNullOrWhiteSpace(viewName))
                return View(viewName, model1);
            return View(model1);
        }
        #endregion
    }
}
