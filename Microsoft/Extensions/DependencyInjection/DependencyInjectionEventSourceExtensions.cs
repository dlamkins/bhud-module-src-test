using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class DependencyInjectionEventSourceExtensions
	{
		private sealed class NodeCountingVisitor : ExpressionVisitor
		{
			public int NodeCount { get; private set; }

			[return: _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ENotNullIfNotNull("e")]
			public override Expression Visit(Expression e)
			{
				base.Visit(e);
				NodeCount++;
				return e;
			}
		}

		public static void ExpressionTreeGenerated(this DependencyInjectionEventSource source, ServiceProvider provider, Type serviceType, Expression expression)
		{
			if (source.IsEnabled(EventLevel.Verbose, EventKeywords.All))
			{
				NodeCountingVisitor nodeCountingVisitor = new NodeCountingVisitor();
				nodeCountingVisitor.Visit(expression);
				source.ExpressionTreeGenerated(serviceType.ToString(), nodeCountingVisitor.NodeCount, provider.GetHashCode());
			}
		}
	}
}
