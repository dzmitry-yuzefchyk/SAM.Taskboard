using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Taskboard.DataProvider.Utility
{
    public abstract class Pagination<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }
    }
}
