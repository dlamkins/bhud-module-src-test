using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections
{
	internal class MultiplexedConnectionBuilder : IMultiplexedConnectionBuilder
	{
		private readonly IList<Func<MultiplexedConnectionDelegate, MultiplexedConnectionDelegate>> _components = new List<Func<MultiplexedConnectionDelegate, MultiplexedConnectionDelegate>>();

		public IServiceProvider ApplicationServices { get; }

		public MultiplexedConnectionBuilder(IServiceProvider applicationServices)
		{
			ApplicationServices = applicationServices;
		}

		public IMultiplexedConnectionBuilder Use(Func<MultiplexedConnectionDelegate, MultiplexedConnectionDelegate> middleware)
		{
			_components.Add(middleware);
			return this;
		}

		public MultiplexedConnectionDelegate Build()
		{
			MultiplexedConnectionDelegate multiplexedConnectionDelegate = (MultiplexedConnectionContext features) => Task.CompletedTask;
			foreach (Func<MultiplexedConnectionDelegate, MultiplexedConnectionDelegate> item in _components.Reverse())
			{
				multiplexedConnectionDelegate = item(multiplexedConnectionDelegate);
			}
			return multiplexedConnectionDelegate;
		}
	}
}
