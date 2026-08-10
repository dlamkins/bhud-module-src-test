namespace Microsoft.Extensions.Options
{
	internal interface IValidateOptions<TOptions> where TOptions : class
	{
		ValidateOptionsResult Validate(string? name, TOptions options);
	}
}
