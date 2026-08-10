using System.Diagnostics;

namespace Microsoft.Extensions.Logging
{
	[DebuggerDisplay("ActivityTrackingOptions = {ActivityTrackingOptions}")]
	internal class LoggerFactoryOptions
	{
		public ActivityTrackingOptions ActivityTrackingOptions { get; set; }
	}
}
