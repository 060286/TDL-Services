using System;
using System.Collections.Generic;
using System.Text;
using TDL.Domain.Entities;
using TDL.Services.Dto.Color;

namespace TDL.Services.Services.v1.Interfaces
{
    public interface IColorService
    {
        IList<ColorDto> GetPriorityColor(IList<Tag> priorities);

        ColorDto PriorityColor(string text);
    }
}
