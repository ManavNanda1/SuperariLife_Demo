using SuperariLife.Model.Token;

namespace SuperariLife.Service.JWTAuthentication
{
    public interface IJWTAuthenticationService
    {
        AccessTokenModel GenerateToken(TokenModel userToken, string JWT_Secret, int JWT_Validity_Mins);
        TokenModel GetTokenData(string jwtToken);
    }
}
