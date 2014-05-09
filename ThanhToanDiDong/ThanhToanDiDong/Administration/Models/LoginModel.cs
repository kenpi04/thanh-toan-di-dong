using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThanhToanDiDong.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="Vui lòng nhập tên đăng nhập")]
        public string User { get; set; }
        [Required(ErrorMessage="Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }
}