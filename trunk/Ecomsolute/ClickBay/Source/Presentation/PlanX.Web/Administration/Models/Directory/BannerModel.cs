using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;
using System;

namespace PlanX.Admin.Models.Directory
{
    public partial class BannerModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.PictureId")]
        [UIHint("Picture")]
        public int PictureId { get; set; }


        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.Position")]
        public int Position { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.Type")]
        public int Type { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.Target")]
        public string Target { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.Url")]
        public string Url { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Banner.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }
    }
}