using Autofac;
using Autofac.Core;
using PlanX.Admin.Controllers;
using PlanX.Core.Caching;
using PlanX.Core.Infrastructure;
using PlanX.Core.Infrastructure.DependencyManagement;

namespace PlanX.Admin.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //we cache presentation models between requests
            builder.RegisterType<HomeController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
