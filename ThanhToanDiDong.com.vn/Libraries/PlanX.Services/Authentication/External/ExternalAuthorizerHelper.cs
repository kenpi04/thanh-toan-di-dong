//Contributor:  Nicholas Mayne

using System.Collections.Generic;
using System.Web;
using PlanX.Core.Infrastructure;

namespace PlanX.Services.Authentication.External
{
    public static partial class ExternalAuthorizerHelper
    {
        private static HttpSessionStateBase GetSession()
        {
            var session = EngineContext.Current.Resolve<HttpSessionStateBase>();
            return session;
        }

        public static void StoreParametersForRoundTrip(OpenAuthenticationParameters parameters)
        {
            var session = GetSession();
            session["planx.externalauth.parameters"] = parameters;
        }
        public static OpenAuthenticationParameters RetrieveParametersFromRoundTrip(bool removeOnRetrieval)
        {
            var session = GetSession();
            var parameters = session["planx.externalauth.parameters"];
            if (parameters != null && removeOnRetrieval)
                RemoveParameters();

            return parameters as OpenAuthenticationParameters;
        }

        public static void RemoveParameters()
        {
            var session = GetSession();
            session.Remove("planx.externalauth.parameters");
        }

        public static void AddErrorsToDisplay(string error)
        {
            var session = GetSession();
            var errors = session["planx.externalauth.errors"] as IList<string>;
            if (errors == null)
            {
                errors = new List<string>();
                session.Add("plan.externalauth.errors", errors);
            }
            errors.Add(error);
        }

        public static IList<string> RetrieveErrorsToDisplay(bool removeOnRetrieval)
        {
            var session = GetSession();
            var errors = session["planx.externalauth.errors"] as IList<string>;
            if (errors != null && removeOnRetrieval)
                session.Remove("planx.externalauth.errors");
            return errors;
        }
    }
}