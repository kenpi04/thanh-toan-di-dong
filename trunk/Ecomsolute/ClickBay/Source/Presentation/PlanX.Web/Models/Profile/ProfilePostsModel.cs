using System.Collections.Generic;
using PlanX.Web.Models.Common;

namespace PlanX.Web.Models.Profile
{
    public partial class ProfilePostsModel
    {
        public IList<PostsModel> Posts { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}