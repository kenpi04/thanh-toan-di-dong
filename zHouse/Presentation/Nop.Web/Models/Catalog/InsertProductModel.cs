using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Web.Models.Media;
using FluentValidation.Attributes;
using Nop.Web.Validators.Catalog;

namespace Nop.Web.Models.Catalog
{
     [Validator(typeof(ProductValidator))]
    public class InsertProductModel:BaseNopEntityModel
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



        }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public decimal Price { get; set; }
        public int CateId { get; set; }
        public int Area { get; set; }

        public int Width { get; set; }
        public int Dept { get; set; }
        public string NumberOfHome  { get; set; }
        public string Street { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Desription { get; set; }
        public List<int> SelectedOptionAttributes { get; set; }

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

    }
}