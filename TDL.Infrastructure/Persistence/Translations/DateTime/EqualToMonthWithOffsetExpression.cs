using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace TDL.Infrastructure.Persistence.Translations.DateTime
{
    public class EqualToMonthWithOffsetExpression : BaseSqlBinaryDateTimeOffsetExpression
    {
        public EqualToMonthWithOffsetExpression(SqlExpression left, SqlExpression right, string requestTimeZone)
            : base(left, right, "month", requestTimeZone)
        {
        }
    }
}
