using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Models.Errors
{
    public class RequestError
    {
        public bool Succeeded { get; set; }
        public ErrorsList[] Errors { get; set; }

        public RequestError()
        {

        }

        public RequestError(bool succeeded, string description)
        {
            Succeeded = succeeded;
            Errors = new ErrorsList[]
            {
                new ErrorsList(null, description)
            };
        }
    }

    public class ErrorsList
    {
        public string Exception { get; set; }
        public string Description { get; set; }

        public ErrorsList()
        {

        }
        public ErrorsList(string exception, string description)
        {
            Exception = exception;
            Description = description;
        }
    }
}
