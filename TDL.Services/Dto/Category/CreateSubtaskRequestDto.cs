using System;

namespace TDL.Services.Dto.Category
{
    public class CreateSubtaskRequestDto
    {
        public string Name { get; set; }

        public Guid TodoId { get; set; }
    }
}