using System;

namespace TDL.Services.Dto.Category
{
    public class CreateSubTaskResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool isCompleted { get; set; } = false;
    }
}
