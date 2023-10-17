using System.Collections.Generic;

namespace BhModule.Community.ErrorSubmissionModule
{
	public class EtmConfig
	{
		public string BaseDsn { get; set; } = "https://a3aeb0597daa404199a7dedba9e6fe87@sentry.blishhud.com:2083/2";


		public int MaxReports { get; set; } = 10;


		public double TracesSampleRate { get; set; } = 0.2;


		public string SupportedEtm { get; set; } = ">=0.0.0";


		public bool DisableTaskExceptions { get; set; } = true;


		public string SupportedBlishHUD { get; set; } = ">=0.7.0";


		public bool ApiHookEnabled { get; set; }

		public string ApiReportUri { get; set; } = "https://sam.blishhud.com/";


		public bool ApiReportSuccess { get; set; } = true;


		public List<ModuleDetails> Modules { get; set; } = new List<ModuleDetails>();

	}
}
