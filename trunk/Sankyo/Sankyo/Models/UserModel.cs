using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Sankyo.Models
{
    public class UserModel
    {
        [MaxLength(20)]
        [Required(ErrorMessage="Nhập username")]
        public string UserName { get; set; }
        [MaxLength(20)]
         [Required(ErrorMessage="Nhập mật khẩu")]
        public string Password { get; set; }
       
    }
    public class RegisterModel
    {
        [MaxLength(20)]
        [Required(ErrorMessage = "Nhập username")]
        public string UserName { get; set; }
        [MaxLength(20)]
        [Required(ErrorMessage = "Nhập mật khẩu")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu không giống")]
        public string RePassword { get; set; }
    }
}