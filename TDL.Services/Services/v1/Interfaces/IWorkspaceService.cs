using System;
using System.Collections.Generic;
using TDL.Domain.Entities;
using TDL.Services.Dto.Workspace;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface IWorkspaceService
    {
        CreateWorkspaceResponseDto CreateWorkspace(CreateWorkspaceRequestDto request, Guid userId);

        void CreateTodoInWorkspace(CreateTodoWorkspaceRequestDto request);

        IList<GetWorkspaceResponseDto> GetAllWorkspaces(string userName);

        GetWorkspaceDetailResponseDto GetWorkspaceById(Guid id, string userName);
    }
}
