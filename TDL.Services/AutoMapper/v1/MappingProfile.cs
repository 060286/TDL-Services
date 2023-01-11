using AutoMapper;
using TDL.Domain.Entities;
using TDL.Services.Dto.MyDayPage;

namespace TDL.Services.AutoMapper.v1
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            MapMyDayPage();
        }

        private void MapMyDayPage()
        {
            CreateMap<Todo, GetMyDayItemDetailResponseDto>();
            CreateMap<CreateSimpleTodoRequestDto, Todo>();
        }
    }
}
