using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.RtApi;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class CornerIconSettingsView : View
	{
		protected SettingService _settings;

		private Label? _rtApiStatusLabel;

		protected override void Build(Container buildPanel)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Expected O, but got Unknown
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			_settings = Service.Settings;
			((View<IPresenter>)this).Build(buildPanel);
			if (RtApiIntegrationHelper.IsEnabled)
			{
				Service.RtApiConnection?.EnsureActive();
			}
			if (Service.RtApiConnection != null)
			{
				Service.RtApiConnection!.ConnectionStateChanged -= new EventHandler<RtApiConnectionState>(OnRtApiConnectionStateChanged);
				Service.RtApiConnection!.ConnectionStateChanged += new EventHandler<RtApiConnectionState>(OnRtApiConnectionStateChanged);
			}
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString("Top-left menu bar icon settings").AddSetting((SettingEntry)(object)_settings.CornerIconEnabled)
				.AddSpace()
				.AddSettingEnum((SettingEntry)(object)_settings.CornerIconLeftClickAction)
				.AddSpace()
				.AddSettingEnum((SettingEntry)(object)_settings.CornerIconTexture)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.CornerIconPriority)
				.AddSpace(40)
				.AddString("External Data Integrations")
				.AddSpace(20)
				.AddSetting((SettingEntry)(object)_settings.RtApiIntegrationEnabled);
			Label val = new Label();
			val.set_Text(RtApiStatusText.ForState(Service.RtApiConnection?.State ?? RtApiConnectionState.NotDetected));
			val.set_AutoSizeWidth(true);
			Control rtApiStatusLabel;
			FlowPanel panel2 = panel.AddFlowControl((Control)val, out rtApiStatusLabel).AddSpace(40);
			StandardButton val2 = new StandardButton();
			val2.set_Text("Update Notes");
			((Control)val2).set_BasicTooltipText("Open the module update notes in your default web browser");
			panel2.AddFlowControl((Control)val2, out var patchNotesButton);
			_rtApiStatusLabel = (Label?)(object)((rtApiStatusLabel is Label) ? rtApiStatusLabel : null);
			_settings.RtApiIntegrationEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnRtApiIntegrationSettingChanged);
			patchNotesButton.add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://pkgs.blishhud.com/Manlaan.CommanderMarkers.html",
					UseShellExecute = true
				});
			});
			Label val3 = new Label();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Location(new Point(20, ((Control)buildPanel).get_Height() - 50));
			val3.set_Text("Special Thank You to the testers: QuitarHero, Kami, and Naru\nand to Metallis for the module icon");
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
		}

		private void OnRtApiConnectionStateChanged(object? sender, RtApiConnectionState state)
		{
			if (_rtApiStatusLabel != null)
			{
				_rtApiStatusLabel!.set_Text(RtApiStatusText.ForState(state));
			}
		}

		private void OnRtApiIntegrationSettingChanged(object? sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				Service.RtApiConnection?.EnsureActive();
			}
			if (_rtApiStatusLabel != null)
			{
				_rtApiStatusLabel!.set_Text(RtApiStatusText.ForState(Service.RtApiConnection?.State ?? RtApiConnectionState.NotDetected));
			}
		}

		public CornerIconSettingsView()
			: this()
		{
		}
	}
}
