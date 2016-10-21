using System.Linq.Expressions;

namespace SmartParkAPI.Shared.Helpers
{
    public class CustomExpressionVisitor<T> : ExpressionVisitor, ICustomExpressionVisitor<T>
    {
        readonly ParameterExpression _parameter;

        public CustomExpressionVisitor(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameter;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == System.Reflection.MemberTypes.Property)
            {
                var memberName = node.Member.Name;
                var otherMember = typeof(T).GetProperty(memberName);
                var memberExpression = Expression.Property(Visit(node.Expression), otherMember);
                return memberExpression;
            }
            return base.VisitMember(node);
        }
    }


    public interface ICustomExpressionVisitor<T>
    {
    }
}
