using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SL.ChatLinks
{
	internal static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddFactoryDelegate<TDelegate>(this IServiceCollection serviceCollecton) where TDelegate : Delegate
		{
			MethodInfo invokeMethod = typeof(TDelegate).GetMethod("Invoke");
			if (invokeMethod.ReturnType == typeof(void))
			{
				throw new ArgumentException("The delegate must have a return type.", "TDelegate");
			}
			ConstantExpression instance = Expression.Constant(ActivatorUtilities.CreateFactory(invokeMethod.ReturnType, (from p in invokeMethod.GetParameters()
				select p.ParameterType).ToArray()));
			MethodInfo factoryMethod = typeof(ObjectFactory).GetMethod("Invoke");
			ParameterExpression[] parameterExpressions = (from p in invokeMethod.GetParameters()
				select Expression.Parameter(p.ParameterType)).ToArray();
			NewArrayExpression arrayExpression = Expression.NewArrayInit(initializers: parameterExpressions.Select((ParameterExpression p) => Expression.TypeAs(p, typeof(object))), type: typeof(object));
			ParameterExpression serviceProviderParameterExpression = Expression.Parameter(typeof(IServiceProvider));
			Func<IServiceProvider, object> compiledDelegateFactory = Expression.Lambda<Func<IServiceProvider, object>>(Expression.Lambda<TDelegate>(Expression.Convert(Expression.Call(instance, factoryMethod, serviceProviderParameterExpression, arrayExpression), invokeMethod.ReturnType), parameterExpressions), new ParameterExpression[1] { serviceProviderParameterExpression }).Compile();
			serviceCollecton.Add(new ServiceDescriptor(typeof(TDelegate), compiledDelegateFactory, ServiceLifetime.Singleton));
			return serviceCollecton;
		}
	}
}
