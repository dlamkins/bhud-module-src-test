using System;
using System.Diagnostics;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
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
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString(Strings.Fractals_Selection_Prompt).AddSetting((SettingEntry)(object)_settings.ChallengeMotes)
				.AddSetting((SettingEntry)(object)_settings.DailyTierN)
				.AddSetting((SettingEntry)(object)_settings.DailyRecs)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.TomorrowTierN);
			Image val = new Image();
			val.set_Texture(AsyncTexture2D.op_Implicit(Service.Textures!.BaseLogo));
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(300, 65));
			((Control)val).set_Size(PointExtensions.Scale(new Point(400, 278), 0.5f));
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(10, ((Control)buildPanel).get_Bottom() - 50));
			val2.set_AutoSizeWidth(true);
			val2.set_Text("Special thank you to Invisi for providing challenge mote Instabilities information");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://github.com/Invisi/gw2-fotm-instabilities",
					UseShellExecute = true
				});
			});
		}
	}
}
