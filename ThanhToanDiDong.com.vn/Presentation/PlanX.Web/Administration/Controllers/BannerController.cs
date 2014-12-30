using System;
using System.Linq;
using System.Web.Mvc;
using PlanX.Admin.Models.Topics;
using PlanX.Core.Domain.Topics;
using PlanX.Services.Localization;
using PlanX.Services.Security;
using PlanX.Services.Common;
using PlanX.Web.Framework.Controllers;
using Telerik.Web.Mvc;
using PlanX.Admin.Models.Common;
using PlanX.Core.Domain.Common;
using PlanX.Services.Logging;
using PlanX.Services.Media;

namespace PlanX.Admin.Controllers
{
    [AdminAuthorize]
    public partial class BannerController : BaseNopController
    {
        #region Fields

        private readonly IBannerService _bannerService;
        private readonly IPermissionService _permissionService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPictureService _pictureService;

        #endregion Fields

        #region Constructors

        public BannerController(IBannerService bannerService,
            IPermissionService permissionService,
            AdminAreaSettings adminAreaSettings,
            ILocalizationService localizationService,
            ICustomerActivityService customerActivityService,
            IPictureService pictureService
            )
        {
            this._bannerService = bannerService;
            this._permissionService = permissionService;
            this._adminAreaSettings = adminAreaSettings;
            this._localizationService = localizationService;
            this._customerActivityService = customerActivityService;
            this._pictureService = pictureService;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected void UpdatePictureSeoNames(Banner banner)
        {
            var picture = _pictureService.GetPictureById(banner.PictureId);
            if (picture != null)
                _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(banner.Name));
        }

        #endregion

        #region List

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            return View();
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var banners = _bannerService.GetAllBanners(0, 0, true);
            var gridModel = new GridModel<BannerModel>
            {
                Data = banners.Select(x => x.ToModel()),
                Total = banners.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion

        #region Create / Edit / Delete

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var model = new BannerModel();

            model.Published = true;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Create(BannerModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var banner = model.ToEntity();
                banner.StartDate = model.StartDate;
                banner.EndDate = model.EndDate;

                _bannerService.InsertBanner(banner);

                UpdatePictureSeoNames(banner);

                //activity log
                _customerActivityService.InsertActivity("AddNewBanner", _localizationService.GetResource("ActivityLog.AddNewBanner"), banner.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Banners.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = banner.Id }) : RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var banner = _bannerService.GetBannerById(id);
            if (banner == null)
                //No FrtBanner found with the specified id
                return RedirectToAction("List");

            var model = banner.ToModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Edit(BannerModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var banner = _bannerService.GetBannerById(model.Id);
            if (banner == null)
                //No FrtBanner found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                int prevPictureId = banner.PictureId;
                banner = model.ToEntity(banner);
                _bannerService.UpdateBanner(banner);
                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != banner.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }
                //update picture seo file name
                UpdatePictureSeoNames(banner);

                //activity log
                _customerActivityService.InsertActivity("EditBanner", _localizationService.GetResource("ActivityLog.EditBanner"), banner.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Banners.Updated"));
                return continueEditing ? RedirectToAction("Edit", banner.Id) : RedirectToAction("List");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var banner = _bannerService.GetBannerById(id);
            if (banner == null)
                //No FrtBanner found with the specified id
                return RedirectToAction("List");

            _bannerService.DeleteBanner(banner);

            //activity log
            _customerActivityService.InsertActivity("DeleteBanner", _localizationService.GetResource("ActivityLog.DeleteBanner"), banner.Name);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Banners.Deleted"));
            return RedirectToAction("List");
        }

        #endregion
    }
}
