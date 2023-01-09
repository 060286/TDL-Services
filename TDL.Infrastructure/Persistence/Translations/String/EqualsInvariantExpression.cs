using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using TDL.Infrastructure.Constants;

namespace TDL.Infrastructure.Persistence.Translations.String
{
    public class EqualsInvariantExpression : BaseSqlBinaryStringExpression
    {
        public EqualsInvariantExpression(SqlExpression left, SqlExpression right) 
            : base (ExpressionType.Equal, BuildLowerExpr(left), BuildLowerExpr(right), typeof(string), 
                  new StringTypeMapping(DataBaseConstant.StringVarcharType))
        {
        }
    }
}
