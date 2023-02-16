using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TDL.Domain.Entities;
using TDL.Infrastructure.Enums;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Infrastructure.Utilities;
using TDL.Services.Dto.Category;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWorkProvider _uowProvider;
        private readonly IRepository<TodoCategory> _todoCategoryRepository;
        private readonly IRepository<Todo> _todoRepository;
        private readonly IRepository<SubTask> _subtaskRepository;

        public CategoryService(IUnitOfWorkProvider uowProvider,
            IRepository<TodoCategory> todoCategoryRepository, 
            IRepository<Todo> todoRepository, 
            IRepository<SubTask> subtaskRepository)
        {
            _uowProvider = uowProvider;
            _todoCategoryRepository = todoCategoryRepository;
            _todoRepository = todoRepository;
            _subtaskRepository = subtaskRepository;
        }
        
        public IList<MyListCategoryItem> GetCategoryByUserName(string userName)
        {
            using var scope = _uowProvider.Provide();

            var result = _todoCategoryRepository.GetAll(true)
                .Where(x => x.CreatedBy.EqualsInvariant(userName))
                .Select(x => new MyListCategoryItem()
                {
                    Id = x.Id,
                    Title = x.Title,
                    TotalItem = _todoRepository.GetAll(true)
                        .Count(td => td.CreatedBy.EqualsInvariant(userName) && td.CategoryId == x.Id),
                }).ToList();

            return result;
        }

        public void CreateCategoryItem(CreateCategoryItemRequestDto request)
        {
            using var scope = _uowProvider.Provide();
            string duplicateError = "Todo category is exist!";

            bool isDuplicate = _todoCategoryRepository.GetAll(true)
                .Any(tdc => tdc.Title.EqualsInvariant(request.Title));
            
            Guard.ThrowByCondition<BusinessLogicException>(isDuplicate, duplicateError);
            
            TodoCategory todoCategory = new TodoCategory()
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description
            };
            
            _todoCategoryRepository.Add(todoCategory);
            
            scope.Complete();
        }

        public IList<MyListTodoItemResponse> GetMyListTodosItem(MyListTodoItemRequestDto request, string userName)
        {
            using var scope = _uowProvider.Provide();
            bool isSortByNull = request.SortBy == null;
            bool isSortTypeNull = request.SortType == null;
            request.SortBy = isSortByNull ? request.SortBy = SortBy.UpdatedAt : request.SortBy;
            request.SortType = isSortTypeNull ? request.SortType = SortType.Asc : request.SortType;

            var result = _todoRepository.GetAll(true)
                .Include(x => x.SubTasks)
                .Where(x => x.CategoryId.Equals(request.CategoryId));
            
            if (request.SortType == SortType.Asc)
            {
                result = SortBy.Title == request.SortBy
                    ? result.OrderBy(x => x.Title)
                    : result.OrderBy(x => x.UpdatedAt);
            }

            if (request.SortType == SortType.Desc)
            {
                result = SortBy.Title == request.SortBy
                    ? result.OrderByDescending(x => x.Title)
                    : result.OrderByDescending(x => x.UpdatedAt);
            }
            
            return result.Select(x => new MyListTodoItemResponse
            {
                Id = x.Id,
                IsCompleted = x.IsCompleted,
                Title = x.Title,
                TotalSubtask = x.SubTasks.Count(),
                CompletedSubtask = x.SubTasks.Count(st => st.CreatedBy.EqualsInvariant(userName))
            }).ToList();
        }

        public void CreateSubtask(CreateSubtaskRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            SubTask subtask = new SubTask
            {
                Id = Guid.NewGuid(),
                Title = request.Name,
                TodoId = request.TodoId,
            };
            
            _subtaskRepository.Add(subtask);
            
            scope.Complete();
        }
    }
}