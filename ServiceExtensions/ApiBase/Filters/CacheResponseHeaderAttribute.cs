using Extensions.CoreBase;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Extensions.ApiBase.Filters
{
    //this is responsible for parsing the cache header on controller actions and add cache for those actions
    public class CacheResponseHeaderAttribute : ActionFilterAttribute
    {
        private int _cacheSeconds; 

        public CacheResponseHeaderAttribute(int cacheSeconds)
        {
            _cacheSeconds = cacheSeconds; 
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (_cacheSeconds > 0)
                context.HttpContext.Response.Headers.Add(HeaderConstants.CacheControlHeaderName, _cacheSeconds.ToString());
        }
    }
}
