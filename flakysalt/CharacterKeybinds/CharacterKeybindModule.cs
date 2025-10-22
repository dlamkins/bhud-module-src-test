using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Presenter;
using flakysalt.CharacterKeybinds.Services;
using flakysalt.CharacterKeybinds.Views;

namespace flakysalt.CharacterKeybinds
{
	[Export(typeof(Module))]
	public class CharacterKeybindModule : Module
	{
		internal static CharacterKeybindModule moduleInstance;

		private CharacterKeybindsSettings _settingsModel;

		private MainWindowView mainWindowView;

		private MainWindowPresenter mainWindowPresenter;

		private CharacterKeybindsCornerButton _cornerButtonView;

		private AutoClickerView _autoClickerView;

		private ContentService contentService;

		private Gw2ApiService apiService;

		private MainWindowModel mainWindowModel;

		private ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		private Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsWindow(_settingsModel, mainWindowView, _autoClickerView);
		}

		[ImportingConstructor]
		public CharacterKeybindModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			moduleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingsModel = new CharacterKeybindsSettings(settings);
		}

		protected override Task LoadAsync()
		{
			CreateServices();
			CreateViews();
			CreatePresenters();
			AttachEvents();
			return Task.CompletedTask;
		}

		private void CreatePresenters()
		{
			mainWindowPresenter = new MainWindowPresenter(apiService, _settingsModel, mainWindowView, new MainWindowModel());
		}

		private void CreateViews()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			_cornerButtonView = new CharacterKeybindsCornerButton(contentService, _settingsModel);
			_autoClickerView = new AutoClickerView(_settingsModel, ContentsManager);
			mainWindowView = new MainWindowView(ContentsManager, AsyncTexture2D.FromAssetId(155997), new Rectangle(24, 30, 545, 600), new Rectangle(82, 30, 467, 600));
		}

		private void CreateServices()
		{
			contentService = new ContentService(ContentsManager);
			apiService = new Gw2ApiService(Gw2ApiManager);
		}

		private void AttachEvents()
		{
			CharacterKeybindsCornerButton cornerButtonView = _cornerButtonView;
			cornerButtonView.OnCornerButtonClicked = (Action)Delegate.Combine(cornerButtonView.OnCornerButtonClicked, new Action(mainWindowView.ToggleWindow));
		}

		protected override void Update(GameTime gameTime)
		{
			mainWindowPresenter.Update(gameTime);
		}

		protected override void Unload()
		{
			CharacterKeybindsCornerButton cornerButtonView = _cornerButtonView;
			cornerButtonView.OnCornerButtonClicked = (Action)Delegate.Remove(cornerButtonView.OnCornerButtonClicked, new Action(mainWindowView.ToggleWindow));
			mainWindowView?.Dispose();
			_autoClickerView?.Dispose();
			_cornerButtonView?.Dispose();
			_cornerButtonView = null;
			mainWindowView = null;
			_autoClickerView = null;
			moduleInstance = null;
		}
	}
}
