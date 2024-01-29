using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastacture.Models
{
    public class Response<T>
    {
        public Response()
        {

        }

        public Response(T result)
        {
            Result = result;
            Code = HttpStatusCode.OK;
            IsError = false;
        }

        public T Result { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}
