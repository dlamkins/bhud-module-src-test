using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal abstract class ServiceCallSite
	{
		public abstract Type ServiceType { get; }

		public abstract Type? ImplementationType { get; }

		public abstract CallSiteKind Kind { get; }

		public ResultCache Cache { get; }

		public object? Value { get; set; }

		public object? Key { get; set; }

		public bool CaptureDisposable
		{
			get
			{
				if (!(ImplementationType == null) && !typeof(IDisposable).IsAssignableFrom(ImplementationType))
				{
					return typeof(IAsyncDisposable).IsAssignableFrom(ImplementationType);
				}
				return true;
			}
		}

		protected ServiceCallSite(ResultCache cache)
		{
			Cache = cache;
		}
	}
}
