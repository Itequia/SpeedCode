using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Itequia.SpeedCode.Entities.Exception
{
    public interface IExceptionBase
    {
        int ErrorCode { get;}
        string ErrorDescription { get; }
        public HttpStatusCode HttpStatus { get; }
        string ReasonPhrase { get; }
        LogLevel LogLevel { get; }
        string LogMessage { get; }
    }
}
