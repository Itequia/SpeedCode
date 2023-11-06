using Itequia.SpeedCode.Filters.Extensions;

namespace Itequia.SpeedCode.Filters.FilterConditions
{
    public static class FilterConditionFactory
    {
        public static FilterCondition Create(string colName, FilterModel model, TextSearchOption poorMansGridOptions)
        {

            if (model.FieldType == "text")
                return new TextFilter(colName, model, poorMansGridOptions);

            if (model.FieldType == "number")
                return new NumberFilter(colName, model);

            return new DateFilter(colName, model);

        }
    }
}