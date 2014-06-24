using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework.Security;
using Nop.Services.Directory;
using Nop.Web.Models.Common;
using Nop.Core.Domain.Directory;
using Nop.Services.Media;
using System.Linq;
using Nop.Core;

namespace Nop.Web.Controllers
{
    public partial class FptBannerController : BaseNopController
    {
        #region Fields
        private readonly IBannerService _bannerService;
        private readonly IPictureService _pictureService;
        private readonly IWorkContext _workContext;
        #endregion

        #region Constructors
        public FptBannerController(IBannerService bannerService, IPictureService pictureService, IWorkContext workContext)
        {
            this._bannerService = bannerService;
            this._pictureService = pictureService;
            this._workContext = workContext;
        }
        #endregion

        #region Methods
       
        [ChildActionOnly]
        public ActionResult BannerShow(int?Position,int?StateId,int?StoreId,string viewName)
        { 
            var model=new FptBannerShowModel();
             

            int _stateId = 0, position=0;
            if(Position.HasValue)
                position=Position.Value;
            if (StateId.HasValue)
                _stateId = StateId.Value;
            else
            {
                string stateidString = Request.QueryString["stat"];
                int.TryParse(stateidString, out _stateId);
            }
            var banners = _bannerService.GetAllBanners(position);

            if (banners.Count() == 0)
            {
                banners = _bannerService.GetAllBanners(position);
                if(banners.Count()==0)
                    return Content("");
            }
            
            model.Banners = banners.Select(x =>
            {
             var item=  new FptBannerShowModel.FptBannerItemModel{
                 Id=x.Id,
                 BannerType=x.Type,
                 Name=x.Name,
                
                
             };
             if (x.Type == 1)
             {
                 item.Url = x.Url;
                 item.Target = x.Target;
                 item.PictureUrl = _pictureService.GetPictureUrl(x.PictureId,0,true);
             }
             else
             {
                 item.Description = x.Description;
             }
             return item;

            }).ToList();
            model.Position = banners.FirstOrDefault().Position;
            if (!string.IsNullOrWhiteSpace(viewName))
                return View(viewName, model);
            return View(model);
        }
        #endregion



    }
}
