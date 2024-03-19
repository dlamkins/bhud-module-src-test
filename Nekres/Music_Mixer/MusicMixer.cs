using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Music_Mixer.Core.Services;
using Nekres.Music_Mixer.Core.Services.Audio;
using Nekres.Music_Mixer.Core.Services.Data;
using Nekres.Music_Mixer.Core.UI.Library;
using Nekres.Music_Mixer.Core.UI.Playlists;

namespace Nekres.Music_Mixer
{
	[Export(typeof(Module))]
	public class MusicMixer : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(MusicMixer));

		internal static MusicMixer Instance;

		internal SettingEntry<float> MasterVolume;

		internal SettingEntry<YtDlpService.AudioBitrate> AverageBitrate;

		internal SettingEntry<bool> MuteWhenInBackground;

		internal ResourceService Resources;

		internal YtDlpService YtDlp;

		internal AudioService Audio;

		internal DataService Data;

		internal Gw2StateService Gw2State;

		private TabbedWindow2 _moduleWindow;

		private CornerIcon _cornerIcon;

		private Texture2D _cornerTexture;

		private Texture2D _mountTabIcon;

		private Texture2D _defeatedIcon;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public string ModuleDirectory { get; private set; }

		[ImportingConstructor]
		public MusicMixer([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection audio = settings.AddSubCollection("audio", true, (Func<string>)(() => "Sound Options"));
			MasterVolume = audio.DefineSetting<float>("master_volume", 50f, (Func<string>)(() => "Master Volume"), (Func<string>)(() => "Sets the audio volume."));
			MuteWhenInBackground = audio.DefineSetting<bool>("mute_in_background", false, (Func<string>)(() => "Mute when GW2 is in the background"), (Func<string>)null);
			AverageBitrate = audio.DefineSetting<YtDlpService.AudioBitrate>("average_bitrate", YtDlpService.AudioBitrate.B320, (Func<string>)(() => "Average bitrate limit"), (Func<string>)(() => "Sets the average bitrate of the audio used in streaming."));
		}

		protected override void Initialize()
		{
			ModuleDirectory = DirectoriesManager.GetFullDirectoryPath("music_mixer");
			Resources = new ResourceService();
			YtDlp = new YtDlpService();
			Data = new DataService();
			Gw2State = new Gw2StateService();
			Audio = new AudioService();
		}

		protected override void Update(GameTime gameTime)
		{
			Gw2State.Update();
		}

		public IProgress<string> GetModuleProgressHandler()
		{
			return new Progress<string>(UpdateModuleLoading);
		}

		private void UpdateModuleLoading(string loadingMessage)
		{
			if (_cornerIcon != null)
			{
				_cornerIcon.set_LoadingMessage(loadingMessage);
			}
		}

		protected override async Task LoadAsync()
		{
			await YtDlp.Update(GetModuleProgressHandler());
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Expected O, but got Unknown
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Expected O, but got Unknown
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Expected O, but got Unknown
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			YtDlp.RemoveCache();
			Data.RemoveAudioUrls();
			MasterVolume.set_Value(MathHelper.Clamp(MasterVolume.get_Value(), 0f, 100f));
			_cornerTexture = ContentsManager.GetTexture("corner_icon.png");
			Rectangle windowRegion = default(Rectangle);
			((Rectangle)(ref windowRegion))._002Ector(40, 26, 913, 691);
			TabbedWindow2 val = new TabbedWindow2(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), windowRegion, new Rectangle(100, 36, 839, 605));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Background Music");
			((WindowBase2)val).set_Emblem(_cornerTexture);
			((WindowBase2)val).set_Subtitle("Mounted");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("MusicMixer_d42b52ce-74f1-4e6d-ae6b-a8724029f0a3");
			((Control)val).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - windowRegion.Width) / 2);
			((Control)val).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - windowRegion.Height) / 2);
			_moduleWindow = val;
			_mountTabIcon = ContentsManager.GetTexture("tabs/raptor.png");
			Tab mountTab = new Tab(AsyncTexture2D.op_Implicit(_mountTabIcon), (Func<IView>)(() => (IView)(object)new MountPlaylistsView()), "Mounted", (int?)null);
			_moduleWindow.get_Tabs().Add(mountTab);
			_defeatedIcon = ContentsManager.GetTexture("tabs/downed_enemy.png");
			Tab defeatedTab = new Tab(AsyncTexture2D.op_Implicit(_defeatedIcon), (Func<IView>)delegate
			{
				if (!Data.GetDefeatedPlaylist(out var context))
				{
					context = new Playlist
					{
						ExternalId = "Defeated",
						Tracks = new List<AudioSource>()
					};
				}
				return (IView)(object)new BgmLibraryView(context, "Defeated");
			}, "Defeated", (int?)null);
			_moduleWindow.get_Tabs().Add(defeatedTab);
			CornerIcon val2 = new CornerIcon();
			val2.set_Icon(AsyncTexture2D.op_Implicit(_cornerTexture));
			_cornerIcon = val2;
			((Control)_cornerIcon).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnModuleIconClick);
			_moduleWindow.add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Audio?.Dispose();
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			((WindowBase2)_moduleWindow).set_Subtitle(e.get_NewValue().get_Name());
		}

		public void OnModuleIconClick(object o, MouseEventArgs e)
		{
			TabbedWindow2 moduleWindow = _moduleWindow;
			if (moduleWindow != null)
			{
				((WindowBase2)moduleWindow).ToggleWindow();
			}
		}

		protected override void Unload()
		{
			if (_moduleWindow != null)
			{
				_moduleWindow.remove_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
				((Control)_moduleWindow).Dispose();
			}
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnModuleIconClick);
				((Control)_cornerIcon).Dispose();
			}
			Audio?.Dispose();
			Gw2State?.Dispose();
			Data?.Dispose();
			Resources?.Dispose();
			Texture2D cornerTexture = _cornerTexture;
			if (cornerTexture != null)
			{
				((GraphicsResource)cornerTexture).Dispose();
			}
			Texture2D defeatedIcon = _defeatedIcon;
			if (defeatedIcon != null)
			{
				((GraphicsResource)defeatedIcon).Dispose();
			}
			Texture2D mountTabIcon = _mountTabIcon;
			if (mountTabIcon != null)
			{
				((GraphicsResource)mountTabIcon).Dispose();
			}
			AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
			Instance = null;
		}
	}
}
