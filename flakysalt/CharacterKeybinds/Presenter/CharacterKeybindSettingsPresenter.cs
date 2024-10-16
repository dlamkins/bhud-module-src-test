using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Util;
using flakysalt.CharacterKeybinds.Views;
using flakysalt.CharacterKeybinds.Views.UiElements;

namespace flakysalt.CharacterKeybinds.Presenter
{
	public class CharacterKeybindSettingsPresenter : Presenter<CharacterKeybindsTab, CharacterKeybindModel>, IDisposable
	{
		private readonly Logger Logger = Logger.GetLogger<CharacterKeybindSettingsPresenter>();

		private static object taskLock = new object();

		private static bool isTaskStarted;

		private double _updateCharactersRunningTime;

		private double updateTime = 5000.0;

		private Gw2ApiManager _Gw2ApiManager;

		private Autoclicker _autoClicker;

		public CharacterKeybindSettingsPresenter(CharacterKeybindsTab view, CharacterKeybindModel model, Gw2ApiManager apiManager, Autoclicker autoclicker)
			: base(view, model)
		{
			_Gw2ApiManager = apiManager;
			_autoClicker = autoclicker;
			AttachToGameServices();
			AttachViewHandler();
			AttachModelHandler();
			LoadCharacterInformationAsync();
		}

		public void Update(GameTime gameTime)
		{
			GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			_updateCharactersRunningTime += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_updateCharactersRunningTime > updateTime)
			{
				_updateCharactersRunningTime = 0.0;
				Task.Run((Func<Task>)LoadCharacterInformationAsync);
			}
		}

		private void AttachToGameServices()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
		}

		public void Dispose()
		{
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
			CharacterKeybindsTab view = base.get_View();
			view.OnAddButtonClicked = (EventHandler)Delegate.Combine(view.OnAddButtonClicked, new EventHandler(OnAddButtonPressed));
			CharacterKeybindsTab view2 = base.get_View();
			view2.OnApplyDefaultKeymapClicked = (EventHandler<string>)Delegate.Combine(view2.OnApplyDefaultKeymapClicked, new EventHandler<string>(OnApplyDefaultKeymap));
			CharacterKeybindsTab view3 = base.get_View();
			view3.OnDefaultKeymapChanged = (EventHandler<string>)Delegate.Combine(view3.OnDefaultKeymapChanged, new EventHandler<string>(OnChangeDefaultKeymap));
		}

		private void AttachModelHandler()
		{
			base.get_Model().BindCharacterDataChanged(OnKeymapsChanged);
			base.get_Model().BindKeymapChanged(OnKeymapsChanged);
		}

		private void OnKeymapsChanged()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			base.get_View().ClearKeybindEntries();
			base.get_View().SetDefaultKeybindOptions(CharacterKeybindFileUtil.GetKeybindFiles(base.get_Model().GetKeybindsFolder()), base.get_Model().GetDefaultKeybind());
			foreach (CharacterKeybind keymap in base.get_Model().GetKeymaps())
			{
				int iconAssetId = 0;
				Character character = base.get_Model().GetCharacter(keymap.characterName);
				if (character != null)
				{
					RenderUrl icon = base.get_Model().GetProfession(character.get_Profession()).get_Icon();
					iconAssetId = int.Parse(Path.GetFileNameWithoutExtension(((RenderUrl)(ref icon)).get_Url().AbsoluteUri));
				}
				KeybindFlowContainer container = base.get_View().AddKeybind();
				base.get_View().SetKeybindOptions(container, base.get_Model().GetCharacterNames(), base.get_Model().GetProfessionSpecializations(keymap.characterName), CharacterKeybindFileUtil.GetKeybindFiles(base.get_Model().GetKeybindsFolder()));
				base.get_View().SetKeybindValues(container, keymap, iconAssetId);
				base.get_View().AttachListeners(container, OnApplyKeymap, OnKeymapChange, OnKeymapRemoved);
			}
		}

		public void OnApplyKeymap(object sender, CharacterKeybind characterKeybind)
		{
			ChangeKeybinds(characterKeybind.keymap, base.get_Model().GetKeybindsFolder());
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

		public void OnKeymapChange(object sender, KeymapEventArgs keymapArgs)
		{
			base.get_Model().UpdateKeymap(keymapArgs.OldCharacterKeybind, keymapArgs.NewCharacterKeybind);
		}

		public void OnKeymapRemoved(object sender, CharacterKeybind characterKeybind)
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
					Specialization currentSpecialization = await ((IBulkExpandableClient<Specialization, int>)(object)_Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).GetAsync(specialization, default(CancellationToken));
					string keymap = base.get_Model().GetKeymapName(newCharacterName, currentSpecialization)?.keymap ?? base.get_Model().GetDefaultKeybind();
					if (keymap != base.get_Model().currentKeybinds)
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
			base.get_Model().currentKeybinds = sourceFileName;
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
			List<TokenPermission> apiKeyPermissions = new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)3
			};
			base.get_View().SetBlocker(!_Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)apiKeyPermissions));
			try
			{
				await LoadResources();
				CharacterKeybindModel model = base.get_Model();
				model.SetCharacters((IReadOnlyCollection<Character>)(await ((IAllExpandableClient<Character>)(object)_Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken))));
				OnKeymapsChanged();
			}
			catch (Exception e)
			{
				Logger.Info($"Failed to load data from the API \n {e}");
			}
		}

		private async Task LoadResources()
		{
			new List<Specialization>();
			new List<Profession>();
			IEnumerable<Profession> professions = (IEnumerable<Profession>)(await ((IAllExpandableClient<Profession>)(object)_Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).AllAsync(default(CancellationToken)));
			foreach (Specialization specialization in (IEnumerable<Specialization>)(await ((IAllExpandableClient<Specialization>)(object)_Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).AllAsync(default(CancellationToken))))
			{
				if (specialization.get_Elite())
				{
					Profession profesion = professions.First((Profession p) => p.get_Id() == specialization.get_Profession());
					base.get_Model().AddProfessionEliteSpecialization(profesion, specialization);
				}
			}
		}
	}
}
