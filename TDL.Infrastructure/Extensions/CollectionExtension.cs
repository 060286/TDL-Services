using System.Collections.Generic;
using System.Linq;

namespace TDL.Infrastructure.Extensions
{
    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty<TEntity>(this IEnumerable<TEntity> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
