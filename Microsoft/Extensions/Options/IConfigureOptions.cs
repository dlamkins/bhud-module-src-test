namespace Microsoft.Extensions.Options
{
	internal interface IConfigureOptions<in TOptions> where TOptions : class
	{
		void Configure(TOptions options);
	}
}
