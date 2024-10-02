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
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			_cornerIcon = ContentsManager.GetTexture("icon.png");
			_cornerIconHover = ContentsManager.GetTexture("hover_icon.png");
			_emblem = ContentsManager.GetTexture("emblem.png");
			CornerIcon val = new CornerIcon(AsyncTexture2D.op_Implicit(_cornerIcon), AsyncTexture2D.op_Implicit(_cornerIconHover), ((Module)this).get_Name());
			val.set_Priority(4861143);
			_moduleIcon = val;
			((Control)_moduleIcon).add_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
			((Module)this).OnModuleLoaded(e);
		}

		public void OnModuleIconClick(object o, MouseEventArgs e)
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
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
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
				((WindowBase2)val).set_Id("MumbleInfoModule_MainWindow_aeabf2ad8a954af6a0d9c4b95f9682fe");
				((Control)val).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - windowRegion.Width) / 2);
				((Control)val).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - windowRegion.Height) / 2);
				_moduleWindow = val;
			}
			_moduleWindow.ToggleWindow((IView)(object)new MumbleView());
		}

		protected override void Unload()
		{
			((Control)_moduleIcon).remove_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
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
