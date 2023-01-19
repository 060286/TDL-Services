using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TDL.Infrastructure.Constants;

namespace TDL.Infrastructure.Persistence.Translations.DateTime
{
    public class BaseSqlBinaryDateTimeOffsetExpression : SqlBinaryExpression
    {
        protected BaseSqlBinaryDateTimeOffsetExpression(SqlExpression left, SqlExpression right, string datePart, string requestTimeZone)
            : base(ExpressionType.Equal, ExtractDatePartFromDateExpr(left, datePart, requestTimeZone), right, right.Type, right.TypeMapping)
        {
        }

        private static SqlExpression ExtractDatePartFromDateExpr(SqlExpression left, string datePart, string requestTimeZone)
        {
            int offset = 0;
            string hourPart = "hh";

            if (!string.IsNullOrEmpty(requestTimeZone))
            {
                var timeZone = DateTimeZoneProviders.Tzdb[requestTimeZone.Trim()];

                var utcOffset = timeZone.GetUtcOffset(SystemClock.Instance.GetCurrentInstant());

                offset = int.TryParse(utcOffset.ToString(), out var result) ? result : default;
            }

            var specifiedLeftExpr = SqlFunctionExpression.Create(DataBaseConstant.DateAddFunc, new SqlExpression[]
            {
                new SqlFragmentExpression(hourPart),
                new SqlConstantExpression(Constant(offset), new IntTypeMapping(DataBaseConstant.IntType)),
                left
            }, left.Type, left.TypeMapping);

            var datePartExpr = new SqlFragmentExpression(datePart);

            var leftExpr = SqlFunctionExpression.Create(DataBaseConstant.DatePartFunc, new SqlExpression[]
                {
                    datePartExpr,
                    specifiedLeftExpr
                },
                specifiedLeftExpr.Type, specifiedLeftExpr.TypeMapping);

            return leftExpr;
        }
    }
}
