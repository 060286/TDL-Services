using System;

namespace TDL.Services.Dto.MyDayPage
{
    public class SuggestionTodoItemResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string CategoryName { get; set; }

        public string DateRemind { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
