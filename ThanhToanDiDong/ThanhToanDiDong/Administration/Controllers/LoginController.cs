using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Services;
using ThanhToanDiDong.Admin.Models;
using ThanhToanDiDong.Admin.Filter;
using Domain.Entity;

namespace ThanhToanDiDong.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
       
        public ActionResult Index()
        {
            var model = new LoginModel();
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            int id = new UserService().Login(model.User, model.Password);
            if (id!=-1)
            {
                Session["User"]=id;
                return Redirect("/admin");
            }
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
            return View(model);
        }
       
        public ActionResult Register()
        {
            var model = new LoginModel();
            return View(model);
        }
      [HttpPost]
        public ActionResult Register(LoginModel model)
        {
            new UserService().InsertOrUpdate(new User
            {
                UserName=model.User,
                Password=Domain.Unilities.MD5Encrypt.MD5Hash(model.Password)
            });
            return View(model);
        }


    }
}
