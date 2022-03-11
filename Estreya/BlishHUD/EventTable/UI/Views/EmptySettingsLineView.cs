using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class EmptySettingsLineView : View
	{
		private int Height { get; set; }

		public EmptySettingsLineView(int height)
			: this()
		{
			Height = height;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Height(Height);
		}
	}
}
