using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class FractalSelectionView : View
	{
		private readonly FractalSettings _settings;

		public FractalSelectionView(FractalSettings settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString(Strings.Fractals_Selection_Prompt).AddSetting((SettingEntry)(object)_settings.DailyTierN)
				.AddSetting((SettingEntry)(object)_settings.DailyRecs)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.TomorrowTierN);
			Image val = new Image();
			val.set_Texture(AsyncTexture2D.op_Implicit(Service.Textures!.BaseLogo));
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(300, 65));
			((Control)val).set_Size(PointExtensions.Scale(new Point(400, 278), 0.5f));
		}
	}
}
