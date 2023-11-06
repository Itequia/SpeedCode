using Itequia.SpeedCode.Filters;
using ApiGateway.Dtos;

namespace ApiGateway.Services
{
    public abstract class ApiFilterService : IApiFilterService
    {
        private readonly IFilterService _filterService;
        public ApiFilterService(IFilterService filterService)
        {
            _filterService = filterService;
        }

        public FilteredResult<T> FilterData<T>(IQueryable<T> dataToFilter, FilterOptions filterOptions) where T : BaseAggregateDto
        {
            var personalizedFilteredData = PersoFilterData(dataToFilter, filterOptions);
            return _filterService.Filter(personalizedFilteredData, filterOptions);
        }

        public abstract IQueryable<T> PersoFilterData<T>(IQueryable<T> dataToFilter, FilterOptions filterOptions);

    }
}
