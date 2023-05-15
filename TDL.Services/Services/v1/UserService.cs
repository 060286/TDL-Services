using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TDL.Domain.Entities;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Exceptions;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
using TDL.Infrastructure.Utilities;
using TDL.Services.Dto.User;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class UserService : IUserService
    {
        #region Constructor

        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWorkProvider _uowProvider;
        private readonly IRepository<TodoCategory> _todoCategoryRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Todo> _todoRepository;

        public UserService(IRepository<User> userRepository, IUnitOfWorkProvider uowProvider,
            IRepository<TodoCategory> todoCategoryRepository,
            IRepository<Tag> tagRepository,
            IRepository<Todo> todoRepository)
        {
            _userRepository = userRepository;
            _uowProvider = uowProvider;
            _todoCategoryRepository = todoCategoryRepository;
            _tagRepository = tagRepository;
            _todoRepository = todoRepository;
        }

        #endregion

        public void ResertPassword(ResetPasswordRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            var user = _userRepository.GetAll()
                .FirstOrDefault(user => user.Email.EqualsInvariant(request.Email));

            Guard.ThrowByCondition<NotFoundException>(user == null, "Cannot fid user by email");
            Guard.ThrowByCondition<BusinessLogicException>(!request.ConfirmPassword.EqualsInvariant(request.NewPassword),
                "Confirm password and new password do not match");
            Guard.ThrowByCondition<BusinessLogicException>(user.Password.EqualsInvariant(request.ConfirmPassword) ||
                user.Password.EqualsInvariant(request.NewPassword),
                "Old password is matched with new password");

            user.Password = request.NewPassword;

            _userRepository.Update(user);
            scope.Complete();
        }

        public UserLoginResponseDto LoginAndGetUserToken(UserLoginRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            var user = _userRepository.GetAll()
                .FirstOrDefault(us => us.Email.EqualsInvariant(request.UserName)
                                      && us.Password.EqualsInvariant(request.Password));

            Guard.ThrowIfNull<UnauthorizedAccessException>(user, ExceptionConstant.RestrictedResource);

            var response = new UserLoginResponseDto(token: CreateToken(user));

            user.LoginCount = ++user.LoginCount;

            scope.Complete();

            return response;
        }

        public UserInfoDetailResponseDto GetUserInfo(Guid userId)
        {
            using var scope = _uowProvider.Provide();
            string errorMessage = "Cannot get user info";

            var response = _userRepository.GetAll(true)
                .Select(x => new UserInfoDetailResponseDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    Img = x.Img,
                    UserName = x.UserName,
                    FullName = $"{x.FirstName} {x.LastName}",
                }).FirstOrDefault(us => us.Id == userId);

            Guard.ThrowIfNull<NotFoundException>(response, errorMessage);

            CheckIfLoggedFirstTime(userId);

            scope.Complete();

            return response;
        }

        public void CreateDummyTag(TagDummyRequestDto tag)
        {
            using var scope = _uowProvider.Provide();

            var response = _tagRepository.GetAll(true)
                .FirstOrDefault(x => x.Title.EqualsInvariant(tag.Title));

            Guard.ThrowByCondition<BusinessLogicException>(response != null, "Can not create duplicate tag");

            Tag result = new Tag()
            {
                Id = Guid.NewGuid(),
                Title = tag.Title,
                Color = tag.Color,
                TodoId = tag.TodoId,
            };

            _tagRepository.Add(result);

            scope.Complete();
        }

        public void RegisterAccount(RegisterAccountRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            bool isCorrectPassword = request.Password.EqualsInvariant(request.ConfirmedPassword);
            Guid userId = Guid.NewGuid();
            Guid categoryId = Guid.NewGuid();
            string defaultCategory = "Personal";

            Guard.ThrowByCondition<BusinessLogicException>(!isCorrectPassword, "Please input correct password!");

            User newUser = new User
            {
                Id = userId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                Password = request.Password,
            };

            TodoCategory newTodoCategory = new TodoCategory
            {
                Id = categoryId,
                Description = defaultCategory,
                Title = defaultCategory,
            };

            _userRepository.Add(newUser);
            _todoCategoryRepository.Add(newTodoCategory);

            scope.Complete();
        }

        public IList<UserInfoReponseDto> SearchUserInfo(string keyword)
        {
            using var scope = _uowProvider.Provide();

            var result = _userRepository.GetAll(true)
                .Where(us => string.IsNullOrEmpty(keyword) || us.UserName.ContainInvariant(keyword) ||
                       us.Email.ContainInvariant(keyword) || us.PhoneNumber.ContainInvariant(keyword))
                .Select(re => new UserInfoReponseDto
                {
                    Id = re.Id,
                    Email = re.Email,
                    Img = re.Img,
                    UserName = re.UserName
                }).ToList();

            return result;
        }

        #region PrivateMethod

        private string CreateToken(User user)
        {
            string keyValue = "my top secret key";

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyValue));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        private void CheckIfLoggedFirstTime(Guid userId)
        {
            var userInfo = _userRepository.Get(userId);

            Guard.ThrowIfNull<NotFoundException>(userInfo, string.Format(ExceptionConstant.NotFound, typeof(User)));

            bool isLoggedFirstTime = userInfo.LoginCount == 0;

            if (isLoggedFirstTime)
            {
                _todoCategoryRepository.Add(new TodoCategory()
                {
                    Id = new Guid(),
                    Description = TodoCategoryConstant.DefualtTodoCategory,
                    Title = TodoCategoryConstant.DefualtTodoCategory
                });
            }
        }

        public GetAnalyticTodoResponseDto GetAnalyticTodo(string userName)
        {
            return new GetAnalyticTodoResponseDto
            {
                SevenDayAnalytic = GetAnalyticTodoItem(DateTime.Now, 6, userName, "Weekly Report"),
                OneMonthAnalytic = GetAnalyticTodoItem(DateTime.Now, 30, userName, "Monthly Report"),
            };
        }

        private GetAnalyticTodoItemResponseDto GetAnalyticTodoItem(DateTime now, int dayNumber, string userName, string title)
        {
            using var scope = _uowProvider.Provide();

            var totalCountFromDate = _todoRepository.GetAll(true)
                .Where(x => x.CreatedBy.EqualsInvariant(userName))
                .Count(x => x.CreatedAt.Value.Date >= now.AddDays(-dayNumber).Date && x.CreatedAt.Value.Date <= now);

            var totalCountCompletedFromDate = _todoRepository.GetAll(true)
                .Where(x => x.CreatedBy.EqualsInvariant(userName))
                .Where(x => x.IsCompleted)
                .Count(x => x.CreatedAt.Value.Date >= now.AddDays(-dayNumber).Date && x.CreatedAt.Value.Date <= now);

            var percentage = totalCountFromDate - totalCountCompletedFromDate;

            scope.Complete();

            return new GetAnalyticTodoItemResponseDto
            {
                Id = Guid.NewGuid(),
                Percentage = percentage,
                Title = title,
            };
        }

        #endregion
    }
}
