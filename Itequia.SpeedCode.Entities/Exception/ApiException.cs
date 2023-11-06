using Microsoft.Extensions.Logging;
using System;
using System.Net;
using ExceptionClass = System.Exception;

namespace Itequia.SpeedCode.Entities.Exception
{
    public class ApiException : ExceptionBase
    {
     
        public ApiException(int? errorCode,
                            string errorDescription,
                            int httpStatus,
                            string reasonPhrase,
                            LogLevel? logLevel,
                            string logMessage, ExceptionClass ex) : base(reasonPhrase, ex)
        {
            if (errorCode.HasValue)
                _errorCode = errorCode.Value;

            _errorDescription = errorDescription;
            _httpStatus = httpStatus;
            _reasonPhrase = reasonPhrase;
            _logLevel = logLevel;
            _logMessage = logMessage;

        }

        public ApiException(string reasonPhrase, ExceptionClass ex) : base(reasonPhrase, ex)
        {
            _reasonPhrase = reasonPhrase;
        }

       
    }
}
