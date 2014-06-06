using Nop.Web.Framework.UI.Paging;

namespace Nop.Web.Models.News
{
    public partial class NewsPagingFilteringModel : BasePageableModel
    {
        public int CateId { get; set; }
    }
}