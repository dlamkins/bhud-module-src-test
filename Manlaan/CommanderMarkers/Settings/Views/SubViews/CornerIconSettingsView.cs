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
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			_settings = Service.Settings;
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString("Top-left menu bar icon settings").AddSetting((SettingEntry)(object)_settings.CornerIconEnabled)
				.AddSpace()
				.AddSettingEnum((SettingEntry)(object)_settings.CornerIconLeftClickAction)
				.AddSpace()
				.AddSettingEnum((SettingEntry)(object)_settings.CornerIconTexture)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.CornerIconPriority)
				.AddSpace(40);
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
