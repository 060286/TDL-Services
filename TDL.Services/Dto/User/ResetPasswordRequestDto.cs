using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDL.Services.Dto.User
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}