using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Services.Dto
{
    public class PaginationDto<TData>
    {
        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public IList<TData> Items { get; set; }
    }
}
