using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhToanDiDong.Models.Payment;
using Domain.Services;
using System.Text.RegularExpressions;
using Domain.Entity;
using ThanhToanDiDong.Unities;

namespace ThanhToanDiDong.Controllers
{
    public class PaymentController : Controller
    {
        CardMobileService _cardMobileService = new CardMobileService();
        CategoryCardMobileService _cateService = new CategoryCardMobileService();
        OrderService _orderService = new OrderService();
        PayooService _payooService = new PayooService();


        //
        // GET: /Payment/
        public ActionResult Topup()
        {
            var model = new TopupModel();
            //var cate = _cateService.GetAll().Where(x=> x.IsSupportTopup && !x.IsCard).ToDictionary(x => x.PictureUrl, x => x.DauSo);
            //ViewBag.Cate = cate;
            return View(model);
        }
        [HttpPost]
        public ActionResult Topup(TopupModel model)
        {
            if (ModelState.IsValid)
            {
                var cardmobile = _cardMobileService.GetById(model.PriceListId);
                if (cardmobile != null)
                {
                    var order = new Order
                    {

                        CardMobileId = model.PriceListId,
                        UnitPrice = cardmobile.UnitPrice,
                        UnitSellingPrice = cardmobile.UnitSellingPrice,
                        Quantity = 1,
                        OrderStatusId = (int)OrderStatusEnum.PENDING,//trang thai don hang
                        Price = cardmobile.UnitSellingPrice,
                        NumberPhone = model.Phone,
                        PartnerId = (int)ProviderEnum.PAYOO, //nha cung cap thanh toan
                        OrderTypeId = (int)OrderType.TOPUP, // loai giao dich
                        ProviderId = 0,
                        CustomerIp = Helper.GetIp(),
                        PaymentStatusId = (int)PaymentStatus.CHUATHANHTOAN, // trang thai thanh toan
                        TotalAmount = cardmobile.UnitSellingPrice,
                        OrderGuid = Guid.NewGuid(),
                        CreatedOn = DateTime.UtcNow



                    };
                    _orderService.InsertOrUpdate(order);

                    return RedirectToAction("Index", "OnePay", new { orderId = order.Id });
                }
                ViewBag.Error = "Không tìm thấy dịch vụ. Vui lòng thử lại.";
                return View(model);
            }
            ViewBag.Error = "Lỗi! Vui lòng kiểm tra lại dữ liệu.";
            return View(model);
        }
        public ActionResult PaymentSuccess()
        {
            if (Session["ORDERID"] == null)
              return RedirectToRoute("HomePage");
            var order = _orderService.GetById(Convert.ToInt32(Session["ORDERID"]));
            if (order == null)
                return RedirectToRoute("HomePage");
            Session["ORDERID"] = null;
            var model = new OrderModel
            {
                Id = order.Id,
                CustomerPhone = order.NumberPhone,
                Price = String.Format("{0:0,0 đ}", order.Price),
                Quantity = order.Quantity,
                TotalPrice = String.Format("{0:0,0 đ}", order.TotalAmount),
                CreateDate = order.CreatedOn.ToString(),
                CardId=order.CardId,
                ExpiredDate=order.Expired,
                Name=_cardMobileService.GetById(order.CardMobileId).CategoryCardMobile.Name,
                CardSerie=order.SeriNumber,
                Status=order.OrderStatusId==(int)OrderStatusEnum.COMPLETE?"Thành công":"Đang xử lý",
                OrderType=(order.OrderTypeId==(int)OrderType.CARD)?"Mua card":(order.OrderTypeId==(int)OrderType.TOPUP?"Nạp thẻ":"Thanh toán hóa đơn"),
                OrderStatusId=order.OrderTypeId


            };
            
            int i = 0;
            if (order.OrderTypeId == (int)OrderType.TOPUP)
            {
                i = _payooService.TopupPaymentBE(order);
                if (i != 0)
                {
                    ViewBag.Error = "Có phát sinh lỗi trong quá trình xử lý đơn hàng của bạn! Vui lòng liên hệ với chúng tôi để được giải quyết";
                    return View(model);
                }
            }
            else if (order.OrderTypeId == (int)OrderType.CARD)
            {
                if (!_payooService.CodePaymentBE(order))
                {
                    ViewBag.Error = "Có phát sinh lỗi trong quá trình xử lý đơn hàng của bạn! Vui lòng liên hệ với chúng tôi để được giải quyết";
                    return View(model);
                }
            }

            if (order.OrderTypeId == (int)OrderType.CARD)
            {
                model.Name = "Mua thẻ mệnh giá " + order.UnitPrice;
            }
            else
            {
                model.Name = "Thanh toán hóa đơn";
            }


            return View(model);
        }
        [HttpGet]
        public ActionResult GetCategory(int? id, string phone)
        {
            if (id.HasValue)
            {
                var catex = _cateService.GetById(id.Value);
                var listCardType = catex.CardMobile.Select(x => new
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    price = x.UnitSellingPrice
                }).ToList();
                return Json(listCardType, JsonRequestBehavior.AllowGet);
            }

