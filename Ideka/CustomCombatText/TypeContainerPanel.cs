using Blish_HUD.Controls;
using Ideka.BHUDCommon;

namespace Ideka.CustomCombatText
{
	public class TypeContainerPanel : Panel
	{
		private const int Spacing = 10;

		private ModelTypeContainer? _target;

		private readonly Label _label;

		public ModelTypeContainer? Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = null;
				bool visible;
				((Control)this).set_Enabled(visible = value != null);
				((Control)this).set_Visible(visible);
				_target = value;
			}
		}

		public TypeContainerPanel()
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			((Panel)this).set_Title("Container Area");
			((Panel)this).set_ShowTint(true);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Container areas do not show any messages, they are only meant to contain other areas.");
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(false);
			_label = val;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (_label != null)
			{
				Label label = _label;
				int left;
				((Control)_label).set_Top(left = 10);
				((Control)label).set_Left(left);
				((Control)(object)_label).WidthFillRight();
				((Container)(object)this).MatchHeightToBottom((Control)(object)_label, 10);
			}
		}
	}
}
