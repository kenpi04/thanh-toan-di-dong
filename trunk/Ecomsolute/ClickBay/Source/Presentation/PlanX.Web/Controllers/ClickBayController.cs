using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanX.Core.Domain.ClickBay;
using PlanX.Services.Clickbay;
using PlanX.Web.Models.ClickBay;

namespace PlanX.Web.Controllers
{
    public class ClickBayController : Controller
    {
        private readonly IClickbayService _clickBayService;
        public ClickBayController(IClickbayService clickBayService)
        {
            this._clickBayService = clickBayService;
        }

        [HttpGet]
        public ActionResult GetCountries()
        {
            var data = _clickBayService.GetCountry().Result;
            return new JsonResult
            {
                Data = data.Select(x => new { x.Code, x.Name, x.Id }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult Getcity(int? countryId)
        {
            var data = _clickBayService.GetCity().Result;
            if (countryId.HasValue)
                data = data.Where(x => x.CountryId == countryId.Value);
            return new JsonResult
            {
                Data = data.Select(x => new { x.Code, x.Name, x.Id }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private IEnumerable<TicketModel> PrepairingTicketModel(IEnumerable<Ticket> data)
        {
            if (data == null)
                return new List<TicketModel>();
            return data.Select(x => new TicketModel
            {
                //Id = x.Id,
                //Price = x.Price,

                //ToId = x.ToPlaceId,
                //BrandCode = x.AirlineCode
            });
            
        }




	}
}