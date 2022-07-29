using AfinitiPortalAPI.services;
using AfinitiPortalAPI.Shared;
using AfinitiPortalAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.ActionFilters
{
    public class HandleException : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Log.Error("OnException:context Exception");
            Log.Fatal("WebApi fired an exception! Request Path: {@path} - Message: {@message} - Stacktrace: {@stackTrace}", context.HttpContext.Request.Path.Value, context.Exception.GetBaseException().Message, context.Exception.StackTrace);

            Log.Error("OnException:context Exception", context.HttpContext.Response.StatusCode);
            context.HttpContext.Response.StatusCode = 500;
            context.Result = new JsonResult(ApiResponse.Failure());
            Log.Error("OnException:context Exception", context.Result);
            base.OnException(context);
        }
    }
}
