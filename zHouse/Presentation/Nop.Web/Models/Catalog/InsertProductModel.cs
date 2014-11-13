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
using System.ComponentModel.DataAnnotations;

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
            BathRooms = new List<SelectListItem>();
            Directors = new List<SelectListItem>();
            Facilities = new List<SelectListItem>();
            Environments = new List<SelectListItem>();
            PictureModels = new List<PictureModel>();
            Districts = new List<SelectListItem>();
            PictureIds = new List<PictureUploadModel>();
            NumberBlocks = new List<SelectListItem>();
            PhapLy = new List<SelectListItem>();
            ThichHop = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
        }
        public string NoteFacilities { get; set; }
        public string NoteEnvironments { get; set; }
        public string NoteThichHop { get; set; }

        public string NotePhapLy { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Name")]
        public string Name { get; set; }        
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.ContactName")]
        public string ContactName { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.ContactPhone")]
        public string ContactPhone { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Email")]
        public string Email { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Price")]
        public decimal Price { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.CateId")]
        public int CateId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Area")]
        [Range(1,999999, ErrorMessage="Diện tích phải lớn hơn 0 và nhỏ hơn 100.000")]
        public decimal Area { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Width")]
        [Range(1, 999999, ErrorMessage = "Chiều rộng phải lớn hơn 0 và nhỏ hơn 100.000")]
        public decimal Width { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Dept")]
        [Range(1, 999999, ErrorMessage = "Chiều dài phải lớn hơn 0 và nhỏ hơn 100.000")]
        public decimal Dept { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.NumberOfHome")]
        public string NumberOfHome { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.StreetId")]
        public int StreetId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.DistrictId")]
        public int DistrictId { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.WardId")]
        public int WardId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Description")]
        [AllowHtml]
        public string FullDescription { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.FullAddress")]
        public string FullAddress { get; set; }
        /// <summary>
        /// dac diem noi bat / du an: Ten goi khac
        /// </summary>
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.UserAgreementText")]
        public string DacDiemNoiBat { get; set; }
        /// <summary>
        /// Khuyen mai / du an:ten du an
        /// </summary>
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Promotion")]
        public string Promotion { get; set; }
        /// <summary>
        /// Duong truoc nha
        /// </summary>
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Length")]
        public decimal Length { get; set; }
        /// <summary>
        /// No hau
        /// </summary>
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Weight")]
        public decimal Weight { get; set; }
        /// <summary>
        /// Ten du an: RequiredProductIds
        /// </summary>
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.ProjectName")]
        public string ProjectName { get; set; }

        public List<int> SelectedOptionAttributes { get; set; }
        public List<PictureUploadModel> PictureIds { get; set; }

        #region select
        public IList<SelectListItem> Categories { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.NumberFloors")]
        public IList<SelectListItem> NumberFloors { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.BadRooms")]
        public IList<SelectListItem> BathRooms { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.BedRooms")]
        public IList<SelectListItem> BedRooms { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Directors")]
        public IList<SelectListItem> Directors { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Facilities")]
        public IList<SelectListItem> Facilities { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Environments")]
        public IList<SelectListItem> Environments { get; set; }
        public IList<PictureModel> PictureModels { get; set; }
        public IList<SelectListItem> Districts { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.PhapLy")]
        public IList<SelectListItem> PhapLy { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.NumberBlocks")]
        public IList<SelectListItem> NumberBlocks { get; set; }
        #endregion

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.AreaUse")]
        public decimal AreaUse { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.AvailableStartDateTime")]
        public DateTime? AvailableStartDateTime { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.AvailableEndDateTime")]        
        public DateTime? AvailableEndDateTime { get; set; }
        public string LatTiTudeGoogleMap { get; set; }
        /// <summary>
        /// Gets or sets the Longtitude google map - vi do
        /// </summary>
        public string LongTiTudeGoogleMap { get; set; }

        public class PictureUploadModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
        public int ProductType { get; set; }
        public List<SelectListItem> ThichHop { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.StatusList")]
        public List<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// thoi gian khoi cong
        /// </summary>
        public string StartConstructionDate { get; set; }
        /// <summary>
        /// Thoi gian hoan thanh
        /// </summary>
        public string FinishConstructionDate { get; set; }
        /// <summary>
        /// chu dau tu: ManufacturerPartNumber
        /// </summary>
        public string ChuDauTu { get; set; }
        /// <summary>
        /// Don vi thi cong: Gtin
        /// </summary>
        public string DonViThiCong { get; set; }
        public Nop.Web.Models.Customer.CustomerNavigationModel NavigationModel { get; set; }
    }
}