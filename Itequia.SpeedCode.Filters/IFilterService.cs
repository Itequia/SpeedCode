using Itequia.SpeedCode.Filters.Extensions;
using System.Linq;

namespace Itequia.SpeedCode.Filters
{
    public interface IFilterService
    {
        FilteredResult<T> Filter<T>(IQueryable<T> query, FilterOptions options);
    }
}