using System;

namespace TDL.Services.Dto.MyDayPage
{
    public class CreateSimpleTodoResponseRequestDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        //public string CategoryName { get; set; }
    }
}
