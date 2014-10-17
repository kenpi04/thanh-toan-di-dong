using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;
using Nop.Core.Domain.Catalog;
using Nop.Web.Models.Customer;
using Nop.Core.Domain.News;

namespace Nop.Web.Models.News
{
    public partial class FNewListModel : BaseNopEntityModel
    {

        public FNewListModel()
        {
            FNews = new List<FNewsModel>();
            FNews1 = new List<FNewsModel>();
            FNewsMainTopic = new List<FNewsModel>();
            FCategoryNews = new List<CategoryNews>();
            Fcategorygroup = new List<CategoryNews>();
            FNewsHotTopic = new List<FNewsModel>();
        }
        public List<FNewsModel> FNews { get; set; }
        public List<FNewsModel> FNews1 { get; set; }
        public List<FNewsModel> FNewsMainTopic { get; set; }
        public IList<CategoryNews> FCategoryNews { get; set; }
        public IList<CategoryNews> Fcategorygroup { get; set; }
        public List<FNewsModel> FNewsHotTopic { get; set; }
        
        public string NameCateNew { get; set; }
        public int PageSize { get; set; }
        public bool AllowCustomersToSelectPageSize { get; set; }
    }
    public partial class FNewsModel : BaseNopEntityModel
    {

        public FNewsModel()
        {
            DefaultPictureModel = new PictureModel();
            Comments = new List<NewsCommentModel>();
            AddNewComment = new AddNewsCommentModel();
            LoginModel = new LoginModel();
        }
        
        public string Name { get; set; }
        
        //vudh - quan ly url sename nop280
        public string SeName { get; set; }
        //----------------------------------


        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool FIsMainNew { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDateUtc { get; set; }
        //16/01/2013 Add STT
        public int CategoryNewsId { get; set; }
        public LoginModel LoginModel { set; get; }
        //picture
        public PictureModel DefaultPictureModel { get; set; }



        public string Title { get; set; }

        public string Short { get; set; }

        public string Full { get; set; }

        public bool AllowComments { get; set; }

        public int NumberOfComments { get; set; }

        public DateTime CreatedOn { get; set; }
        public bool Active { get; set; }
        public IList<NewsCommentModel> Comments { get; set; }
        public AddNewsCommentModel AddNewComment { get; set; }
    }
}