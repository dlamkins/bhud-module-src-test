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
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Presenter;
using flakysalt.CharacterKeybinds.Views;

namespace flakysalt.CharacterKeybinds
{
	[Export(typeof(Module))]
	public class CharacterKeybindModule : Module
	{
		internal static CharacterKeybindModule moduleInstance;

		private Texture2D _cornerTexture;

		private CornerIcon _cornerIcon;

		public CharacterKeybindsSettings settingsModel;

		private CharacterKeybindsTab moduleWindowView;

		private CharacterKeybindSettingsPresenter presenter;

		public Autoclicker autoclickerView;

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsWindow(settingsModel, moduleWindowView, autoclickerView);
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
			autoclickerView = new Autoclicker();
			autoclickerView.Init(settingsModel, ContentsManager);
			LoadModuleWindow();
			CreateCornerIconWithContextMenu();
		}

		private void LoadModuleWindow()
		{
			moduleWindowView = new CharacterKeybindsTab(ContentsManager);
			CharacterKeybindModel model = new CharacterKeybindModel(settingsModel);
			presenter = new CharacterKeybindSettingsPresenter(moduleWindowView, model, Gw2ApiManager, autoclickerView);
		}

		private void CreateCornerIconWithContextMenu()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			if (settingsModel.displayCornerIcon.get_Value())
			{
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(_cornerTexture));
				((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
				val.set_Priority(1);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)val).set_Visible(true);
				_cornerIcon = val;
				((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					moduleWindowView.ToggleWindow();
				});
			}
			((SettingEntry)settingsModel.displayCornerIcon).add_PropertyChanged((PropertyChangedEventHandler)EnableOrCreateCornerIcon);
		}

		private void EnableOrCreateCornerIcon(object sender, PropertyChangedEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			if (_cornerIcon == null)
			{
				CornerIcon val = new CornerIcon();
				val.set_Icon(AsyncTexture2D.op_Implicit(_cornerTexture));
				((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
				val.set_Priority(1);
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				_cornerIcon = val;
			}
			((Control)_cornerIcon).set_Visible(settingsModel.displayCornerIcon.get_Value());
		}

		protected override void Update(GameTime gameTime)
		{
			presenter.Update(gameTime);
		}

		protected override void Unload()
		{
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
