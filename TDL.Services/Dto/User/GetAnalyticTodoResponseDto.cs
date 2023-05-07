using System;
namespace TDL.Services.Dto.User
{
    public class GetAnalyticTodoResponseDto
    {
        public GetAnalyticTodoItemResponseDto SevenDayAnalytic { get; set; }

        public GetAnalyticTodoItemResponseDto OneMonthAnalytic { get; set; }

        public GetAnalyticTodoItemResponseDto PendingAnalytic { get; set; }
    }

    public class GetAnalyticTodoItemResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public double Percentage { get; set; }
    }
}