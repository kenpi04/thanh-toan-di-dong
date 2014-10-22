using System;
using System.ComponentModel.DataAnnotations;

namespace ThanhToanDiDong.Admin.Models
{
    public class PromotionEventModel
    {
        public PromotionEventModel()
        {
            Id = 0;
        }
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedOn { get; set; }        
    }
}