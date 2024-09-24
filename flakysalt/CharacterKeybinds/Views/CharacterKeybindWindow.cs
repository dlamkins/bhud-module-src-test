using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Views.UiElements;

namespace flakysalt.CharacterKeybinds.Views
{
	public class CharacterKeybindWindow : View, IDisposable
	{
		private readonly Logger Logger;

		private Gw2ApiManager Gw2ApiManager;

		private CharacterKeybindsSettings model;

		private DirectoriesManager directoriesManager;

		private TroubleshootWindow troubleshootWindow;

		public StandardWindow WindowView;

		private StandardButton addEntryButton;

		private FlowPanel scrollView;

		private FlowPanel mainFlowPanel;

		private Label blockerOverlay;

		private Dictionary<string, List<Specialization>> professionSpezialisations = new Dictionary<string, List<Specialization>>();

		private List<KeybindFlowContainer> keybindUIData = new List<KeybindFlowContainer>();

		private IEnumerable<Profession> professionsResponse = new List<Profession>();

		private IEnumerable<Character> characterResponse = new List<Character>();

		private double _updateCharactersRunningTime;

		private double updateTime = 5000.0;

		private static object taskLock = new object();

		private static bool isTaskStarted = false;

		public CharacterKeybindWindow(Logger Logger)
			: this()
		{
			this.Logger = Logger;
		}

		public void Dispose()
		{
			((Control)addEntryButton).remove_Click((EventHandler<MouseEventArgs>)OnAddKeybindClick);
			((Control)WindowView).remove_Hidden((EventHandler<EventArgs>)AssignmentView_Hidden);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
			((View<IPresenter>)this).Unload();
		}

