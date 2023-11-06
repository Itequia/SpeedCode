using Itequia.SpeedCode.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Itequia.SpeedCode.Filters.FilterConditions
{
    public abstract class FilterCondition
    {
        protected string Condition { get; set; }
        protected List<object> Values { get; set; }


        protected FilterCondition()
        {
            Values = new List<object>();
        }

        internal IQueryable<T> AddFilterToQuery<T>(IQueryable<T> query) =>
            Condition.IsNullOrEmpty() ? query :
            Values.Count == 0 ? query.Where(Condition) : query.Where(Condition, Values.ToArray());
    }
}
