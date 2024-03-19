using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace LiteDB
{
	internal static class ExpressionExtensions
	{
		private static readonly Regex _removeSelect = new Regex("\\.Select\\s*\\(\\s*\\w+\\s*=>\\s*\\w+\\.", RegexOptions.Compiled);

		private static readonly Regex _removeList = new Regex("\\.get_Item\\(\\d+\\)", RegexOptions.Compiled);

		private static readonly Regex _removeArray = new Regex("\\[\\d+\\]", RegexOptions.Compiled);

		public static string GetPath(this Expression expr)
		{
			while (expr.NodeType == ExpressionType.Convert || expr.NodeType == ExpressionType.ConvertChecked)
			{
				expr = ((UnaryExpression)expr).Operand;
			}
			while (expr.NodeType == ExpressionType.Lambda)
			{
				UnaryExpression unary = ((LambdaExpression)expr).Body as UnaryExpression;
				if (unary == null)
				{
					break;
				}
				expr = unary.Operand;
			}
			string str = expr.ToString();
			int firstDelim = str.IndexOf('.');
			string path = ((firstDelim < 0) ? str : str.Substring(firstDelim + 1).TrimEnd(')'));
			path = _removeList.Replace(path, "");
			path = _removeArray.Replace(path, "");
			return _removeSelect.Replace(path, ".").Replace(")", "");
		}
	}
}
