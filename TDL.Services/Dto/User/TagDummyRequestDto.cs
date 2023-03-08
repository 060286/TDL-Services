using System;

namespace TDL.Services.Dto.User
{
    public class TagDummyRequestDto
    {
        public string Title { get; set; }

        public string Color { get; set; }

        public Guid TodoId { get; set; }
    }
}