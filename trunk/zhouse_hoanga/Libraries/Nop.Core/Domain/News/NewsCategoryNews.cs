using Nop.Core.Domain.News;

namespace Nop.Core.Domain.News
{
    public partial class NewsCategoryNews : BaseEntity
    {
        public virtual int NewsId { get; set; }
        public virtual int CategoryNewsId { get; set; }

        public virtual CategoryNews CategoryNews { get; set; }
        public virtual NewsItem NewsItem { get; set; }
    }

}
