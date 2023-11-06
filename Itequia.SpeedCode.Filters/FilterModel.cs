using System;

namespace Itequia.SpeedCode.Filters
{
    public class FilterModel
    {
        /// <summary>
        /// The type of the field we will be filtering
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// The type of filter we will be applying
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The value to compare the filter to (in binary filters it can be used as Filter From)
        /// </summary>
        public object Filter { get; set; }
        /// <summary>
        /// In binary filters the highest value you can reach
        /// </summary>
        public object FilterTo { get; set; }

        public FilterModel() { }
    }
}