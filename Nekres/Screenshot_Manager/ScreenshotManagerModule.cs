using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Screenshot_Manager.Core;
using Nekres.Screenshot_Manager.Properties;
using Nekres.Screenshot_Manager.UI.Models;
using Nekres.Screenshot_Manager.UI.Views;

namespace Nekres.Screenshot_Manager
{
	[Export(typeof(Module))]
	public class ScreenshotManagerModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(ScreenshotManagerModule));

		internal static ScreenshotManagerModule ModuleInstance;

		internal SettingEntry<bool> MuteSound;

		internal SettingEntry<bool> DisableNotification;

		internal SettingEntry<bool> SendToRecycleBin;

		internal SettingEntry<List<string>> Favorites;

		private Texture2D _icon64;

		private SoundEffect[] _deleteSfx;

		private CornerIcon _moduleCornerIcon;

		private WindowTab _moduleTab;

		private FileWatcherFactory _fileWatcherFactory;

		public const int FileTimeOutMilliseconds = 10000;

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
			MuteSound = settings.DefineSetting<bool>("muteSound", false, (Func<string>)(() => Resources.Mute_Screenshot_Sound), (Func<string>)(() => Resources.Mutes_the_sound_alert_when_a_new_screenshot_has_been_captured_));
			DisableNotification = settings.DefineSetting<bool>("disableNotification", false, (Func<string>)(() => Resources.Disable_Screenshot_Notification), (Func<string>)(() => Resources.Disables_the_notification_when_a_new_screenshot_has_been_captured_));
			SendToRecycleBin = settings.DefineSetting<bool>("sendToRecycleBin", true, (Func<string>)(() => Resources.Delete_sends_to_Recycle_Bin), (Func<string>)(() => Resources.By_default__screenshots_are_sent_to_the_Recycle_Bin_so_that_they_can_be_recovered_if_needed__nWhen_this_feature_is_disabled__deleted_screenshots_are_removed_from_the_hard_disk_and_their_space_is_marked_as_overwriteable_));
			SettingCollection selfManagedSettings = settings.AddSubCollection("ManagedSettings", false, false);
			Favorites = selfManagedSettings.DefineSetting<List<string>>("favorites", new List<string>(), (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			_fileWatcherFactory = new FileWatcherFactory();
			LoadTextures();
			_moduleTab = GameService.Overlay.get_BlishHudWindow().AddTab(((Module)this).get_Name(), AsyncTexture2D.op_Implicit(_icon64), (Func<IView>)(() => (IView)(object)new ScreenshotManagerView(new ScreenshotManagerModel(_fileWatcherFactory))), 0);
			CornerIcon val = new CornerIcon();
			val.set_IconName(((Module)this).get_Name());
			val.set_Icon(AsyncTexture2D.op_Implicit(_icon64));
			val.set_Priority(((Module)this).get_Name().GetHashCode());
			_moduleCornerIcon = val;
			((Control)_moduleCornerIcon).add_Click((EventHandler<MouseEventArgs>)ModuleCornerIconClicked);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView(new CustomSettingsModel(SettingsManager.get_ModuleSettings()));
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
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			_fileWatcherFactory.Dispose();
			((Control)_moduleCornerIcon).remove_Click((EventHandler<MouseEventArgs>)ModuleCornerIconClicked);
			((Control)_moduleCornerIcon).Dispose();
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
