using System;

namespace TDL.Services.Dto.MyDayPage
{
    public class ChangeTodoCategoryRequestDto
    {
        public Guid TodoId { get; set; }

        public string CategoryName { get; set; }
    }
}