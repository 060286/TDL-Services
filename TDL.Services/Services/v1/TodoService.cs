using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TDL.Domain.Entities;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Infrastructure.Utilities;
using TDL.Services.Dto.MyDayPage;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class TodoService : ITodoService
    {
        #region constructor

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

        #endregion constructor

        #region public method

        public void CreateSimpleTodo(CreateSimpleTodoRequestDto request)
        {
            using var scope = _uow.Provide();

            Guid id = Guid.NewGuid();

            Todo response = BuildSimpleTodo(id, request.Title);

            _todoRepository.Add(response);

            scope.Complete();
        }

        public IList<Todo> GetAllTodo()
        {
            using var scope = _uow.Provide();

            var response = _todoRepository.GetAll(true).ToList();

            return response;
        }

        public GetMyDayItemDetailResponseDto GetTodoById(Guid todoId)
        {
            using var scope = _uow.Provide();

            var response = _todoRepository.Get(todoId);

            var result = new GetMyDayItemDetailResponseDto()
            {
                Id = response.Id,
                Title = response.Title,
                Description = response.Description,
                IsCompleted = response.IsCompleted,
                RemindedAt = response.RemindedAt,
                SubTasks =  response.SubTasks?.Select(x => x.Title).ToList(),
                Tag = null,
            };

            return result;
        }

        public void UpdateTodo(UpdateTodoRequestDto request)
        {
            using var scope = _uow.Provide(); 

            var todo = _todoRepository.Get(request.Id);

            Guard.ThrowIfNull<NotFoundException>(todo, nameof(Todo));

            // Update 
            todo.CategoryName = request.TodoCategoryName;
            todo.Title = request.Tittle;
            todo.Description = request.Description;
            todo.RemindedAt = request.RemindedAt;
            todo.IsArchieved = request.IsArchieved;
            todo.IsCompleted = request.IsCompleted;
            
            _todoRepository.Update(todo);

            scope.Complete();
        }

        #endregion public method

        #region private method

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

        #endregion private method
    }
}
