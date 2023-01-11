using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Domain.Entities;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Services.Dto.MyDayPage;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<Todo> _todoRepository;
        private readonly IUnitOfWorkProvider _uow;
        private IMapper _mapper;

        public TodoService(IRepository<Todo> todoRepository,
            IUnitOfWorkProvider uow, 
            IMapper mapper)
        {
            _todoRepository = todoRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public void CreateSimpleTodo(CreateSimpleTodoRequestDto request)
        {
            using var scope = _uow.Provide();

            Guid id = Guid.NewGuid();

            Todo response = BuildSimpleTodo(id, request.Title);

            _todoRepository.Add(response);

            scope.Complete();
        }

        public GetMyDayItemDetailResponseDto GetTodoById(Guid todoId)
        {
            using var scope = _uow.Provide();

            var user = _todoRepository.Get(todoId);

            var result = new GetMyDayItemDetailResponseDto()
            {
                Id = user.Id,
                Title = user.Title
            };

            return result;
        }

        private Todo BuildSimpleTodo(Guid id, string Title)
        {
            return new Todo
            {
                Id = id,
                Title = Title,
                IsArchieved = false,
                IsCompleted = false,
            };
        }
    }
}