		public async Task Init(ContentsManager ContentsManager, Gw2ApiManager Gw2ApiManager, CharacterKeybindsSettings model, DirectoriesManager directoriesManager, TroubleshootWindow autoclickView)
		{
			this.model = model;
			this.Gw2ApiManager = Gw2ApiManager;
			this.directoriesManager = directoriesManager;
			troubleshootWindow = autoclickView;
			AsyncTexture2D windowBackgroundTexture = AsyncTexture2D.FromAssetId(155997);
			Texture2D _emblem = ContentsManager.GetTexture("images/logo.png");
			CharacterKeybindWindow characterKeybindWindow = this;
			StandardWindow val = new StandardWindow(windowBackgroundTexture, new Rectangle(25, 26, 600, 600), new Rectangle(40, 50, 580, 550));
			((WindowBase2)val).set_Emblem(_emblem);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Character Keybinds");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("flakysalt_CharacterKeybinds");
			((WindowBase2)val).set_CanClose(true);
			characterKeybindWindow.WindowView = val;
			CharacterKeybindWindow characterKeybindWindow2 = this;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)WindowView);
			((Control)val2).set_ZIndex(4);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Size(((Control)WindowView).get_Size());
			((Control)val2).set_Visible(false);
			val2.set_Text("");
			((Control)val2).set_BackgroundColor(Color.get_Black());
			characterKeybindWindow2.blockerOverlay = val2;
			CharacterKeybindWindow characterKeybindWindow3 = this;
			FlowPanel val3 = new FlowPanel();
			Rectangle contentRegion = ((Container)WindowView).get_ContentRegion();
			((Control)val3).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(5f, 2f));
			val3.set_OuterControlPadding(new Vector2(0f, 15f));
			((Control)val3).set_Parent((Container)(object)WindowView);
			characterKeybindWindow3.mainFlowPanel = val3;
			CharacterKeybindWindow characterKeybindWindow4 = this;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Add Binding (Loading Characters...)");
			((Control)val4).set_Parent((Container)(object)mainFlowPanel);
			((Control)val4).set_Width(((Control)mainFlowPanel).get_Width() - 20);
			((Control)val4).set_Enabled(false);
			characterKeybindWindow4.addEntryButton = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Size(((Control)mainFlowPanel).get_Size());
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Control)val5).set_Parent((Container)(object)mainFlowPanel);
			FlowPanel ScrollViewPanel = val5;
			CharacterKeybindWindow characterKeybindWindow5 = this;
			FlowPanel val6 = new FlowPanel();
			((Panel)val6).set_CanScroll(true);
			((Panel)val6).set_ShowBorder(true);
			((Control)val6).set_Size(new Point(((Control)ScrollViewPanel).get_Size().X - 20, ((Control)ScrollViewPanel).get_Height()));
			val6.set_FlowDirection((ControlFlowDirection)3);
			((Control)val6).set_Parent((Container)(object)ScrollViewPanel);
			characterKeybindWindow5.scrollView = val6;
			((Control)new Scrollbar((Container)(object)scrollView)).set_Height(((Control)ScrollViewPanel).get_Height());
			FlowPanel val7 = new FlowPanel();
			val7.set_FlowDirection((ControlFlowDirection)0);
			((Container)val7).set_WidthSizingMode((SizingMode)1);
			((Container)val7).set_HeightSizingMode((SizingMode)1);
			((Control)val7).set_Parent((Container)(object)mainFlowPanel);
			LoadMappingFromSettings();
			await LoadResources();
			((Control)addEntryButton).add_Click((EventHandler<MouseEventArgs>)OnAddKeybindClick);
			((Control)WindowView).add_Hidden((EventHandler<EventArgs>)AssignmentView_Hidden);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
		}

		private void PlayerCharacter_SpecializationChanged(object sender, ValueEventArgs<int> newSpezialisation)
		{
			lock (taskLock)
			{
				if (model.changeKeybindsWhenSwitchingSpecialization.get_Value() && !isTaskStarted)
				{
					isTaskStarted = true;
					Task.Run(() => SetupKeybinds(GameService.Gw2Mumble.get_PlayerCharacter().get_Name(), newSpezialisation.get_Value()));
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

		public async Task SetupKeybinds(string newCharacterName = "", int spezialisation = -1)
		{
			_ = 1;
			try
			{
				if (string.IsNullOrEmpty(newCharacterName))
				{
					return;
				}
				Specialization currentSpezialisation = await ((IBulkExpandableClient<Specialization, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).GetAsync(spezialisation, default(CancellationToken));
				KeybindFlowContainer selectedCharacterData = null;
				foreach (KeybindFlowContainer keybindData2 in keybindUIData)
				{
					if (keybindData2.characterNameDropdown.get_SelectedItem() == newCharacterName)
					{
						if (!currentSpezialisation.get_Elite() && keybindData2.specializationDropdown.get_SelectedItem() == "Core")
						{
							selectedCharacterData = keybindData2;
						}
						if (keybindData2.specializationDropdown.get_SelectedItem() == currentSpezialisation.get_Name())
						{
							selectedCharacterData = keybindData2;
						}
					}
				}
				if (selectedCharacterData == null)
				{
					foreach (KeybindFlowContainer keybindData in keybindUIData)
					{
						if (keybindData.characterNameDropdown.get_SelectedItem() == newCharacterName && keybindData.specializationDropdown.get_SelectedItem() == "All Spezialisations")
						{
							selectedCharacterData = keybindData;
						}
					}
				}
				if (selectedCharacterData != null && !(selectedCharacterData.keymapDropdown.get_SelectedItem() == "None"))
				{
					await ChangeKeybinds(selectedCharacterData.keymapDropdown.get_SelectedItem());
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

		private async Task ChangeKeybinds(string sourceFileName)
		{
			string sourceFile = Path.Combine(model.gw2KeybindsFolder.get_Value(), "Cache", sourceFileName + ".xml");
			string destFile = Path.Combine(model.gw2KeybindsFolder.get_Value(), "CharacterKeybinds.xml");
			try
			{
				if (File.Exists(Path.Combine(model.gw2KeybindsFolder.get_Value(), sourceFileName + ".xml")))
				{
					MoveAllXmlFiles(model.gw2KeybindsFolder.get_Value(), Path.Combine(model.gw2KeybindsFolder.get_Value(), "Cache"));
					File.Copy(sourceFile, destFile);
					await troubleshootWindow.ClickInOrder();
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
				MoveAllXmlFiles(Path.Combine(model.gw2KeybindsFolder.get_Value(), "Cache"), model.gw2KeybindsFolder.get_Value());
			}
		}

		private void MoveAllXmlFiles(string sourcePath, string destinationPath)
		{
			string[] files = Directory.GetFiles(sourcePath, "*.xml");
			if (!Directory.Exists(destinationPath))
			{
				Directory.CreateDirectory(destinationPath);
			}
			string[] array = files;
			foreach (string obj in array)
			{
				string fileName = Path.GetFileName(obj);
				string destPath = Path.Combine(destinationPath, fileName);
				File.Move(obj, destPath);
			}
		}

		private void OpenClickerOptions_Click(object sender, MouseEventArgs e)
		{
			((Control)troubleshootWindow.WindowView).Show();
		}

		public void Update(GameTime gameTime)
		{
			GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
			_updateCharactersRunningTime += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (_updateCharactersRunningTime > updateTime)
			{
				_updateCharactersRunningTime = 0.0;
				Task.Run((Func<Task>)LoadCharacters);
			}
		}

		private void AssignmentView_Hidden(object sender, EventArgs e)
		{
			List<CharacterKeybind> characterSpecializations = new List<CharacterKeybind>();
			foreach (KeybindFlowContainer keybindData in keybindUIData)
			{
				CharacterKeybind keybind = new CharacterKeybind
				{
					characterName = keybindData.characterNameDropdown.get_SelectedItem(),
					spezialisation = keybindData.specializationDropdown.get_SelectedItem(),
					keymap = keybindData.keymapDropdown.get_SelectedItem()
				};
				characterSpecializations.Add(keybind);
			}
			model.characterKeybinds.set_Value(characterSpecializations);
		}

		private async Task LoadResources()
		{
			IEnumerable<Specialization> specializations = new List<Specialization>();
			try
			{
				professionsResponse = (IEnumerable<Profession>)(await ((IAllExpandableClient<Profession>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).AllAsync(default(CancellationToken)));
				specializations = (IEnumerable<Specialization>)(await ((IAllExpandableClient<Specialization>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).AllAsync(default(CancellationToken)));
			}
			catch (Exception e)
			{
				Logger.Info($"Failed to get spezializations from api.\n Exception {e}");
			}
			foreach (Specialization specialization in specializations)
			{
				if (specialization.get_Elite())
				{
					if (professionSpezialisations.ContainsKey(specialization.get_Profession()))
					{
						professionSpezialisations[specialization.get_Profession()].Add(specialization);
						continue;
					}
					professionSpezialisations[specialization.get_Profession()] = new List<Specialization> { specialization };
				}
			}
		}

		private async Task LoadCharacters()
		{
			List<TokenPermission> apiKeyPermissions = new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)3
			};
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)apiKeyPermissions))
			{
				blockerOverlay.set_Text("API token missing or not available yet.\n\nMake sure you have added an API token to Blish HUD \nand it has the neccessary permissions!\n(Previously setup keybinds will still work!)");
				((Control)blockerOverlay).set_Visible(true);
				return;
			}
			try
			{
				characterResponse = (IEnumerable<Character>)(await ((IAllExpandableClient<Character>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken)));
				((Control)addEntryButton).set_Enabled(true);
				((Control)blockerOverlay).set_Visible(false);
				addEntryButton.set_Text("Add Binding");
				foreach (KeybindFlowContainer binding in keybindUIData)
				{
					UpdateKeybind(binding);
				}
			}
			catch (Exception e)
			{
				Logger.Info($"Failed to get character names from api.\n {e}");
			}
		}

		private void LoadMappingFromSettings()
		{
			foreach (CharacterKeybind binding in model.characterKeybinds.get_Value())
			{
				AddKeybind(binding.characterName, binding.spezialisation, binding.keymap);
			}
		}

		private void OnAddKeybindClick(object sender, MouseEventArgs e)
		{
			if (Directory.Exists(model.gw2KeybindsFolder.get_Value()))
			{
				KeybindFlowContainer uielement = AddKeybind();
				UpdateKeybind(uielement);
			}
		}

		private void UpdateKeybind(KeybindFlowContainer keybindFlowContainer)
		{
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			string[] xmlFiles = Directory.GetFiles(model.gw2KeybindsFolder.get_Value(), "*.xml");
			for (int i = 0; i < xmlFiles.Length; i++)
			{
				xmlFiles[i] = Path.GetFileNameWithoutExtension(xmlFiles[i]);
			}
			keybindFlowContainer.SetKeymapOptions(xmlFiles.ToList());
			keybindFlowContainer.SetNameOptions(characterResponse.Select((Character character) => character.get_Name()).ToList());
			Character currentCharacter = (characterResponse as List<Character>).Find((Character item) => keybindFlowContainer.characterNameDropdown.get_SelectedItem() == item.get_Name());
			if (currentCharacter == null)
			{
				return;
			}
			foreach (Profession profession2 in professionsResponse)
			{
				if (currentCharacter.get_Profession() == profession2.get_Name())
				{
					RenderUrl icon = profession2.get_Icon();
					int iconAssetId = int.Parse(Path.GetFileNameWithoutExtension(((RenderUrl)(ref icon)).get_Url().AbsoluteUri));
					keybindFlowContainer.professionImage.set_Texture(AsyncTexture2D.FromAssetId(iconAssetId));
				}
			}
			foreach (KeyValuePair<string, List<Specialization>> profession in professionSpezialisations)
			{
				if (currentCharacter.get_Profession() == profession.Key)
				{
					List<string> specializationNames = new List<string> { "All Spezialisations", "Core" };
					specializationNames.AddRange(profession.Value.Select((Specialization specialization) => specialization.get_Name()));
					keybindFlowContainer.SetSpecializationOptions(specializationNames);
				}
			}
		}

		private KeybindFlowContainer AddKeybind(string selectedName = "", string selectedSpezialisations = "", string selectedKeymap = "")
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			KeybindFlowContainer keybindFlowContainer2 = new KeybindFlowContainer(selectedName, selectedSpezialisations, selectedKeymap);
			((Control)keybindFlowContainer2).set_Parent((Container)(object)scrollView);
			((Control)keybindFlowContainer2).set_Size(new Point(((Control)scrollView).get_Width(), 50));
			((Panel)keybindFlowContainer2).set_CanScroll(false);
			((FlowPanel)keybindFlowContainer2).set_FlowDirection((ControlFlowDirection)0);
			KeybindFlowContainer keybindFlowContainer = keybindFlowContainer2;
			keybindUIData.Add(keybindFlowContainer);
			keybindFlowContainer.characterNameDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				Character val = (characterResponse as List<Character>).Find((Character item) => keybindFlowContainer.characterNameDropdown.get_SelectedItem() == item.get_Name());
				if (val != null)
				{
					foreach (Profession current in professionsResponse)
					{
						if (val.get_Profession() == current.get_Name())
						{
							RenderUrl icon = current.get_Icon();
							int num = int.Parse(Path.GetFileNameWithoutExtension(((RenderUrl)(ref icon)).get_Url().AbsoluteUri));
							keybindFlowContainer.professionImage.set_Texture(AsyncTexture2D.FromAssetId(num));
						}
					}
					foreach (KeyValuePair<string, List<Specialization>> current2 in professionSpezialisations)
					{
						if (val.get_Profession() == current2.Key)
						{
							List<string> list = new List<string> { "All Spezialisations", "Core" };
							keybindFlowContainer.specializationDropdown.set_SelectedItem("All Spezialisations");
							list.AddRange(current2.Value.Select((Specialization specialization) => specialization.get_Name()));
							keybindFlowContainer.SetSpecializationOptions(list);
						}
					}
					((Control)keybindFlowContainer.keymapDropdown).set_Enabled(true);
					((Control)keybindFlowContainer.specializationDropdown).set_Enabled(true);
				}
			});
			((Control)keybindFlowContainer.removeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				keybindUIData.Remove(keybindFlowContainer);
			});
			keybindFlowContainer.OnApplyPressed += delegate(object sender, string selectedKeybinds)
			{
				Task.Run(() => ChangeKeybinds(selectedKeybinds));
			};
			return keybindFlowContainer;
		}
	}
}
