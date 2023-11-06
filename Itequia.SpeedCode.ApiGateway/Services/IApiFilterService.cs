

using ApiGateway.Dtos;
using Itequia.SpeedCode.Filters;

namespace ApiGateway.Services
{
    public interface IApiFilterService
    {
        FilteredResult<T> FilterData<T>(IQueryable<T> dataToFilter, FilterOptions filterOptions) where T : BaseAggregateDto;
    }
}
