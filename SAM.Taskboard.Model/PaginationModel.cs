using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Taskboard.Model
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int RowsCount { get; set; }
        public int PagesCount
        {
            get { return (int)Math.Ceiling((decimal)RowsCount / PageSize); }
        }
        public List<int> Pages { get; set; }

        public void GetPages()
        {
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
            else if (CurrentPage > PagesCount)
            {
                CurrentPage = PagesCount;
            }

            int startPage, endPage;
            if (PagesCount <= PageSize)
            {
                startPage = 1;
                endPage = PagesCount;
            }
            else
            {
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)PageSize / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)PageSize / (decimal)2) - 1;
                if (CurrentPage <= maxPagesBeforeCurrentPage)
                {
                    startPage = 1;
                    endPage = PageSize;
                }
                else if (CurrentPage + maxPagesAfterCurrentPage >= PagesCount)
                {
                    startPage = PagesCount - PageSize + 1;
                    endPage = PagesCount;
                }
                else
                {
                    startPage = CurrentPage - maxPagesBeforeCurrentPage;
                    endPage = CurrentPage + maxPagesAfterCurrentPage;
                }
            }

            var pages = Enumerable.Range(startPage, (endPage + 1) - startPage).ToList();

            CurrentPage = CurrentPage;
            Pages = pages;
        }
    }
}
