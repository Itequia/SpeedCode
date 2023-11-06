using System.Collections.Generic;

namespace Itequia.SpeedCode.Filters
{
    public class FilteredResult<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
    }
}