using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class GeoGuessCreateView : AccountRestrictedView
	{
		[CompilerGenerated]
		private GeoGuessWindowStateService _003CWindowState_003EP;

		protected readonly int IMAGE_PADDING;

		protected Container? _buildPanel;

		protected TextBox _title;

		protected StandardButton _saveButton;

		protected StandardButton _takePicture;

		protected Label _imagePreview;

		protected Bitmap _rawScreenGrab;

		protected Location _mumbleCoord;

		protected AsyncTexture2D _texture;

		protected Image _image;

		protected double _aspectRatio;

		private static readonly int DELAY = 150;

		public GeoGuessCreateView(GeoGuessWindowStateService WindowState)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			_003CWindowState_003EP = WindowState;
			IMAGE_PADDING = 30;
			_title = new TextBox();
			StandardButton val = new StandardButton();
			val.set_Text("Save & Publish");
			((Control)val).set_BasicTooltipText("Save your new GeoGuess game to share with your selected guild");
			((Control)val).set_Width(200);
			((Control)val).set_Height(40);
			val.set_Icon(Service.Textures.DatAsset(156108));
			((Control)val).set_Visible(true);
			((Control)val).set_Enabled(false);
			_saveButton = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Take Picture");
			((Control)val2).set_BasicTooltipText("Take a picture of the current location");
			((Control)val2).set_Width(200);
			((Control)val2).set_Height(40);
			_takePicture = val2;
			_imagePreview = new Label();
			_rawScreenGrab = new Bitmap(1, 1);
			_mumbleCoord = new Location();
			_texture = AsyncTexture2D.op_Implicit(Textures.get_Pixel());
			_image = new Image();
			_aspectRatio = 1.0;
			base._002Ector(showBackButton: true);
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Expected O, but got Unknown
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Expected O, but got Unknown
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Expected O, but got Unknown
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Expected O, but got Unknown
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_controlsHeader);
			((Control)val).set_Location(new Point(((Control)_backButton).get_Right() + 5, 0));
			val.set_AutoSizeWidth(true);
			((Control)val).set_Height(40);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
			val.set_TextColor(Color.get_PaleGoldenrod());
			val.set_Text("Create a new GeoGuess Game");
			_texture = new AsyncTexture2D(AsyncTexture2D.op_Implicit(Service.Textures.BlishWaterColor));
			_aspectRatio = (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height();
			((Panel)_flowPanel).set_CanScroll(true);
			((Control)_flowPanel).set_Width(((Control)buildPanel).get_Width());
			bool isCompetitiveMode = GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode();
			if (isCompetitiveMode)
			{
				_flowPanel.AddString("Cannot create puzzles in competitive mode maps (PvP, WvW, etc.)", out var warningLabel);
				warningLabel.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2));
				warningLabel.set_TextColor(Color.get_Orange());
				_flowPanel.AddSpace();
			}
			FlowPanel container = _flowPanel.AddString("New GeoGuess Title");
			TextBox val2 = new TextBox();
			((Control)val2).set_Width(600);
			((TextInputBase)val2).set_MaxLength(64);
			((Control)val2).set_Height(40);
			((TextInputBase)val2).set_Text(GameService.Gw2Mumble.get_PlayerCharacter().get_Name() + "'s GeoGuess");
			((Control)val2).set_BasicTooltipText("New GeoGuess Title, Visible to guildmates, max length 64");
			FlowPanel container2 = container.AddControl<TextBox>(val2, out _title).AddSpace();
			FlowPanel val3 = new FlowPanel();
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			val3.set_FlowDirection((ControlFlowDirection)0);
			val3.set_ControlPadding(new Vector2(5f, 0f));
			FlowPanel actionsFlow;
			FlowPanel container3 = container2.AddControl<FlowPanel>(FlowPanelExtensions.AddControl<StandardButton>(val3, _takePicture).AddControl<StandardButton>(_saveButton), out actionsFlow);
			FlowPanel val4 = new FlowPanel();
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_FlowDirection((ControlFlowDirection)0);
			val4.set_ControlPadding(new Vector2(5f, 0f));
			FlowPanel container4 = FlowPanelExtensions.AddSetting(val4, (SettingEntry)(object)Service.Settings.SendHideGw2Ui, 200).AddSetting((SettingEntry)(object)Service.Settings.SendHideBlishUi, 125).AddSetting((SettingEntry)(object)Service.Settings.SendHideArcDpsUi, 125)
				.AddSetting((SettingEntry)(object)Service.Settings.SendHideNexusUi, 125);
			StandardButton val5 = new StandardButton();
			val5.set_Text("Keybind Settings");
			((Control)val5).set_Width(125);
			StandardButton openKeybinds;
			FlowPanel optionsFlow;
			FlowPanel container5 = container3.AddControl<FlowPanel>(container4.AddControl<StandardButton>(val5, out openKeybinds), out optionsFlow).AddSpace();
			Label val6 = new Label();
			val6.set_Text("Picture Preview");
			val6.set_AutoSizeWidth(true);
			FlowPanel container6 = container5.AddControl<Label>(val6, out _imagePreview);
			Image val7 = new Image();
			val7.set_Texture(_texture);
			((Control)val7).set_Size(new Point(((Control)buildPanel).get_Width() - IMAGE_PADDING, (int)((double)(((Control)buildPanel).get_Width() - IMAGE_PADDING) / _aspectRatio)));
			container6.AddControl<Image>(val7, out _image).AddSpace(30);
			if (isCompetitiveMode)
			{
				((Control)_takePicture).set_Enabled(false);
				((Control)_takePicture).set_BasicTooltipText("Cannot take pictures in competitive mode maps (PvP, WvW, etc.)");
				((Control)_saveButton).set_Enabled(false);
				((Control)_saveButton).set_BasicTooltipText("Cannot save puzzles in competitive mode maps (PvP, WvW, etc.)");
			}
			((Control)openKeybinds).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.SettingsWindow.ShowKeybinds();
			});
			_buildPanel = buildPanel;
			((Control)_buildPanel).add_Resized((EventHandler<ResizedEventArgs>)BuildPanel_Resized);
			((Control)_takePicture).add_Click((EventHandler<MouseEventArgs>)HandleTakePicture);
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)HandleSave);
		}

		private void HandleTakePicture(object sender, MouseEventArgs e)
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				ScreenNotification.ShowNotification("Cannot take pictures in competitive mode maps (PvP, WvW, etc.)", (NotificationType)6, (Texture2D)null, 3);
				return;
			}
			Task.Run(async delegate
			{
				ToggleHud();
				await Task.Delay(DELAY);
				_rawScreenGrab = ScreenCaptureService.CaptureBitmap();
				await Task.Delay(DELAY);
				ToggleHud();
				using MemoryStream memStream = new MemoryStream();
				_rawScreenGrab.Save(memStream, ImageFormat.Png);
				_texture.SwapTexture(TextureUtil.FromStreamPremultiplied((Stream)memStream));
				_mumbleCoord = Location.SetFromMumble();
				_imagePreview.set_Text("Picture Preview");
				((Control)_saveButton).set_Visible(true);
				((Control)_saveButton).set_Enabled(true);
			});
		}

		private void ToggleHud()
		{
			if (Service.Settings.SendHideGw2Ui.get_Value())
			{
				InputHelper.DoHotKey(Service.Settings.HideGw2Ui.get_Value());
			}
			if (Service.Settings.SendHideBlishUi.get_Value())
			{
				InputHelper.DoHotKey(Service.Settings.HideBlishUi.get_Value());
			}
			if (Service.Settings.SendHideArcDpsUi.get_Value())
			{
				InputHelper.DoHotKey(Service.Settings.HideArcDpsUi.get_Value());
			}
			if (Service.Settings.SendHideNexusUi.get_Value())
			{
				InputHelper.DoHotKey(Service.Settings.HideNexusUi.get_Value());
			}
		}

		private void HandleSave(object sender, MouseEventArgs e)
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				ScreenNotification.ShowNotification("Cannot save puzzles in competitive mode maps (PvP, WvW, etc.)", (NotificationType)6, (Texture2D)null, 3);
				return;
			}
			if (Service.GeoServerWrapper == null)
			{
				ScreenNotification.ShowNotification("Service not available", (NotificationType)6, (Texture2D)null, 3);
				return;
			}
			((Control)_saveButton).set_Enabled(false);
			Task.Run(async delegate
			{
				try
				{
					Puzzle model = new Puzzle
					{
						AccountName = _003CWindowState_003EP.Account.get_Name(),
						GuildId = _003CWindowState_003EP.SelectedGuild.Id.ToString(),
						Duration = Service.Config.DefaultDurationMinutes,
						CreatedAt = DateTime.Now,
						ExpiresAt = DateTime.Now.AddMinutes(Service.Config.DefaultDurationMinutes),
						Title = ((TextInputBase)_title).get_Text(),
						Location = _mumbleCoord
					};
					await Service.GeoServerWrapper.SubmitNewPuzzle(_003CWindowState_003EP.SelectedGuild.Id.ToString(), model, _rawScreenGrab, new Action<string>(callback));
				}
				catch (Exception ex)
				{
					Logger.GetLogger<Module>().Error(ex, "Error saving puzzle");
					((Control)_saveButton).set_Enabled(true);
					ScreenNotification.ShowNotification("Error saving puzzle", (NotificationType)6, (Texture2D)null, 3);
				}
			});
			void callback(string? result)
			{
				if (result != null)
				{
					_003CWindowState_003EP.SwapToGuildList();
					ScreenNotification.ShowNotification("GeoGuess Game Saved", (NotificationType)5, (Texture2D)null, 3);
				}
				else
				{
					((Control)_saveButton).set_Enabled(true);
					ScreenNotification.ShowNotification("Error Saving GeoGuess Game, Please try again", (NotificationType)6, (Texture2D)null, 3);
				}
			}
		}

		private void BuildPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			((Control)_image).set_Size(new Point(e.get_CurrentSize().X - IMAGE_PADDING, (int)((double)(e.get_CurrentSize().X - IMAGE_PADDING) / _aspectRatio)));
		}

		private void GoBackClickHandler(object sender, MouseEventArgs e)
		{
			Service.GeoGuessWindow.State.SwapToGuildList();
		}

		protected override void Unload()
		{
			((Control)_takePicture).remove_Click((EventHandler<MouseEventArgs>)HandleTakePicture);
			((Control)_backButton).remove_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			((Control)_saveButton).remove_Click((EventHandler<MouseEventArgs>)HandleSave);
			if (_buildPanel != null)
			{
				((Control)_buildPanel).remove_Resized((EventHandler<ResizedEventArgs>)BuildPanel_Resized);
			}
			base.Unload();
		}
	}
}
