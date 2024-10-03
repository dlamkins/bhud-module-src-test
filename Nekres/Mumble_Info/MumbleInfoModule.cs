using System;
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
using Microsoft.Xna.Framework.Input;
using Nekres.Mumble_Info.Core.Services;
using Nekres.Mumble_Info.Core.UI;

namespace Nekres.Mumble_Info
{
	[Export(typeof(Module))]
	public class MumbleInfoModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(MumbleInfoModule));

		internal static MumbleInfoModule Instance;

		private Texture2D _cornerIcon;

		private Texture2D _cornerIconHover;

		private Texture2D _emblem;

		private CornerIcon _moduleIcon;

		private StandardWindow _moduleWindow;

		internal SettingEntry<MumbleConfig> MumbleConfig;

		internal ApiService Api;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public MumbleInfoModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			MumbleConfig = settings.DefineSetting<MumbleConfig>("mumble_config", new MumbleConfig(), (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			Api = new ApiService();
		}

		protected override async Task LoadAsync()
		{
			await Api.Init();
		}

		protected override void Update(GameTime gameTime)
		{
			Api.Update(gameTime);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0026: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			MumbleConfig value = MumbleConfig.get_Value();
			if (value.Shortcut == null)
			{
				KeyBinding val = new KeyBinding((Keys)187);
				KeyBinding val2 = val;
				value.Shortcut = val;
			}
			MumbleConfig.get_Value().Shortcut.set_Enabled(true);
			MumbleConfig.get_Value().Shortcut.set_IgnoreWhenInTextField(true);
			_cornerIcon = ContentsManager.GetTexture("icon.png");
			_cornerIconHover = ContentsManager.GetTexture("hover_icon.png");
			_emblem = ContentsManager.GetTexture("emblem.png");
			CornerIcon val3 = new CornerIcon(AsyncTexture2D.op_Implicit(_cornerIcon), AsyncTexture2D.op_Implicit(_cornerIconHover), ((Module)this).get_Name());
			val3.set_Priority(4861143);
			_moduleIcon = val3;
			((Control)_moduleIcon).add_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
			MumbleConfig.get_Value().Shortcut.add_Activated((EventHandler<EventArgs>)OnShortcutActivated);
			MumbleConfig.add_SettingChanged((EventHandler<ValueChangedEventArgs<MumbleConfig>>)OnMumbleConfigChanged);
			MumbleConfig.get_Value().Shortcut.add_BindingChanged((EventHandler<EventArgs>)OnShortcutBindingChanged);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnShortcutBindingChanged(object sender, EventArgs e)
		{
			if (_moduleWindow != null)
			{
				((WindowBase2)_moduleWindow).set_Subtitle("[" + MumbleConfig.get_Value().Shortcut.GetBindingDisplayText() + "]");
			}
		}

		private void OnMumbleConfigChanged(object sender, ValueChangedEventArgs<MumbleConfig> e)
		{
			if (e.get_NewValue()?.Shortcut != null)
			{
				e.get_NewValue().Shortcut.remove_Activated((EventHandler<EventArgs>)OnShortcutActivated);
				e.get_NewValue().Shortcut.remove_BindingChanged((EventHandler<EventArgs>)OnShortcutBindingChanged);
				e.get_NewValue().Shortcut.add_Activated((EventHandler<EventArgs>)OnShortcutActivated);
				e.get_NewValue().Shortcut.add_BindingChanged((EventHandler<EventArgs>)OnShortcutBindingChanged);
			}
		}

		private void OnShortcutActivated(object sender, EventArgs e)
		{
			ToggleWindow();
		}

		public void OnModuleIconClick(object o, MouseEventArgs e)
		{
			ToggleWindow();
		}

		private void CreateWindow()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			if (_moduleWindow == null)
			{
				Rectangle windowRegion = default(Rectangle);
				((Rectangle)(ref windowRegion))._002Ector(40, 26, 913, 691);
				StandardWindow val = new StandardWindow(GameService.Content.GetTexture("controls/window/502049"), windowRegion, new Rectangle(52, 36, 887, 605));
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((WindowBase2)val).set_Emblem(_emblem);
				((Control)val).set_Size(new Point(435, 900));
				((WindowBase2)val).set_CanResize(true);
				((WindowBase2)val).set_SavesPosition(true);
				((WindowBase2)val).set_SavesSize(true);
				((WindowBase2)val).set_Title(((Module)this).get_Name());
				((WindowBase2)val).set_Subtitle("[" + MumbleConfig.get_Value().Shortcut.GetBindingDisplayText() + "]");
				((WindowBase2)val).set_Id("MumbleInfoModule_MainWindow_aeabf2ad8a954af6a0d9c4b95f9682fe");
				((Control)val).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - windowRegion.Width) / 2);
				((Control)val).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - windowRegion.Height) / 2);
				_moduleWindow = val;
			}
		}

		public void ToggleWindow()
		{
			CreateWindow();
			_moduleWindow.ToggleWindow((IView)(object)new MumbleView());
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new CustomSettingsView();
		}

		protected override void Unload()
		{
			MumbleConfig.get_Value().Shortcut.remove_Activated((EventHandler<EventArgs>)OnShortcutActivated);
			MumbleConfig.get_Value().Shortcut.remove_BindingChanged((EventHandler<EventArgs>)OnShortcutBindingChanged);
			((Control)_moduleIcon).remove_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
			MumbleConfig.remove_SettingChanged((EventHandler<ValueChangedEventArgs<MumbleConfig>>)OnMumbleConfigChanged);
			CornerIcon moduleIcon = _moduleIcon;
			if (moduleIcon != null)
			{
				((Control)moduleIcon).Dispose();
			}
			StandardWindow moduleWindow = _moduleWindow;
			if (moduleWindow != null)
			{
				((Control)moduleWindow).Dispose();
			}
			Texture2D cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((GraphicsResource)cornerIcon).Dispose();
			}
			Texture2D cornerIconHover = _cornerIconHover;
			if (cornerIconHover != null)
			{
				((GraphicsResource)cornerIconHover).Dispose();
			}
			Texture2D emblem = _emblem;
			if (emblem != null)
			{
				((GraphicsResource)emblem).Dispose();
			}
			Api?.Dispose();
			Instance = null;
		}
	}
}
