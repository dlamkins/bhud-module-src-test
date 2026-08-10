using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class CallSiteChain
	{
		private readonly struct ChainItemInfo
		{
			public int Order { get; }

			public Type ImplementationType { get; }

			public ChainItemInfo(int order, Type implementationType)
			{
				Order = order;
				ImplementationType = implementationType;
			}
		}

		private readonly Dictionary<ServiceIdentifier, ChainItemInfo> _callSiteChain;

		public CallSiteChain()
		{
			_callSiteChain = new Dictionary<ServiceIdentifier, ChainItemInfo>();
		}

		public void CheckCircularDependency(ServiceIdentifier serviceIdentifier)
		{
			if (_callSiteChain.ContainsKey(serviceIdentifier))
			{
				throw new InvalidOperationException(CreateCircularDependencyExceptionMessage(serviceIdentifier));
			}
		}

		public void Remove(ServiceIdentifier serviceIdentifier)
		{
			_callSiteChain.Remove(serviceIdentifier);
		}

		public void Add(ServiceIdentifier serviceIdentifier, Type? implementationType = null)
		{
			_callSiteChain[serviceIdentifier] = new ChainItemInfo(_callSiteChain.Count, implementationType);
		}

		private string CreateCircularDependencyExceptionMessage(ServiceIdentifier serviceIdentifier)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.CircularDependencyException, _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ETypeNameHelper.GetTypeDisplayName(serviceIdentifier.ServiceType)));
			stringBuilder.AppendLine();
			AppendResolutionPath(stringBuilder, serviceIdentifier);
			return stringBuilder.ToString();
		}

		private void AppendResolutionPath(StringBuilder builder, ServiceIdentifier currentlyResolving)
		{
			List<KeyValuePair<ServiceIdentifier, ChainItemInfo>> list = new List<KeyValuePair<ServiceIdentifier, ChainItemInfo>>(_callSiteChain);
			list.Sort((KeyValuePair<ServiceIdentifier, ChainItemInfo> a, KeyValuePair<ServiceIdentifier, ChainItemInfo> b) => a.Value.Order.CompareTo(b.Value.Order));
			foreach (KeyValuePair<ServiceIdentifier, ChainItemInfo> item in list)
			{
				ServiceIdentifier key = item.Key;
				Type implementationType = item.Value.ImplementationType;
				if (implementationType == null || key.ServiceType == implementationType)
				{
					builder.Append(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ETypeNameHelper.GetTypeDisplayName(key.ServiceType));
				}
				else
				{
					builder.Append(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ETypeNameHelper.GetTypeDisplayName(key.ServiceType)).Append('(').Append(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ETypeNameHelper.GetTypeDisplayName(implementationType))
						.Append(')');
				}
				builder.Append(" -> ");
			}
			builder.Append(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ETypeNameHelper.GetTypeDisplayName(currentlyResolving.ServiceType));
		}
	}
}
