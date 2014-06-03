using System.Collections.Generic;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.News
{
    public partial class FNewsItemListModel 
    {

        public FNewsItemListModel()
        {
            PagingFilteringContext1 = new NewsPagingFilteringModel();
           // PagingFilteringContext2 = new NewsPagingFilteringModel1();
            NewsItems = new List<NewsItemModel>();
       
            FNewListByCateHv1 = new FNewListModel();
            FNewListByCateHv2 = new FNewListModel();
        }
        public int CateId { get; set; }
        public int WorkingLanguageId { get; set; }
        
        public int PageIndex { get; set; }
        public int Pagesize { get; set; }

        public NewsPagingFilteringModel PagingFilteringContext1{ get; set; }
        //public NewsPagingFilteringModel1 PagingFilteringContext2{ get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }


        //vudh list baiviet chia theo cate
        public FNewListModel FNewListByCateHv1 { get; set; }
        public FNewListModel FNewListByCateHv2 { get; set; }






        public string CateName { get; set; }
    }
}