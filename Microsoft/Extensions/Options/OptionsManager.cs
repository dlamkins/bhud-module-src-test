using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options
{
	internal class OptionsManager<[_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMembers(_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EDynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class
	{
		private readonly IOptionsFactory<TOptions> _factory;

		private readonly OptionsCache<TOptions> _cache = new OptionsCache<TOptions>();

		public TOptions Value => Get(Options.DefaultName);

		public OptionsManager(IOptionsFactory<TOptions> factory)
		{
			_factory = factory;
		}

		public virtual TOptions Get(string? name)
		{
			if (name == null)
			{
				name = Options.DefaultName;
			}
			if (!_cache.TryGetValue(name, out var options))
			{
				IOptionsFactory<TOptions> localFactory = _factory;
				string localName = name;
				return _cache.GetOrAdd(name, () => localFactory.Create(localName));
			}
			return options;
		}
	}
}
