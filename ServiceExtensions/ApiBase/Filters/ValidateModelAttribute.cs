using ApiBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Extensions.ApiBase.Filters
{
    /// <summary>
    /// model validation attribute
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
          
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                var response = new ValidationErrorResponse("Validation Error", new List<KeyValuePair<string, string>>());

                foreach(var key in context.ModelState.Keys)
                {
                    foreach(var error in context.ModelState[key].Errors)
                    {
                        response.Messages.Add(new KeyValuePair<string, string>(key, error.ErrorMessage));
                    }
                }

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
