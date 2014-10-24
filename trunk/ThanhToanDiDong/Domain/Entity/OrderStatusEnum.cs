
namespace Domain.Entity
{
    /// <summary>
    /// trang thai don hang
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// cho <=> chua thanh toan
        /// </summary>
        PENDING = 0,
        /// <summary>
        /// dang giao dich <=> da thanh toan ^ chua nhan hang
        /// </summary>
        PROCESSING = 10,
        /// <summary>
        /// da hoan thanh <=> da thanh toan & nhan hang
        /// </summary>
        COMPLETE = 20,
        /// <summary>
        /// da huy
        /// </summary>
        CANCEL = 30
    }
    /// <summary>
    /// Nha cung cap dich vu thanh toan
    /// </summary>
    public enum ProviderEnum
    {
        PAYOO = 10,
        MSERVICE = 20
    }
    /// <summary>
    /// Loai don hang
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Nap tien dien thoai
        /// </summary>
        TOPUP = 10,
        /// <summary>
        /// Mua the dien thoai
        /// </summary>
        CARD = 20,
        /// <summary>
        /// Thanh toan hoa don
        /// </summary>
        BILLING = 30,
        /// <summary>
        /// Mua the game
        /// </summary>
        CARDGAME = 25

    }
    /// <summary>
    /// Trang thai thanh toan - qua Internet Banking
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Chua thanh toan
        /// </summary>
        CHUATHANHTOAN = 0,
        /// <summary>
        /// Da thanh toan
        /// </summary>
        DATHANHTOAN = 10,
        /// <summary>
        /// Da huy
        /// </summary>
        HUY = 20
    }
}
