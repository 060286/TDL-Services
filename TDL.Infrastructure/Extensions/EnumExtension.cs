using System;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using TDL.Infrastructure.Attributes;

namespace TDL.Infrastructure.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Parse string to enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetDescription<T>(this T source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            FieldInfo fieldInfo = source.GetType().GetField(source.ToString());

            if (fieldInfo == null)
            {
                return string.Empty;
            }

            DescriptionAttribute[] customAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return customAttributes.Length != 0 ? customAttributes[0].Description : source.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetSortColumn<T>(this T source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            FieldInfo fieldInfo = source.GetType().GetField(source.ToString());

            if (fieldInfo == null)
            {
                return string.Empty;
            }

            SortAttribute[] customAttributes = (SortAttribute[])fieldInfo.GetCustomAttributes(typeof(SortAttribute), false);

            return customAttributes.Length != 0 ? customAttributes[0].ColumnName : source.ToString();
        }
    }
}
