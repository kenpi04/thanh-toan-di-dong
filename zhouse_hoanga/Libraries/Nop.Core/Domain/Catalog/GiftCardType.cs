namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a gift card type
    /// </summary>
    public enum GiftCardType
    {
        /// <summary>
        /// Virtual: Đang giao dịch
        /// </summary>
        Virtual = 0,
        /// <summary>
        /// Physical
        /// </summary>
        Physical = 1,
        DaBan=10,
        BanGap=20,
        Moi=30,
        DangXay=40,
    }
}
