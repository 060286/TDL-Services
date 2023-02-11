using System;
using System.Collections.Generic;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }

        public string Img { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public long LoginCount { get; set; } = 0;

        public IList<UserWorkspace> UserWorkspaces { get; set; }
    }
}
