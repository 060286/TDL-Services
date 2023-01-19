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
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWorkProvider _uowProvider;

        public UserService(IRepository<User> userRepository, IUnitOfWorkProvider uowProvider)
        {
            _userRepository = userRepository;
            _uowProvider = uowProvider;
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
                .Where(us => string.IsNullOrEmpty(keyword) || us.UserName.Contains(keyword) ||
                       us.Email.Contains(keyword) || us.PhoneNumber.Contains(keyword))
                .Select(re => new UserInfoReponseDto
                {
                    Id = re.Id,
                    Email = re.Email,
                    Img = re.Img,
                    UserName = re.UserName
                }).ToList();

            return result;
        }
    }
}
