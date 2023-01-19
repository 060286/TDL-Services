using System;

namespace TDL.Services.Dto.User
{
    public class UserInfoReponseDto
    {
        public Guid Id { get; set; }

        public string Img { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
