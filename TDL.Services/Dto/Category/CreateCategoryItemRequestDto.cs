using System;

namespace TDL.Services.Dto.Category
{
    public class CreateCategoryItemRequestDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Description { get; set; }
    }
}