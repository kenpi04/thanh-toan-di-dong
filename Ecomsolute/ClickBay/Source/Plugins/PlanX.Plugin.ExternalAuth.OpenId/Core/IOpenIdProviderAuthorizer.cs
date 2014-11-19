//Contributor:  Nicholas Mayne


using PlanX.Services.Authentication.External;

namespace PlanX.Plugin.ExternalAuth.OpenId.Core
{
    public interface IOpenIdProviderAuthorizer : IExternalProviderAuthorizer
    {
        string EnternalIdentifier { get; set; } // mayne - refactor this out
        bool IsOpenIdCallback { get; }
    }
}