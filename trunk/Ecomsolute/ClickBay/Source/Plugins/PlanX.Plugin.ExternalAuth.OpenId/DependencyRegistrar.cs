using Autofac;
using Autofac.Integration.Mvc;
using PlanX.Core.Infrastructure;
using PlanX.Core.Infrastructure.DependencyManagement;
using PlanX.Plugin.ExternalAuth.OpenId.Core;

namespace PlanX.Plugin.ExternalAuth.OpenId
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<OpenIdProviderAuthorizer>().As<IOpenIdProviderAuthorizer>().InstancePerHttpRequest();
            builder.RegisterType<OpenIdRelyingPartyService>().As<IOpenIdRelyingPartyService>().InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
