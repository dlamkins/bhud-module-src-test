using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Services;
using flakysalt.CharacterKeybinds.Views;

namespace flakysalt.CharacterKeybinds.Presenter
{
	public class MainWindowPresenter : Presenter<MainWindowView, MainWindowModel>
	{
		private Dictionary<IView, IPresenter> subPresenters;

		private CharacterKeybindsTabPresenter keybindsTabPresenter;

		private MigrationTabPresenter migrationTabPresenter;

		private readonly Gw2ApiService _apiService;

		private readonly CharacterKeybindsSettings _settingsModel;

		public MainWindowPresenter(Gw2ApiService apiService, CharacterKeybindsSettings settingsModel, MainWindowView view, MainWindowModel model)
			: base(view, model)
		{
			subPresenters = new Dictionary<IView, IPresenter>();
			_settingsModel = settingsModel;
			_apiService = apiService;
			CreateSubPresenters();
			base.get_View().TabChanged += OnTabChanged;
			base.get_View().WindowShown += OnWindowShown;
		}

		private void OnWindowShown(object sender, EventArgs e)
		{
			if (subPresenters.TryGetValue(base.get_View().SelectedTab.get_View()(), out var value))
			{
				value.DoUpdateView();
			}
		}

		public void Update(GameTime gameTime)
		{
			keybindsTabPresenter.Update(gameTime);
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			if (subPresenters.TryGetValue(e.get_NewValue().get_View()(), out var value))
			{
				value.DoUpdateView();
			}
		}

		private void CreateSubPresenters()
		{
			keybindsTabPresenter = new CharacterKeybindsTabPresenter(base.get_View().KeybindsTab, new CharacterKeybindsModel(_settingsModel, _apiService), _apiService, _settingsModel, AutoClickerView.Instance);
			migrationTabPresenter = new MigrationTabPresenter(base.get_View().MigrationTab, new MigrationTabModel(_settingsModel, _apiService));
			subPresenters.Add((IView)(object)base.get_View().KeybindsTab, (IPresenter)(object)keybindsTabPresenter);
			subPresenters.Add((IView)(object)base.get_View().MigrationTab, (IPresenter)(object)migrationTabPresenter);
		}
	}
}
