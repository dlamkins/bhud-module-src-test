using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace BhModule.Community.Pathing.UI.Views
{
	public class IndividualPackSettingsView : View
	{
		private const int LOADOPTION_DROPDOWNWIDTH = 128;

		private Label _packNameLabel;

		private Dropdown _packLoadOptionDropdown;

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("");
			((Control)val).set_Width(128);
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val).set_Parent(buildPanel);
			_packNameLabel = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Width(128);
			((Control)val2).set_Left(((Control)_packNameLabel).get_Right() + 16);
			((Control)val2).set_Parent(buildPanel);
			_packLoadOptionDropdown = val2;
		}

		public IndividualPackSettingsView()
			: this()
		{
		}
	}
}
