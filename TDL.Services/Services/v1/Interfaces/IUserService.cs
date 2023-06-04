using System;
using System.Collections.Generic;
using TDL.Services.Dto.User;
using TDL.Services.Dto.Workspace;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface IUserService
    {
        void RegisterAccount(RegisterAccountRequestDto request);

        IList<UserInfoReponseDto> SearchUserInfo(string keyword, Guid userId);

        UserLoginResponseDto LoginAndGetUserToken(UserLoginRequestDto request);

        UserInfoDetailResponseDto GetUserInfo(Guid id);

        void CreateDummyTag(TagDummyRequestDto tag);

        GetAnalyticTodoResponseDto GetAnalyticTodo(string userName);

        void ResertPassword(ResetPasswordRequestDto request);

        IList<GetNotificationResponseDto> GetNotifications(Guid userId);
    }
}