            int lengt = 3;
            if (phone.Length > 10)
                lengt = 4;

            Regex rg = new Regex(phone.Substring(0, lengt));
            var cate = _cateService.GetAll(x => rg.IsMatch(x.DauSo)).FirstOrDefault();
            if (cate != null)
            {

                var listCardType = cate.CardMobile.Select(x => new
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    price = x.UnitSellingPrice
                }).ToList();
                return Json(listCardType, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuyCard()
        {
            var model = new BuyCardModel();
            
            var cate = _cateService.GetAll().Where(x => x.IsCard).OrderBy(x=>x.DisplayOrder).ToList();
            model.CateCards = cate.Select(x => new BuyCardModel.CateCard
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.PictureUrl
            }).ToList();
            
            return View(model);
        }
        [HttpPost]
        public ActionResult BuyCard(BuyCardModel model)
        {
            if (ModelState.IsValid)
            {
                var cardmobile = _cardMobileService.GetById(model.CardId);
                if (cardmobile != null)
                {
                    var order = new Order
                    {

                        CardMobileId = cardmobile.Id,
                        UnitPrice = cardmobile.UnitPrice,
                        UnitSellingPrice = cardmobile.UnitSellingPrice,
                        Quantity = model.Quantity,
                        OrderStatusId = (int)OrderStatusEnum.PENDING,//
                        Price = cardmobile.UnitSellingPrice,
                        PartnerId = (int)ProviderEnum.PAYOO,//
                        OrderTypeId = (int)(model.IsCardGame ? OrderType.CARD : OrderType.CARDGAME),//
                        ProviderId = 0,
                        CustomerIp = Helper.GetIp(),
                        PaymentStatusId = (int)PaymentStatus.CHUATHANHTOAN,//
                        TotalAmount = cardmobile.UnitSellingPrice * model.Quantity,//
                        OrderGuid = Guid.NewGuid(),
                        CreatedOn = DateTime.Now



                    };
                    _orderService.InsertOrUpdate(order);
                    return RedirectToAction("Index", "OnePay", new { orderId = order.Id });
                }
                ViewBag.Error = "Không tìm thấy dịch vụ. Vui lòng thử lại.";
                return View(model);
            }
            ViewBag.Error = "Lỗi! Vui lòng kiểm tra lại dữ liệu.";
            return View(model);
        }

        public ActionResult BuyCardGame()
        {
            var model = new BuyCardModel();

            var cate = _cateService.GetAll().Where(x=> x.IsCardGame).OrderBy(x=>x.DisplayOrder).ToList();
            model.CateCards = cate.Select(x => new BuyCardModel.CateCard
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.PictureUrl
            }).ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult BuyCardGame(BuyCardModel model)
        {
            if (ModelState.IsValid)
            {
                var cardmobile = _cardMobileService.GetById(model.CardId);                
                if (cardmobile != null)
                {
                    var order = new Order
                    {

                        CardMobileId = cardmobile.Id,
                        UnitPrice = cardmobile.UnitPrice,
                        UnitSellingPrice = cardmobile.UnitSellingPrice,
                        Quantity = model.Quantity,
                        OrderStatusId = (int)OrderStatusEnum.PENDING,//
                        Price = cardmobile.UnitSellingPrice,
                        PartnerId = (int)ProviderEnum.PAYOO,//
                        OrderTypeId = (int)(OrderType.CARDGAME),//
                        ProviderId = 0,
                        CustomerIp = Helper.GetIp(),
                        PaymentStatusId = (int)PaymentStatus.CHUATHANHTOAN,//
                        TotalAmount = cardmobile.UnitSellingPrice * model.Quantity,//
                        OrderGuid = Guid.NewGuid(),
                        CreatedOn = DateTime.Now



                    };
                    _orderService.InsertOrUpdate(order);
                    return RedirectToAction("Index", "OnePay", new { orderId = order.Id });
                }
                ViewBag.Error = "Không tìm thấy dịch vụ. Vui lòng thử lại.";
                return View(model);
            }
            ViewBag.Error = "Lỗi! Vui lòng kiểm tra lại dữ liệu.";
            return View(model);
        }

    }
}
