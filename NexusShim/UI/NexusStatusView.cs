using System.Timers;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using NexusShim.Nexus;

namespace NexusShim.UI
{
	internal class NexusStatusView : View
	{
		private NexusIntegration _nexus;

		private Timer _updateTimer;

		private Label _statusLable;

		public NexusStatusView(NexusIntegration nexus)
			: this()
		{
			_nexus = nexus;
			_updateTimer = new Timer
			{
				AutoReset = true,
				Interval = 1000.0
			};
			_updateTimer.Elapsed += UpdateTimer_Elapsed;
		}

		private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (_statusLable != null && _nexus != null)
			{
				_statusLable.set_Text(_nexus.Active ? "Connected to Nexus." : "Not connected to Nexus.");
			}
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Loading...");
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			val.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val).set_Parent(buildPanel);
			_statusLable = val;
			_updateTimer.Enabled = true;
			UpdateTimer_Elapsed(null, null);
		}

		protected override void Unload()
		{
			_updateTimer.Elapsed -= UpdateTimer_Elapsed;
			_updateTimer.Dispose();
		}
	}
}
