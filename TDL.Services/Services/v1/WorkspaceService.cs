using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using TDL.Domain.Entities;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Infrastructure.Utilities;
using TDL.Services.Dto.Workspace;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IRepository<Workspace> _workspaceRepository;
        private readonly IUnitOfWorkProvider _uow;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserWorkspace> _userWorkspaceRepository;
        private readonly IRepository<Todo> _todoRepository;
        private readonly IRepository<Section> _sectionRepository;

        public WorkspaceService(IRepository<Workspace> workspaceRepository,
            IUnitOfWorkProvider uow,
            IRepository<User> userRepository,
            IRepository<UserWorkspace> userWorkspaceRepository,
            IRepository<Todo> todoRepository, 
            IRepository<Section> sectionRepository)
        {
            _uow = uow;
            _workspaceRepository = workspaceRepository;
            _userRepository = userRepository;
            _userWorkspaceRepository = userWorkspaceRepository;
            _todoRepository = todoRepository;
            _sectionRepository = sectionRepository;
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
                    Description = $"Desc of {SectionNameConstant.Todo}",
                    WorkspaceId = workspaceId,
                },
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
                    Description = $"Desc of {SectionNameConstant.Todo}",
                    WorkspaceId = workspaceId,

                },
            };

            _sectionRepository.AddRange(sections);


            scope.Complete();

            return new CreateWorkspaceResponseDto
            {
                Id = workspaceId,
                Name = request.Name,
                Description = request.Description,
            };
        }

        public void CreateTodoInWorkspace(CreateTodoWorkspaceRequestDto request)
        {
            using var scope = _uow.Provide();

            var workspace = _workspaceRepository.Get(request.WorkspaceId);

            Guard.ThrowIfNull<NotFoundException>(workspace, nameof(Workspace));

            var todo = new Todo
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                RemindedAt = request.RemindedAt,
                Title = request.Title,
                Status = request.Status,
            };

            _todoRepository.Add(todo);

            scope.Complete();
        }

        public IList<GetWorkspaceResponseDto> GetAllWorkspaces(string userName)
        {
            using var scope = _uow.Provide();

            var workspaces = _workspaceRepository.GetAll(true)
                .Where(ws => ws.CreatedBy.EqualsInvariant(userName))
                .Select(ws => new GetWorkspaceResponseDto
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    Description = ws.Description,
                })
                .ToList();

            return workspaces;   
        }

        public GetWorkspaceDetailResponseDto GetWorkspaceById(Guid id, string userName)
        {
            using var scope = _uow.Provide();

            var response = _workspaceRepository.GetAll(true)
                .Include(ws => ws.UserWorkspaces)
                .Where(ws => ws.Id == id && ws.CreatedBy.EqualsInvariant(userName))
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
                            Img = user.Img
                        }).ToList()
                })
                .FirstOrDefault();

            Guard.ThrowIfNull<NotFoundException>(response, $"Not found {nameof(Workspace)}");

            return response;
        }
    }
}
