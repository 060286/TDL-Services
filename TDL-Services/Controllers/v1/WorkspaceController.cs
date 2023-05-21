using Microsoft.AspNetCore.Mvc;
using System;
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
            var result = _workspaceService.CreateWorkspace(request, UserId);

            return Ok(result);
        }

        [HttpPost("todo-in-workspace")]
        public IActionResult CreateTodoInWorkspace([FromBody] CreateTodoWorkspaceRequestDto request)
        {
            _workspaceService.CreateTodoInWorkspace(request);

            return Ok();
        }

        [HttpGet("workspaces")]
        public IActionResult GetWorkspace()
        {
            var result = _workspaceService.GetAllWorkspaces(UserName);

            return Ok(result);
        }

        [HttpGet("{workspaceId}/user-in-workspace")]
        public IActionResult GetUserInWorkspaceById(Guid workspaceId)
        {
            var result = _workspaceService.GetWorkspaceById(workspaceId, UserName);

            return Ok(result);
        }

        [HttpGet("{workspaceId}/todos-in-workspace")]
        public IActionResult GetTodosInWorkspaceById(Guid workspaceId)
        {
            return Ok();
        }
    }
}
