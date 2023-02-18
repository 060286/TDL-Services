using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class Todo : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        //public IFormFile AttachmentFile { get; set; }

        public string FileName { get; set; }

        public DateTime? RemindedAt { get; set; }

        public bool IsCompleted { get; set; } = false;

        public bool IsArchieved { get; set; } = false;

        public Guid CategoryId { get; set; }

        public Guid? WorkspaceId { get; set; }
        
        public virtual TodoCategory TodoCategory { get; set; }

        public virtual IList<SubTask> SubTasks { get; set; }

        public virtual IList<Tag> Tags { get; set; }
        
        public virtual Workspace Workspace { get; set; }
    }
}
