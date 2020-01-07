using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Maximiz.Infrastructure.Querying
{

    /// <summary>
    /// Contains utility functionality to translate a property expression to
    /// the correct property name.
    /// </summary>
    internal static class PropertyUtility
    {

        /// <summary>
        /// Converts a property expression to the property name.
        /// </summary>
        /// <typeparam name="TObject">Any object</typeparam>
        /// <param name="expression"><see cref="Expression"/></param>
        /// <returns>Property name<returns>
        internal static string GetName<TObject>(Expression<Func<TObject, object>> expression)
        {
            if (expression == null) { throw new ArgumentNullException(nameof(expression)); }
            var propertyRefExpr = expression.Body;

            if (propertyRefExpr == null)
            {
                throw new ArgumentNullException("propertyRefExpr", "propertyRefExpr is null.");
            }

            MemberExpression memberExpr = propertyRefExpr as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyRefExpr as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
                return memberExpr.Member.Name;

            throw new ArgumentException("No property reference expression was found.",
                             "propertyRefExpr");
        }

    }
}
