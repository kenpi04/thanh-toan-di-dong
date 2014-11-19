//Contributor:  Nicholas Mayne


namespace PlanX.Services.Authentication.External
{
    public partial interface IClaimsTranslator<T>
    {
        UserClaims Translate(T response);
    }
}