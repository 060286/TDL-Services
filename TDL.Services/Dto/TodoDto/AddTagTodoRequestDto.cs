using System;
using TDL.Infrastructure.Enums;

namespace TDL.Services.Dto.TodoDto
{
    public class AddTagTodoRequestDto
    {
        public Guid TodoId { get; set; }
        public TagDefinition Tag { get; set; }
    }
}