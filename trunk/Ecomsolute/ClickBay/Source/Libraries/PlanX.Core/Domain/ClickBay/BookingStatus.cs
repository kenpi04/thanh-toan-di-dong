
namespace PlanX.Core.Domain.ClickBay
{
    public enum BookingStatus
    {
        /// <summary>
        /// Chua xu ly: moi insert
        /// </summary>
        ChuaXyLy=10,
        /// <summary>
        /// Da giua cho cho khach: chua giao ve
        /// </summary>
        GiuCho=20,
        /// <summary>
        /// Hoan thanh: da giao ve+thanh toan
        /// </summary>
        HoanThanh=30,
        /// <summary>
        /// Hoan ve: khach da thanh toan + tra ve
        /// </summary>
        HoanVe=40,
        /// <summary>
        /// Huy ve: chua thanh toan+chua giao ve
        /// </summary>
        DaHuy=50,        
    }
}
