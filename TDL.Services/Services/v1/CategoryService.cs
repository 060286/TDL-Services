using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using TDL.Domain.Entities;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Services.Dto.Category;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWorkProvider _uowProvider;
        private readonly IRepository<TodoCategory> _todoCategoryRepository;
        private readonly IRepository<Todo> _todoRepository;

        public CategoryService(IUnitOfWorkProvider uowProvider,
            IRepository<TodoCategory> todoCategoryRepository, 
            IRepository<Todo> todoRepository)
        {
            _uowProvider = uowProvider;
            _todoCategoryRepository = todoCategoryRepository;
            _todoRepository = todoRepository;
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
                        .Count(x => x.CreatedBy.EqualsInvariant(userName) && x.CategoryName.EqualsInvariant(x.Title)),
                }).ToList();

            return result;
        }

        public void CreateCategoryItem(CreateCategoryItemRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            TodoCategory todoCategory = new TodoCategory()
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description
            };
            
            _todoCategoryRepository.Add(todoCategory);
            
            scope.Complete();
        }


        public IList<MyListTodoItemResponse> GetMyListTodosItem(MyListTodoItemRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            return null;
        }
    }
}