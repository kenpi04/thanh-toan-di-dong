using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ThanhToanDiDong.Models.Payment
{
    public class TopupModel
    {
        public TopupModel()
        {
            PriceList = new List<SelectListItem>();
        }
        /// <summary>
        /// so dien thoai
        /// </summary>
        [Required(ErrorMessage="Nhập số điện thoại")]       
        [MinLength(10,ErrorMessage=("Số điện thoại ít nhất 10 số"))]
        [MaxLength(12, ErrorMessage = ("Số điện thoại không quá 12 số"))]
        public string Phone { get; set; }
        /// <summary>
        /// danh sach menh gia
        /// </summary>
        [Required(ErrorMessage="Vui lòng chọn mệnh giá")]
        public int PriceListId { get; set; }
        public IList<SelectListItem> PriceList { get; set; }
        /// <summary>
        /// Hinh thuc thue bao:1:tra truoc -2:tra sau
        /// </summary>
        [Required(ErrorMessage="Vui lòng chọn hình thức thuê bao")]
        public int PhoneType { get; set; }
        public string Email { get; set; }
       
    }
}