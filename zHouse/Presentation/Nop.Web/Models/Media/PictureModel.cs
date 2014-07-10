﻿using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Media
{
    public partial class PictureModel : BaseNopModel
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }

        public string Description { get; set; }

        public int PictureId { get; set; }

        public int Id { get; set; }
    }
}