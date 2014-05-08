﻿using System;
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
            var cate = _cateService.GetAll().ToDictionary(x => x.PictureUrl, x => x.DauSo);
            ViewBag.Cate = cate;
            return View(model);
        }
        [HttpPost]
        public ActionResult Topup(TopupModel model)
        {

            var cardmobile = _cardMobileService.GetById(model.PriceListId);
            if (cardmobile == null)
                throw new Exception("Thẻ đã hết !");
            var order = new Order
            {

                CardMobileId = model.PriceListId,
                UnitPrice = cardmobile.UnitPrice,
                UnitSellingPrice = cardmobile.UnitSellingPrice,
                Quantity = 1,
                OrderStatusId = (int)OrderStatusEnum.PENDING,
                Price = cardmobile.UnitSellingPrice,
                NumberPhone = model.Phone,
                PartnerId = (int)ProviderEnum.PAYOO,
                OrderTypeId = (int)OrderType.CARD,
                ProviderId = 0,
                CustomerIp = Helper.GetIp(),
                PaymentStatusId = (int)PaymentStatus.CHUATHANHTOAN,
                TotalAmount = cardmobile.UnitSellingPrice,
                OrderGuid = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow



            };
            _orderService.InsertOrUpdate(order);

            return RedirectToAction("Index", "OnePay", new { orderId = order.Id });

        }
        public ActionResult PaymentSuccess(int id)
        {
            var order = _orderService.GetById(id);
            var model = new OrderModel
            {
                Id = order.Id,
                CustomerPhone = order.NumberPhone,
                Price = String.Format("{0:0,0 đ}", order.Price),
                Quantity = order.Quantity,
                TotalPrice = String.Format("{0:0,0 đ}", order.TotalAmount),
                CreateDate = order.CreatedOn.ToString(),
                

            };
            if (order.OrderTypeId == (int)OrderType.CARD)
            {
                model.Name = "Mua thẻ mệnh giá " + order.UnitPrice;
            }
            else
            {
                model.Name = "Thanh toán hóa đơn";
            }
            if (order == null)
                return RedirectToAction("/");
            _orderService.InsertOrUpdate(order);
            if (order.OrderTypeId == (int)OrderType.CARD)
            {
                int i = _payooService.TopupPaymentBE(order);
                if (i != 0)
                {
                    ViewBag.Error = "Có phát sinh lỗi trong quá trình xử lý đơn hàng của bạn!";
                    return View(model);
                }
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult GetCategory(int? id,string phone)
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
            var cate = _cateService.GetAll();
            model.CateCards = cate.Select(x => new BuyCardModel.CateCard
            {
                Id=x.Id,
                Name=x.Name,
                Image=x.PictureUrl
               
            }).ToList();
            
           
            return View(model);
        }
        [HttpPost]
        public ActionResult BuyCard(BuyCardModel model)
        {
            var cardmobile = _cardMobileService.GetById(model.CardId);
            var order = new Order
            {

                CardMobileId = cardmobile.Id,
                UnitPrice = cardmobile.UnitPrice,
                UnitSellingPrice = cardmobile.UnitSellingPrice,
                Quantity = 1,
                OrderStatusId = (int)OrderStatusEnum.PENDING,
                Price = cardmobile.UnitSellingPrice,              
                PartnerId = (int)ProviderEnum.PAYOO,
                OrderTypeId = (int)OrderType.CARD,
                ProviderId = 0,
                CustomerIp = Helper.GetIp(),
                PaymentStatusId = (int)PaymentStatus.CHUATHANHTOAN,
                TotalAmount = cardmobile.UnitSellingPrice,
                OrderGuid = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow



            };
            _orderService.InsertOrUpdate(order);
            return RedirectToAction("Index", "OnePay", new { orderId = order.Id });

        }

    }
}
