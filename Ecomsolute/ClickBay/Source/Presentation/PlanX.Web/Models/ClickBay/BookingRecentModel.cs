using System;

namespace PlanX.Web.Models.ClickBay
{
    public class BookingRecentModel
    {
        /// <summary>
        /// id booking
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ten noi di
        /// </summary>
        public string FromPlace { get; set; }
        /// <summary>
        /// ma noi di
        /// </summary>
        public string FromPlaceCode { get; set; }
        /// <summary>
        /// ten noi den
        /// </summary>
        public string ToPlace { get; set; }
        /// <summary>
        /// ma noi den
        /// </summary>
        public string ToPlaceCode { get; set; }
        /// <summary>
        /// hang hang khong: code
        /// </summary>
        public string BrandCode { get; set; }
        /// <summary>
        /// hang hang khong: name
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// Loai chuyen bay: khu hoi / 1 chieu
        /// </summary>
        public bool RoudTrip { get; set; }
        /// <summary>
        /// Ngay di
        /// </summary>
        public DateTime DepartDatetime { get; set; }
        /// <summary>
        /// Ngay ve
        /// </summary>
        public DateTime? ReturnDatetime { get; set; }
        /// <summary>
        /// Ngay booking
        /// </summary>
        public DateTime CreateDate { get; set; }
        public string CreateDateString { get; set; }
        /// <summary>
        /// Gia net
        /// </summary>
        public decimal PriceNet { get; set; }
        /// <summary>
        /// Tong ve
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Ma tien te: default VND
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// So luong nguoi lon
        /// </summary>
        public Int16 Adult { get; set; }
        /// <summary>
        /// So luong tre em
        /// </summary>
        public Int16 Child { get; set; }
        /// <summary>
        /// So luong tre so sinh
        /// </summary>
        public Int16 Infant { get; set; }
    }
}