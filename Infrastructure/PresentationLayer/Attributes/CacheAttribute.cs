using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Attributes
{
    public class CacheAttribute(int DurationInSeconds = 100) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Create Cache Key 
            string cacheKey = CreateCacheKey(context.HttpContext.Request);

            //Search For Value with this Key {Redis}
            var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheValue = await _cacheService.GetAsync(cacheKey);

            //Return Cached Value if Not Null
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            //In Case Of Null --> Invoke Next
            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult result) 
            {
                await _cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromSeconds(DurationInSeconds));
            }

            //Set Value With Cache Key
        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append($"{request.Path}?");
            foreach (var item in request.Query.OrderBy(x => x.Key))
            {
                key.Append($"{item.Key} = {item.Value}&");
            }
            return key.ToString();
        }
    }
}
