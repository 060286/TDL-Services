using System.Collections.Generic;
using TDL.Domain.Entities;

namespace TDL.Services.Dto.Workspace
{
    public class GetTodoListInWorkspaceResponseDto
    {
        public IList<Todo> TodoList { get; set; }

        public IList<Todo> InProgressList { get; set; }

        public IList<Todo> InReviewList { get; set; }

        public IList<Todo> CompletedList { get; set; }
    }
}
