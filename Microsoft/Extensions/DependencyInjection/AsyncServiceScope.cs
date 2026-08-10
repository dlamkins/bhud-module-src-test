using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
	[DebuggerDisplay("{ServiceProvider,nq}")]
	internal readonly struct AsyncServiceScope : IServiceScope, IDisposable, IAsyncDisposable
	{
		private readonly IServiceScope _serviceScope;

		public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

		public AsyncServiceScope(IServiceScope serviceScope)
		{
			_003Cae67097c_002D2257_002D48ab_002Dbc5e_002D426f210f9bd0_003EThrowHelper.ThrowIfNull(serviceScope, "serviceScope");
			_serviceScope = serviceScope;
		}

		public void Dispose()
		{
			_serviceScope.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			IAsyncDisposable asyncDisposable = _serviceScope as IAsyncDisposable;
			if (asyncDisposable != null)
			{
				return asyncDisposable.DisposeAsync();
			}
			_serviceScope.Dispose();
			return default(ValueTask);
		}
	}
}
