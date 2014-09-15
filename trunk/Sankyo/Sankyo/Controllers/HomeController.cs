using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Domain.Services;
using Sankyo.Models;
using Sankyo.Filters;

namespace Sankyo.Controllers
{
    public class HomeController : Controller
    {
        TopicServices _topicService;
        UserServices _userService;
        public HomeController()
        {
            _topicService = new TopicServices();
            _userService = new UserServices();
           
            var cookies = Request.Cookies["language-id"];
            if (cookies != null)
            {
                int.TryParse(cookies.Value, out Common.CurrentLanguageId);

            }
            else
                Common.CurrentLanguageId = 1;
        }

        public ActionResult Index(string sename)
        {
            var model = new TopicModel();
            if (string.IsNullOrWhiteSpace(sename))
            {
                var topicHome = _topicService.GetPage().FirstOrDefault(x => x.IsHomePage);
                if (topicHome != null)
                    sename = topicHome.Name;
            }
          
                var topic = _topicService.GetByName(sename);
                if (topic == null)
                    return HttpNotFound("topic không tồn tại");
                model.Id = topic.Id;
                model.Title = topic.Title;
                model.Content = topic.Content;           
            return View(model);
        }

        public ActionResult About()
        {


            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Login()
        {
            var model = new UserModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            try
            {
                var user = _userService.Login(model.UserName, model.Password);
                if (user == null)
                {
                    ViewBag.Error = "Tên đăng nhập mật khẩu không tồn tại!";
                    model = new UserModel();
                    return View(model);
                }
                Session["SessionUser"] = user;
                return RedirectToRoute("ListTopic");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi!" + ex.Message;
                return View(new UserModel());
            }
        }

        [Auth]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }
        [Auth]
        public ActionResult ListTopic()
        {
            var topics = _topicService.GetPage().ToList();
            return View(topics);
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            var user = new UserServices();
            if (user != null)
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
            }
            try
            {
                var entity = new User
                {
                    Username = model.UserName,
                    Password = model.Password
                };
                _userService.InsertOrUpdate(entity);
                model = new RegisterModel();
                ViewBag.Error = "Thêm thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(model);
        }
        [Auth]
        public ActionResult AddTopic(int id = 0)
        {
            var model = new TopicModel();
            ViewBag.Action = "Thêm trang mới";
            if (id > 0)
            {
                var entity = _topicService.GetById(id);
                if (entity == null)
                    throw new Exception("Topic null");
                model = new TopicModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Content = entity.Content,
                    Title = entity.Title,
                    AddToMenu = entity.AddToMenu,
                    DisplayOrder = entity.DisplayOrder
                };
                ViewBag.Action = "Sửa trang";
            }
            return View(model);
        }

        [Auth]
        public ActionResult DeleteTopic(int id)
        {
            var entity = _topicService.GetById(id);
            if (entity == null)
                throw new Exception("Topic null");
            _topicService.Delete(entity);
            return RedirectToAction("ListTopic");
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTopic(TopicModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Topic();                    
                    if (model.Id > 0)
                    {
                        entity = _topicService.GetById(model.Id);
                        if (entity == null)
                        {
                            ViewBag.Error = "Page không tồn tại.";
                            return View(model);
                        }                       
                    }
                    entity.Title = model.Title;
                    entity.Name = RemoveSign4VietnameseString(model.Title);
                    entity.Content = model.Content;
                    //entity.Id=model.Id;
                    entity.AddToMenu = model.AddToMenu;
                    entity.DisplayOrder = model.DisplayOrder;
                    _topicService.InsertOrUpdate(entity);
                    ViewBag.Error = "Cập nhật thành công.";
                    return RedirectToRoute("EditTopic", new { id = entity.Id });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Lỗi! " + ex.Message;
                }
            }
            model = new TopicModel();
            return View(model);
        }

        public ActionResult Menu()
        {
            var menuList = _topicService.GetPage().Where(p => p.AddToMenu&&p.LanguageId.Equals(Common.CurrentLanguageId)).OrderBy(p => p.DisplayOrder).Select(x => new TopicModel { Id = x.Id, Name = x.Name, Title = x.Title });
            return View(menuList);
        }
        private static readonly string[] VietnameseSigns = new string[]{
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
        };
        public string RemoveSign4VietnameseString(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            str = str.Replace(' ', '-');
            char[] myChar = new char[]{'/','\\', '.', '*', ',', '?', '!', '@', '#', '$', '%', '^', '&', '(', ')', '<', '>'};
            foreach(var c in myChar)
            {
                int index = str.IndexOf(c);
                if(index >-1)
                    str = str.Remove(index,1);
            }
            return str;
        }
        public ActionResult ChangeLanguage(int id)
        {
            HttpCookie c = new HttpCookie("language-id");
            c.Expires = DateTime.Now.AddDays(1);
            c.Value = id.ToString();
            Response.Cookies.Add(c);
            return Json("OK");
        }
    }
}
