using Microsoft.AspNetCore.Mvc;

namespace TDL.Services.Dto
{
    public class PaginationRequestDto
    {
        [FromQuery(Name = "pageIndex")]
        public int PageIndex { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
    }
}
