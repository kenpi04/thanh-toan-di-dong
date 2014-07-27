using Nop.Admin.Models.Messages;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Services.Configuration;
using Nop.Services.Messages;
using Nop.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace Nop.Admin.Controllers
{
    public class MessageController : Controller
    {
        #region Fields
        private readonly IMessagesService _messagesService;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        //private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors
        public MessageController(
             ISettingService settingService,
            IPermissionService permissionService, 
            //ILocalizationService localizationService,
            IMessagesService messagesService)
		{
            this._messagesService = messagesService;
            this._settingService = settingService;
            this._permissionService = permissionService;
            //this._localizationService = localizationService;
		}

        #endregion

        public ActionResult List()
        {
            var model = new MessageListModel();
            model.ToDate = DateTime.Now;
            model.FromDate = DateTime.Now.AddYears(-1);
            model.Type.Add(new SelectListItem() { Value = "0", Text = "Tất cả", Selected = true });
            model.Type.Add(new SelectListItem() { Value = Convert.ToString((int)MessageType.Book), Text = "Đặt lịch" });
            model.Type.Add(new SelectListItem() { Value = Convert.ToString((int)MessageType.Contact), Text = "Liên hệ" });
            model.Type.Add(new SelectListItem() { Value = Convert.ToString((int)MessageType.EmailAFriend), Text = "Gửi tin nhắn" });
            model.Type.Add(new SelectListItem() { Value = Convert.ToString((int)MessageType.WrongNews), Text = "Báo tin sai" });
            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult MessageSelect(GridCommand command, MessageListModel model)
        {
            var mess = _messagesService.GetMessage(model.FromDate, model.ToDate, model.SearchType, model.SearchName).Select(x => x.ToModel()).ToList();
            var page = new PagedList<MessageModel>(mess, command.Page - 1, command.PageSize);

            var gird = new GridModel<MessageModel>
            {
                Data = page,
                Total = page.TotalCount
            };

            return new JsonResult
            {
                Data = gird
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult MessageDelete(int id, GridCommand command, MessageListModel model)
        {
            var mess = _messagesService.GetMessageById(id);
            if (mess == null)
                throw new ArgumentException("No mess found with the specified id");
            mess.Deleted = true;
            _messagesService.UpdateMessage(mess);
            return MessageSelect(command, model);
        }
    }
}