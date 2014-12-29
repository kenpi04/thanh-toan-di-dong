﻿using System.Web.Mvc;
using PlanX.Core;
using PlanX.Core.Caching;
using PlanX.Services.Localization;
using PlanX.Services.Topics;
using PlanX.Web.Infrastructure.Cache;
using PlanX.Web.Models.Topics;
using System.Collections.Generic;
using System.Linq;

namespace PlanX.Web.Controllers
{
    public partial class TopicController : BaseNopController
    {
        #region Fields

        private readonly ITopicService _topicService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructors

        public TopicController(ITopicService topicService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICacheManager cacheManager)
        {
            this._topicService = topicService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._localizationService = localizationService;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected TopicModel PrepareTopicModel(string systemName)
        {
            //load by store
            var topic = _topicService.GetTopicBySystemName(systemName, _storeContext.CurrentStore.Id);
            if (topic == null)
                return null;

            var model = new TopicModel()
            {
                Id = topic.Id,
                SystemName = topic.SystemName,
                IncludeInSitemap = topic.IncludeInSitemap,
                IsPasswordProtected = topic.IsPasswordProtected,
                Title = topic.IsPasswordProtected ? "" : topic.GetLocalized(x => x.Title),
                Body = topic.IsPasswordProtected ? "" : topic.GetLocalized(x => x.Body),
                MetaKeywords = topic.GetLocalized(x => x.MetaKeywords),
                MetaDescription = topic.GetLocalized(x => x.MetaDescription),
                MetaTitle = topic.GetLocalized(x => x.MetaTitle),
            };
            return model;
        }

        [NonAction]
        protected List<TopicModel> PrepareListTopicModel(int groupId)
        {
            //load by store
            var topics = _topicService.GetAllTopics(_storeContext.CurrentStore.Id, 0);
            if (topics == null)
                return null;

            var model = topics.Select(topic =>
            new TopicModel
             {
                 Id = topic.Id,
                 SystemName = topic.SystemName,
                 IncludeInSitemap = topic.IncludeInSitemap,
                 IsPasswordProtected = topic.IsPasswordProtected,
                 Title = topic.IsPasswordProtected ? "" : topic.Title,
             }).ToList();

            return model;
        }

        #endregion

        #region Methods

        public ActionResult TopicDetails(string systemName)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.TOPIC_MODEL_KEY, systemName, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () => PrepareTopicModel(systemName));

            if (cacheModel == null)
                return RedirectToRoute("HomePage");
            return View("TopicDetails", cacheModel);
        }

        public ActionResult TopicDetailsPopup(string systemName)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.TOPIC_MODEL_KEY, systemName, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () => PrepareTopicModel(systemName));

            if (cacheModel == null)
                return RedirectToRoute("HomePage");

            ViewBag.IsPopup = true;
            return View("TopicDetails", cacheModel);
        }

        [ChildActionOnly]
        public ActionResult TopicBlock(string systemName)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.TOPIC_MODEL_KEY, systemName, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cacheModel = _cacheManager.Get(cacheKey, () => PrepareTopicModel(systemName));

            if (cacheModel == null)
                return Content("");

            return PartialView(cacheModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Authenticate(int id, string password)
        {
            var authResult = false;
            var title = string.Empty;
            var body = string.Empty;
            var error = string.Empty;

            var topic = _topicService.GetTopicById(id);

            if (topic != null)
            {
                if (topic.Password != null && topic.Password.Equals(password))
                {
                    authResult = true;
                    title = topic.GetLocalized(x => x.Title);
                    body = topic.GetLocalized(x => x.Body);
                }
                else
                {
                    error = _localizationService.GetResource("Topic.WrongPassword");
                }
            }
            return Json(new { Authenticated = authResult, Title = title, Body = body, Error = error });
        }

        public ActionResult TopicBlockLink(int groupId, string viewName)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.TOPIC_LIST_MODEL_KEY, groupId, viewName);
            var cacheModel = _cacheManager.Get(cacheKey, () => PrepareListTopicModel(groupId));

            if (cacheModel == null)
                return Content("");

            if(!string.IsNullOrEmpty(viewName))
                return PartialView(viewName,cacheModel);
            return PartialView(cacheModel);
        }

        #endregion
    }
}
