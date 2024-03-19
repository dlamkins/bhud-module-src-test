using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
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
using Nekres.ChatMacros.Core.Services;
using Nekres.ChatMacros.Core.UI.Configs;
using Nekres.ChatMacros.Core.UI.Library;
using Nekres.ChatMacros.Core.UI.Settings;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros
{
	[Export(typeof(Module))]
	public class ChatMacros : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<ChatMacros>();

		internal static ChatMacros Instance;

		private TabbedWindow2 _moduleWindow;

		private CornerIcon _cornerIcon;

		private Texture2D _cornerTexture;

		internal SettingEntry<InputConfig> InputConfig;

		internal SettingEntry<LibraryConfig> LibraryConfig;

		internal SettingEntry<ControlsConfig> ControlsConfig;

		internal Gw2WebApiService Gw2Api;

		internal ResourceService Resources;

		internal DataService Data;

		internal MacroService Macro;

		internal SpeechService Speech;

		private Tab _libraryTab;

		private Tab _settingsTab;

		private double _lastRun;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public string ModuleDirectory { get; private set; }

		public IReadOnlyList<string> BasePaths { get; private set; }

		[ImportingConstructor]
		public ChatMacros([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection selfManaged = settings.AddSubCollection("configs", false, false);
			InputConfig = selfManaged.DefineSetting<InputConfig>("input_config", Nekres.ChatMacros.Core.UI.Configs.InputConfig.Default, (Func<string>)null, (Func<string>)null);
			LibraryConfig = selfManaged.DefineSetting<LibraryConfig>("library_config", Nekres.ChatMacros.Core.UI.Configs.LibraryConfig.Default, (Func<string>)null, (Func<string>)null);
			ControlsConfig = selfManaged.DefineSetting<ControlsConfig>("controls_config", Nekres.ChatMacros.Core.UI.Configs.ControlsConfig.Default, (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			ModuleDirectory = DirectoriesManager.GetFullDirectoryPath("chat_shorts");
			_cornerTexture = ContentsManager.GetTexture("corner_icon.png");
			BasePaths = new List<string>
			{
				ModuleDirectory,
				AppDomain.CurrentDomain.BaseDirectory
			};
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			ValidateSettings();
			Data = new DataService();
			Gw2Api = new Gw2WebApiService();
			Resources = new ResourceService();
			Macro = new MacroService();
			Speech = new SpeechService();
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerTexture));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
			((Module)this).OnModuleLoaded(e);
		}

		private void ValidateSettings()
		{
			SettingEntry<ControlsConfig> controlsConfig = ControlsConfig;
			ControlsConfig @default;
			if (controlsConfig.get_Value() == null)
			{
				controlsConfig.set_Value(@default = Nekres.ChatMacros.Core.UI.Configs.ControlsConfig.Default);
			}
			SettingEntry<InputConfig> inputConfig = InputConfig;
			InputConfig default2;
			if (inputConfig.get_Value() == null)
			{
				inputConfig.set_Value(default2 = Nekres.ChatMacros.Core.UI.Configs.InputConfig.Default);
			}
			@default = ControlsConfig.get_Value();
			if (@default.OpenQuickAccess == null)
			{
				KeyBinding val = (@default.OpenQuickAccess = Nekres.ChatMacros.Core.UI.Configs.ControlsConfig.Default.OpenQuickAccess);
			}
			@default = ControlsConfig.get_Value();
			if (@default.ChatMessage == null)
			{
				KeyBinding val = (@default.ChatMessage = Nekres.ChatMacros.Core.UI.Configs.ControlsConfig.Default.ChatMessage);
			}
			@default = ControlsConfig.get_Value();
			if (@default.SquadBroadcastMessage == null)
			{
				KeyBinding val = (@default.SquadBroadcastMessage = Nekres.ChatMacros.Core.UI.Configs.ControlsConfig.Default.SquadBroadcastMessage);
			}
			default2 = InputConfig.get_Value();
			if (default2.PushToTalk == null)
			{
				KeyBinding val = (default2.PushToTalk = Nekres.ChatMacros.Core.UI.Configs.InputConfig.Default.PushToTalk);
			}
			ControlsConfig.get_Value().OpenQuickAccess.set_Enabled(true);
			ControlsConfig.get_Value().OpenQuickAccess.set_IgnoreWhenInTextField(true);
			InputConfig.get_Value().PushToTalk.set_Enabled(true);
			InputConfig.get_Value().PushToTalk.set_IgnoreWhenInTextField(true);
			ControlsConfig.get_Value().ChatMessage.set_Enabled(false);
			ControlsConfig.get_Value().SquadBroadcastMessage.set_Enabled(false);
		}

		private void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			_libraryTab.set_Name(Nekres.ChatMacros.Properties.Resources.Library);
			_settingsTab.set_Name(Nekres.ChatMacros.Properties.Resources.Settings);
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			((WindowBase2)_moduleWindow).set_Subtitle(e.get_NewValue().get_Name());
		}

		public void OnModuleIconClick(object o, MouseEventArgs e)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected O, but got Unknown
			if (_moduleWindow == null)
			{
				Rectangle windowRegion = default(Rectangle);
				((Rectangle)(ref windowRegion))._002Ector(40, 26, 913, 691);
				TabbedWindow2 val = new TabbedWindow2(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), windowRegion, new Rectangle(100, 36, 839, 605));
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val).set_Emblem(_cornerTexture);
				((WindowBase2)val).set_SavesPosition(true);
				((WindowBase2)val).set_SavesSize(true);
				((WindowBase2)val).set_Title(((Module)this).get_Name());
				((WindowBase2)val).set_Id("ChatMacros_42d3a11e-ffa7-4c82-8fd9-ee9d9a118914");
				((Control)val).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - windowRegion.Width) / 2);
				((Control)val).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - windowRegion.Height) / 2);
				_moduleWindow = val;
				_libraryTab = new Tab(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155156), (Func<IView>)(() => (IView)(object)new LibraryView(LibraryConfig.get_Value())), Nekres.ChatMacros.Properties.Resources.Library, (int?)null);
				_settingsTab = new Tab(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155052), (Func<IView>)(() => (IView)(object)new SettingsView()), Nekres.ChatMacros.Properties.Resources.Settings, (int?)null);
				_moduleWindow.get_Tabs().Add(_libraryTab);
				_moduleWindow.get_Tabs().Add(_settingsTab);
				_moduleWindow.add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
				GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			}
			((WindowBase2)_moduleWindow).ToggleWindow();
		}

		protected override void Update(GameTime gameTime)
		{
			Speech?.Update(gameTime);
			if (!(gameTime.get_TotalGameTime().TotalMilliseconds - _lastRun < 10.0))
			{
				_lastRun = gameTime.get_ElapsedGameTime().TotalMilliseconds;
				Macro?.Update(gameTime);
			}
		}

		protected override void Unload()
		{
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
				((Control)_cornerIcon).Dispose();
			}
			TabbedWindow2 moduleWindow = _moduleWindow;
			if (moduleWindow != null)
			{
				((Control)moduleWindow).Dispose();
			}
			Texture2D cornerTexture = _cornerTexture;
			if (cornerTexture != null)
			{
				((GraphicsResource)cornerTexture).Dispose();
			}
			Speech?.Dispose();
			Macro?.Dispose();
			Resources?.Dispose();
			Gw2Api?.Dispose();
			Data?.Dispose();
			Instance = null;
		}
	}
}
