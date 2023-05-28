using System;

namespace TDL.Services.Dto.Workspace
{
    public class GetNotificationResponseDto
    {
        public Guid Id { get; set; } 
        
        public string Type { get; set; } 
        
        public string Email { get; set; } 
        
        public string FullName { get; set; } 
        
        public string Content { get; set; } 
        
        public bool IsRead { get; set; } 
        
        public DateTime Time { get; set; } 
        
        public string Url { get; set; }
    }
}
