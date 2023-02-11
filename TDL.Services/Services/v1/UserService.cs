using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TDL.Domain.Entities;
using TDL.Infrastructure.Extensions;
using TDL.Infrastructure.Persistence.Repositories.Repositories;
using TDL.Infrastructure.Persistence.UnitOfWork.Interfaces;
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

        public UserService(IRepository<User> userRepository, IUnitOfWorkProvider uowProvider, IRepository<TodoCategory> todoCategoryRepository)
        {
            _userRepository = userRepository;
            _uowProvider = uowProvider;
            _todoCategoryRepository = todoCategoryRepository;
        }

        #endregion

        public UserLoginResponseDto LoginAndGetUserToken(UserLoginRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            var user = _userRepository.GetAll()
                .FirstOrDefault(us => us.UserName.EqualsInvariant(request.UserName) 
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

        public void RegisterAccount(RegisterAccountRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            Guid userId = Guid.NewGuid();

            User account = new User
            {
                Id = userId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            _userRepository.Add(account);

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

        #endregion
    }
}
