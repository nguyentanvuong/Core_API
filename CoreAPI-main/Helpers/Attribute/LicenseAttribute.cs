using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using WebApi.Helpers.Common;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class LicenseAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (GlobalVariable.IsUsingLicense)
        {
            bool license = (bool)context.HttpContext.Items["License"];
            if (license == false)
            {
                context.Result = new JsonResult(Codetypes.Err_InvalidLicense) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}

