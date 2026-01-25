using System;
using System.Collections.Generic;
using System.Diagnostics;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Features.Fractals.Services;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class FractalSelectionView : View
	{
		private readonly FractalSettings _settings;

		private readonly FractalSettingsPersistance _fractalSettings;

		public FractalSelectionView(FractalSettings settings)
			: this()
		{
			_settings = settings;
			_fractalSettings = Service.FractalSettings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString(Strings.Fractals_Selection_Prompt).AddSetting((SettingEntry)(object)_settings.ChallengeMotes);
			List<SettingEntry<bool>> challengeMotes = new List<SettingEntry<bool>>();
			int[] challengeMotes2 = Service.FractalMapData.ChallengeMotes;
			foreach (int scale in challengeMotes2)
			{
				FractalMap fractal = Service.FractalMapData.GetFractalForScale(scale);
				if (fractal.ApiLabel != "undefined")
				{
					challengeMotes.Add(_fractalSettings.GetChallengeMoteVisible(fractal));
				}
			}
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(((Control)panel).get_Width() - 40);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_OuterControlPadding(new Vector2(20f, 0f));
			panel.AddFlowControl((Control)(object)FlowPanelExtensions.AddHorizontalSpace(val, 20).AddSetting((IEnumerable<SettingEntry>?)challengeMotes));
			panel.AddSpace().AddSetting((SettingEntry)(object)_settings.DailyTierN).AddSetting((SettingEntry)(object)_settings.DailyRecs)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.TomorrowTierN);
			Image val2 = new Image();
			val2.set_Texture(AsyncTexture2D.op_Implicit(Service.Textures!.BaseLogo));
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(300, 65));
			((Control)val2).set_Size(PointExtensions.Scale(new Point(400, 278), 0.5f));
			Label val3 = new Label();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Location(new Point(10, ((Control)buildPanel).get_Bottom() - 60));
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Height(50);
			val3.set_WrapText(true);
			val3.set_Text(Strings.FractalSelection_ThanksInvisi);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
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
