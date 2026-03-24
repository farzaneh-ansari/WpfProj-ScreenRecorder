using System.Linq.Expressions;

namespace WpfProj.Model
{
    public class ExpressionHelper
    {
        public static MemberInitExpression CreateMemberInitExpression<T, TProp>(Expression<Func<T, TProp>> propertySelector, TProp value) where T : new()
        {
            NewExpression newExpression = Expression.New(typeof(T));

            MemberExpression memberExpression = propertySelector.Body as MemberExpression;

            MemberAssignment memberBinding = Expression.Bind(memberExpression.Member, Expression.Constant(value, typeof(TProp)));

            MemberInitExpression memberInitExpression = Expression.MemberInit(newExpression, memberBinding);

            return memberInitExpression;
        }

        public static T CreateObjectFromExpression<T>(MemberInitExpression memberInitExpression) where T : new()
        {
            Expression<Func<T>> lambda = Expression.Lambda<Func<T>>(memberInitExpression);
            Func<T> compiledLambda = lambda.Compile();
            return compiledLambda();
        }
    }
}
