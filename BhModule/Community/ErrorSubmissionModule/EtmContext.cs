using Blish_HUD.Contexts;
using Etm.Sdk;
using Sentry;

namespace BhModule.Community.ErrorSubmissionModule
{
	public class EtmContext : Context, IEtmContext
	{
		public IPerformanceTransaction StartPerformanceTransaction(string name, string operation, string description = null)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)((Context)this).get_State() == 3 || !SentrySdk.IsEnabled)
			{
				return null;
			}
			return new PerformanceTransaction(SentrySdk.StartTransaction(name, operation, description));
		}

		public EtmContext()
			: this()
		{
		}
	}
}
