using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.Screenshot_Manager.Core;
using Nekres.Screenshot_Manager.Properties;
using Nekres.Screenshot_Manager.UI.Controls;
using Nekres.Screenshot_Manager.UI.Models;
using Nekres.Screenshot_Manager.UI.Views;

namespace Nekres.Screenshot_Manager
{
	[Export(typeof(Module))]
	public class ScreenshotManagerModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(ScreenshotManagerModule));

		internal static ScreenshotManagerModule ModuleInstance;

		internal SettingEntry<KeyBinding> ScreenshotNormalBinding;

		internal SettingEntry<bool> MuteSound;

		internal SettingEntry<bool> DisableNotification;

		internal SettingEntry<bool> SendToRecycleBin;

		internal SettingEntry<bool> HideCornerIcon;

		internal SettingEntry<List<string>> Favorites;

		internal SettingEntry<ImageFormatExt> Format;

		internal SettingEntry<bool> CopyToClipboard;

		private Texture2D _icon64;

		private SoundEffect[] _deleteSfx;

		private CornerIcon _moduleCornerIcon;

		private WindowTab _moduleTab;

		private FileWatcherFactory _fileWatcherFactory;

		public const int FILE_TIME_OUT_MILLISECONDS = 10000;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public SoundEffect ScreenShotSfx { get; private set; }

		public SoundEffect DeleteSfx => _deleteSfx[RandomUtil.GetRandom(0, 1)];

		[ImportingConstructor]
		public ScreenshotManagerModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected O, but got Unknown
			SettingCollection generalOptions = settings.AddSubCollection("general", true, false, (Func<string>)(() => Resources.General_Options));
			HideCornerIcon = generalOptions.DefineSetting<bool>("hideCornerIcon", false, (Func<string>)(() => Resources.Hide_Corner_Icon), (Func<string>)(() => Resources.Disables_the_corner_icon_in_the_navigation_menu_));
			SettingCollection soundOptions = settings.AddSubCollection("audio", true, false, (Func<string>)(() => Resources.Sound_Options));
			MuteSound = soundOptions.DefineSetting<bool>("muteSound", false, (Func<string>)(() => Resources.Mute_Screenshot_Sound), (Func<string>)(() => Resources.Mutes_the_sound_alert_when_a_new_screenshot_has_been_captured_));
			DisableNotification = soundOptions.DefineSetting<bool>("disableNotification", false, (Func<string>)(() => Resources.Disable_Screenshot_Notification), (Func<string>)(() => Resources.Disables_the_notification_when_a_new_screenshot_has_been_captured_));
			SettingCollection screenshotOptions = settings.AddSubCollection("capture", true, false, (Func<string>)(() => Resources.Screenshot_Options));
			ScreenshotNormalBinding = screenshotOptions.DefineSetting<KeyBinding>("normalKey", new KeyBinding((Keys)121), (Func<string>)(() => Resources.Normal), (Func<string>)(() => Resources.Take_a_normal_screenshot_));
			Format = screenshotOptions.DefineSetting<ImageFormatExt>("imageFormat", ImageFormatExt.Png, (Func<string>)(() => Resources.Image_Format), (Func<string>)(() => Resources.Choose_your_preferred_image_format_));
			CopyToClipboard = screenshotOptions.DefineSetting<bool>("copyToClipboard", true, (Func<string>)(() => Resources.Copy_to_Clipboard), (Func<string>)(() => Resources.Copies_the_screenshot_to_the_clipboard_after_it_has_been_taken_));
			SendToRecycleBin = screenshotOptions.DefineSetting<bool>("sendToRecycleBin", true, (Func<string>)(() => Resources.Delete_sends_to_Recycle_Bin), (Func<string>)(() => $"{Resources.By_default__screenshots_are_sent_to_the_Recycle_Bin_so_that_they_can_be_recovered_if_needed_}\n{Resources.When_this_feature_is_disabled__deleted_screenshots_are_removed_from_the_hard_disk_and_their_space_is_marked_as_overwriteable_}"));
			SettingCollection selfManagedSettings = settings.AddSubCollection("ManagedSettings", false, false);
			Favorites = selfManagedSettings.DefineSetting<List<string>>("favorites", new List<string>(), (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			_fileWatcherFactory = new FileWatcherFactory();
			LoadTextures();
			_moduleTab = GameService.Overlay.get_BlishHudWindow().AddTab(((Module)this).get_Name(), AsyncTexture2D.op_Implicit(_icon64), (Func<IView>)(() => (IView)(object)new ScreenshotManagerView(new ScreenshotManagerModel(_fileWatcherFactory))), 0);
			CreateOrDisposeCornerIcon(HideCornerIcon.get_Value());
		}

		public void CreateOrDisposeCornerIcon(bool dispose)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			if (dispose)
			{
				CornerIcon moduleCornerIcon = _moduleCornerIcon;
				if (moduleCornerIcon != null)
				{
					((Control)moduleCornerIcon).Dispose();
				}
			}
			else
			{
				CornerIcon val = new CornerIcon();
				val.set_IconName(((Module)this).get_Name());
				val.set_Icon(AsyncTexture2D.op_Implicit(_icon64));
				val.set_Priority(((Module)this).get_Name().GetHashCode());
				_moduleCornerIcon = val;
				((Control)_moduleCornerIcon).add_Click((EventHandler<MouseEventArgs>)ModuleCornerIconClicked);
			}
		}

		private void OnHideCornerIconSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			CreateOrDisposeCornerIcon(e.get_NewValue());
		}

		private void LoadTextures()
		{
			ScreenShotSfx = ContentsManager.GetSound("audio\\screenshot.wav");
			_deleteSfx = (SoundEffect[])(object)new SoundEffect[2]
			{
				ContentsManager.GetSound("audio\\crumbling-paper-1.wav"),
				ContentsManager.GetSound("audio\\crumbling-paper-2.wav")
			};
			_icon64 = ContentsManager.GetTexture("screenshots_icon_64x64.png");
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			ScreenshotNormalBinding.get_Value().set_Enabled(true);
			HideCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHideCornerIconSettingChanged);
			ScreenshotNormalBinding.get_Value().add_Activated((EventHandler<EventArgs>)OnScreenshotNormalBindingActivated);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		private async void OnScreenshotNormalBindingActivated(object o, EventArgs e)
		{
			ImageFormat format;
			string ext;
			switch (Format.get_Value())
			{
			default:
				return;
			case ImageFormatExt.Jpeg:
				format = ImageFormat.Jpeg;
				ext = "jpg";
				break;
			case ImageFormatExt.Png:
				format = ImageFormat.Png;
				ext = "png";
				break;
			case ImageFormatExt.Bmp:
				format = ImageFormat.Bmp;
				ext = "bmp";
				break;
			}
			string name = FileUtil.IndexedFilename(Path.Combine(DirectoryUtil.get_ScreensPath(), "gw"), ext);
			if (!WindowUtil.GetInnerBounds(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), out var bounds))
			{
				return;
			}
			using Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, new Size(bounds.Width, bounds.Height));
			}
			await bitmap.SaveOnNetworkShare(name, format);
			if (CopyToClipboard.get_Value())
			{
				bitmap.SaveToClipboard(format);
			}
		}

		protected override void Unload()
		{
			ResponsiveThumbnail.DisposeTextures();
			HideCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnHideCornerIconSettingChanged);
			ScreenshotNormalBinding.get_Value().remove_Activated((EventHandler<EventArgs>)OnScreenshotNormalBindingActivated);
			_fileWatcherFactory.Dispose();
			if (_moduleCornerIcon != null)
			{
				((Control)_moduleCornerIcon).remove_Click((EventHandler<MouseEventArgs>)ModuleCornerIconClicked);
				((Control)_moduleCornerIcon).Dispose();
			}
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_moduleTab);
			SoundEffect[] deleteSfx = _deleteSfx;
			foreach (SoundEffect obj in deleteSfx)
			{
				if (obj != null)
				{
					obj.Dispose();
				}
			}
			SoundEffect screenShotSfx = ScreenShotSfx;
			if (screenShotSfx != null)
			{
				screenShotSfx.Dispose();
			}
			ModuleInstance = null;
		}

		private void ModuleCornerIconClicked(object o, MouseEventArgs e)
		{
			if (((Control)GameService.Overlay.get_BlishHudWindow()).get_Visible())
			{
				((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
				return;
			}
			((Control)GameService.Overlay.get_BlishHudWindow()).Show();
			GameService.Overlay.get_BlishHudWindow().Navigate((IView)(object)new ScreenshotManagerView(new ScreenshotManagerModel(_fileWatcherFactory)), true);
		}
	}
}
