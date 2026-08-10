namespace Microsoft.Extensions.DependencyInjection
{
	internal class ServiceProviderOptions
	{
		internal static readonly ServiceProviderOptions Default = new ServiceProviderOptions();

		public bool ValidateScopes { get; set; }

		public bool ValidateOnBuild { get; set; }
	}
}
