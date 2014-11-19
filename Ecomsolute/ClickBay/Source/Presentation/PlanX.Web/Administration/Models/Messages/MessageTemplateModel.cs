using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using PlanX.Admin.Models.Stores;
using PlanX.Admin.Validators.Messages;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Localization;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Messages
{
    [Validator(typeof(MessageTemplateValidator))]
    public partial class MessageTemplateModel : BaseNopEntityModel, ILocalizedModel<MessageTemplateLocalizedModel>
    {
        public MessageTemplateModel()
        {
            Locales = new List<MessageTemplateLocalizedModel>();
            AvailableEmailAccounts = new List<EmailAccountModel>();
            AvailableStores = new List<StoreModel>();
        }


        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.AllowedTokens")]
        public string AllowedTokens { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.BccEmailAddresses")]
        [AllowHtml]
        public string BccEmailAddresses { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Subject")]
        [AllowHtml]
        public string Subject { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Body")]
        [AllowHtml]
        public string Body { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.IsActive")]
        [AllowHtml]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.EmailAccount")]
        public int EmailAccountId { get; set; }
        public IList<EmailAccountModel> AvailableEmailAccounts { get; set; }

        //Store mapping
        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public int[] SelectedStoreIds { get; set; }
        //comma-separated list of stores used on the list page
        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToStores")]
        public string ListOfStores { get; set; }



        public IList<MessageTemplateLocalizedModel> Locales { get; set; }
    }

    public partial class MessageTemplateLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.BccEmailAddresses")]
        [AllowHtml]
        public string BccEmailAddresses { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Subject")]
        [AllowHtml]
        public string Subject { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Body")]
        [AllowHtml]
        public string Body { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.EmailAccount")]
        public int EmailAccountId { get; set; }
    }
}