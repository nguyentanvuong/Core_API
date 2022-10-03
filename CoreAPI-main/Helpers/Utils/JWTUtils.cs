using System;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Helpers.Utils
{
    public class JWTUtils
    {
        public static DateTime? GetExpiryTimeJWT(string token)
        {
            DateTime expiry; 
            try
            {
                var jwtToken = new JwtSecurityToken(token);
                expiry = jwtToken.ValidTo;
                return expiry;
            }
            catch
            {
                return null;
            }
        }
    }
}
