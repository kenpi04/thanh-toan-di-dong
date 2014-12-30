using PlanX.Web.Framework.UI.Paging;

namespace PlanX.Web.Models.News
{
    public partial class NewsPagingFilteringModel : BasePageableModel
    {
        public int CategoryId { get; set; }
    }
}