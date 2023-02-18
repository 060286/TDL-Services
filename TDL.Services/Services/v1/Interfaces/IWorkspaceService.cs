using TDL.Services.Dto.Workspace;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface IWorkspaceService
    {
        void CreateWorkspace(CreateWorkspaceRequestDto request);

        void CreateTodoInWorkspace(CreateTodoWorkspaceRequestDto request);
    }
}
