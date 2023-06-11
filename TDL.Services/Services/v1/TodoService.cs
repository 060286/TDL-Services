using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IColorService _colorService;
        private readonly IRepository<User> _userRepository;

        public TodoService(IRepository<Todo> todoRepository,
            IUnitOfWorkProvider uow,
            IRepository<TodoCategory> todoCategoryRepository,
            IRepository<SubTask> subtaskRepository,
            IMapper mapper,
            IRepository<Tag> tagRepository,
            IColorService colorService,
            IRepository<User> userRepository)
        {
            _todoRepository = todoRepository;
            _uow = uow;
            _todoCategoryRepository = todoCategoryRepository;
            _subtaskRepository = subtaskRepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
            _colorService = colorService;
            _userRepository = userRepository;
        }

        #endregion constructor

        #region public method

        public ViewArchivedTaskReportResponseDto ViewArchivedTaskRequest(string userName)
        {
            using var scope = _uow.Provide();
            var date = DateTime.Now;
            var diff = date.DayOfWeek - DayOfWeek.Monday;
            var weekStartDate = date.AddDays(-diff).Date;
            var weekEndDate = weekStartDate.AddDays(6);

            var totalTaskArchivedCounter = _todoRepository.GetAll(true)
                .IgnoreQueryFilters()
                .Count(todo => todo.IsArchieved && todo.CreatedBy.EqualsInvariant(userName));

            var totalTaskArchivedThisWeekCounter = _todoRepository.GetAll(true)
                .IgnoreQueryFilters()
                .Where(todo => todo.CreatedAt >= weekStartDate && todo.CreatedAt <= weekEndDate)
                .Count(todo => todo.IsArchieved && todo.CreatedBy.EqualsInvariant(userName));

            var totalTaskArchivedTodayCounter = _todoRepository.GetAll(true)
                .IgnoreQueryFilters()
                .Count(todo => todo.IsArchieved &&
                    todo.CreatedBy.EqualsInvariant(userName) &&
                    todo.CreatedAt.Value.Date == date.Date);

            var archivedDataList = _todoRepository.GetAll(true)
                .IgnoreQueryFilters()
                .Where(todo => todo.IsArchieved &&
                    todo.CreatedBy.EqualsInvariant(userName))
                .Select(x => x.CreatedAt.Value.Date)
                .Distinct()
                .ToList();

            IList<ViewArchivedTaskItem> viewArchivedTaskItems = new List<ViewArchivedTaskItem>();

            foreach (var dateItem in archivedDataList)
            {
                var response = new ViewArchivedTaskItem
                {
                    ArchivedDate = dateItem.Date,
                    ArchivedTasks = _todoRepository.GetAll(true)
                        .IgnoreQueryFilters()
                        .Include(td => td.TodoCategory)
                        .Where(todo => todo.IsArchieved &&
                            todo.CreatedBy.EqualsInvariant(userName))
                        .Select(td => new ViewArchivedTask
                        {
                            CategoryTitle = td.TodoCategory.Title,
                            IsArchived = true,
                            Title = td.Title
                        })
                        .ToList()
                };

                viewArchivedTaskItems.Add(response);
            }

            return new ViewArchivedTaskReportResponseDto
            {
                ArchivedCounter = new ViewArchivedTaskCountItem
                {
                    ThisWeekCount = totalTaskArchivedThisWeekCounter,
                    TodayCount = totalTaskArchivedTodayCounter,
                    TotalItemCount = totalTaskArchivedCounter
                },
                ArchivedTaskList = viewArchivedTaskItems
            };
        }

        public SearchTodoResponseDto SearchTodo(SearchTodoRequestDto request, string UserName)
        {
            using var scope = _uow.Provide();

            var tasks = _todoRepository.GetAll(true)
                .Include(x => x.TodoCategory)
                .Where(x => x.CreatedBy.EqualsInvariant(UserName))
                .Where(x => string.IsNullOrEmpty(request.Keyword) || x.Title.ContainInvariant(request.Keyword))
                .Where(x => !x.IsArchieved)
                .Select(x => new SearchTodoItemResponseDto
                {
                    CategoryId = x.TodoCategory.Id,
                    Id = x.Id,
                    Title = x.Title,
                    CategoryName = x.TodoCategory.Title
                }).ToList();

            var subTasks = _subtaskRepository.GetAll(true)
                .Include(st => st.Todo.TodoCategory)
                .Where(st => st.CreatedBy.EqualsInvariant(UserName))
                .Where(st => string.IsNullOrEmpty(request.Keyword) || st.Title.ContainInvariant(request.Keyword))
                .Where(st => !st.Todo.IsArchieved)
                .Select(st => new SearchTodoItemResponseDto
                {
                    Id = st.Todo.Id,
                    Title = st.Todo.Title,
                    CategoryName = st.Todo.TodoCategory.Title,
                    CategoryId = st.Todo.TodoCategory.Id,
                    SubTaskId = st.Id,
                    SubTaskTitle = st.Title
                }).ToList();

            return new SearchTodoResponseDto
            {
                SubTasks = subTasks,
                Tasks = tasks
            };
        }

        public TodoOfDateResponseDto CreateSimpleTodo(CreateSimpleTodoRequestDto request, string userName)
        {
            using var scope = _uow.Provide();

            string categoryDefault = "Personal";
            Guid id = Guid.NewGuid();
            Guid? categoryId = request.CategoryId;
            int priorityNumber = 1;
            request.TodoDate = request.TodoDate ?? DateTime.UtcNow;

            if (!request.CategoryId.HasValue)
            {
                var result = _todoCategoryRepository.GetAll(true)
                    .FirstOrDefault(tdc => tdc.Title.EqualsInvariant(categoryDefault)
                                           && tdc.CreatedBy.EqualsInvariant(userName));

                if(result == null)
                {
                    //Create default 

                    categoryId = Guid.NewGuid();

                    TodoCategory newTodoCategory = new TodoCategory
                    {
                        Id = (Guid)categoryId,
                        Description = categoryDefault,
                        Title = categoryDefault,
                    };

                    _todoCategoryRepository.Add(newTodoCategory);
                    scope.SaveChanges();
                }
                else
                {
                    categoryId = result.Id;
                }
            }

            // Get max priority 
            var largestTodo = _todoRepository.GetAll(true)
                .Where(x => x.TodoDate.Date == request.TodoDate.Value.Date
                    && x.CreatedBy.EqualsInvariant(userName)
                    && x.CategoryId == categoryId)
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault();

            if (largestTodo != null)
            {
                priorityNumber = largestTodo.Priority + 1;
            }

            Todo response = BuildSimpleTodo(id, request.Title, (Guid)categoryId, priorityNumber, (DateTime)request.TodoDate);

            _todoRepository.Add(response);
            scope.SaveChanges();
            scope.Complete();

            return new TodoOfDateResponseDto
            {
                Id = response.Id,
                Title = response?.Title,
                TodoDate = response.TodoDate,
                Description = response?.Description,
                IsCompleted = response.IsCompleted,
                Priority = response.Priority,
                RemindedAt = response.RemindedAt,
                Tag = _colorService.PriorityColor(response.Tag),
                FileName = response.FileName,
                Status = response.Status,
                IsArchieved = response.IsArchieved,
                SubTasks = response.SubTasks?.Select(st => new SubTaskResponse()
                {
                    IsCompleted = st.IsCompleted,
                    Id = st.Id,
                    Name = st.Title
                }).ToList(),
                //SubTasks = new List<SubTaskResponse>(),
                CategoryName = _todoCategoryRepository.GetAll(true)
                    .FirstOrDefault(x => x.Id == categoryId).Title, // TodoCategory.Title is null here
            };
        }

        public IList<Todo> GetAllTodo()
        {
            using var scope = _uow.Provide();

            var response = _todoRepository.GetAll(true).ToList();

            return response;
        }

        public IList<SuggestionTodoItemResponse> GetSuggestTodoList(string keyword, string userName)
        {
            using var scope = _uow.Provide();

            DateTime now = DateTime.Now;
            DateTime sevenDaysBefore = now.AddDays(-7);

            var response = _todoRepository.GetAll(true)
                .Include(x => x.TodoCategory)
                .Where(td => td.TodoDate > sevenDaysBefore && td.TodoDate != now)
                .Where(td => string.IsNullOrEmpty(keyword) || td.Title.ContainInvariant(keyword))
                .Where(td => td.CreatedBy.EqualsInvariant(userName))
                .Take(20)
                .OrderByDescending(td => td.CreatedAt)
                .Select(td => new SuggestionTodoItemResponse
                {
                    Id = td.Id,
                    CategoryName = td.TodoCategory.Title,
                    Title = td.Title,
                    TodoDate = td.TodoDate,
                    Description = td.Description,
                    IsCompleted = td.IsCompleted,
                    Priority = td.Priority,
                    RemindedAt = td.RemindedAt,
                    Tag = td.Tag,
                    FileName = td.FileName,
                    Status = td.Status,
                    IsArchieved = td.IsArchieved,
                    SubTasks = td.SubTasks.Select(st => new SubTaskResponse()
                    {
                        IsCompleted = st.IsCompleted,
                        Id = st.Id,
                        Name = st.Title
                    }).ToList(),
                }).ToList();

            foreach (var item in response)
            {
                item.DateRemind = BuildDateRemind(item.TodoDate);
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

            todo.IsArchieved = true;

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
                .Where(x => x.IsCompleted && x.TodoDate.Equals(dateTime))
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
                TodoDate = todo.TodoDate,
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
                TodoDate = todo.TodoDate,
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

            _todoRepository.Update(todo);

            scope.Complete();

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

            if (request.IsSameColumn)
            {
                var sortTodos = new List<Todo>();
                //Kéo từ trên xuống
                if (dragTodo.Priority < request.Priority)
                {

                    sortTodos = _todoRepository.GetAll()
                        .Where(td => td.TodoDate.Date == request.DropDate.Date && td.Priority <= request.Priority && td.Priority > dragTodo.Priority && td.Id != request.DragId)
                        .ToList();

                    foreach (var todo in sortTodos)
                    {
                        todo.Priority -= 1;
                    }
                    dragTodo.Priority = request.Priority;
                }
                else
                {
                    sortTodos = _todoRepository.GetAll()
                        .Where(td => td.TodoDate.Date == request.DropDate.Date && td.Priority >= request.Priority && td.Priority < dragTodo.Priority && td.Id != request.DragId)
                        .ToList();
                    foreach (var todo in sortTodos)
                    {
                        todo.Priority += 1;
                    }
                    dragTodo.Priority = request.Priority;
                }
                _todoRepository.Update(dragTodo);
                _todoRepository.UpdateRange(sortTodos);
                scope.Complete();
                return new GetMyDayItemDetailResponseDto()
                {
                    Id = dragTodo.Id,
                    Description = dragTodo.Description,
                    IsCompleted = dragTodo.IsCompleted,
                    Title = dragTodo.Title,
                };
            }

            var dragTodoList = _todoRepository.GetAll()
                .Where(td => td.TodoDate.Date == dragTodo.TodoDate.Date && td.Priority > dragTodo.Priority)
                .ToList();

            var dropTodo = _todoRepository.GetAll()
                .Where(td => td.TodoDate.Date == request.DropDate.Date
                    && td.Priority >= request.Priority)
                .ToList();

            foreach (var todo in dragTodoList)
            {
                todo.Priority -= 1;
            }

            foreach (var todo in dropTodo)
            {
                todo.Priority += 1;
            }


            dragTodo.TodoDate = request.DropDate;
            dragTodo.Priority = request.Priority;

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

        public void RemoveSubTaskById(Guid id)
        {
            using var scope = _uow.Provide();

            var subTask = _subtaskRepository.GetAll(true)
                .FirstOrDefault(sb => sb.Id == id);

            Guard.ThrowIfNull<NotFoundException>(subTask, "Not found Sub Task");

            _subtaskRepository.Delete(subTask);

            scope.Complete();
        }

        public void UpdateRemindAt(DateTime? remindAt, Guid todoId)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll().FirstOrDefault(x => x.Id == todoId);

            Guard.ThrowIfNull<NotFoundException>(todo, "Not found Todo");

            todo.RemindedAt = remindAt;

            _todoRepository.Update(todo);

            scope.Complete();
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
                    Tag = _colorService.PriorityColor(td.Tag),
                    SubTasks = td.SubTasks
                        .Select(st => new SubTaskResponse()
                        {
                            Id = st.Id,
                            IsCompleted = st.IsCompleted,
                            Name = st.Title
                        }).ToList(),
                    AssignUserInfo = _userRepository.GetAll(true)
                        .Where(us => us.Id == td.AssginmentUserId).Select(res => new AssignUserInfo
                        {
                            Email = res.Email,
                            Img = res.Img,
                            UserName = res.UserName
                        }).FirstOrDefault(),
                    TodoDate = td.TodoDate
                }).FirstOrDefault(td => td.Id == todoId);

            return response;
        }

        public IList<TodoOfDateResponseDto> GetTodoOfDate(DateTime dateTime)
        {
            using var scope = _uow.Provide();

            var (date, month, year) = (dateTime.Date, dateTime.Month, dateTime.Year);

            var response = _todoRepository.GetAll(true)
                .Include(td => td.TodoCategory)
                .Where(td => td.TodoDate.Date == dateTime.Date
                    && td.TodoDate.Month == dateTime.Month
                    && td.TodoDate.Year == dateTime.Year)
                .OrderBy(td => td.Priority)
                .Select(td => new TodoOfDateResponseDto()
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
                    TodoCategory = td.TodoCategory.Title,
                    SubTasks = td.SubTasks.Select(st => new SubTaskResponse()
                    {
                        IsCompleted = st.IsCompleted,
                        Id = st.Id,
                        Name = st.Title
                    }).ToList(),
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
            string dateRemind = $"From {Math.Abs(numberOfDays)} days ago";

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
