using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThanhToanDiDong.Models.Payment
{
    public class TopupModel
    {
        public TopupModel()
        {
            PriceList = new List<SelectListItem>();
        }
        [Required(ErrorMessage="Nhập số điện thoại")]
        [DataType(DataType.PhoneNumber,ErrorMessage="Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [Required(ErrorMessage="Vui lòng chọn mệnh giá")]
        public int PriceListId { get; set; }
        public IList<SelectListItem> PriceList { get; set; }
       
    }
}