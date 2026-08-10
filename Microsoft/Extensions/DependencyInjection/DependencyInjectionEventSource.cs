using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Text;
using Microsoft.Extensions.DependencyInjection.ServiceLookup;

namespace Microsoft.Extensions.DependencyInjection
{
	[EventSource(Name = "Microsoft-Extensions-DependencyInjection")]
	internal sealed class DependencyInjectionEventSource : EventSource
	{
		public static class Keywords
		{
			public const EventKeywords ServiceProviderInitialized = (EventKeywords)1L;
		}

		public static readonly DependencyInjectionEventSource Log = new DependencyInjectionEventSource();

		private const int MaxChunkSize = 10240;

		private readonly List<WeakReference<ServiceProvider>> _providers = new List<WeakReference<ServiceProvider>>();

		private DependencyInjectionEventSource()
			: base(EventSourceSettings.EtwSelfDescribingEventFormat)
		{
		}

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Parameters to this method are primitive and are trimmer safe.")]
		[Event(1, Level = EventLevel.Verbose)]
		private void CallSiteBuilt(string serviceType, string callSite, int chunkIndex, int chunkCount, int serviceProviderHashCode)
		{
			WriteEvent(1, serviceType, callSite, chunkIndex, chunkCount, serviceProviderHashCode);
		}

		[Event(2, Level = EventLevel.Verbose)]
		public void ServiceResolved(string serviceType, int serviceProviderHashCode)
		{
			WriteEvent(2, serviceType, serviceProviderHashCode);
		}

		[Event(3, Level = EventLevel.Verbose)]
		public void ExpressionTreeGenerated(string serviceType, int nodeCount, int serviceProviderHashCode)
		{
			WriteEvent(3, serviceType, nodeCount, serviceProviderHashCode);
		}

		[Event(4, Level = EventLevel.Verbose)]
		public void DynamicMethodBuilt(string serviceType, int methodSize, int serviceProviderHashCode)
		{
			WriteEvent(4, serviceType, methodSize, serviceProviderHashCode);
		}

		[Event(5, Level = EventLevel.Verbose)]
		public void ScopeDisposed(int serviceProviderHashCode, int scopedServicesResolved, int disposableServices)
		{
			WriteEvent(5, serviceProviderHashCode, scopedServicesResolved, disposableServices);
		}

		[Event(6, Level = EventLevel.Error)]
		public void ServiceRealizationFailed(string? exceptionMessage, int serviceProviderHashCode)
		{
			WriteEvent(6, exceptionMessage, serviceProviderHashCode);
		}

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Parameters to this method are primitive and are trimmer safe.")]
		[Event(7, Level = EventLevel.Informational, Keywords = (EventKeywords)1L)]
		private void ServiceProviderBuilt(int serviceProviderHashCode, int singletonServices, int scopedServices, int transientServices, int closedGenericsServices, int openGenericsServices)
		{
			WriteEvent(7, serviceProviderHashCode, singletonServices, scopedServices, transientServices, closedGenericsServices, openGenericsServices);
		}

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Parameters to this method are primitive and are trimmer safe.")]
		[Event(8, Level = EventLevel.Informational, Keywords = (EventKeywords)1L)]
		private void ServiceProviderDescriptors(int serviceProviderHashCode, string descriptors, int chunkIndex, int chunkCount)
		{
			WriteEvent(8, serviceProviderHashCode, descriptors, chunkIndex, chunkCount);
		}

		[NonEvent]
		public void ServiceResolved(ServiceProvider provider, Type serviceType)
		{
			if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
			{
				ServiceResolved(serviceType.ToString(), provider.GetHashCode());
			}
		}

		[NonEvent]
		public void CallSiteBuilt(ServiceProvider provider, Type serviceType, ServiceCallSite callSite)
		{
			if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
			{
				string text = CallSiteJsonFormatter.Instance.Format(callSite);
				int num = text.Length / 10240 + ((text.Length % 10240 > 0) ? 1 : 0);
				int hashCode = provider.GetHashCode();
				for (int i = 0; i < num; i++)
				{
					CallSiteBuilt(serviceType.ToString(), text.Substring(i * 10240, Math.Min(10240, text.Length - i * 10240)), i, num, hashCode);
				}
			}
		}

		[NonEvent]
		public void DynamicMethodBuilt(ServiceProvider provider, Type serviceType, int methodSize)
		{
			if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
			{
				DynamicMethodBuilt(serviceType.ToString(), methodSize, provider.GetHashCode());
			}
		}

