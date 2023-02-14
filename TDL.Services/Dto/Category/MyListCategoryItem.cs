using System;

namespace TDL.Services.Dto.Category
{
    public class MyListCategoryItem
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public int? TotalItem { get; set; }
    }
}