using System.Collections.Generic;
using TDL.Services.Dto.Workspace;

namespace TDL.Services.Dto.User
{
    public class NotificatioWrapperResponseDto
    {
        public IList<GetNotificationResponseDto> Notifications { get; set; }

        public int NotViewedCount { get; set; }
    }
}
