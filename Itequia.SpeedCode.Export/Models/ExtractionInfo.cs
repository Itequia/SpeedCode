using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Export.Models
{
    public class ExtractionInfo
    {
        public string Entity { get; set; }
        public string FullUserName { get; set; }
        public DateTime Date { get; set; }
        public int FileRows { get; set; }
        public string View { get; set; }
        public string Filters { get; set; }
        public string ExtractedFields { get; set; }
        public string Logo { get; set; }

        public int LogoWidth { get; set; }
        public int LogoHeight { get; set; }

        public ExtractionInfo(string entity, string name/*, RentaFilterOptions options*/)
        {
            Entity = entity;
            FullUserName = name;
            Date = DateTime.Now;
            View = "Default";
            //Filters = options.FilterModelsToString();
            //ExtractedFields = options.ExcelColumnDisplayNamesToString();
        }
    }
}
