using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Settings.Controls;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class AutoMarkerSettingsView : View
	{
		protected SettingService _settings;

		protected override void Build(Container buildPanel)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			_settings = Service.Settings;
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString("AutoMarker Feature").AddString("Allows for rapidly placing saved marker sets")
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.AutoMarker_FeatureEnabled)
				.AddSetting((SettingEntry)(object)_settings.AutoMarker_OnlyWhenCommander)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.AutoMarker_ShowTrigger)
				.AddSetting((SettingEntry)(object)_settings.AutoMarker_ShowPreview)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.AutoMarker_PlacementDelay);
			Label val = new Label();
			val.set_Text($"  Delay Time: {_settings.AutoMarker_PlacementDelay.get_Value()} ms");
			val.set_AutoSizeWidth(true);
			panel.AddFlowControl((Control)val, out var delayLabel);
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text("Press and hold Ctrl and Shift to activate the button");
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(0, ((Control)buildPanel).get_Height() - 65));
			NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
			((Control)nuclearOptionButton).set_Parent(buildPanel);
			((StandardButton)nuclearOptionButton).set_Text("Reset Library To Default");
			((Control)nuclearOptionButton).set_BasicTooltipText("Warning: This will delete ALL marker sets in your Library\nand restore the default markers.\n\nPress and hold Ctrl and Shift to activate the button");
			((Control)nuclearOptionButton).set_Width(200);
			((Control)nuclearOptionButton).set_Location(new Point(0, ((Control)buildPanel).get_Height() - 35));
			((Control)nuclearOptionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.MarkersListing.ResetToDefault();
				ScreenNotification.ShowNotification("AutoMarker Library has been reset", (NotificationType)3, (Texture2D)null, 4);
			});
			Image val3 = new Image();
			((Control)val3).set_Parent(buildPanel);
			val3.set_Texture(AsyncTexture2D.op_Implicit(Service.Textures!._blishHeart));
			((Control)val3).set_Size(new Point(96, 96));
			((Control)val3).set_Location(new Point(((Control)buildPanel).get_Width() - 96, ((Control)buildPanel).get_Height() - 96));
			_settings.AutoMarker_PlacementDelay.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
			{
				Control obj = delayLabel;
				((Label)((obj is Label) ? obj : null)).set_Text($"  Delay Time: {_settings.AutoMarker_PlacementDelay.get_Value()} ms");
			});
		}

		public AutoMarkerSettingsView()
			: this()
		{
		}
	}
}
