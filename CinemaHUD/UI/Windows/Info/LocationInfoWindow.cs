using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule;
using CinemaModule.Models;
using Microsoft.Xna.Framework;

namespace CinemaHUD.UI.Windows.Info
{
	public class LocationInfoWindow : StandardWindow
	{
		private static readonly Logger Logger = Logger.GetLogger<LocationInfoWindow>();

		private WorldLocationPresetData _currentPreset;

		private Image _screenshotImage;

		private Label _descriptionLabel;

		private StandardButton _waypointButton;

		public LocationInfoWindow(AsyncTexture2D backgroundTexture)
			: this(backgroundTexture, new Rectangle(25, 26, 435, 480), new Rectangle(40, 30, 415, 440))
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Location Info");
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(global::CinemaModule.CinemaModule.Instance.TextureService.GetEmblem()));
			((Control)this).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)this).get_Width()) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)this).get_Height()) / 2));
			((WindowBase2)this).set_SavesPosition(false);
			((WindowBase2)this).set_CanResize(false);
			BuildContent();
		}

		private void BuildContent()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel panel = val;
			Image val2 = new Image();
			((Control)val2).set_Width(380);
			((Control)val2).set_Height(350);
			((Control)val2).set_Parent((Container)(object)panel);
			_screenshotImage = val2;
			Label val3 = new Label();
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val3).set_Parent((Container)(object)panel);
			_descriptionLabel = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Copy Waypoint");
			((Control)val4).set_Width(150);
			((Control)val4).set_Parent((Container)(object)panel);
			_waypointButton = val4;
			((Control)_waypointButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CopyWaypoint();
			});
		}

		public void ShowPreset(WorldLocationPresetData preset)
		{
			_currentPreset = preset;
			((WindowBase2)this).set_Title(preset.Name ?? "Location Info");
			_descriptionLabel.set_Text(preset.Description ?? string.Empty);
			_screenshotImage.set_Texture(preset.PictureTexture);
			((Control)_waypointButton).set_Visible(!string.IsNullOrEmpty(preset.Waypoint));
			((Control)this).Show();
		}

		private void CopyWaypoint()
		{
			string waypoint = _currentPreset?.Waypoint;
			if (!string.IsNullOrEmpty(waypoint))
			{
				try
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(waypoint);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to copy waypoint to clipboard");
				}
			}
		}
	}
}
