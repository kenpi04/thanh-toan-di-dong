using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
  public  enum ProductAttributeEnum
    {
      NumberOfFloor=1,//so tang
      NumberOfBedRoom=2,//so phong ngu
      NumberOfBadRoom=3,//sp phong tam
      CoSoVatChat=4,//co so vat chat
      Enviroment=5,//moi truong xung quanh
      ChuDauTu=6,//chu dau tu
      Director=7,//huong:dong,tay,nam,bac
      Status=8,//tinh trang bat dong san: cu, moi, dang xay,...
      NumberBlock=11,//so block - ap dung cho chung cu
      PhapLy=9,//phap ly
      ThichHop=12,//thich hop: lam van phong, cho thue, nha o,...
      TienIch=15//tien ich xung quanh: gan cho, truong hoc,...
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
