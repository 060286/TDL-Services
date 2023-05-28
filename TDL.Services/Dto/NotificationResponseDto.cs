using System;

namespace TDL.Services.Dto
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }
    }

    public enum NotificationType
    {
        AddUserWorkspace
    }
}
