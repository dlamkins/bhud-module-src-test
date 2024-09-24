using System;
using System.ComponentModel;
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
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Views;

namespace flakysalt.CharacterKeybinds
{
	[Export(typeof(Module))]
	public class CharacterKeybindModule : Module
	{
		internal static CharacterKeybindModule moduleInstance;

		private static readonly Logger Logger = Logger.GetLogger<CharacterKeybindModule>();

		private Texture2D _cornerTexture;

		private CornerIcon _cornerIcon;

		public CharacterKeybindsSettings settingsModel;

		private CharacterKeybindWindow moduleWindowView;

		public TroubleshootWindow autoclickerView;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsWindow(settingsModel, moduleWindowView, autoclickerView, DirectoriesManager, Logger);
		}

		[ImportingConstructor]
		public CharacterKeybindModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			moduleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			settingsModel = new CharacterKeybindsSettings(settings);
		}

		protected override async Task LoadAsync()
		{
			_cornerTexture = ContentsManager.GetTexture("images/logo_small.png");
			moduleWindowView = new CharacterKeybindWindow(Logger);
			autoclickerView = new TroubleshootWindow();
			autoclickerView.Init(settingsModel, ContentsManager);
			await moduleWindowView.Init(ContentsManager, Gw2ApiManager, settingsModel, DirectoriesManager, autoclickerView);
			CreateCornerIconWithContextMenu();
		}

		private void CreateCornerIconWithContextMenu()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerTexture));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
			val.set_Priority(1);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Visible(settingsModel.displayCornerIcon.get_Value());
			val.set_DynamicHide(true);
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)moduleWindowView.WindowView).ToggleWindow();
			});
			((SettingEntry)settingsModel.displayCornerIcon).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				((Control)_cornerIcon).set_Visible(settingsModel.displayCornerIcon.get_Value());
			});
		}

		protected override void Update(GameTime gameTime)
		{
			moduleWindowView.Update(gameTime);
		}

		protected override void Unload()
		{
			CharacterKeybindWindow characterKeybindWindow = moduleWindowView;
			if (characterKeybindWindow != null)
			{
				StandardWindow windowView = characterKeybindWindow.WindowView;
				if (windowView != null)
				{
					((Control)windowView).Dispose();
				}
			}
			moduleWindowView?.Dispose();
			autoclickerView?.Dispose();
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Texture2D cornerTexture = _cornerTexture;
			if (cornerTexture != null)
			{
				((GraphicsResource)cornerTexture).Dispose();
			}
			moduleWindowView = null;
			autoclickerView = null;
			moduleInstance = null;
		}
	}
}
