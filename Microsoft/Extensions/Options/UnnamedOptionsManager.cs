using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Microsoft.Extensions.Options
{
	internal sealed class UnnamedOptionsManager<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptions<TOptions> where TOptions : class
	{
		private readonly IOptionsFactory<TOptions> _factory;

		private volatile object _syncObj;

		private volatile TOptions _value;

		public TOptions Value
		{
			get
			{
				TOptions value = _value;
				if (value != null)
				{
					return value;
				}
				lock (_syncObj ?? Interlocked.CompareExchange(ref _syncObj, new object(), null) ?? _syncObj)
				{
					return _value ?? (_value = _factory.Create(Options.DefaultName));
				}
			}
		}

		public UnnamedOptionsManager(IOptionsFactory<TOptions> factory)
		{
			_factory = factory;
		}
	}
}
