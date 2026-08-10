namespace Microsoft.Extensions.Options
{
	internal interface IConfigureNamedOptions<in TOptions> : IConfigureOptions<TOptions> where TOptions : class
	{
		void Configure(string? name, TOptions options);
	}
}
