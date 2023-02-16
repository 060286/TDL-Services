using System;
using System.Text.Json.Serialization;

namespace TDL.Services.Dto.Category
{
    public class CreateCategoryItemRequestDto
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Description { get; set; }
    }
}