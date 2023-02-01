using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TDL.Domain.Entities;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.Extensions;
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

        public TodoOfDateResponseDto CreateSimpleTodo(CreateSimpleTodoRequestDto request)
        {
            using var scope = _uow.Provide();

            Guid id = Guid.NewGuid();

            Todo response = BuildSimpleTodo(id, request.Title);

            _todoRepository.Add(response);

            scope.Complete();

            return new TodoOfDateResponseDto
            {
                Id = id,
                Title = request.Title,
                CreatedAt = DateTime.Now,
                IsCompleted = false,
                IsPinned = false,
                TodoCategory = string.Empty
            };
        }

        public IList<Todo> GetAllTodo()
        {
            using var scope = _uow.Provide();

            var response = _todoRepository.GetAll(true).ToList();

            return response;
        }

        public IList<SuggestionTodoItemResponse> GetSuggestTodoList(string keyword)
        {
            using var scope = _uow.Provide();

            DateTime now = DateTime.Now;
            DateTime sevenDaysBefore = now.AddDays(-7);

            var response = _todoRepository.GetAll(true)
                .Where(td => td.CreatedAt > sevenDaysBefore && td.CreatedAt.Value != now)
                .Where(td => string.IsNullOrEmpty(keyword) || td.Title.ContainInvariant(keyword))
                .Take(20)
                .OrderByDescending(td => td.CreatedAt)
                .Select(td => new SuggestionTodoItemResponse
                {
                    Id = td.Id,
                    CategoryName = td.CategoryName,
                    Title = td.Title,
                    CreatedAt = td.CreatedAt
                }).ToList();

            foreach(var item in response)
            {
                item.DateRemind = BuildDateRemind((DateTime)item.CreatedAt);
            }

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

        public IList<TodoOfDateResponseDto> GetTodoOfDate(DateTime dateTime)
        {
            using var scope = _uow.Provide();

            var response = _todoRepository.GetAll(true)
                .Where(td => td.CreatedAt.Value.Date == dateTime.Date &&
                        td.CreatedAt.Value.Month == dateTime.Month &&
                        td.CreatedAt.Value.Year == dateTime.Year)
                .Select(td => new TodoOfDateResponseDto()
                {
                    Id = td.Id,
                    Title = td.Title,
                    TodoCategory = td.CategoryName,
                    CreatedAt = (DateTime)td.CreatedAt,
                    IsCompleted= td.IsCompleted,
                    IsPinned = false
                })
                .ToList();

            return response;
        }

        public void UpdateCompletedOfTask(Guid id)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .FirstOrDefault(td => td.Id == id);

            Guard.ThrowIfNull<NotFoundException>(todo, nameof(Todo));

            todo.IsCompleted = !todo.IsCompleted;

            scope.Complete();
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

        private string BuildDateRemind(DateTime dateTime)
        {
            DateTime now = DateTime.Now;
            double numberOfDays = Math.Ceiling((now - dateTime).TotalDays);
            string dateRemind = $"From {numberOfDays} days ago";

            return dateRemind;
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

        #endregion private method
    }
}
