using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using TDL.Domain.Entities;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Enums;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Infrastructure.Utilities;
using TDL.Services.Dto;
using TDL.Services.Dto.Workspace;
using TDL.Services.Services.v1.Interfaces;
using TDL.Services.SignalR.Hubs;
using TDL.Services.SignalR.Hubs.Interfaces;

namespace TDL.Services.Services.v1
{
    public class WorkspaceService : IWorkspaceService
    {
        #region constructor

        private readonly IRepository<Workspace> _workspaceRepository;
        private readonly IUnitOfWorkProvider _uow;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserWorkspace> _userWorkspaceRepository;
        private readonly IRepository<Todo> _todoRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly IRepository<Notification> _notificationRepository;

        public WorkspaceService(IRepository<Workspace> workspaceRepository,
            IUnitOfWorkProvider uow,
            IRepository<User> userRepository,
            IRepository<UserWorkspace> userWorkspaceRepository,
            IRepository<Todo> todoRepository, 
            IRepository<Section> sectionRepository,
            IHubContext<NotificationHub, INotificationClient> hubContext, 
            IRepository<Notification> notificationRepository)
        {
            _uow = uow;
            _workspaceRepository = workspaceRepository;
            _userRepository = userRepository;
            _userWorkspaceRepository = userWorkspaceRepository;
            _todoRepository = todoRepository;
            _sectionRepository = sectionRepository;
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
        }

        #endregion

        #region implement method

        public AssignUserResponseDto AssignUser(AssignUserRequestDto request)
        {
            using var scope = _uow.Provide();

            var todo = _todoRepository.GetAll()
                .FirstOrDefault(td => td.Id == request.TodoId);

            var user = _userRepository.GetAll(true)
                .FirstOrDefault(user => user.Email.EqualsInvariant(request.Email));

            Guard.ThrowIfNull<NotFoundException>(todo, "Not found todo");
            Guard.ThrowIfNull<NotFoundException>(user, "Not found user");

            todo.AssginmentUserId = user.Id;

            _todoRepository.Update(todo);
            scope.Complete();

            return new AssignUserResponseDto
            {
                Email = user.Email,
                Img = user.Img,
                UserName = user.UserName
            };
        }

        public void DragDropTodoInWorkspace(DragDropTodoInWorkspaceRequestDto requestDto)
        {
            using var scope = _uow.Provide();

            var dragTodo = _todoRepository.GetAll(true)
                .Include(td => td.Section)
                .FirstOrDefault(td => td.Id == requestDto.TodoId);

            requestDto.SectionName = requestDto.DroppableId switch
            {
                0 => "To Do",
                1 => "In Progress",
                2 => "In Review",
                3 => "Completed",
                _ => "To do"
            };

            var section = _sectionRepository.GetAll(true)
                .Where(sc => sc.Name.EqualsInvariant(requestDto.SectionName) && sc.WorkspaceId == dragTodo.WorkspaceId)
                .FirstOrDefault();

            Guard.ThrowIfNull<NotFoundException>(dragTodo, "Cannot find Todo");

            bool isTheSameSection = dragTodo.Section.Name.EqualsInvariant(requestDto.SectionName);

            if (isTheSameSection)
            {
                var sortTodos = new List<Todo>();

                if (dragTodo.Priority < requestDto.Priority)
                {

                    sortTodos = _todoRepository.GetAll()
                        .Where(td => td.Priority <= requestDto.Priority && 
                            td.Priority > dragTodo.Priority && 
                            td.Id != requestDto.TodoId &&
                            td.Section.Name.EqualsInvariant(requestDto.SectionName))
                        .ToList();

                    foreach (var todo in sortTodos)
                    {
                        todo.Priority -= 1;
                    }
                    dragTodo.Priority = requestDto.Priority;
                }
                else
                {
                    sortTodos = _todoRepository.GetAll()
                        .Where(td => td.Priority >= requestDto.Priority && 
                            td.Priority < dragTodo.Priority && 
                            td.Id != requestDto.TodoId &&
                            td.Section.Name.EqualsInvariant(requestDto.SectionName))
                        .ToList();

                    foreach (var todo in sortTodos)
                    {
                        todo.Priority += 1;
                    }
                    dragTodo.Priority = requestDto.Priority;
                }

                _todoRepository.Update(dragTodo);
                _todoRepository.UpdateRange(sortTodos);
                scope.Complete();

                return;
            }

            // Is The Same Column = False

            var dragTodoList = _todoRepository.GetAll()
               .Where(td => td.TodoDate.Date == dragTodo.TodoDate.Date && td.Priority > dragTodo.Priority)
               .ToList();

            var dropTodo = _todoRepository.GetAll()
                .Where(td => td.SectionId == section.Id
                    && td.Priority >= requestDto.Priority)
                .ToList();

            foreach (var todo in dragTodoList)
            {
                todo.Priority -= 1;
            }

            foreach (var todo in dropTodo)
            {
                todo.Priority += 1;
            }


            dragTodo.SectionId = section.Id;
            dragTodo.Priority = requestDto.Priority;

            _todoRepository.Update(dragTodo);
            _todoRepository.UpdateRange(dropTodo);
            _todoRepository.UpdateRange(dragTodoList);

            scope.SaveChanges();
            scope.Complete();
        }

