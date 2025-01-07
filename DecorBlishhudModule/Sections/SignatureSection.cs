using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace DecorBlishhudModule.Sections
{
	public class SignatureSection
	{
		private readonly Container _parentWindow;

		private Label _signatureLabel;

		private Image _signatureImage;

		public SignatureSection(Container parentWindow)
		{
			_parentWindow = parentWindow;
			AddSignatureLabel();
			PositionSignatureLabel();
		}

		private void AddSignatureLabel()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(_parentWindow);
			val.set_Text("With      from Terter.4125");
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_WrapText(true);
			val.set_StrokeText(true);
			val.set_ShowShadow(true);
			val.set_ShadowColor(new Color(0, 0, 0));
			((Control)val).set_Height(185);
			((Control)val).set_Width(235);
			_signatureLabel = val;
			Image val2 = new Image();
			((Control)val2).set_Parent(_parentWindow);
			val2.set_Texture(AsyncTexture2D.op_Implicit(DecorModule.DecorModuleInstance.Heart));
			((Control)val2).set_Location(new Point(((Control)_signatureLabel).get_Right() + 693, ((Control)_signatureLabel).get_Bottom() + 489));
			((Control)val2).set_Size(new Point(25, 25));
			_signatureImage = val2;
			((Control)_parentWindow).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				PositionSignatureLabel();
			});
		}

		private void PositionSignatureLabel()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			int offsetX = 20;
			int offsetY = 20;
			((Control)_signatureLabel).set_Location(new Point(((Control)_parentWindow).get_Width() - ((Control)_signatureLabel).get_Width() - offsetX, ((Control)_parentWindow).get_Height() - ((Control)_signatureLabel).get_Height() - offsetY));
			((Control)_signatureLabel).Invalidate();
		}

		public void UpdateFlowPanelPosition(bool isBigView)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			((Control)_signatureLabel).set_Width(isBigView ? 215 : 235);
			((Control)_signatureLabel).set_Height(isBigView ? 135 : 185);
			PositionSignatureLabel();
			((Control)_signatureImage).set_Location(new Point(((Control)_signatureLabel).get_Location().X + 33, ((Control)_signatureLabel).get_Location().Y + (isBigView ? 54 : 79)));
			((Control)_signatureImage).Invalidate();
		}
	}
}
