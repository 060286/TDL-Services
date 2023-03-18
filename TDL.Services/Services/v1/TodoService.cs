using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TDL.Domain.Entities;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Enums;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Infrastructure.Utilities;
using TDL.Services.Dto.Color;
using TDL.Services.Dto.MyDayPage;
using TDL.Services.Dto.TodoDto;
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
        private readonly IRepository<Tag> _tagRepository;

        public TodoService(IRepository<Todo> todoRepository,
            IUnitOfWorkProvider uow, 
            IRepository<TodoCategory> todoCategoryRepository,
            IRepository<SubTask> subtaskRepository,
            IMapper mapper,
            IRepository<Tag> tagRepository)
        {
            _todoRepository = todoRepository;
            _uow = uow;
            _todoCategoryRepository = todoCategoryRepository;
            _subtaskRepository = subtaskRepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
        }

        #endregion constructor

        #region public method

        public TodoOfDateResponseDto CreateSimpleTodo(CreateSimpleTodoRequestDto request, string userName)
        {
            using var scope = _uow.Provide();
            string categoryDefault = "Personal";
            Guid id = Guid.NewGuid();
            Guid? categoryId = request.CategoryId;
            int priorityNumber = 1;

            if (!request.CategoryId.HasValue)
            {
                categoryId = _todoCategoryRepository.GetAll(true)
                    .FirstOrDefault(tdc => tdc.Title.EqualsInvariant(categoryDefault)
                                           && tdc.CreatedBy.EqualsInvariant(userName))!
                    .Id;
            }

            // Get max priority 
            var largestTodo = _todoRepository.GetAll(true)
                .Where(x => x.TodoDate == request.TodoDate)
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault();

            if (largestTodo != null)
            {
                priorityNumber = largestTodo.Priority + 1;
            }
            
            Todo response = BuildSimpleTodo(id, request.Title, (Guid)categoryId, priorityNumber, request.TodoDate);

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

        public Todo CompletedTodo(Guid id)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .FirstOrDefault(td => td.Id == id);
            
            Guard.ThrowIfNull<NotFoundException>(todo, nameof(Todo));

            todo.IsCompleted = !todo.IsCompleted;

            scope.SaveChanges();
            scope.Complete();

            return todo;
        }

        public int CountTaskNotCompleted(DateTime dateTime, string userName)
        {
            using var scope = _uow.Provide();

            var notCompletedCount = _todoRepository.GetAll(true)
                .Where(x => x.IsCompleted && x.CreatedAt.Equals(dateTime))
                .Count(x => x.CreatedBy.EqualsInvariant(userName));

            return notCompletedCount;
        }

        public void UpdateSubTaskStatus(Guid id)
        {
            using var scope = _uow.Provide();

            var subTask = _subtaskRepository.GetAll()
                .FirstOrDefault(st => st.Id.Equals(id));

            Guard.ThrowIfNull<NotFoundException>(subTask, string.Format(ExceptionConstant.NotFound, nameof(SubTask)));

            subTask.IsCompleted = !subTask.IsCompleted;

            scope.Complete();
        }

        public GetMyDayItemDetailResponseDto UpdateTodoTitle(Guid id, string title)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .Include(x => x.SubTasks)
                .FirstOrDefault(td => td.Id == id);

            Guard.ThrowIfNull<NotFoundException>(todo, "Not Found Todo");
            
            todo.Title = title;

            scope.SaveChanges();
            scope.Complete();

            return new GetMyDayItemDetailResponseDto()
            {
                Id = todo.Id,
                Title = title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                RemindedAt = todo.RemindedAt,
                SubTasks = todo.SubTasks.Select(st => new SubTaskResponse()
                {
                    Id = st.Id,
                    IsCompleted = st.IsCompleted,
                    Name = st.Title
                }).ToList(),
            };
        }

        public GetMyDayItemDetailResponseDto UpdateTodoDescription(Guid id, string description)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .Include(x => x.SubTasks)
                .FirstOrDefault(td => td.Id == id);

            Guard.ThrowIfNull<NotFoundException>(todo, "Not Found Todo");

            todo.Description = description;

            scope.SaveChanges();
            scope.Complete();

            return new GetMyDayItemDetailResponseDto()
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = description,
                IsCompleted = todo.IsCompleted,
                RemindedAt = todo.RemindedAt,
                SubTasks = todo.SubTasks.Select(st => new SubTaskResponse()
                {
                    Id = st.Id,
                    IsCompleted = st.IsCompleted,
                    Name = st.Title
                }).ToList(),
            };
        }

        public IList<ColorDto> GetTagList()
        {
            IList<ColorDto> tagList = new List<ColorDto>()
            {
                new ColorDto()
                {
                    Text = ColorConstant.Nothing,
                    BackgroundColor = "#Ecc506",
                    Color = "#FFFFFF"
                },
                new ColorDto()
                {
                    Text = ColorConstant.Priority,
                    BackgroundColor = "#F8D220",
                    Color = "#FFFFFF"
                },
                new ColorDto()
                {
                    Text = ColorConstant.Important,
                    BackgroundColor = "#FF0000",
                    Color = "#FFFFFF"
                },
                new ColorDto()
                {
                    Text = ColorConstant.TrackBack,
                    BackgroundColor = "#47ec06",
                    Color = "#FFFFFF"
                },
            };

            return tagList;
        }

        public ColorDto AddTagTodo(AddTagTodoRequestDto request)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .FirstOrDefault(td => td.Id == request.TodoId);

            Guard.ThrowIfNull<NotFoundException>(todo, "Not Found Todo");

            todo.Tag = request.Tag.ToString();
                    
            if (request.Tag == TagDefinition.Important)
            {
                return new ColorDto()
                {
                    Text = ColorConstant.Important,
                    BackgroundColor = "#FF0000",
                    Color = "#FFFFFF"
                };
            }
            
            if (request.Tag == TagDefinition.Nothing)
            {
                return new ColorDto()
                {
                    Text = ColorConstant.Nothing,
                    BackgroundColor = "#Ecc506",
                    Color = "#FFFFFF"
                };
            }
            
            if (request.Tag == TagDefinition.Priority)
            {
                return new ColorDto()
                {
                    Text = ColorConstant.Priority,
                    BackgroundColor = "#F8D220",
                    Color = "#FFFFFF"
                };
            }
            
            if (request.Tag == TagDefinition.TrackBack)
            {
                return new ColorDto()
                {
                    Text = ColorConstant.TrackBack,
                    BackgroundColor = "#47ec06",
                    Color = "#FFFFFF"
                };
            }
            
            return new ColorDto()
            {
                Text = ColorConstant.Priority,
                BackgroundColor = "#F8D220",
                Color = "#FFFFFF"
            };
        }

        public IList<GetTodoCategoryResponseDto> GetTodoCategoryList(string userName)
        {
            using var scope = _uow.Provide();

            var todoCategories = _todoCategoryRepository.GetAll(true)
                .Where(tdc => tdc.CreatedBy.EqualsInvariant(userName))
                .Select(tdc => new GetTodoCategoryResponseDto()
                {
                    Id = tdc.Id,
                    Title = tdc.Title
                }).ToList();

            return todoCategories;
        }

        public GetMyDayItemDetailResponseDto DragAndDropTodo(DragDropTodoRequest request)
        {
            using var scope = _uow.Provide();

            var dragTodo = _todoRepository.GetAll()
                .FirstOrDefault(td => td.Id == request.DragId);

            Guard.ThrowIfNull<NotFoundException>(dragTodo, nameof(Todo));

            var dropTodo = _todoRepository.GetAll()
                .Where(td => td.TodoDate.Date == request.DropDate.Date 
                    && td.Priority >= request.Priority)
                .ToList();

            var dragTodoList = _todoRepository.GetAll()
                .Where(td => td.TodoDate.Date == dragTodo.TodoDate && td.Priority < dragTodo.Priority)
                .ToList();

            if (!dropTodo.IsNullOrEmpty())
            {
                dragTodo.TodoDate = request.DropDate;
                dragTodo.Priority = request.Priority;

                foreach (var todo in dropTodo)
                {
                    todo.Priority += 1;
                }

                foreach (var todo in dragTodoList)
                {
                    todo.Priority -= 1;
                }
            }

            _todoRepository.Update(dragTodo);
            _todoRepository.UpdateRange(dropTodo);
            _todoRepository.UpdateRange(dragTodoList);

            scope.SaveChanges();
            scope.Complete();

            return new GetMyDayItemDetailResponseDto()
            {
                Id = dragTodo.Id,
                Description = dragTodo.Description,
                IsCompleted = dragTodo.IsCompleted,
                Title = dragTodo.Title,
            };
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

        private Todo BuildSimpleTodo(Guid id, string title, Guid categoryId, int priority, DateTime todoDate)
        {
            return new Todo
            {
                Id = id,
                TodoDate = todoDate,
                Title = title,
                IsArchieved = false,
                IsCompleted = false,
                CategoryId = categoryId,
                Priority = priority,
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsu"
            };
        }

        #endregion private method
    }
}
