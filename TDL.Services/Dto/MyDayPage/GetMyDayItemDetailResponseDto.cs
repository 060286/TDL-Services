﻿using System;
using System.Collections.Generic;
using TDL.Services.Dto.Color;

namespace TDL.Services.Dto.MyDayPage
{
    public class GetMyDayItemDetailResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? RemindedAt { get; set; }

        public DateTime? TodoDate { get; set; }

        public IList<SubTaskResponse> SubTasks { get; set; }

        public ColorDto Tag { get; set; }

        public AssignUserInfo AssignUserInfo { get; set; }
    }

    public class SubTaskResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }
    }

    public class AssignUserInfo
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Img { get; set; }
    }
}
