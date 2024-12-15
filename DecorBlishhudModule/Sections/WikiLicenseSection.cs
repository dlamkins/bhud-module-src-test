using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace DecorBlishhudModule.Sections
{
	public class WikiLicenseSection
	{
		private readonly Container _parentWindow;

		private Label _licenseLabel;

		public WikiLicenseSection(Container parentWindow)
		{
			_parentWindow = parentWindow;
			AddLicenseLabel();
			PositionLicenseLabel();
		}

		private void AddLicenseLabel()
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
			Label val = new Label();
			((Control)val).set_Parent(_parentWindow);
			val.set_Text("Images © Guild Wars 2 Wiki");
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_WrapText(true);
			val.set_StrokeText(true);
			val.set_ShowShadow(true);
			val.set_ShadowColor(new Color(0, 0, 0));
			((Control)val).set_Height(195);
			((Control)val).set_Width(600);
			_licenseLabel = val;
			((Control)_parentWindow).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				PositionLicenseLabel();
			});
		}

		private void PositionLicenseLabel()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			int offsetX = 15;
			int offsetY = 15;
			((Control)_licenseLabel).set_Location(new Point(((Control)_parentWindow).get_Width() - ((Control)_licenseLabel).get_Width() - offsetX, ((Control)_parentWindow).get_Height() - ((Control)_licenseLabel).get_Height() - offsetY));
			((Control)_licenseLabel).Invalidate();
		}

		public void UpdateWidthBasedOnFlowPanel(bool isBigView)
		{
			((Control)_licenseLabel).set_Width(isBigView ? 1110 : 600);
			PositionLicenseLabel();
		}
	}
}
