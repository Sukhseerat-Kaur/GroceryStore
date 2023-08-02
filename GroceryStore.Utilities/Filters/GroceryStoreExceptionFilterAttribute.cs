using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GroceryStoreCore.Filters
{
    public class GroceryStoreExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger<GroceryStoreExceptionFilterAttribute> _logger;

        public GroceryStoreExceptionFilterAttribute(ILogger<GroceryStoreExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError($"Unhandled exception with Message: {context.Exception.Message} is thrown from : {context.Exception.Source}\n\nStackTrace: {context.Exception.StackTrace}");


            var result = new ObjectResult(new
            {
                Message = "Internal Server Error.",
                context.Exception.Source,
                ExceptionType = context.Exception.GetType().FullName
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            context.ExceptionHandled = true;
            context.Result = result;
        }
    }
}
