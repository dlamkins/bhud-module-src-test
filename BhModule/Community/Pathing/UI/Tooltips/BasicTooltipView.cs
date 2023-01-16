using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace BhModule.Community.Pathing.UI.Tooltips
{
	internal class BasicTooltipView : View, ITooltipView, IView
	{
		private const int MAX_WIDTH = 500;

		private readonly Label _tooltipLabel;

		public string Text
		{
			get
			{
				return _tooltipLabel.get_Text();
			}
			set
			{
				UpdateLabelValueAndWidth(value);
			}
		}

		public BasicTooltipView(string text)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			Label val = new Label();
			val.set_ShowShadow(true);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			_tooltipLabel = val;
			Text = text;
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_tooltipLabel).set_Parent(buildPanel);
		}

		private void UpdateLabelValueAndWidth(string value)
		{
			_tooltipLabel.set_WrapText(false);
			_tooltipLabel.set_AutoSizeWidth(true);
			_tooltipLabel.set_Text(value);
			if (((Control)_tooltipLabel).get_Width() > 500)
			{
				_tooltipLabel.set_AutoSizeWidth(false);
				_tooltipLabel.set_WrapText(true);
				((Control)_tooltipLabel).set_Width(500);
			}
		}
	}
}
