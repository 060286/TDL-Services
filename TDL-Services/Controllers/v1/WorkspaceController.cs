using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDL.Services.Dto.Workspace;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/workspace-page")]
    public class WorkspaceController : BaseController
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        [HttpPost("workspace")]
        public IActionResult CreateWorkspace([FromBody] CreateWorkspaceRequestDto request)
        {
            _workspaceService.CreateWorkspace(request);

            return Ok();
        }

        [HttpPost("todo-in-workspace")]
        public IActionResult CreateTodoInWorkspace([FromBody] CreateTodoWorkspaceRequestDto request)
        {
            _workspaceService.CreateTodoInWorkspace(request);

            return Ok();
        }
    }
}
