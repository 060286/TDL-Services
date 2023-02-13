using System;
using System.Collections.Generic;
using System.Text;

namespace TDL.Services.Dto.User
{
    public class UserLoginRequestDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
