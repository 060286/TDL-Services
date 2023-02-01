using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Services.Dto.User
{
    public class UserInfoDetailResponseDto
    {
        public Guid Id { get; set; }

        public string Img { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
    }
}
