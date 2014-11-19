using Autofac;
using Autofac.Integration.Mvc;
using PlanX.Core.Infrastructure;
using PlanX.Core.Infrastructure.DependencyManagement;
using PlanX.Plugin.ExternalAuth.Facebook.Core;

namespace PlanX.Plugin.ExternalAuth.Facebook
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<FacebookProviderAuthorizer>().As<IOAuthProviderFacebookAuthorizer>().InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
