using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using TDL.Infrastructure.Constants;

namespace TDL.Infrastructure.Persistence.Translations.String
{
    public class ContainInvariantExpression : BaseSqlBinaryStringExpression
    {
        public ContainInvariantExpression(SqlExpression left, SqlExpression right)
    : base(ExpressionType.GreaterThan, BuildStringContainExpr(left, right),
        BuildConstantZeroExpr(), typeof(string), new StringTypeMapping(DataBaseConstant.StringVarcharType))
        {
        }

        private static SqlExpression BuildConstantZeroExpr()
        {
            return new SqlConstantExpression(Constant(0), new IntTypeMapping(DataBaseConstant.IntType));
        }

        private static SqlExpression BuildStringContainExpr(SqlExpression left, SqlExpression right)
        {
            return SqlFunctionExpression.Create(DataBaseConstant.StringContainFunc, new[] { BuildLowerExpr(right), BuildLowerExpr(left) },
                typeof(string), new StringTypeMapping(DataBaseConstant.StringVarcharType));
        }
    }
}
