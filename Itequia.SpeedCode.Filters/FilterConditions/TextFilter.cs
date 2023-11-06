using Itequia.SpeedCode.Filters.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Itequia.SpeedCode.Filters.FilterConditions
{
    public class TextFilter : FilterCondition
    {
        public TextFilter(string columnName, FilterModel model, TextSearchOption options)
        {
            GenerateTextCondition(model, columnName, options);
        }

        private void GenerateTextCondition(FilterModel model, string columnName, TextSearchOption? options)
        {
                      
            var insensitiveCased = options == TextSearchOption.ForceCaseInsensitive ? ".ToLower()" : string.Empty;
            var cleanCharacters = ".ToLower().Replace(\" \", \"\").Replace(\"'\", \"\").Replace(\".\", \"\").Replace(\",\", \"\").Replace(\"-\", \"\").Replace(\";\", \"\")";
            
            AddValues(model.Filter);

            Condition = model.Type switch
            {
                "equals" => string.Join(" OR ", Values.Select(x => $@"{columnName}{insensitiveCased} = ""{x}""{insensitiveCased}")),
                "notEquals" => $@"{columnName}{insensitiveCased} != ""{model.Filter}""{insensitiveCased}",
                "contains" => $"{columnName}{insensitiveCased}.Contains(@0{insensitiveCased})",
                "notContains" => $"!{columnName}{insensitiveCased}.Contains(@0{insensitiveCased})",
                "startsWith" => $"{columnName}{insensitiveCased}.StartsWith(@0{insensitiveCased})",
                "notStartsWith" => $"!{columnName}{insensitiveCased}.StartsWith(@0{insensitiveCased})",
                "endsWith" => $"{columnName}{insensitiveCased}.EndsWith(@0{insensitiveCased})",
                "notEndsWith" => $"!{columnName}{insensitiveCased}.EndsWith(@0{insensitiveCased})",
                "specialContains"=> $"{columnName}{cleanCharacters}.Contains(@0{cleanCharacters})",
                _ => throw new ArgumentOutOfRangeException($"A filter of type {model.Type} cannot be applied to a Text filter.")
            };
        }
        
        private void AddValues(object filter)
        {
            if (filter == null) return;
            else if (filter is string) Values.Add(filter);
            else if (filter is IEnumerable<string> || filter is IEnumerable<object>)
            {
                IEnumerable<object> filterValues = filter as IEnumerable<string>;
                filterValues = filterValues == null ? filter as IEnumerable<object> : filterValues;

                Values.AddRange(filterValues);
            }
            else Values.Add(filter);
        }
    }
}