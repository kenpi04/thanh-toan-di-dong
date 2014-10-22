using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Domain.Services;
using ThanhToanDiDong.Admin.Models;
using ThanhToanDiDong.Admin.Filter;

namespace ThanhToanDiDong.Admin.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        OrderService _orderService = new OrderService();
        ProviderService _providerService = new ProviderService();
        ServicesService _service = new ServicesService();
        CardMobileService _cardMobileService = new CardMobileService();
        PromotionEventService _promotionEventService = new PromotionEventService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderList()
        {
            var model = new OrderListModel();
            model.CateMobiles = new CategoryCardMobileService().GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.PagingModel = new PagingInfo
            {
                CurrentPage = 1,
                ItemsPerPage = 1
            };
            return View(model);

        }
        [HttpGet]
        public ActionResult OrderPage(OrderListModel model)
        {
            int? cate = model.CateId;
            if (model.CateId == 0)
                cate = null;
            DateTime? startDateValue = (model.StartDate == null) ? null
                            : (DateTime?)model.StartDate;

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)model.EndDate;

            var order = _orderService.GetPage(cate, true, startDateValue, endDateValue, OrderType.CARD, (OrderStatusEnum)model.Status, model.PagingModel.CurrentPage, model.PagingModel.ItemsPerPage);
            return PartialView("_OrderList", order);
        }
        public ActionResult BillList()
        {
            var model = new OrderListModel();
            model.PagingModel = new PagingInfo
            {
                CurrentPage = 1,
                ItemsPerPage = 1
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult BillPage(OrderListModel model)
        {

            DateTime? startDateValue = (model.StartDate == null) ? null
                            : (DateTime?)model.StartDate;

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)model.EndDate;

            var order = _orderService.GetPage(null, false, startDateValue, endDateValue, OrderType.BILLING, (OrderStatusEnum)model.Status, model.PagingModel.CurrentPage, model.PagingModel.ItemsPerPage).Select(x =>
            {
                var provider = _providerService.GetById(x.ProviderId);
                return new OrderItemModel
                {
                    DateCreate = x.CreatedOn,
                    Id = x.Id,
                    ProviderName = provider.ProviderName,
                    ServiceName = provider.Service.ServiceName,
                    Status = Enum.GetName(typeof(OrderStatusEnum), x.OrderStatusId),
                    TotalPrice = string.Format("{0:0,0}", x.TotalAmount)

                };

            });
            return PartialView("_BillList", order);

        }

        public ActionResult Note(int id)
        {
            var note = new OrderNoteService().GetAll(x => x.OrderId == id);
            return View(note);
        }
        public ActionResult ChangePrice()
        {
            var model = new List<CardMobile>();
            model = _cardMobileService.GetAll().ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditPrice(int id, decimal price)
        {
            var card = _cardMobileService.GetById(id);
            if (card == null)
                return Json(new
                {
                    success = false,
                    mgs = "lỗi ! ko tìm thấy card này"
                });
            card.UnitSellingPrice = price;
            _cardMobileService.InsertOrUpdate(card);
            return Json(new
            {
                success = true
               ,
                mgs = string.Format("{0:0,0}", card.UnitSellingPrice)
            });
        }

        
    }
}
