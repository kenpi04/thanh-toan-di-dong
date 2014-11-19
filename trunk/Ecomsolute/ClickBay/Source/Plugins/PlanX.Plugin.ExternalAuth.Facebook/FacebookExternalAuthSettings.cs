using PlanX.Core.Configuration;

namespace PlanX.Plugin.ExternalAuth.Facebook
{
    public class FacebookExternalAuthSettings : ISettings
    {
        public string ClientKeyIdentifier { get; set; }
        public string ClientSecret { get; set; }
    }
}
