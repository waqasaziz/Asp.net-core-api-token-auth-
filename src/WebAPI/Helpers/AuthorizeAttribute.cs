//using System;
//using Domain.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class AuthorizeAttribute : Attribute, IAuthorizationFilter
//{
//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        var merchant = (Merchant)context.HttpContext.Items["User"];
//        if (merchant == null)
//        {
//            context.Result = new JsonResult(new { message = "Unauthorized" })
//            {
//                StatusCode = StatusCodes.Status401Unauthorized
//            };
//        }
//    }
//}