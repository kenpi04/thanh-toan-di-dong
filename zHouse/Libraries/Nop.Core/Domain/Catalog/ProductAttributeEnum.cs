using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
  public  enum ProductAttributeEnum
    {
      NumberOfFloor=1,
      NumberOfBedRoom=2,
      NumberOfBadRoom=3,
      CoSoVatChat=4,
      Enviroment=5,
      ChuDauTu=6,
      Director=7,
      Status=8,
      NumberBlock=11,
      PhapLy=9,
      ThichHop=12,
      TienIch=15
    }
    /// <summary>
    /// Trang thai tin:con trong, da dat coc, da ban
    /// </summary>
    public enum ProductStatusText
    {
       CONTRONG=0,
       DADATCOC=1,
       DABAN=2
       
    }
}
