using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
   public enum ProductStatusEnum
    {
        /// <summary>
        /// Luu tam
        /// </summary>
        Saved = 0,

        /// <summary>
        /// Cho duyet
        /// </summary>
        PendingAproved = 1,

        /// <summary>
        /// Da duyet
        /// </summary>
        Approved = 2,

        /// <summary>
        /// khong duyet
        /// </summary>
        NotApproved = 3,
    }
}
