using AutoMapper;
using System;
using System.Linq;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Services.Dto.AllMyTask;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class AllMyTaskPageService : IAllMyTaskPageService
    {
        private readonly IRepository<Todo> _todoRepository;
        private readonly IUnitOfWorkProvider _uow;
        private readonly IMapper _mapper;

        public AllMyTaskPageService(IRepository<Todo> todoRepository, 
            IUnitOfWorkProvider uow,
            IMapper mapper)
        {
            _todoRepository = todoRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public AllMyTaskListResponseDto GetAllTask(DateTime datetime)
        {
            using var scope = _uow.Provide();

            DateTime nextday = datetime.AddDays(1);
            DateTime firstUpcomingDay = datetime.AddDays(2);
            DateTime lastUpcomingDay = datetime.AddDays(8);

            var todayTask = _todoRepository.GetAll(true)
                .Where(td => td.CreatedAt.Value.Date == datetime)
                .Select(x => AllTaskItemResponseDto.FromTodo(x))
                .ToList();

            var tomorrowTask = _todoRepository.GetAll(true)
                .Where(td => td.CreatedAt.Value.Date == nextday)
                .Select(x => AllTaskItemResponseDto.FromTodo(x))
                .ToList();

            var upCommingDayTask = _todoRepository.GetAll(true)
                .Where(td => td.CreatedAt.Value.Date > firstUpcomingDay && 
                             td.CreatedAt.Value.Date < lastUpcomingDay)
                .Select(x => AllTaskItemResponseDto.FromTodo(x))
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