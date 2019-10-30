using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Exp = GeolocationNetStandard.ExpressionHelperExtensionMethods;

namespace GeolocationNetStandard
{
    public static class ExpressionExtensionMethods
    {
        public static Expression ToRadiansExpression(this Expression degrees)
        {
            return Expression.Multiply(degrees, (Expression.Divide(Expression.Constant(Math.PI), Expression.Constant(180.0))));
        }
        public static Expression<Func<T, double>> ToRadians<T>(Expression<Func<T, double>> input)
        {
            return Exp.AsExpressionOfFunc(ToRadiansExpression, input);
        }
        public static Func<double, double> ToRadiansFunc = Exp.CompileExpression<double, double>(ToRadiansExpression);


        public static Expression DiffRadiansExpression(this Expression subtract, Expression from)
        {
            return Expression.Subtract(from.ToRadiansExpression(), subtract.ToRadiansExpression());
        }
        public static Func<double, double, double> DiffRadiansFunc = Exp.CompileExpression<double, double, double>(DiffRadiansExpression);


        public static Expression ToDegreesExpression(this Expression radians)
        {
            return Expression.Multiply(radians, (Expression.Divide(Expression.Constant(180.0), Expression.Constant(Math.PI))));
        }
        public static Func<double, double> ToDegreesFunc = Exp.CompileExpression<double, double>(ToDegreesExpression);


        public static Expression ToBearingExpression(this Expression radians)
        {
            var degreesExpression = radians.ToDegreesExpression();
            var constant360 = Expression.Constant(360d);
            return Expression.Modulo((Expression.Add(degreesExpression, constant360)), constant360);
        }
        public static Func<double, double> ToBearingFunc = Exp.CompileExpression<double, double>(ToBearingExpression);

    }

}
