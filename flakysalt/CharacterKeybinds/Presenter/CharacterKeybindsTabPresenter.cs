using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Services;
using flakysalt.CharacterKeybinds.Util;
using flakysalt.CharacterKeybinds.Views;
using flakysalt.CharacterKeybinds.Views.UiElements;

namespace flakysalt.CharacterKeybinds.Presenter
{
	public class CharacterKeybindsTabPresenter : Presenter<CharacterKeybindsTab, CharacterKeybindsModel>, IDisposable
	{
		private readonly Logger Logger = Logger.GetLogger<CharacterKeybindsTabPresenter>();

		private static object taskLock = new object();

		private static bool isTaskStarted;

		private double _updateCharactersRunningTime;

		private double _updateTime = 5000.0;

		private readonly Gw2ApiService _apiService;

		private CharacterKeybindsSettings _settingsModel;

		private readonly AutoClickerView _autoClicker;

		public CharacterKeybindsTabPresenter(CharacterKeybindsTab view, CharacterKeybindsModel model, Gw2ApiService apiService, CharacterKeybindsSettings settingsModel, AutoClickerView autoClickerView)
			: base(view, model)
		{
			_apiService = apiService;
			_autoClicker = autoClickerView;
			_settingsModel = settingsModel;
			AttachToGameServices();
			AttachViewHandler();
			AttachModelHandler();
			LoadCharacterInformationAsync();
			base.DoUpdateView();
		}

		public void Update(GameTime gameTime)
		{
			_updateCharactersRunningTime += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_updateCharactersRunningTime > _updateTime)
			{
				_updateCharactersRunningTime = 0.0;
				Task.Run((Func<Task>)LoadCharacterInformationAsync);
			}
		}

