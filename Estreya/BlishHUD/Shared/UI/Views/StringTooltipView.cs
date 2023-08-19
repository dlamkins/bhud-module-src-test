using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class StringTooltipView : View, ITooltipView, IView
	{
		private string Message { get; }

		private int MaxWidth { get; } = 200;


		public StringTooltipView(string message)
			: this()
		{
			Message = message;
		}

		public StringTooltipView(string message, int maxWidth)
			: this()
		{
			Message = message;
			MaxWidth = maxWidth;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			buildPanel.set_HeightSizingMode((SizingMode)1);
			buildPanel.set_WidthSizingMode((SizingMode)1);
			Label val = new Label();
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(false);
			((Control)val).set_Width(MaxWidth);
			((Control)val).set_Padding(new Thickness(0f, 8f, 0f, 0f));
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_VerticalAlignment((VerticalAlignment)1);
			val.set_TextColor(StandardColors.get_DisabledText());
			val.set_WrapText(true);
			val.set_Text(Message);
			((Control)val).set_Parent(buildPanel);
		}
	}
}
