﻿using System.Web.Mvc;
using PlanX.Core;
using PlanX.Core.Domain.Customers;
using PlanX.Core.Plugins;
using PlanX.Plugin.ExternalAuth.OpenId.Core;
using PlanX.Plugin.ExternalAuth.OpenId.Models;
using PlanX.Services.Authentication.External;
using PlanX.Web.Framework;

namespace PlanX.Plugin.ExternalAuth.OpenId.Controllers
{
    public class ExternalAuthOpenIdController : Controller
    {
        private readonly IOpenIdProviderAuthorizer _openIdProviderAuthorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly IStoreContext _storeContext;
        private readonly IPluginFinder _pluginFinder;

        public ExternalAuthOpenIdController(IOpenIdProviderAuthorizer openIdProviderAuthorizer,
            IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            IStoreContext storeContext,
            IPluginFinder pluginFinder)
        {
            this._openIdProviderAuthorizer = openIdProviderAuthorizer;
            this._openAuthenticationService = openAuthenticationService;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._storeContext = storeContext;
            this._pluginFinder = pluginFinder;
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View("PlanX.Plugin.ExternalAuth.OpenId.Views.ExternalAuthOpenId.PublicInfo");
        }

        public ActionResult Login(string returnUrl)
        {
            var processor = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName("ExternalAuth.OpenId");
            if (processor == null ||
                !processor.IsMethodActive(_externalAuthenticationSettings) ||
                !processor.PluginDescriptor.Installed ||
                !_pluginFinder.AuthenticateStore(processor.PluginDescriptor, _storeContext.CurrentStore.Id))
                throw new NopException("OpenID module cannot be loaded");

            if (!_openIdProviderAuthorizer.IsOpenIdCallback)
            {
                var viewModel = new LoginModel();
                TryUpdateModel(viewModel);
                _openIdProviderAuthorizer.EnternalIdentifier = viewModel.ExternalIdentifier;
            }

            var result = _openIdProviderAuthorizer.Authorize(returnUrl);
            switch (result.AuthenticationStatus)
            {
                case OpenAuthenticationStatus.Error:
                    {
                        if (!result.Success)
                            foreach (var error in result.Errors)
                                ExternalAuthorizerHelper.AddErrorsToDisplay(error);

                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AssociateOnLogon:
                    {
                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AutoRegisteredEmailValidation:
                    {
                        //result
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation });
                    }
                case OpenAuthenticationStatus.AutoRegisteredAdminApproval:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.AdminApproval });
                    }
                case OpenAuthenticationStatus.AutoRegisteredStandard:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                    }
                default:
                    break;
            }

            if (result.Result != null) return result.Result;
            return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
    }
}