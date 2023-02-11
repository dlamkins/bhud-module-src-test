using Blish_HUD.Settings;

namespace Estreya.BlishHUD.Shared.State
{
	public class ComplianceUpdated
	{
		public SettingEntry SettingEntry { get; }

		public IComplianceRequisite NewCompliance { get; }

		public ComplianceUpdated(SettingEntry settingEntry, IComplianceRequisite newCompliance)
		{
			SettingEntry = settingEntry;
			NewCompliance = newCompliance;
		}
	}
}
