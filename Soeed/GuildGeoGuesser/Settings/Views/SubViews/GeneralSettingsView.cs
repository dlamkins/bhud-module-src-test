using System;
using System.Diagnostics;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
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
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString("Puzzle List Settings").AddSetting((SettingEntry)(object)_settings.PuzzlesPerPage)
				.AddSetting((SettingEntry)(object)_settings.PuzzleSort)
				.AddSetting((SettingEntry)(object)_settings.ShowCameraModeFilter)
				.AddSpace()
				.AddString("Top-left menu bar icon settings")
				.AddSetting((SettingEntry)(object)_settings.CornerIconPriority)
				.AddSpace(40);
			if (!Service.UserManager.TutorialState.PublicUnlocked)
			{
				NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
				((StandardButton)nuclearOptionButton).set_Text("Unlock all puzzles - skip practice mode");
				((Control)nuclearOptionButton).set_BasicTooltipText("Unlock public puzzles without completing a practice puzzle. Hold CTRL+SHIFT and click to confirm.");
				((Control)nuclearOptionButton).set_Width(300);
				panel.AddControl<NuclearOptionButton>(nuclearOptionButton, out NuclearOptionButton skipTutorial);
				((Control)skipTutorial).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					TutorialProgress progress = await Service.GeoServerWrapper.OptOutOfTutorialAsync();
					if (progress == null)
					{
						ScreenNotification.ShowNotification("Failed to skip practice mode. Try again later.", (NotificationType)2, (Texture2D)null, 4);
					}
					else
					{
						Service.UserManager.SetTutorialState(progress);
						await Service.UserManager.RefreshGuildsAsync();
						ScreenNotification.ShowNotification("Practice Mode skipped. All puzzles are now available.", (NotificationType)5, (Texture2D)null, 4);
						((Control)skipTutorial).set_Visible(false);
					}
				});
				panel.AddSpace(40);
			}
			StandardButton val = new StandardButton();
			val.set_Text("Update Notes");
			((Control)val).set_BasicTooltipText("Open the module update notes in your default web browser");
			StandardButton patchNotesButton;
			FlowPanel container = panel.AddControl<StandardButton>(val, out patchNotesButton).AddSpace(40);
			NuclearOptionButton nuclearOptionButton2 = new NuclearOptionButton();
			((StandardButton)nuclearOptionButton2).set_Text("Purge downloaded images cache");
			((Control)nuclearOptionButton2).set_BasicTooltipText("Remove all downloaded images. Press and hold CTRL and SHIFT, then click the button");
			((Control)nuclearOptionButton2).set_Width(300);
			container.AddControl<NuclearOptionButton>(nuclearOptionButton2, out NuclearOptionButton resetCache);
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
