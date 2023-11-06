using ApiGateway.Dtos;
using ApiGateway.Services;
using Itequia.SpeedCode.Filters;
using Itequia.SpeedCode.FiltersAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiGateway.Controllers
{
    [ServiceFilter(typeof(GlobalExceptionFilterAttribute))]
    public class FilterBaseController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiFilterService _apiFilterService;
        public FilterBaseController(IHttpClientFactory httpClientFactory,
                                       IApiFilterService apiFilterService)
        {
            _httpClientFactory = httpClientFactory;
            _apiFilterService = apiFilterService;
        }


        #region Private methods
        protected async Task<FilteredResult<T>> GetFilteredData<T>(FilterOptions filterOptions, string targetUrl) where T : BaseAggregateDto
        {
            string requestUri = targetUrl;
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, requestUri);
            HttpClient httpClient = _httpClientFactory.CreateClient();            
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Request.Headers.Authorization);
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                throw new Exception(errorMessage);
            }
            List<T> data = JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);            
            return _apiFilterService.FilterData(data.AsQueryable(), filterOptions); ;
        }
        #endregion
    }
}
