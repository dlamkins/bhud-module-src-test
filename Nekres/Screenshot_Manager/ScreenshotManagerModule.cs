using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
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

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		public SoundEffect ScreenShotSfx { get; private set; }

		public SoundEffect DeleteSfx => _deleteSfx[RandomUtil.GetRandom(0, 1)];

		[ImportingConstructor]
		public ScreenshotManagerModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection generalOptions = settings.AddSubCollection("general", renderInUi: true, lazyLoaded: false, () => Resources.General_Options);
			HideCornerIcon = generalOptions.DefineSetting("hideCornerIcon", defaultValue: false, () => Resources.Hide_Corner_Icon, () => Resources.Disables_the_corner_icon_in_the_navigation_menu_);
			SettingCollection soundOptions = settings.AddSubCollection("audio", renderInUi: true, lazyLoaded: false, () => Resources.Sound_Options);
			MuteSound = soundOptions.DefineSetting("muteSound", defaultValue: false, () => Resources.Mute_Screenshot_Sound, () => Resources.Mutes_the_sound_alert_when_a_new_screenshot_has_been_captured_);
			DisableNotification = soundOptions.DefineSetting("disableNotification", defaultValue: false, () => Resources.Disable_Screenshot_Notification, () => Resources.Disables_the_notification_when_a_new_screenshot_has_been_captured_);
			SettingCollection screenshotOptions = settings.AddSubCollection("capture", renderInUi: true, lazyLoaded: false, () => Resources.Screenshot_Options);
			ScreenshotNormalBinding = screenshotOptions.DefineSetting("normalKey", new KeyBinding((Keys)121), () => Resources.Normal, () => Resources.Take_a_normal_screenshot_);
			Format = screenshotOptions.DefineSetting("imageFormat", ImageFormatExt.Png, () => Resources.Image_Format, () => Resources.Choose_your_preferred_image_format_);
			CopyToClipboard = screenshotOptions.DefineSetting("copyToClipboard", defaultValue: true, () => Resources.Copy_to_Clipboard, () => Resources.Copies_the_screenshot_to_the_clipboard_after_it_has_been_taken_);
			SendToRecycleBin = screenshotOptions.DefineSetting("sendToRecycleBin", defaultValue: true, () => Resources.Delete_sends_to_Recycle_Bin, () => $"{Resources.By_default__screenshots_are_sent_to_the_Recycle_Bin_so_that_they_can_be_recovered_if_needed_}\n{Resources.When_this_feature_is_disabled__deleted_screenshots_are_removed_from_the_hard_disk_and_their_space_is_marked_as_overwriteable_}");
			SettingCollection selfManagedSettings = settings.AddSubCollection("ManagedSettings", renderInUi: false, lazyLoaded: false);
			Favorites = selfManagedSettings.DefineSetting("favorites", new List<string>());
		}

		protected override void Initialize()
		{
			_fileWatcherFactory = new FileWatcherFactory();
			LoadTextures();
			_moduleTab = GameService.Overlay.BlishHudWindow.AddTab(base.Name, _icon64, () => new ScreenshotManagerView(new ScreenshotManagerModel(_fileWatcherFactory)));
			CreateOrDisposeCornerIcon(HideCornerIcon.Value);
		}

		public void CreateOrDisposeCornerIcon(bool dispose)
		{
			if (dispose)
			{
				_moduleCornerIcon?.Dispose();
				return;
			}
			_moduleCornerIcon = new CornerIcon
			{
				IconName = base.Name,
				Icon = _icon64,
				Priority = base.Name.GetHashCode()
			};
			_moduleCornerIcon.Click += ModuleCornerIconClicked;
		}

		private void OnHideCornerIconSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			CreateOrDisposeCornerIcon(e.NewValue);
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
			ScreenshotNormalBinding.Value.Enabled = true;
			HideCornerIcon.SettingChanged += OnHideCornerIconSettingChanged;
			ScreenshotNormalBinding.Value.Activated += OnScreenshotNormalBindingActivated;
			base.OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		private async void OnScreenshotNormalBindingActivated(object o, EventArgs e)
		{
			ImageFormat format;
			string ext;
			switch (Format.Value)
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
			string name = FileUtil.IndexedFilename(Path.Combine(DirectoryUtil.ScreensPath, "gw"), ext);
			if (!WindowUtil.GetInnerBounds(GameService.GameIntegration.Gw2Instance.Gw2WindowHandle, out var bounds))
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
			if (CopyToClipboard.Value)
			{
				bitmap.SaveToClipboard(format);
				ScreenNotification.ShowNotification(Resources.Copied_to_Clipboard_);
			}
		}

		protected override void Unload()
		{
			ResponsiveThumbnail.DisposeTextures();
			HideCornerIcon.SettingChanged -= OnHideCornerIconSettingChanged;
			ScreenshotNormalBinding.Value.Activated -= OnScreenshotNormalBindingActivated;
			_fileWatcherFactory.Dispose();
			if (_moduleCornerIcon != null)
			{
				_moduleCornerIcon.Click -= ModuleCornerIconClicked;
				_moduleCornerIcon.Dispose();
			}
			GameService.Overlay.BlishHudWindow.RemoveTab(_moduleTab);
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
			if (GameService.Overlay.BlishHudWindow.Visible)
			{
				GameService.Overlay.BlishHudWindow.Hide();
				return;
			}
			GameService.Overlay.BlishHudWindow.Show();
			GameService.Overlay.BlishHudWindow.Navigate(new ScreenshotManagerView(new ScreenshotManagerModel(_fileWatcherFactory)));
		}
	}
}
