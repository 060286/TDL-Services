using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace TDL.Infrastructure.Persistence.Translations.DateTime
{
    public class EqualToYearWithOffsetExpression : BaseSqlBinaryDateTimeOffsetExpression
    {
        public EqualToYearWithOffsetExpression(SqlExpression left, SqlExpression right, string requestTimeZone) 
            : base(left, right, "year", requestTimeZone)
        {
        }
    }
}
