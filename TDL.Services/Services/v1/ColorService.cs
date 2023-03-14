using System.Collections.Generic;
using TDL.Domain.Entities;
using TDL.Infrastructure.Constants;
using TDL.Infrastructure.Extensions;
using TDL.Services.Dto.Color;
using TDL.Services.Services.v1.Interfaces;

namespace TDL.Services.Services.v1
{
    public class ColorService : IColorService
    {
        public IList<ColorDto> GetPriorityColor(IList<Tag> priorities)
        {
            var result = new List<ColorDto>();

            foreach (var priority in priorities)
            {
                var item = BuildColorItem(priority.Title);
                
                result.Add(item);
            }

            return result;
        }

        public ColorDto PriorityColor(string text)
        {
            ColorDto response = BuildColorItem(text);

            return response;
        }

        private ColorDto BuildColorItem(string text)
        {
            if (text.EqualsInvariant(ColorConstant.Important))
            {
                return new ColorDto()
                {
                    Text = ColorConstant.Important,
                    BackgroundColor = "#FF0000",
                    Color = "#FFFFFF"
                };
            }
            
            if (text.EqualsInvariant(ColorConstant.Nothing))
            {
                return new ColorDto()
                {
                    Text = ColorConstant.Nothing,
                    BackgroundColor = "#Ecc506",
                    Color = "#FFFFFF"
                };
            }
            
            if (text.EqualsInvariant(ColorConstant.Priority))
            {
                return new ColorDto()
                {
                    Text = ColorConstant.Important,
                    BackgroundColor = "#F8D220",
                    Color = "#FFFFFF"
                };
            }
            
            if (text.EqualsInvariant(ColorConstant.TrackBack))
            {
                return new ColorDto()
                {
                    Text = ColorConstant.TrackBack,
                    BackgroundColor = "#47ec06",
                    Color = "#FFFFFF"
                };
            }
            
            return new ColorDto()
            {
                Text = ColorConstant.Important,
                BackgroundColor = "#F8D220",
                Color = "#FFFFFF"
            };
        }
    }
}
