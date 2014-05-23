using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Web.Models.Media;
using FluentValidation.Attributes;
using Nop.Web.Validators.Catalog;
using Nop.Web.Framework;

namespace Nop.Web.Models.Catalog
{
    [Validator(typeof(ProductValidator))]
    public class InsertProductModel : BaseNopEntityModel
    {
        public InsertProductModel()
        {
            SelectedOptionAttributes = new List<int>();
            Categories = new List<SelectListItem>();
            NumberFloors = new List<SelectListItem>();
            BedRooms = new List<SelectListItem>();
            BadRooms = new List<SelectListItem>();
            Directors = new List<SelectListItem>();
            Facilities = new List<SelectListItem>();
            Environments = new List<SelectListItem>();
            PictureModels = new List<PictureModel>();
            Districts = new List<SelectListItem>();
            PictureIds = new List<int>();



        }
        [NopResourceDisplayName("Product.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Product.Fields.ContactName")]
        public string ContactName { get; set; }
        [NopResourceDisplayName("Product.Fields.ContactPhone")]
        public string ContactPhone { get; set; }
        [NopResourceDisplayName("Product.Fields.Email")]
        public string Email { get; set; }
        [NopResourceDisplayName("Product.Fields.Price")]
        public decimal Price { get; set; }
        [NopResourceDisplayName("Product.Fields.CateId")]
        public int CateId { get; set; }
        [NopResourceDisplayName("Product.Fields.Area")]
        public decimal Area { get; set; }
        [NopResourceDisplayName("Product.Fields.Width")]
        public decimal Width { get; set; }
        [NopResourceDisplayName("Product.Fields.Dept")]
        public decimal Dept { get; set; }
        [NopResourceDisplayName("Product.Fields.NumberOfHome")]
        public string NumberOfHome { get; set; }
        [NopResourceDisplayName("Product.Fields.StreetId")]
        public int StreetId { get; set; }
        [NopResourceDisplayName("Product.Fields.DistrictId")]
        public int DistrictId { get; set; }
        [NopResourceDisplayName("Product.Fields.WardId")]
        public int WardId { get; set; }
      
        [NopResourceDisplayName("Product.Fields.Desription")]
        [AllowHtml]
        public string Desription { get; set; }
        [NopResourceDisplayName("Product.Fields.FullAddress")]
        public string FullAddress { get; set; }
       
       

        public List<int> SelectedOptionAttributes { get; set; }
        public List<int> PictureIds { get; set; }

        #region select
        public IList<SelectListItem> Categories { get; set; }
        public IList<SelectListItem> NumberFloors { get; set; }
        public IList<SelectListItem> BadRooms { get; set; }
        public IList<SelectListItem> BedRooms { get; set; }
        public IList<SelectListItem> Directors { get; set; }
        public IList<SelectListItem> Facilities { get; set; }
        public IList<SelectListItem> Environments { get; set; }
        public IList<PictureModel> PictureModels { get; set; }
        public IList<SelectListItem> Districts { get; set; }
        #endregion


        public decimal AreaUse { get; set; }
    }
}