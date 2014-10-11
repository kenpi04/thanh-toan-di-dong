

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Trang thai duyet
    /// </summary>
   public enum ProductStatusEnum
    {
        /// <summary>
        /// Luu tam
        /// </summary>
        Saved = 1,

        /// <summary>
        /// Cho duyet
        /// </summary>
        PendingAproved = 10,

        /// <summary>
        /// Da duyet
        /// </summary>
        Approved = 20,

        /// <summary>
        /// khong duyet
        /// </summary>
        NotApproved = 30
    }
}
