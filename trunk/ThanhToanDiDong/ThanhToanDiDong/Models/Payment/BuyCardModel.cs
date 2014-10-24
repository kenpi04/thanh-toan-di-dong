using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThanhToanDiDong.Models.Payment
{
    public class BuyCardModel
    {
        public IList<CateCard> CateCards { get; set; }
        [Required(ErrorMessage="Vui lòng chọn thẻ")]
        public int CardId { get; set; }
        /// <summary>
        /// so the mua: default = 1
        /// </summary>
        [Range(1,5,ErrorMessage="Số thẻ được mua ít nhất 1 và nhiều nhất là 5")]
        public int Quantity { get; set; }
        /// <summary>
        /// email nhan thẻ
        /// </summary>
        [EmailAddress(ErrorMessage="Email không đúng")]
        public string Email { get; set; }
        /// <summary>
        /// 1: the dien thoai / 2: the game
        /// </summary>
        public bool IsCardGame { get; set; }
        public class CateCard {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
        }
      
    }
}