		[NonEvent]
		public void ServiceRealizationFailed(Exception exception, int serviceProviderHashCode)
		{
			if (IsEnabled(EventLevel.Error, EventKeywords.All))
			{
				ServiceRealizationFailed(exception.ToString(), serviceProviderHashCode);
			}
		}

		[NonEvent]
		public void ServiceProviderBuilt(ServiceProvider provider)
		{
			lock (_providers)
			{
				_providers.Add(new WeakReference<ServiceProvider>(provider));
			}
			WriteServiceProviderBuilt(provider);
		}

		[NonEvent]
		public void ServiceProviderDisposed(ServiceProvider provider)
		{
			lock (_providers)
			{
				for (int num = _providers.Count - 1; num >= 0; num--)
				{
					WeakReference<ServiceProvider> weakReference = _providers[num];
					if (!weakReference.TryGetTarget(out var target) || target == provider)
					{
						_providers.RemoveAt(num);
					}
				}
			}
		}

		[NonEvent]
		private void WriteServiceProviderBuilt(ServiceProvider provider)
		{
			if (!IsEnabled(EventLevel.Informational, (EventKeywords)1L))
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			StringBuilder stringBuilder = new StringBuilder("{ \"descriptors\":[ ");
			bool flag = true;
			ServiceDescriptor[] descriptors = provider.CallSiteFactory.Descriptors;
			foreach (ServiceDescriptor serviceDescriptor in descriptors)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(", ");
				}
				AppendServiceDescriptor(stringBuilder, serviceDescriptor);
				switch (serviceDescriptor.Lifetime)
				{
				case ServiceLifetime.Singleton:
					num++;
					break;
				case ServiceLifetime.Scoped:
					num2++;
					break;
				case ServiceLifetime.Transient:
					num3++;
					break;
				}
				if (serviceDescriptor.ServiceType.IsGenericType)
				{
					if (serviceDescriptor.ServiceType.IsConstructedGenericType)
					{
						num4++;
					}
					else
					{
						num5++;
					}
				}
			}
			stringBuilder.Append(" ] }");
			int hashCode = provider.GetHashCode();
			ServiceProviderBuilt(hashCode, num, num2, num3, num4, num5);
			string text = stringBuilder.ToString();
			int num6 = text.Length / 10240 + ((text.Length % 10240 > 0) ? 1 : 0);
			for (int j = 0; j < num6; j++)
			{
				ServiceProviderDescriptors(hashCode, text.Substring(j * 10240, Math.Min(10240, text.Length - j * 10240)), j, num6);
			}
		}

		[NonEvent]
		private static void AppendServiceDescriptor(StringBuilder builder, ServiceDescriptor descriptor)
		{
			builder.Append("{ \"serviceType\": \"");
			builder.Append(descriptor.ServiceType);
			builder.Append("\", \"lifetime\": \"");
			builder.Append(descriptor.Lifetime);
			builder.Append("\", ");
			if (descriptor.HasImplementationType())
			{
				builder.Append("\"implementationType\": \"");
				builder.Append(ServiceDescriptorExtensions.GetImplementationType(descriptor));
			}
			else if (!descriptor.IsKeyedService && descriptor.ImplementationFactory != null)
			{
				builder.Append("\"implementationFactory\": \"");
				builder.Append(descriptor.ImplementationFactory!.Method);
			}
			else if (descriptor.IsKeyedService && descriptor.KeyedImplementationFactory != null)
			{
				builder.Append("\"implementationFactory\": \"");
				builder.Append(descriptor.KeyedImplementationFactory!.Method);
			}
			else if (descriptor.HasImplementationInstance())
			{
				object implementationInstance = descriptor.GetImplementationInstance();
				builder.Append("\"implementationInstance\": \"");
				builder.Append(implementationInstance.GetType());
				builder.Append(" (instance)");
			}
			else
			{
				builder.Append("\"unknown\": \"");
			}
			builder.Append("\" }");
		}

		protected override void OnEventCommand(EventCommandEventArgs command)
		{
			if (command.Command != EventCommand.Enable)
			{
				return;
			}
			lock (_providers)
			{
				foreach (WeakReference<ServiceProvider> provider in _providers)
				{
					if (provider.TryGetTarget(out var target))
					{
						WriteServiceProviderBuilt(target);
					}
				}
			}
		}
	}
}