        public GetTodoListInWorkspaceResponseDto GetTodoListInWorkspace(GetTodoListInWorkspaceRequestDto request)
        {
            using var scope = _uow.Provide();

            var todoList = GetTodoListBySectionName(sectionName: SectionNameConstant.Todo, request.Id);
            var inProgressList = GetTodoListBySectionName(sectionName: SectionNameConstant.InProgress, request.Id);
            var inReviewList = GetTodoListBySectionName(sectionName: SectionNameConstant.InReview, request.Id);
            var completedList = GetTodoListBySectionName(sectionName: SectionNameConstant.Completed, request.Id);

            return new GetTodoListInWorkspaceResponseDto
            {
                TodoList = todoList,
                InProgressList = inProgressList,
                CompletedList = completedList,
                InReviewList = inReviewList,
            };
        }

        public CreateWorkspaceResponseDto CreateWorkspace(CreateWorkspaceRequestDto request, Guid userId)
        {
            using var scope = _uow.Provide();
            Guid workspaceId = Guid.NewGuid();

            Workspace workspace = new Workspace
            {
                Id = workspaceId,
                Name = request.Name,
                Description = request.Description,
            };

            _workspaceRepository.Add(workspace);
            scope.SaveChanges();

            Guid userWorkspaceId = Guid.NewGuid();
            bool isExits = _userRepository.GetAll()
                .Any(x => x.Id == userId);

            Guard.ThrowByCondition<NotFoundException>(!isExits, nameof(User));

            UserWorkspace userWorkspace = new UserWorkspace
            {
                Id = userWorkspaceId,
                UserId = userId,
                WorkspaceId = workspaceId,
            };
            _userWorkspaceRepository.Add(userWorkspace);

            var sections = BuildSection(workspaceId);

            _sectionRepository.AddRange(sections);


            scope.Complete();

            return new CreateWorkspaceResponseDto
            {
                Id = workspaceId,
                Name = request.Name,
                Description = request.Description,
            };
        }

        public IList<GetWorkspaceResponseDto> GetAllWorkspaces(string userName, Guid userId)
        {
            using var scope = _uow.Provide();

            var workspaces = _workspaceRepository.GetAll(true)
                .Include(x => x.UserWorkspaces)
                .Where(x => x.CreatedBy.EqualsInvariant(userName) ||
                    x.UserWorkspaces.FirstOrDefault(uws => uws.WorkspaceId == x.Id && uws.UserId == userId) != null)
                .Select(uw => new GetWorkspaceResponseDto
                {
                    Id = uw.Id,
                    Description = uw.Description,
                    Name = uw.Name,
                })
                .ToList();

            return workspaces;   
        }

        public GetWorkspaceDetailResponseDto GetWorkspaceById(Guid id, string userName)
        {
            using var scope = _uow.Provide();

            var response = _workspaceRepository.GetAll(true)
                .Include(ws => ws.UserWorkspaces)
                .Where(ws => ws.Id == id || ws.CreatedBy.EqualsInvariant(userName))
                .Select(ws => new GetWorkspaceDetailResponseDto
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    TotalUser = ws.UserWorkspaces.Count(),
                    Users = ws.UserWorkspaces.Join(_userRepository.GetAll(true),
                        workspace => workspace.UserId,
                        user => user.Id,
                        (workspace, user) => new UserInfoDetail
                        {
                            Id = user.Id,
                            Img = user.Img,
                            UserName = $"{user.FirstName} {user.LastName}"
                        }).ToList()
                })
                .FirstOrDefault();

            Guard.ThrowIfNull<NotFoundException>(response, $"Not found {nameof(Workspace)}");

            return response;
        }

        public void AddUserIntoWorkspace(AddUserIntoWorkspaceRequestDto requestDto, Guid userId)
        {
            using var scope = _uow.Provide();

            var owner = _userRepository.GetAll(true)
                .FirstOrDefault(us => us.Email.EqualsInvariant(requestDto.Email));

            var actor = _userRepository.GetAll(true)
                .FirstOrDefault(us => us.Id == userId);

            var workspace = _workspaceRepository.GetAll(true)
                .FirstOrDefault(ws => ws.Id == requestDto.WorkspaceId);

            Guard.ThrowIfNull<NotFoundException>(owner, "Cannot found owner");
            Guard.ThrowIfNull<NotFoundException>(actor, "Cannot found actor");
            Guard.ThrowIfNull<NotFoundException>(workspace, "Cannot found Workspace");

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                OwnerId = owner.Id,
                ActorId = actor.Id,
                Content = $"{actor.FirstName} {actor.LastName} added you into workspace ${workspace.Name}",
                IsViewed = false,
                Title = "Add User Into Workspace",
            };

            var userWorkspace = new UserWorkspace
            {
                Id = Guid.NewGuid(),
                WorkspaceId = requestDto.WorkspaceId,
                UserId = owner.Id,
            };

