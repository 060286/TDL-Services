using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TDL.Domain.Entities;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Services.Dto.NextSevenDayPage;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class NextSevenDayPageService : INextSevenDayPageService
    {
        private readonly IUnitOfWorkProvider _uowProvider;
        private readonly IRepository<Todo> _todoRepository;
        private readonly IColorService _colorService;
        //private readonly IRepository<>

        public NextSevenDayPageService(IUnitOfWorkProvider uowProvider, 
            IRepository<Todo> todoRepository,
            IColorService colorService)
        {
            _uowProvider = uowProvider;
            _todoRepository = todoRepository;
            _colorService = colorService;
        }

        public GetTodoNextSevenDayResponseDto GetNextSevenDay(GetTodoNextSevenDayRequestDto request, string userName)
        {
            using var scope = _uowProvider.Provide();

            var result = new GetTodoNextSevenDayResponseDto();

            var dayOneTask = GetTodoItemNextSevenDayPage(request.DateTime, userName);
            var dayTwoTask = GetTodoItemNextSevenDayPage(request.DateTime.AddDays(1), userName);
            var dayThreeTask = GetTodoItemNextSevenDayPage(request.DateTime.AddDays(2), userName);
            var dayFourTask = GetTodoItemNextSevenDayPage(request.DateTime.AddDays(3), userName);
            var dayFiveTask = GetTodoItemNextSevenDayPage(request.DateTime.AddDays(4), userName);
            var daySixTask = GetTodoItemNextSevenDayPage(request.DateTime.AddDays(5), userName);
            var daySevenTask = GetTodoItemNextSevenDayPage(request.DateTime.AddDays(6), userName);

            result.DayOne = dayOneTask;
            result.DayTwo = dayTwoTask;
            result.DayThree = dayThreeTask;
            result.DayFour = dayFourTask;
            result.DayFive = dayFiveTask;
            result.DaySix = daySixTask;
            result.DaySeven = daySevenTask;
            
            return result;
        }

        private IList<GetTodoNextSevenDayItemResponseDto> GetTodoItemNextSevenDayPage(DateTime datetime, string userName)
        {
            var todayTask = _todoRepository.GetAll(true)
                .Include(td => td.Tags)
                .Include(td => td.TodoCategory)
                .Where(td => td.CreatedAt.Value.Date == datetime.Date && td.CreatedBy.EqualsInvariant(userName))
                .Select(td => new GetTodoNextSevenDayItemResponseDto()
                {
                    Id = td.Id,
                    Category = td.TodoCategory.Title,
                    IsCompleted = td.IsCompleted,
                    Title = td.Title,
                    HaveSubTask = td.SubTasks.Count > 0,
                    Priority = _colorService.PriorityColor(td.Priority)
                }).ToList();

            return todayTask;
        }
    }
}
