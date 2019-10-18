using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Exp = GeolocationNetStandard.ExpressionHelperExtensionMethods;
using Geolocation;

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


        static MethodInfo _cos = typeof(Math).GetRuntimeMethod("Cos", new[] { typeof(double) });
        static MethodInfo _sin = typeof(Math).GetRuntimeMethod("Sin", new[] { typeof(double) });
        static MethodInfo _asin = typeof(Math).GetRuntimeMethod("Asin", new[] { typeof(double) });
        static MethodInfo _sqrt = typeof(Math).GetRuntimeMethod("Sqrt", new[] { typeof(double) });
        static MethodInfo _pow = typeof(Math).GetRuntimeMethod("Pow", new[] { typeof(double), typeof(double) });
        static MethodInfo _min = typeof(Math).GetRuntimeMethod("Min", new[] { typeof(double), typeof(double) });

        public static IQueryable<T> CalculateDistanceInDatabase<T>(this IQueryable<T> source, Expression<Func<T, double>> setter, double originLatitude, double originLongitude, Expression<Func<T, double>> latitudeProperty = null, Expression<Func<T, double>> longitudeProperty = null, DistanceUnit distanceUnit = DistanceUnit.Miles)
        {
            if (latitudeProperty is null)
            {
                return source.CalculateFieldInDb(
                    setter,
                    ExpressionExtensionMethods.CalculateDistanceFrom<T>(originLatitude, originLongitude, distanceUnit));
            }
            else
            {
                return source.CalculateFieldInDb(
                    setter,
                    ExpressionExtensionMethods.CalculateDistanceFrom<T>(originLatitude, originLongitude, latitudeProperty, longitudeProperty, distanceUnit));
            }
        }

        public static Expression<Func<T, double>> CalculateDistanceFrom<T>(double originLatitude, double originLongitude, DistanceUnit distanceUnit)
        {
            return CalculateDistanceFrom<T>(originLatitude, originLongitude, "Latitude", "Longitude", distanceUnit);
        }
        public static Expression<Func<T, double>> CalculateDistanceFrom<T>(double originLatitude, double originLongitude, Expression<Func<T, double>> latitudeProperty, Expression<Func<T, double>> longitudeProperty, DistanceUnit distanceUnit)
        {
            return CalculateDistanceFrom<T>(originLatitude, originLongitude, (latitudeProperty?.Body as MemberExpression).Member.Name, (longitudeProperty?.Body as MemberExpression).Member.Name, distanceUnit);
        }
        public static Expression<Func<T, double>> CalculateDistanceFrom<T>(double originLatitude, double originLongitude, string latitudePropertyName, string longitudePropertyName, DistanceUnit distanceUnit)
        {
            var earthsRadiusConstantExpression = Expression.Constant(GeoCalculator.GetRadius(distanceUnit));
            var inputExpression = Expression.Parameter(typeof(T), "x");
            var originLatitudeExpression = Expression.Constant(originLatitude);
            var originLongitudeExpression = Expression.Constant(originLongitude);

            var destinationLatitudeExpression = Expression.Convert(Expression.Property(inputExpression, latitudePropertyName), typeof(double));
            var destinationLongitudeExpression = Expression.Convert(Expression.Property(inputExpression, longitudePropertyName), typeof(double));

            if (originLatitude < -90 || originLatitude > 90 || originLongitude < -180 || originLongitude > 180)
            {
                return Expression.Lambda<Func<T, double>>(Expression.Constant(-1.0), inputExpression);
            }

            return Expression.Lambda<Func<T, double>>(
                    Expression.Multiply(
                        earthsRadiusConstantExpression,
                        Expression.Multiply(
                            Expression.Constant(2.0),
                            Expression.Call(_asin,
                                    // MIN isn't supported in SQL
                                    // Tested all (4B+) combinations of valid Lat/Long and calculation was never greater than 1
                                    //Expression.Call(_min,
                                    //    Expression.Constant(1.0),
                                    Expression.Call(_sqrt,
                                        Expression.Add(
                                            Expression.Call(_pow,
                                                Expression.Call(_sin,
                                                    Expression.Divide(
                                                        originLatitudeExpression.DiffRadiansExpression(destinationLatitudeExpression),
                                                        Expression.Constant(2.0)
                                                    )
                                                ),
                                                Expression.Constant(2.0)
                                            ),
                                            Expression.Multiply(
                                                Expression.Call(_cos, originLatitudeExpression.ToRadiansExpression()),
                                                Expression.Multiply(
                                                    Expression.Call(_cos, destinationLatitudeExpression.ToRadiansExpression()),
                                                    Expression.Call(_pow,
                                                        Expression.Call(_sin,
                                                            Expression.Divide(
                                                                originLongitudeExpression.DiffRadiansExpression(destinationLongitudeExpression),
                                                                Expression.Constant(2.0)
                                                            )
                                                        ),
                                                        Expression.Constant(2.0)
                                                    )
                                                )
                                            )
                                        )
                                    )
                            //)
                            )
                        )
                    ),
                inputExpression);
        }

    }

}
