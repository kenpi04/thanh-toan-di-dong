//Contributor:  Nicholas Mayne


namespace PlanX.Services.Authentication.External
{
    public partial interface IExternalProviderAuthorizer
    {
        /// <summary>
        /// Authorize response
        /// </summary>
        /// <param name="returnUrl">Return URL</param>
        /// <param name="verifyResponse">true - Verify response;false - request authentication;null - determine automatically</param>
        /// <returns>Authorize state</returns>
        AuthorizeState Authorize(string returnUrl, bool? verifyResponse = null);
    }
}