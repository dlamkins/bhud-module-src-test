using System;
using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Soeed.GuildGeoGuesser.Settings.Controls;
using Soeed.GuildGeoGuesser.Settings.Services;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Settings.Views.SubViews
{
	public class GeneralSettingsView : View
	{
		protected SettingService _settings = Service.Settings;

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel container = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString("Puzzle List Settings").AddSetting((SettingEntry)(object)_settings.PuzzlesPerPage)
				.AddSetting((SettingEntry)(object)_settings.PuzzleSort)
				.AddSetting((SettingEntry)(object)_settings.ShowCameraModeFilter)
				.AddSpace()
				.AddString("Top-left menu bar icon settings")
				.AddSetting((SettingEntry)(object)_settings.CornerIconPriority)
				.AddSpace(40);
			StandardButton val = new StandardButton();
			val.set_Text("Update Notes");
			((Control)val).set_BasicTooltipText("Open the module update notes in your default web browser");
			StandardButton patchNotesButton;
			FlowPanel container2 = container.AddControl<StandardButton>(val, out patchNotesButton).AddSpace(40);
			NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
			((StandardButton)nuclearOptionButton).set_Text("Purge downloaded images cache");
			((Control)nuclearOptionButton).set_BasicTooltipText("Remove all downloaded images. Press and hold CTRL and SHIFT, then click the button");
			((Control)nuclearOptionButton).set_Width(300);
			container2.AddControl<NuclearOptionButton>(nuclearOptionButton, out NuclearOptionButton resetCache);
			((Control)patchNotesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://pkgs.blishhud.com/Soeed.GuildGeoGuesser.html",
					UseShellExecute = true
				});
			});
			((Control)resetCache).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.Textures.ResetDownloadCache();
			});
		}

		public GeneralSettingsView()
			: this()
		{
		}
	}
}
