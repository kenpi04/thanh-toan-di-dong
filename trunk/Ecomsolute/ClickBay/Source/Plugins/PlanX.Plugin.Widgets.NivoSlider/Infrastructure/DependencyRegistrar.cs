using Autofac;
using Autofac.Core;
using PlanX.Core.Caching;
using PlanX.Core.Infrastructure;
using PlanX.Core.Infrastructure.DependencyManagement;
using PlanX.Plugin.Widgets.NivoSlider.Controllers;

namespace PlanX.Plugin.Widgets.NivoSlider.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //we cache presentation models between requests
            builder.RegisterType<WidgetsNivoSliderController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
