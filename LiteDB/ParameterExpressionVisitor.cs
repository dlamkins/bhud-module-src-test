using System.Linq.Expressions;

namespace LiteDB
{
	internal class ParameterExpressionVisitor : ExpressionVisitor
	{
		public bool IsParameter { get; private set; }

		protected override Expression VisitParameter(ParameterExpression node)
		{
			IsParameter = true;
			return base.VisitParameter(node);
		}

		public static bool Test(Expression node)
		{
			ParameterExpressionVisitor parameterExpressionVisitor = new ParameterExpressionVisitor();
			parameterExpressionVisitor.Visit(node);
			return parameterExpressionVisitor.IsParameter;
		}
	}
}
