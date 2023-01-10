using System;

namespace TDL.Infrastructure.Attributes
{
    /// <summary>
    /// Sort attribute using for dynamic linq sort.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SortAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="columnName"></param>
        public SortAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public string ColumnName { get; set; }
    }
}
