namespace Microsoft.Extensions.Options
{
	internal interface IPostConfigureOptions<in TOptions> where TOptions : class
	{
		void PostConfigure(string? name, TOptions options);
	}
}
