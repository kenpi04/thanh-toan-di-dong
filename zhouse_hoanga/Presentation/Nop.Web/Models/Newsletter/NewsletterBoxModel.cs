using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Newsletter
{
    public partial class NewsletterBoxModel : BaseNopModel
    {
        [Required(ErrorMessage="Vui lòng nhập email")]
        [EmailAddress(ErrorMessage="Email không hợp lệ")]
        public string NewsletterEmail { get; set; }
    }
}