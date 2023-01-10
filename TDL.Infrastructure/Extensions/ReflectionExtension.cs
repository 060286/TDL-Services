using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDL.Infrastructure.Extensions
{
    public static class ReflectionExtension
    {
        public static string GetDescription(this Type type)
        {
            var descriptions = (DescriptionAttribute[])
                type.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptions.Length == 0 ? null : descriptions[0].Description;
        }
    }
}
