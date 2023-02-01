using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWorkProvider _uowProvider;

        public UserService(IRepository<User> userRepository, IUnitOfWorkProvider uowProvider)
        {
            _userRepository = userRepository;
            _uowProvider = uowProvider;
        }

        public UserLoginResponseDto LoginAndGetUserToken(UserLoginRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            var user = _userRepository.GetAll(true)
                .FirstOrDefault(us => us.UserName.EqualsInvariant(request.UserName) 
                                      && us.Password.EqualsInvariant(request.Password));

            Guard.ThrowIfNull<UnauthorizedAccessException>(user, ExceptionConstant.RestrictedResource);

            var response = new UserLoginResponseDto(token: CreateToken(user));

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

            return response;
        }

        public void RegisterAccount(RegisterAccountRequestDto request)
        {
            using var scope = _uowProvider.Provide();

            bool isCorrectPassword = request.Password.EqualsInvariant(request.ConfirmedPassword);
            Guid userId = Guid.NewGuid();

            Guard.ThrowByCondition<BusinessLogicException>(!isCorrectPassword, "Please input correct password!");

            _userRepository.Add(new User
            {
                Id = userId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                Password = request.Password,
            });

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

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("my top secret key"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
