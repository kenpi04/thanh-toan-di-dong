﻿using System.Web.Mvc;
using FluentValidation.Attributes;
using PlanX.Web.Framework.Mvc;
using PlanX.Web.Validators.PrivateMessages;

namespace PlanX.Web.Models.PrivateMessages
{
    [Validator(typeof(SendPrivateMessageValidator))]
    public partial class SendPrivateMessageModel : BaseNopEntityModel
    {
        public int ToCustomerId { get; set; }
        public string CustomerToName { get; set; }
        public bool AllowViewingToProfile { get; set; }

        public int ReplyToMessageId { get; set; }

        [AllowHtml]
        public string Subject { get; set; }

        [AllowHtml]
        public string Message { get; set; }
    }
}