            _notificationRepository.Add(notification);
            _userWorkspaceRepository.Add(userWorkspace);

            scope.SaveChanges();
            scope.Complete();

            // Send notification
            _hubContext.Clients.User(owner.UserName).SendNotification(new NotificationResponseDto
            {
                Content = $"{actor.FirstName} {actor.LastName} added you into workspace!",
                Id = Guid.NewGuid(),
                Type = NotificationType.AddUserWorkspace.ToString()
            });
        }

        

        public AddTodoInWorkspaceResponseDto AddTodoInWorkspace(AddTodoIntoWorkspaceRequestDto request)
        {
            using var scope = _uow.Provide();

            var workspace = _workspaceRepository.GetAll()
                .FirstOrDefault(ws => ws.Id == request.WorkspaceId);

            var sectionName = request.Position switch {
                0 => "To do",
                1 => "In Progress",
                2 => "In Review",
                3 => "Completed",
                _ => "To do",
            };

            var section = _sectionRepository.GetAll(true)
                .FirstOrDefault(se => se.WorkspaceId == workspace.Id && se.Name.EqualsInvariant(sectionName));
                

            List<int> maxPriority = _todoRepository.GetAll()
                .Where(td => td.WorkspaceId == request.WorkspaceId)
                .Where(td => td.SectionId == section.Id)
                .Select(x => x.Priority)
                .ToList();

            Guard.ThrowIfNull<NotFoundException>(workspace, $"Cannot find workspace by Id: {request.WorkspaceId}");
            Guard.ThrowIfNull<NotFoundException>(section, $"Cannot find sections by WorkspaceId: {request.WorkspaceId}");

            Todo newTodo = new Todo
            {
                Id = Guid.NewGuid(),
                WorkspaceId = workspace.Id,
                Title = request.Title,
                Description = request.Description,
                SectionId = section.Id,
                IsArchieved = false,
                IsCompleted = false,
                Priority = maxPriority.Any() ? maxPriority.Max() + 1 : 1,
                RemindedAt = null,
                Tag = TagDefinition.Nothing.ToString(),
            };

            _todoRepository.Add(newTodo);

            scope.SaveChanges();
            scope.Complete();

            return new AddTodoInWorkspaceResponseDto
            {
                Id = newTodo.Id,
                WorkspaceId = newTodo.WorkspaceId,
                Description = newTodo.Description,
                IsArchieved = false,
                IsCompleted = false,
                Priority = newTodo.Priority,
                Tag = newTodo.Tag,
                RemindedAt = newTodo.RemindedAt,
                TodoDate = newTodo.TodoDate,
                Title = newTodo.Title,
                SectionId = newTodo.SectionId,
                CategoryId = null,
            };
        }

        #endregion

        #region private method

        private IList<Section> BuildSection(Guid workspaceId)
        {
            // Build Default Section
            IList<Section> sections = new List<Section>
            {
                new Section
                {
                    Id = Guid.NewGuid(),
                    Name = SectionNameConstant.Todo,
                    Priority = 1,
                    Description = $"Desc of {SectionNameConstant.Todo}",
                    WorkspaceId = workspaceId,
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Name = SectionNameConstant.Todo,
                    Priority = 1,
                    Description = $"Desc of {SectionNameConstant.InProgress}",
                    WorkspaceId = workspaceId,
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Name = SectionNameConstant.Todo,
                    Priority = 1,
                    Description = $"Desc of {SectionNameConstant.InReview}",
                    WorkspaceId = workspaceId,
                },
                new Section
                {
                    Id = Guid.NewGuid(),
                    Name = SectionNameConstant.Todo,
                    Priority = 1,
                    Description = $"Desc of {SectionNameConstant.Completed}",
                    WorkspaceId = workspaceId,

                },
            };

            return sections;
        }

        private IList<Todo> GetTodoListBySectionName(string sectionName, Guid id)
        {
            return _todoRepository.GetAll(true)
                .Where(td => td.WorkspaceId == id)
                .Where(td => td.Section.Name.EqualsInvariant(sectionName))
                .OrderBy(x => x.Priority)
                .ToList();
        }

        public IList<SearchUserInWorkspaceResponseDto> SearchUserInWorkspace(SearchUserInWorkspaceRequestDto request)
        {
            using var scope = _uow.Provide();

            var response = _userWorkspaceRepository.GetAll(true)
                .Include(ws => ws.User)
                .Where(ws => ws.WorkspaceId == request.WorkspaceId)
                .Where(ws => string.IsNullOrEmpty(request.Keyword) ||
                    ws.User.Email.ContainInvariant(request.Keyword) ||
                    ws.User.UserName.ContainInvariant(request.Keyword))
                .Select(ws => new SearchUserInWorkspaceResponseDto
                {
                    UserName = ws.User.UserName,
                    Email = ws.User.Email,
                    Id = ws.User.Id,
                    Img = ws.User.Img
                }).ToList();

            return response;
        }

        #endregion
    }
}
