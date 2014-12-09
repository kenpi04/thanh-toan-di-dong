using PlanX.Core.Domain.News;

namespace PlanX.Core.Domain.News
{
    public partial class NewsCategoryNews : BaseEntity
    {
        public virtual int NewsId { get; set; }
        public virtual int CategoryNewsId { get; set; }

        public virtual CategoryNews CategoryNews { get; set; }
        public virtual NewsItem NewsItem { get; set; }
    }

}
