using System;

namespace TDL.Services.Dto.MyDayPage
{
    public class CreateSimpleTodoRequestDto
    {
        public string Title { get; set; }

        public Guid? CategoryId { get; set; }

        public DateTime? TodoDate { get; set; }
    }
}
