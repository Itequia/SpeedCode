using System;

namespace Itequia.SpeedCode.Filters.FilterConditions
{
    public class NumberFilter : FilterCondition
    {
        public NumberFilter(string columnName, FilterModel model)
        {
            GenerateCondition(model, columnName);
        }

        private void GenerateCondition(FilterModel model, string columnName)
        {
            Condition = model.Type switch
            {
                "equals" => $"{columnName} = {model.Filter}",
                "notEquals" => $"{columnName} <> {model.Filter}",
                "lessThan" => $"{columnName} < {model.Filter}",
                "lessThanOrEqual" => $"{columnName} <= {model.Filter}",
                "greaterThan" => $"{columnName} > {model.Filter}",
                "greaterThanOrEqual" => $"{columnName} >= {model.Filter}",
                "inRange" => $"({columnName} >= {model.Filter} AND {columnName} <= {model.FilterTo})",
                _ => throw new ArgumentOutOfRangeException($"A filter of type {model.Type} cannot be applied to a Number filter.")
            };

            Condition = Condition.Replace(",", ".");
        }
    }
}