using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalProject.Core.Utilities
{
    public class CapitalCustomAPIResponseSchema
    {
        public CapitalCustomAPIResponseSchema(List<string> errorMessages)
        {
            Code = 400;
            IsError = true;
            ErrorMessages = errorMessages;
        }

        public CapitalCustomAPIResponseSchema(List<string> errorMessages, object result)
        {
            Code = 400;
            IsError = true;
            ErrorMessages = errorMessages;
            Result = result;
        }


        /// <summary>
        /// Success Response
        /// </summary>
        /// <param name="message"> Success message</param>
        /// <param name="result">Response result</param>
        /// <param name="code">Response code</param>
        public CapitalCustomAPIResponseSchema(string message, object result)
        {
            Code = 200;
            Message = message;
            Result = result;
        }

        public CapitalCustomAPIResponseSchema(string message)
        {
            Code = 200;
            Message = message;
        }


        public int Code { get; set; }

        public bool IsError { get; set; }

        public List<string>? ErrorMessages { get; set; }

        public string? Message { get; set; }
        public Object? Result { get; set; }
    }
}
