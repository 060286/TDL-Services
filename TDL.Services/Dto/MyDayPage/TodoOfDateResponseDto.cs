using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Services.Dto.MyDayPage
{
    public class TodoOfDateResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string TodoCategory { get; set; }

        public bool? IsCompleted { get; set; }

        public bool? IsPinned { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
 