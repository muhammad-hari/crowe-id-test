using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.DataTable
{
    public class DataTableParams
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public SearchParameters Search { get; set; }
        public List<ColumnParameters> Columns { get; set; }
        public List<OrderParameters> Order { get; set; }

        public string GetSortExpression()
        {
            // Generate the sort expression based on the Order parameter
            // This can be used to apply sorting on the database query
            return "desc";
        }
    }
}
