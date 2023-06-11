using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using TDL.Services.Dto.Workspace;
using TDL.Services.Services.v1.Interfaces;
using TDL.Services.SignalR.Hubs.Interfaces;
using TDL.Services.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/workspace-page")]
    public class WorkspaceController : BaseController
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly IHubContext<NotificationHub> _hubContext1;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        public WorkspaceController(IWorkspaceService workspaceService,
            IHubContext<NotificationHub> hubContext1,
            IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _workspaceService = workspaceService;
            _hubContext = hubContext;
            _hubContext1 = hubContext1;
        }

        [HttpPost("post-notify")]
        [AllowAnonymous]
        public async Task<IActionResult> PostNotify(PostNotifyRequestDto requestDto)
        {
            await _hubContext.Clients.User(requestDto.ConnectionId).PostNotify("Send connectionId");
            await _hubContext.Clients.User(UserName).PostNotify("Send userName");
            await _hubContext.Clients.User(UserId.ToString()).PostNotify("Send userId");
            await _hubContext.Clients.User("tamle.dev1@gmail.com").PostNotify("Send email");
            await _hubContext1.Clients.User(requestDto.ConnectionId).SendAsync("tam test");
            //await _hubContext.Clients.User("tamle.dev1").PostNotify(message);
            return Ok();
        }

        [HttpPost("search-user-workspace")]
        public IActionResult SearchUserInWorkspace([FromBody] SearchUserInWorkspaceRequestDto request)
        {
            var result = _workspaceService.SearchUserInWorkspace(request);

            return Ok(result);
        }

        [HttpPost("workspace")]
        public IActionResult CreateWorkspace([FromBody] CreateWorkspaceRequestDto request)
        {
            var result = _workspaceService.CreateWorkspace(request, UserId);

            return Ok(result);
        }

        [HttpGet("workspaces")]
        public IActionResult GetWorkspace()
        {
            var result = _workspaceService.GetAllWorkspaces(UserName, UserId);

            return Ok(result);
        }

        [HttpGet("{workspaceId}/user-in-workspace")]
        public IActionResult GetUserInWorkspaceById(Guid workspaceId)
        {
            var result = _workspaceService.GetWorkspaceById(workspaceId, UserName);

            return Ok(result);
        }


        [HttpPost("add-user-workspace")]
        [AllowAnonymous]
        public IActionResult AddUserIntoWorkspace([FromBody] AddUserIntoWorkspaceRequestDto requestDto)
        {
            _workspaceService.AddUserIntoWorkspace(requestDto, UserId);

            return Ok();
        }

        [HttpPost("send-message")]
        [AllowAnonymous]
        public IActionResult SendMessage([FromBody] string message)
        {
            _hubContext.Clients.User("tamle.dev").Notify("Hello Tam");

            return Ok();
        }

        [HttpGet("{workspaceId}/todos")]
        public IActionResult GetTodoInWorkspaceById(Guid workspaceId)
        {
            var result = _workspaceService.GetTodoListInWorkspace(new GetTodoListInWorkspaceRequestDto
            {
                Id = workspaceId
            });

            return Ok(result);
        }

        [HttpPost("add-todo-workspace")]
        public IActionResult AddTodoInWorkspace([FromBody] AddTodoIntoWorkspaceRequestDto request)
        {
            request.AddDate = DateTime.Now;

            var result = _workspaceService.AddTodoInWorkspace(request);

            return Ok(result);
        }

        [HttpPut("drag-drop-todo-workspace")]
        public IActionResult DragDropTodoInWorkspace([FromBody] DragDropTodoInWorkspaceRequestDto requestDto)
        {
            _workspaceService.DragDropTodoInWorkspace(requestDto);

            return Ok();
        }

        [HttpPut("assign-user")]
        public IActionResult AssignUser([FromBody] AssignUserRequestDto request)
        {
            var result = _workspaceService.AssignUser(request, UserName);

            return Ok(result);
        }
    }

    public class PostNotifyRequestDto
    {
        public string ConnectionId { get; set; }

        public string Message { get; set; }
    }
}
