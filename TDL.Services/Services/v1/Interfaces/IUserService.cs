using System;
using System.Collections.Generic;
using TDL.Services.Dto.User;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface IUserService
    {
        void RegisterAccount(RegisterAccountRequestDto request);

        IList<UserInfoReponseDto> SearchUserInfo(string keyword);

        UserLoginResponseDto LoginAndGetUserToken(UserLoginRequestDto request);

        UserInfoDetailResponseDto GetUserInfo(Guid userId);
    }
}
