using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework.Mvc;



namespace Nop.Web.Models.Catalog
{
    public partial class TopMenuModel : BaseNopModel
    {
        public TopMenuModel()
        {
            Categories = new List<CategorySimpleModel>();
            Districts = new List<SelectListItem>();
            Districts2 = new List<SelectListItem>();
            PriceRange = GetPrice(false);
            PriceRangeRent = GetPrice(true);
            CategoriesNews = new List<SelectListItem>();
            Topics = new List<SelectListItem>();
        }

        public IList<CategorySimpleModel> Categories { get; set; }
        public IList<SelectListItem> Districts { get; set; }
        public IList<SelectListItem> Districts2 { get; set; }     

        public bool BlogEnabled { get; set; }
        public bool RecentlyAddedProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public List<SelectListItem> PriceRange { get; set; }
        public List<SelectListItem> PriceRangeRent { get; set; }
        public List<SelectListItem> CategoriesNews { get; set; }
        public List<SelectListItem> Topics { get; set; }

        /// <summary>
        /// Get danh sach gia ban hoac cho thue
        /// </summary>
        /// <param name="isForRent">la cho thue</param>
        /// <returns>Prices collections</returns>
        private List<SelectListItem> GetPrice(bool isForRent = false)
        {
            List<SelectListItem> listItems = null;
            if(!isForRent)//ban nha
            {
                listItems = new List<SelectListItem>(){
                    new SelectListItem{Value = "0-0", Text ="Chọn mức giá"},
                    new SelectListItem{Value = "0-1000", Text ="Dưới 1 tỷ"},
                    new SelectListItem{Value = "1000-1500", Text ="1 tỷ ~ 1.5 tỷ"},
                    new SelectListItem{Value = "1500-2000", Text ="1.5 tỷ ~ 2 tỷ"},
                    new SelectListItem{Value = "2000-2500", Text ="2 tỷ ~ 2.5 tỷ"},
                    new SelectListItem{Value = "2500-3000", Text ="2.5 tỷ ~ 3 tỷ"},
                    new SelectListItem{Value = "3000-0", Text ="Trên 3 tỷ"},
                };
            }
            else //cho thue
            {
                listItems = new List<SelectListItem>(){
                    new SelectListItem{Value = "0-0", Text ="Chọn mức giá"},
                    new SelectListItem{Value = "0-1", Text ="Dưới 1 triệu"},
                    new SelectListItem{Value = "1-2", Text ="1 triệu ~ 2 triệu"},
                    new SelectListItem{Value = "2-3", Text ="2 triệu ~ 3 triệu"},
                    new SelectListItem{Value = "3-5", Text ="3 triệu ~ 5 triệu"},
                    new SelectListItem{Value = "5-10", Text ="5 triệu ~ 10 triệu"},
                    new SelectListItem{Value = "10-20", Text ="10 triệu ~ 20 triệu"},
                    new SelectListItem{Value = "20-30", Text ="20 triệu ~ 30 triệu"},
                    new SelectListItem{Value = "30-0", Text ="Trên 30 triệu"},
                };
            }
            return listItems;
        }
    }    
}