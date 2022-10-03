using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using WebApi.Entities;
using WebApi.Helpers.Common;
using WebApi.Models.FASTPublic;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
      public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (GlobalVariable.IsUsingJWT)
        {
            var userType = (String)context.HttpContext.Items["Type"];
            if(userType == null)
            {
                string status = (String)context.HttpContext.Items["Status"];
                if (status == "Expired" && context.HttpContext.Request.Path.Value.Contains("api/v1/"))
                {
                    FASTReponse reponse = new FASTReponse();
                    reponse.SetError(4, 10, "JWT token has expired. Please, log in again.");
                    context.Result = new JsonResult(reponse) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else if(context.HttpContext.Request.Path.Value.Substring(1,7) == "api/v1/")
                {
                    FASTReponse reponse = new FASTReponse();
                    reponse.SetError(4, 10, "JWT token is invalid.");
                    context.Result = new JsonResult(reponse) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else
                {
                    context.Result = new JsonResult(Codetypes.Err_Unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            else if(userType == GlobalVariable.FromCore)
            {
                var user = (Users)context.HttpContext.Items["User"];
                if (user == null)
                {
                    context.Result = new JsonResult(Codetypes.Err_Unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            else if (userType == GlobalVariable.FromWeb)
            {
                var user = (SUsrac)context.HttpContext.Items["User"];
                if (user == null)
                {
                    context.Result = new JsonResult(Codetypes.Err_Unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else
                {
                    if(user.expiretime != null)
                    {
                        DateTime expiryDate = (DateTime)user.expiretime;
                        if(expiryDate < DateTime.Now)
                        {
                            context.Result = new JsonResult(Codetypes.UMG_INVALID_EXPDT) { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                    }
                }
            }
            else if (userType == GlobalVariable.UserPublic)
            {
                var user = (SUserpublic)context.HttpContext.Items["User"];
                if (user == null)
                {
                    context.Result = new JsonResult(Codetypes.Err_Unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
            
    }
}

