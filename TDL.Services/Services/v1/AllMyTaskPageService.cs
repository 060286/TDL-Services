using AutoMapper;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Services.Dto.AllMyTask;
using TDL.Services.Dto.MyDayPage;
using TDL.Services.Services.v1.Interfaces;
using TDL.Infrastructure.Extensions;

namespace TDL.Services.Services.v1
{
    public class AllMyTaskPageService : IAllMyTaskPageService
    {
        private readonly IRepository<Todo> _todoRepository;
        private readonly IUnitOfWorkProvider _uow;
        private readonly IMapper _mapper;
        private readonly IColorService _colorService;

        public AllMyTaskPageService(IRepository<Todo> todoRepository, 
            IUnitOfWorkProvider uow,
            IMapper mapper,
            IColorService colorService)
        {
            _todoRepository = todoRepository;
            _uow = uow;
            _mapper = mapper;
            _colorService = colorService;
        }

        public AllMyTaskListResponseDto GetAllTask(DateTime datetime, string userName)
        {
            using var scope = _uow.Provide();

            DateTime nextDay = datetime.AddDays(1);
            DateTime firstUpcomingDay = datetime.AddDays(2);
            DateTime lastUpcomingDay = datetime.AddDays(8);


            var todayTask = _todoRepository.GetAll(true)
                .Where(td => td.TodoDate.Date == datetime.Date && td.CreatedBy.EqualsInvariant(userName))
                .Select(td => new AllTaskItemResponseDto()
                {
                    Id = td.Id,
                    CategoryName = td.TodoCategory.Title,
                    Title = td.Title,
                    TodoDate = td.TodoDate,
                    Description = td.Description,
                    IsCompleted = td.IsCompleted,
                    Priority = td.Priority,
                    RemindedAt = td.RemindedAt,
                    Tag = _colorService.PriorityColor(td.Tag),
                    FileName = td.FileName,
                    Status = td.Status,
                    IsArchieved = td.IsArchieved,
                    SubTasks = td.SubTasks.Select(st => new SubTaskResponse()
                    {
                        IsCompleted = st.IsCompleted,
                        Id = st.Id,
                        Name = st.Title
                    }).ToList(),
                })
                .ToList();

            var tomorrowTask = _todoRepository.GetAll(true)
                .Where(td => td.TodoDate.Date == nextDay.Date && td.CreatedBy.EqualsInvariant(userName))
                .Select(td => new AllTaskItemResponseDto()
                {
                    Id = td.Id,
                    CategoryName = td.TodoCategory.Title,
                    Title = td.Title,
                    TodoDate = td.TodoDate,
                    Description = td.Description,
                    IsCompleted = td.IsCompleted,
                    Priority = td.Priority,
                    RemindedAt = td.RemindedAt,
                    Tag = _colorService.PriorityColor(td.Tag),
                    FileName = td.FileName,
                    Status = td.Status,
                    IsArchieved = td.IsArchieved,
                    SubTasks = td.SubTasks.Select(st => new SubTaskResponse()
                    {
                        IsCompleted = st.IsCompleted,
                        Id = st.Id,
                        Name = st.Title
                    }).ToList(),
                })
                .ToList();

            var upCommingDayTask = _todoRepository.GetAll(true)
                .Where(td => td.TodoDate.Date > firstUpcomingDay.Date && 
                             td.TodoDate.Date < lastUpcomingDay.Date)
                .Where(td => td.CreatedBy.EqualsInvariant(userName))
                .Select(td => new AllTaskItemResponseDto()
                {
                    Id = td.Id,
                    CategoryName = td.TodoCategory.Title,
                    Title = td.Title,
                    TodoDate = td.TodoDate,
                    Description = td.Description,
                    IsCompleted = td.IsCompleted,
                    Priority = td.Priority,
                    RemindedAt = td.RemindedAt,
                    Tag = _colorService.PriorityColor(td.Tag),
                    FileName = td.FileName,
                    Status = td.Status,
                    IsArchieved = td.IsArchieved,
                    SubTasks = td.SubTasks.Select(st => new SubTaskResponse()
                    {
                        IsCompleted = st.IsCompleted,
                        Id = st.Id,
                        Name = st.Title
                    }).ToList(),
                })
                .ToList();

            return new AllMyTaskListResponseDto
            {
                AllTaskToday = todayTask,
                AllTaskTomorrow = tomorrowTask,
                AllTaskUpComming = upCommingDayTask
            };
        }
    }
}