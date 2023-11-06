using Microsoft.Extensions.Logging;
using System.Net;
using ExceptionClass = System.Exception;

namespace Itequia.SpeedCode.Entities.Exception
{
    public class ExceptionBase : ExceptionClass, IExceptionBase
    {
        protected int? _errorCode;
        protected string _errorDescription;
        protected int? _httpStatus;
        protected string _reasonPhrase;
        protected LogLevel? _logLevel;
        protected string _logMessage;

        protected virtual int DefaultErrorCode => 500;
        protected virtual string DefaultErrorDescription => "Internal Server Error";
        protected virtual HttpStatusCode DefaultHttpStatusCode => HttpStatusCode.InternalServerError;


        public int ErrorCode { get => _errorCode.HasValue ? _errorCode.Value : this.DefaultErrorCode; }
        public string ErrorDescription { get => !string.IsNullOrEmpty(_errorDescription) ? _errorDescription : this.DefaultErrorDescription; }
        public HttpStatusCode HttpStatus { get => !_httpStatus.HasValue ? DefaultHttpStatusCode : (HttpStatusCode)_httpStatus.Value; set => _httpStatus = (int)value; }
        public string ReasonPhrase { get => _reasonPhrase; }
        public LogLevel LogLevel { get => _logLevel.HasValue ? _logLevel.Value : LogLevel.Error; }
        public string LogMessage { get => _logMessage; }

        public ExceptionBase(string reasonPhrase,
                           ExceptionClass ex) : base(reasonPhrase, ex)
        {
        }

        public ExceptionBase() : base()
        {
        }

        public ExceptionBase(string message) : base(message)
        {
        }
    }
}
