using System;
using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class CornerIconSettingsView : View
	{
		protected SettingService _settings;

		protected override void Build(Container buildPanel)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			_settings = Service.Settings;
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString("Top-left menu bar icon settings").AddSetting((SettingEntry)(object)_settings.CornerIconEnabled)
				.AddSpace()
				.AddSettingEnum((SettingEntry)(object)_settings.CornerIconLeftClickAction)
				.AddSpace(100);
			StandardButton val = new StandardButton();
			val.set_Text("Update Notes");
			((Control)val).set_BasicTooltipText("Open the module update notes in your default web browser");
			panel.AddFlowControl((Control)val, out var patchNotesButton);
			patchNotesButton.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://pkgs.blishhud.com/Manlaan.CommanderMarkers.html",
					UseShellExecute = true
				});
			});
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(20, ((Control)buildPanel).get_Height() - 30));
			val2.set_Text("Special Thank You to the testers: QuitarHero, Kami, and Naru");
			val2.set_AutoSizeWidth(true);
		}

		public CornerIconSettingsView()
			: this()
		{
		}
	}
}
