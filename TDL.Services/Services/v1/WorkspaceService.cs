using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using TDL.Domain.Entities;
using TDL.Infrastructure.Exceptions;
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

        public WorkspaceService(IRepository<Workspace> workspaceRepository,
            IUnitOfWorkProvider uow,
            IRepository<User> userRepository, 
            IRepository<UserWorkspace> userWorkspaceRepository)
        {
            _uow = uow;
            _workspaceRepository = workspaceRepository;
            _userRepository = userRepository;
            _userWorkspaceRepository = userWorkspaceRepository;
        }

        public void CreateWorkspace(CreateWorkspaceRequestDto request)
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

            foreach (var id in request.UsersId)
            {
                Guid userWorkspaceId = Guid.NewGuid();
                bool isExits = _userRepository.GetAll()
                    .Where(x => x.Id == id).Any();

                Guard.ThrowByCondition<NotFoundException>(!isExits, nameof(User));

                UserWorkspace userWorkspace = new UserWorkspace
                {
                    Id = userWorkspaceId,
                    UserId = id,
                    WorkspaceId = workspaceId,
                };

                _userWorkspaceRepository.Add(userWorkspace);
            }

            scope.Complete();
        }
    }
}
