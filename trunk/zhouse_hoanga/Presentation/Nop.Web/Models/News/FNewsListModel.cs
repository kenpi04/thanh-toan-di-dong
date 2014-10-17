using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.News
{
    public partial class FProductComentListModel : BaseNopEntityModel
    {

        public FProductComentListModel()
        {
            ProductsComment = new List<FProductComentModel>();
        }
        public List< FProductComentModel> ProductsComment { get; set; }
    }


    public partial class FProductComentModel : BaseNopEntityModel
    {

        public FProductComentModel()
        {
            DefaultPictureModel = new PictureModel();
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        //picture
        public PictureModel DefaultPictureModel { get; set; }
    }
}