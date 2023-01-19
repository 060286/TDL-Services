using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq.Expressions;
using TDL.Infrastructure.Constants;

namespace TDL.Infrastructure.Persistence.Translations.String
{
    public class BaseSqlBinaryStringExpression : SqlBinaryExpression
    {
        protected BaseSqlBinaryStringExpression(ExpressionType operatorType, SqlExpression left, SqlExpression right, Type type, RelationalTypeMapping typeMapping)
            : base(operatorType, left, right, type, typeMapping)
        {
        }

        protected static SqlExpression BuildLowerExpr(SqlExpression exp)
        {
            return SqlFunctionExpression.Create(DataBaseConstant.LowerFunc, new[] { exp }, typeof(string), new StringTypeMapping(DataBaseConstant.StringVarcharType));
        }
    }
}
