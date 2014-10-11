using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Nhom attribute cua product
    /// </summary>
    public enum ProductAttributeEnum
    {
        /// <summary>
        /// so tang
        /// </summary>
        NumberOfFloor = 1,//so tang
        /// <summary>
        /// so phong ngu
        /// </summary>
        NumberOfBedRoom = 2,//so phong ngu
        /// <summary>
        /// so phong tam
        /// </summary>
        NumberOfBadRoom = 3,//sp phong tam
        /// <summary>
        /// Co so vat chat
        /// </summary>
        CoSoVatChat = 4,//co so vat chat
        /// <summary>
        /// Moi truong
        /// </summary>
        Enviroment = 5,//moi truong xung quanh
        /// <summary>
        /// Chu dau tu
        /// </summary>
        ChuDauTu = 6,//chu dau tu
        /// <summary>
        /// Huong: dong, tay, nam, bac,...
        /// </summary>
        Director = 7,//huong:dong,tay,nam,bac
        /// <summary>
        /// Tinh trang bat dong san: cu, moi, dang xay
        /// </summary>
        Status = 8,//tinh trang bat dong san: cu, moi, dang xay,...
        /// <summary>
        /// So block
        /// </summary>
        NumberBlock = 11,//so block - ap dung cho chung cu
        /// <summary>
        /// Phap ly: so hong, so do, hop dong
        /// </summary>
        PhapLy = 9,//phap ly
        /// <summary>
        /// thich hop: lam van phong, cho thue, nha o,...
        /// </summary>
        ThichHop = 12,//thich hop: lam van phong, cho thue, nha o,...
        /// <summary>
        /// tien ich xung quanh: gan cho, truong hoc,...
        /// </summary>
        TienIch = 15//tien ich xung quanh: gan cho, truong hoc,...
    }
    /// <summary>
    /// Trang thai giao dich:con trong, da dat coc, da ban
    /// </summary>
    public enum ProductStatusText
    {
        /// <summary>
        /// Con trong: co the giao dich
        /// </summary>
        CONTRONG = 0,
        /// <summary>
        /// Da dat coc: ngung giao dich
        /// </summary>
        DADATCOC = 1,
        /// <summary>
        /// Da ban: ngung giao dich
        /// </summary>
        DABAN = 2

    }
}
