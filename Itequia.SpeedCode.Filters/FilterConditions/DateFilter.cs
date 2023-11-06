using Itequia.SpeedCode.Extensions;
using System;

namespace Itequia.SpeedCode.Filters.FilterConditions
{
    public class DateFilter : FilterCondition
    {

        public DateFilter(string columnName, FilterModel model)
        {
            model.Filter = model.Filter.ToDateTime();
            model.FilterTo = model.FilterTo.ToDateTime();

            GenerateCondition(model, columnName);
        }

        private void GenerateCondition(FilterModel model, string columnName)
        {
            Values.Add(model.Filter);
            if (model.FilterTo != null) Values.Add(model.FilterTo);

            Condition = model.Type switch
            {
                "equals" => $"{columnName} = @0",
                "notEquals" => $"{columnName} <> @0",
                "lessThan" => $"{columnName} < @0",
                "lessThanOrEqual" => $"{columnName} <= @0",
                "greaterThan" => $"{columnName} > @0",
                "greaterThanOrEqual" => $"{columnName} >= @0",
                "inRange" => $"({columnName} >= @0 AND {columnName} <= @1)",
                _ => throw new ArgumentOutOfRangeException($"A filter of type {model.Type} cannot be applied to a Date filter.")
            };
        }
    }
}