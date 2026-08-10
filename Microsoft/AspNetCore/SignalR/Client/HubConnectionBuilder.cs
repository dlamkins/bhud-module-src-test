using System;
using System.ComponentModel;
using System.Net;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.SignalR.Client
{
	internal class HubConnectionBuilder : IHubConnectionBuilder, ISignalRBuilder
	{
		private bool _hubConnectionBuilt;

		public IServiceCollection Services { get; }

		public HubConnectionBuilder()
		{
			Services = new ServiceCollection();
			Services.AddSingleton<HubConnection>();
			Services.AddLogging();
			this.AddJsonProtocol();
		}

		public HubConnection Build()
		{
			if (_hubConnectionBuilt)
			{
				throw new InvalidOperationException("HubConnectionBuilder allows creation only of a single instance of HubConnection.");
			}
			_hubConnectionBuilt = true;
			ServiceProvider provider = Services.BuildServiceProvider();
			if (provider.GetService<IConnectionFactory>() == null)
			{
				throw new InvalidOperationException("Cannot create HubConnection instance. An IConnectionFactory was not configured.");
			}
			if (provider.GetService<EndPoint>() == null)
			{
				throw new InvalidOperationException("Cannot create HubConnection instance. An EndPoint was not configured.");
			}
			return provider.GetRequiredService<HubConnection>();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object? obj)
		{
			return base.Equals(obj);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string? ToString()
		{
			return base.ToString();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Type GetType()
		{
			return base.GetType();
		}
	}
}