		private void AttachToGameServices()
		{
			try
			{
				GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnLocaleChange);
				GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
				GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "[CharacterKeybindSettingsPresenter] Failed to attach to Game Services");
			}
		}

		private void OnLocaleChange(object sender, ValueEventArgs<CultureInfo> info)
		{
			base.get_Model().ClearResources();
			Task.Run((Func<Task>)LoadCharacterInformationAsync);
			SetUpdateInterval(5000.0);
		}

		public void Dispose()
		{
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnLocaleChange);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
			CharacterKeybindsTab view = base.get_View();
			view.OnAddButtonClicked = (EventHandler)Delegate.Remove(view.OnAddButtonClicked, new EventHandler(OnAddButtonPressed));
			CharacterKeybindsTab view2 = base.get_View();
			view2.OnApplyDefaultKeymapClicked = (EventHandler<string>)Delegate.Remove(view2.OnApplyDefaultKeymapClicked, new EventHandler<string>(OnApplyDefaultKeymap));
			CharacterKeybindsTab view3 = base.get_View();
			view3.OnDefaultKeymapChanged = (EventHandler<string>)Delegate.Remove(view3.OnDefaultKeymapChanged, new EventHandler<string>(OnChangeDefaultKeymap));
		}

		private void AttachViewHandler()
		{
			try
			{
				CharacterKeybindsTab view = base.get_View();
				view.OnAddButtonClicked = (EventHandler)Delegate.Combine(view.OnAddButtonClicked, new EventHandler(OnAddButtonPressed));
				CharacterKeybindsTab view2 = base.get_View();
				view2.OnApplyDefaultKeymapClicked = (EventHandler<string>)Delegate.Combine(view2.OnApplyDefaultKeymapClicked, new EventHandler<string>(OnApplyDefaultKeymap));
				CharacterKeybindsTab view3 = base.get_View();
				view3.OnDefaultKeymapChanged = (EventHandler<string>)Delegate.Combine(view3.OnDefaultKeymapChanged, new EventHandler<string>(OnChangeDefaultKeymap));
			}
			catch (Exception e)
			{
				Logger.Error(e, "[CharacterKeybindSettingsPresenter] Failed to attach to View");
			}
		}

		private void AttachModelHandler()
		{
			try
			{
				base.get_Model().BindCharacterDataChanged(((Presenter<CharacterKeybindsTab, CharacterKeybindsModel>)this).UpdateView);
				base.get_Model().BindKeymapChanged(((Presenter<CharacterKeybindsTab, CharacterKeybindsModel>)this).UpdateView);
			}
			catch (Exception e)
			{
				Logger.Error(e, "[CharacterKeybindSettingsPresenter] Failed to attach to Model");
			}
		}

		protected override void UpdateView()
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				base.get_View()?.ClearKeybindEntries();
				string keybindsFolder = base.get_Model().GetKeybindsFolder();
				base.get_View()?.SetDefaultKeybindOptions(CharacterKeybindFileUtil.GetKeybindFiles(keybindsFolder), base.get_Model().GetDefaultKeybind());
				IEnumerable<Keymap> keymaps = base.get_Model().GetKeymaps();
				foreach (Keymap keymap in keymaps ?? Enumerable.Empty<Keymap>())
				{
					int iconAssetId = 0;
					Character character = base.get_Model().GetCharacter(keymap.CharacterName);
					if (character != null)
					{
						RenderUrl icon = base.get_Model().GetProfession(character.get_Profession()).get_Icon();
						iconAssetId = int.Parse(Path.GetFileNameWithoutExtension(((RenderUrl)(ref icon)).get_Url().AbsoluteUri));
					}
					KeybindFlowContainer container = base.get_View()?.AddKeybind();
					if (container != null)
					{
						base.get_View()?.SetKeybindOptions(container, base.get_Model().GetCharacterNames(), base.get_Model().GetProfessionSpecializations(keymap.CharacterName), CharacterKeybindFileUtil.GetKeybindFiles(keybindsFolder));
						base.get_View()?.SetKeybindValues(container, keymap, iconAssetId);
						base.get_View()?.AttachListeners(container, OnApplyKeymap, OnKeymapChange, OnKeymapRemoved);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Fatal($"UpdateView Failed: {ex}");
			}
			finally
			{
				base.get_View()?.SetErrorInfoIcon(IsDataValid(out var error), base.get_Model().IsDataLoaded, error);
			}
			base.UpdateView();
		}

		private bool IsDataValid(out string errorMessage)
		{
			List<string> errors = new List<string>();
			errorMessage = string.Empty;
			bool isValid = true;
			if (!_apiService.HasRequiredPermissions())
			{
				isValid = false;
				errors.Add("Missing API key or insufficient permissions.");
			}
			if (base.get_Model().NeedsMigration)
			{
				isValid = false;
				errors.Add("Old keybinds format found. Please go to the Migration tab to update your keybinds.");
			}
			if (!base.get_Model().KeybindsFoldersValid)
			{
				isValid = false;
				errors.Add("The selected keybinds folder is invalid. Please check your settings.");
			}
			if (!isValid)
			{
				errorMessage = $"{errors.Count} Potential issues detected:\n\n";
				errorMessage += string.Join(Environment.NewLine, errors);
			}
			return isValid;
		}

		public void OnApplyKeymap(object sender, Keymap characterKeybind)
		{
			ChangeKeybinds(characterKeybind.KeymapName, base.get_Model().GetKeybindsFolder());
		}

		public void OnApplyDefaultKeymap(object sender, string keymap)
		{
			ChangeKeybinds(keymap, base.get_Model().GetKeybindsFolder());
		}

		public void OnChangeDefaultKeymap(object sender, string keymap)
		{
			base.get_Model().SetDefaultKeymap(keymap);
		}

		public void OnAddButtonPressed(object sender, EventArgs args)
		{
			AddKeybindEntry();
		}

		public void OnKeybindTabSelected(object sender, EventArgs args)
		{
			((Presenter<CharacterKeybindsTab, CharacterKeybindsModel>)this).UpdateView();
		}

		public void OnKeymapChange(object sender, KeymapEventArgs keymapArgs)
		{
			base.get_Model().UpdateKeymap(keymapArgs.OldCharacterKeymap, keymapArgs.NewCharacterKeymap);
		}

		public void OnKeymapRemoved(object sender, Keymap characterKeybind)
		{
			base.get_Model().RemoveKeymap(characterKeybind);
		}

		public void AddKeybindEntry()
		{
			base.get_Model().AddKeymap();
		}

		public async Task SetupKeybinds(string newCharacterName = "", int specialization = -1)
		{
			_ = 1;
			try
			{
				if (!string.IsNullOrEmpty(newCharacterName))
				{
					Specialization currentSpecialization = await _apiService.GetSpecializationAsync(specialization);
					string keymap = ((!_settingsModel.useDefaultKeybinds.get_Value()) ? base.get_Model().GetKeymapName(newCharacterName, currentSpecialization)?.KeymapName : (base.get_Model().GetKeymapName(newCharacterName, currentSpecialization)?.KeymapName ?? base.get_Model().GetDefaultKeybind()));
					if (keymap != base.get_Model().CurrentKeybinds && !string.IsNullOrEmpty(keymap))
					{
						await ChangeKeybinds(keymap, base.get_Model().GetKeybindsFolder());
					}
				}
			}
			catch (Exception e)
			{
				Logger.Error($"Error Setting up keybinds\n{e}");
			}
			finally
			{
				isTaskStarted = false;
			}
		}

		private async Task ChangeKeybinds(string sourceFileName, string keybindsFolder)
		{
			base.get_Model().CurrentKeybinds = sourceFileName;
			string sourceFile = Path.Combine(keybindsFolder, "Cache", sourceFileName + ".xml");
			string destFile = Path.Combine(keybindsFolder, "CharacterKeybinds.xml");
			try
			{
				if (File.Exists(Path.Combine(keybindsFolder, sourceFileName + ".xml")))
				{
					CharacterKeybindFileUtil.MoveAllXmlFiles(keybindsFolder, Path.Combine(keybindsFolder, "Cache"));
					File.Copy(sourceFile, destFile);
					await _autoClicker.ClickInOrder();
				}
			}
			catch (Exception e)
			{
				Logger.Error($"Error copying files\n{e}");
			}
			finally
			{
				if (File.Exists(destFile))
				{
					File.Delete(destFile);
				}
				CharacterKeybindFileUtil.MoveAllXmlFiles(Path.Combine(keybindsFolder, "Cache"), keybindsFolder);
			}
		}

		private void PlayerCharacter_SpecializationChanged(object sender, ValueEventArgs<int> newSpecialization)
		{
			if (!_settingsModel.changeKeybindsWhenSwitchingSpecialization.get_Value())
			{
				return;
			}
			lock (taskLock)
			{
				if (!isTaskStarted)
				{
					isTaskStarted = true;
					Task.Run(() => SetupKeybinds(GameService.Gw2Mumble.get_PlayerCharacter().get_Name(), newSpecialization.get_Value()));
				}
			}
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> newCharacterName)
		{
			lock (taskLock)
			{
				if (!isTaskStarted)
				{
					isTaskStarted = true;
					Task.Run(() => SetupKeybinds(newCharacterName.get_Value(), GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization()));
				}
			}
		}

		private async Task LoadCharacterInformationAsync()
		{
			try
			{
				base.get_View().SetSpinner(state: true);
				await base.get_Model().LoadResourcesAsync();
				SetUpdateInterval(300000.0);
			}
			catch (Exception e)
			{
				Logger.Info($"Failed to load data from the API \n {e}");
			}
			finally
			{
				base.get_View().SetSpinner(state: false);
			}
		}

		private void SetUpdateInterval(double interval)
		{
			_updateTime = interval;
		}
	}
}
