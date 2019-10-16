using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GeolocationNetStandard
{
    public static class ExpressionHelperExtensionMethods
    {
        public static Func<TI, TO> CompileExpression<TI, TO>(this Func<Expression, Expression> work)
        {
            var inputExpression = Expression.Parameter(typeof(TI));
            Func<TI, TO> lambda = (Expression.Lambda(work(inputExpression), inputExpression).Compile() as Func<TI, TO>);

            return lambda;
        }
        public static Func<TI1, TI2, TO> CompileExpression<TI1, TI2, TO>(this Func<Expression, Expression, Expression> work)
        {
            var inputExpression1 = Expression.Parameter(typeof(TI1));
            var inputExpression2 = Expression.Parameter(typeof(TI2));
            Func<TI1, TI2, TO> lambda = (Expression.Lambda(work(inputExpression1, inputExpression2), inputExpression1, inputExpression2).Compile() as Func<TI1, TI2, TO>);

            return lambda;
        }
        public static TO CompileExpressionAndExecute<TI, TO>(this TI input, Func<Expression, Expression> work)
        {
            var lambda = work.CompileExpression<TI, TO>();

            return lambda(input);
        }
        public static Expression<Func<T, TO>> AsExpressionOfFunc<T, TO>(this Func<Expression, Expression> expression, Expression<Func<T, TO>> input)
        {
            var inputExpression = input.Parameters[0];
            var memberExpression = input.Body as MemberExpression;

            return Expression.Lambda(expression(memberExpression), inputExpression) as Expression<Func<T, TO>>;
        }
        public static IQueryable<TO> ApplyExpression<T, TO>(this IQueryable<T> source, Func<Expression, Expression> expression, Expression<Func<T, TO>> input)
        {
            return source.Select(expression.AsExpressionOfFunc(input));
        }
    }
}
