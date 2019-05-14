using System;

namespace ApiBase.Models
{
    /// <summary>
    /// customized operations exception derived from system.exception
    /// </summary>
    public class OperationException : Exception
    {
        public string Code { get; set; }

        public OperationException(string code,string message):base(message)
        {
            Code = code;
        }
    }
}
