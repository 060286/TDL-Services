using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<TodoCategory> _todoCategoryRepository;
        private readonly IRepository<SubTask> _subtaskRepository;
        private IMapper _mapper;

        public TodoService(IRepository<Todo> todoRepository,
            IUnitOfWorkProvider uow, 
            IRepository<TodoCategory> todoCategoryRepository,
            IRepository<SubTask> subtaskRepository,
            IMapper mapper)
        {
            _todoRepository = todoRepository;
            _uow = uow;
            _todoCategoryRepository = todoCategoryRepository;
            _subtaskRepository = subtaskRepository;
            _mapper = mapper;
        }

        #endregion constructor

        #region public method

        public TodoOfDateResponseDto CreateSimpleTodo(CreateSimpleTodoRequestDto request, string userName)
        {
            using var scope = _uow.Provide();
            string categoryDefault = "Personal";
            Guid id = Guid.NewGuid();
            Guid categoryId = Guid.Empty;
            
            
            // Guid categoryIdDefault = request.CategoryId == Guid.NewGuid()
            //     ? _todoCategoryRepository.GetAll(true)
            //         .FirstOrDefault(tdc => tdc.Title.EqualsInvariant(categoryDefault))!
            //         .Id
            //     : request.CategoryId;

            if (!request.CategoryId.HasValue)
            {
                categoryId = _todoCategoryRepository.GetAll(true)
                    .FirstOrDefault(tdc => tdc.Title.EqualsInvariant(categoryDefault)
                                           && tdc.CreatedBy.EqualsInvariant(userName))!
                    .Id;
            }
            
            Todo response = BuildSimpleTodo(id, request.Title, categoryId);

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
                .Include(x => x.TodoCategory)
                .Where(td => td.CreatedAt > sevenDaysBefore && td.CreatedAt.Value != now)
                .Where(td => string.IsNullOrEmpty(keyword) || td.Title.ContainInvariant(keyword))
                .Take(20)
                .OrderByDescending(td => td.CreatedAt)
                .Select(td => new SuggestionTodoItemResponse
                {
                    Id = td.Id,
                    CategoryName = td.TodoCategory.Title,
                    Title = td.Title,
                    CreatedAt = td.CreatedAt
                }).ToList();

            foreach(var item in response)
            {
                if (item.CreatedAt != null) item.DateRemind = BuildDateRemind((DateTime)item.CreatedAt);
            }

            return response;
        }

        /// <summary>
        /// Change Todo Category of Todo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string ChangeCategoryTitle(ChangeTodoCategoryRequestDto request, string userName)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .FirstOrDefault(td => td.Id.Equals(request.TodoId));
            var oldCategory = _todoCategoryRepository.GetAll(true)
                .FirstOrDefault(tdc => tdc.Id.Equals(todo.CategoryId) &&
                                       tdc.CreatedBy.EqualsInvariant(userName));

            var newCategory = _todoCategoryRepository.GetAll(true)
                .FirstOrDefault(tdc => tdc.Title.EqualsInvariant(request.CategoryName) &&
                                       tdc.CreatedBy.EqualsInvariant(userName));
                
            Guard.ThrowIfNull<NotFoundException>(todo, nameof(Todo));
            Guard.ThrowIfNull<NotFoundException>(oldCategory, nameof(TodoCategory));
            Guard.ThrowIfNull<NotFoundException>(newCategory, nameof(TodoCategory));

            todo.CategoryId = newCategory.Id;

            return newCategory.Title;
        }

        public void ArchieTodo(Guid id)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .IgnoreQueryFilters()
                .FirstOrDefault(tdc => tdc.Id.Equals(id));
            
            Guard.ThrowIfNull<NotFoundException>(todo, nameof(Todo));
            
            _todoRepository.Delete(todo);
            
            scope.Complete();
        }

        public void CompletedTodo(Guid id)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .FirstOrDefault(td => td.Equals(id));
            
            Guard.ThrowIfNull<NotFoundException>(todo, nameof(Todo));

            todo.IsCompleted = !todo.IsCompleted;

            scope.Complete();
        }

        public int CountTaskNotCompleted(DateTime dateTime, string userName)
        {
            using var scope = _uow.Provide();

            var notCompletedCount = _todoRepository.GetAll(true)
                .Where(x => x.IsCompleted && x.CreatedAt.Equals(dateTime))
                .Count(x => x.CreatedBy.EqualsInvariant(userName));

            return notCompletedCount;
        }

        public GetMyDayItemDetailResponseDto GetTodoById(Guid todoId)
        {
            using var scope = _uow.Provide();

            var response = _todoRepository.GetAll(true)
                .Include(td => td.SubTasks)
                .Select(td => new GetMyDayItemDetailResponseDto
                {
                    Id = td.Id,
                    Title = td.Title,
                    IsCompleted = td.IsCompleted,
                    Description = td.Description,
                    RemindedAt = td.RemindedAt,
                    SubTasks = _subtaskRepository.GetAll(true)
                        .Where(st => st.TodoId == td.Id).Select(st => new SubTaskResponse()
                        {
                            Id = st.Id,
                            IsCompleted = st.IsCompleted,
                            Name = st.Title
                        }).ToList()
                }).FirstOrDefault(td => td.Id == todoId);

            return response;
        }

        public IList<TodoOfDateResponseDto> GetTodoOfDate(DateTime dateTime)
        {
            using var scope = _uow.Provide();

            var response = _todoRepository.GetAll(true)
                .Include(td => td.TodoCategory)
                .Where(td => td.CreatedAt.Value.Date == dateTime.Date &&
                        td.CreatedAt.Value.Month == dateTime.Month &&
                        td.CreatedAt.Value.Year == dateTime.Year)
                .Select(td => new TodoOfDateResponseDto()
                {
                    Id = td.Id,
                    Title = td.Title,
                    TodoCategory = td.TodoCategory.Title,
                    CreatedAt = (DateTime)td.CreatedAt,
                    IsCompleted= td.IsCompleted,
                    IsPinned = false
                })
                .ToList();

            return response;
        }

        public void UpdateTodo(UpdateTodoRequestDto request)
        {
            using var scope = _uow.Provide(); 

            var todo = _todoRepository.Get(request.Id);

            Guard.ThrowIfNull<NotFoundException>(todo, nameof(Todo));

            todo.CategoryId = request.TodoCategoryId;
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

        private Todo BuildSimpleTodo(Guid id, string title, Guid categoryId)
        {
            return new Todo
            {
                Id = id,
                Title = title,
                IsArchieved = false,
                IsCompleted = false,
                CategoryId = categoryId
            };
        }

        #endregion private method
    }
}
