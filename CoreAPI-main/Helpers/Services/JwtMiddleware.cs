using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers.Common;
using WebApi.Services;

namespace WebApi.Helpers.Services
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var bearer = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First();

            if (token != null && ((string.IsNullOrEmpty(bearer)? "" : bearer.Trim().ToLower()) == "bearer"))
                attachUserToContext(context, userService, token);
            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var UserType = jwtToken.Claims.FirstOrDefault(x => x.Type == GlobalVariable.UserType).Value;
                var Identity = jwtToken.Claims.FirstOrDefault(x => x.Type == GlobalVariable.Identity).Value;
                // attach user to context on successful jwt validation
                if (UserType == GlobalVariable.FromCore)
                {
                    context.Items["User"] = userService.GetUsersBySession(Identity);
                    context.Items["Type"] = UserType;
                }
                else if (UserType == GlobalVariable.FromWeb)
                {
                    context.Items["User"] = userService.GetUserWebByUsername(Identity);
                    context.Items["Type"] = UserType;
                }
                else if (UserType == GlobalVariable.UserPublic)
                {
                    context.Items["User"] = userService.GetUserPublicByUsername(Identity);
                    context.Items["Type"] = UserType;
                }
            }
            catch(SecurityTokenExpiredException)
            {
                context.Items["Status"] = "Expired";
            }
            catch
            {
                //some thing else
            }
        }
    }
}
