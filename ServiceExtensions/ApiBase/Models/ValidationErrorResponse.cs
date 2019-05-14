using System.Collections.Generic;

namespace ApiBase.Models
{
    /// <summary>
    /// customized validation error response
    /// </summary>
    public class ValidationErrorResponse  
    {
        public string Code { get; set; }

        public ValidationErrorResponse(string summary,List<KeyValuePair<string,string>> messages = null) 
        {
            Summary = summary;
            Messages = messages;
        }

        public string Summary { get; set; }
        public List<KeyValuePair<string, string>> Messages { get; set; }
    }
}
