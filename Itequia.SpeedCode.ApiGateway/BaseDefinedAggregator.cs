
using ApiGateway.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ocelot.Configuration;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Aggregators
{
    public abstract class BaseDefinedAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            foreach (var response in responses)
            {
                HttpStatusCode httpStatus = response.Items.DownstreamResponse()?.StatusCode ?? HttpStatusCode.GatewayTimeout;
                if (httpStatus != HttpStatusCode.OK && httpStatus != HttpStatusCode.NoContent)
                {
                    DownstreamResponse downstreamResponse = response.Items.DownstreamResponse();
                    string contentResponse = downstreamResponse == null ? "" : await downstreamResponse.Content.ReadAsStringAsync();
                    return GetDownstreamResponseFromResponsesAndReturnObject(responses, contentResponse, httpStatus, downstreamResponse?.ReasonPhrase ?? "ERROR");
                }
            }
            try { return await ExecuteAggregate(responses); }
            catch (Exception ex)
            {
                ObjectResult result = new(new
                {
                    ex.Message,
                    ex.Source,
                    ExceptionType = ex.GetType().FullName,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Stacktrace = ex.GetBaseException()?.StackTrace
                });
                return GetDownstreamResponseFromResponsesAndReturnObject(responses, result.Value, HttpStatusCode.InternalServerError, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public abstract Task<DownstreamResponse> ExecuteAggregate(List<HttpContext> responses);

        public DownstreamResponse GetDownstreamResponseFromResponsesAndReturnObject(List<HttpContext> responses, object returnObject, 
            HttpStatusCode statusCode = HttpStatusCode.OK, string reasonPhrase = "OK")
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var stringContent = new StringContent(JsonConvert.SerializeObject(returnObject, settings));
            stringContent = stringContent.AddMediaTypeJsonHeader();
            var headers = responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList();
            return new DownstreamResponse(stringContent, statusCode, headers, reasonPhrase);
        }
    }
}
