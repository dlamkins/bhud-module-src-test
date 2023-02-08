using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Controls;
using Kenedia.Modules.Characters.Controls.SideMenu;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using SemVer;

namespace Kenedia.Modules.Characters.Views
{
	public class MainWindow : StandardWindow
	{
		private readonly SettingsModel _settings;

		private readonly TextureManager _textureManager;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private readonly SearchFilterCollection _searchFilters;

		private readonly SearchFilterCollection _tagFilters;

		private readonly Func<Character_Model> _currentCharacter;

		private readonly Data _data;

		private readonly AsyncTexture2D _windowEmblem = AsyncTexture2D.FromAssetId(156015);

		private readonly ImageButton _toggleSideMenuButton;

		private readonly ImageButton _displaySettingsButton;

		private readonly ImageButton _randomButton;

		private readonly ImageButton _lastButton;

		private readonly ImageButton _clearButton;

		private readonly FlowPanel _dropdownPanel;

		private readonly FlowPanel _buttonPanel;

		private readonly FilterBox _filterBox;

		private readonly bool _created;

		private bool _filterCharacters;

		private double _filterTick;

		private Rectangle _emblemRectangle;

		private Rectangle _titleRectangle;

		private BitmapFont _titleFont;

		public SettingsWindow SettingsWindow { get; set; }

		public SideMenu SideMenu { get; }

		public Version Version { get; set; }

		public List<CharacterCard> CharacterCards { get; } = new List<CharacterCard>();


		public CharacterEdit CharacterEdit { get; set; }

		public DraggingControl DraggingControl { get; set; }

		public FlowPanel CharactersPanel { get; private set; }

		public FlowPanel ContentPanel { get; private set; }

		public MainWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, SettingsModel settings, TextureManager textureManager, ObservableCollection<Character_Model> characterModels, SearchFilterCollection searchFilters, SearchFilterCollection tagFilters, Action toggleOCR, Action togglePotrait, Action refreshAPI, string accountImagePath, TagList tags, Func<Character_Model> currentCharacter, Data data, CharacterSorting characterSorting)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			DraggingControl draggingControl = new DraggingControl();
			((Control)draggingControl).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)draggingControl).set_Visible(false);
			((Control)draggingControl).set_ZIndex(2147483646);
			((Control)draggingControl).set_Enabled(false);
			DraggingControl = draggingControl;
			base._002Ector(AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion);
			MainWindow mainWindow = this;
			_settings = settings;
			_textureManager = textureManager;
			_characterModels = characterModels;
			_searchFilters = searchFilters;
			_tagFilters = tagFilters;
			_currentCharacter = currentCharacter;
			_data = data;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((Control)flowPanel).set_Location(new Point(0, 35));
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)2);
			((Container)flowPanel).set_AutoSizePadding(new Point(5, 5));
			ContentPanel = flowPanel;
			Dummy dummy = new Dummy();
			((Control)dummy).set_Parent((Container)(object)ContentPanel);
			((Control)dummy).set_Width(((Control)ContentPanel).get_Width());
			((Control)dummy).set_Height(3);
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)ContentPanel);
			((Control)flowPanel2).set_Size(((Control)this).get_Size());
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(2f, 4f));
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)2);
			((Panel)flowPanel2).set_CanScroll(true);
			CharactersPanel = flowPanel2;
			((Control)DraggingControl).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)DraggingControl_LeftMouseButtonReleased);
			FlowPanel flowPanel3 = new FlowPanel();
			((Control)flowPanel3).set_Parent((Container)(object)this);
			((Control)flowPanel3).set_Location(new Point(0, 2));
			((FlowPanel)flowPanel3).set_FlowDirection((ControlFlowDirection)2);
			((Container)flowPanel3).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel3).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel3).set_ControlPadding(new Vector2(5f, 0f));
			_dropdownPanel = flowPanel3;
			FilterBox filterBox = new FilterBox();
			((Control)filterBox).set_Parent((Container)(object)_dropdownPanel);
			((TextInputBase)filterBox).set_PlaceholderText(strings.Search);
			((Control)filterBox).set_Width(100);
			filterBox.FilteringDelay = _settings.FilterDelay.get_Value();
			filterBox.EnterPressedAction = FilterBox_EnterPressed;
			filterBox.ClickAction = FilterBox_Click;
			filterBox.PerformFiltering = delegate
			{
				mainWindow.PerformFiltering();
			};
			filterBox.FilteringOnTextChange = true;
			_filterBox = filterBox;
			_settings.FilterDelay.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)FilterDelay_SettingChanged);
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent((Container)(object)this);
			imageButton.Texture = AsyncTexture2D.FromAssetId(2175783);
			imageButton.HoveredTexture = AsyncTexture2D.FromAssetId(2175782);
			imageButton.ClickedTexture = AsyncTexture2D.FromAssetId(2175784);
			((Control)imageButton).set_Size(new Point(20, 20));
			imageButton.SetLocalizedTooltip = () => strings.ClearFilters;
			((Control)imageButton).set_Visible(false);
			imageButton.ClickAction = delegate
			{
				((TextInputBase)mainWindow._filterBox).set_Text((string)null);
				mainWindow._filterCharacters = true;
				mainWindow.SideMenu.ResetToggles();
			};
			_clearButton = imageButton;
			FlowPanel flowPanel4 = new FlowPanel();
			((Control)flowPanel4).set_Parent((Container)(object)_dropdownPanel);
			((FlowPanel)flowPanel4).set_FlowDirection((ControlFlowDirection)2);
			((Container)flowPanel4).set_WidthSizingMode((SizingMode)1);
			((Container)flowPanel4).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel4).set_Padding(new Thickness(15f));
			_buttonPanel = flowPanel4;
			((Control)_buttonPanel).add_Resized((EventHandler<ResizedEventArgs>)ButtonPanel_Resized);
			ImageButton imageButton2 = new ImageButton();
			((Control)imageButton2).set_Parent((Container)(object)_buttonPanel);
			imageButton2.Texture = AsyncTexture2D.FromAssetId(1078535);
			((Control)imageButton2).set_Size(new Point(25, 25));
			imageButton2.Color = Colors.ColonialWhite;
			imageButton2.ColorHovered = Color.get_White();
			imageButton2.SetLocalizedTooltip = () => strings.LastButton_Tooltip;
			((Control)imageButton2).set_Visible(_settings.ShowLastButton.get_Value());
			imageButton2.ClickAction = delegate
			{
				characterModels.Aggregate((Character_Model a, Character_Model b) => (!(b.LastLogin > a.LastLogin)) ? b : a)?.Swap(ignoreOCR: true);
			};
			_lastButton = imageButton2;
			_lastButton.Texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Texture_TextureSwapped);
			ImageButton imageButton3 = new ImageButton();
			((Control)imageButton3).set_Parent((Container)(object)_buttonPanel);
			((Control)imageButton3).set_Size(new Point(25, 25));
			imageButton3.SetLocalizedTooltip = () => strings.RandomButton_Tooltip;
			imageButton3.Texture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Dice));
			imageButton3.HoveredTexture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Dice_Hovered));
			((Control)imageButton3).set_Visible(_settings.ShowRandomButton.get_Value());
			imageButton3.ClickAction = delegate
			{
				List<Control> list = ((IEnumerable<Control>)((Container)mainWindow.CharactersPanel).get_Children()).Where((Control e) => e.get_Visible()).ToList();
				int index = RandomService.Rnd.Next(list.Count);
				((CharacterCard)(object)list[index])?.Character.Swap();
			};
			_randomButton = imageButton3;
			ImageButton imageButton4 = new ImageButton();
			((Control)imageButton4).set_Parent((Container)(object)_buttonPanel);
			imageButton4.Texture = AsyncTexture2D.FromAssetId(155052);
			imageButton4.HoveredTexture = AsyncTexture2D.FromAssetId(157110);
			((Control)imageButton4).set_Size(new Point(25, 25));
			imageButton4.SetLocalizedTooltip = () => string.Format(strings.ShowItem, string.Format(strings.ItemSettings, strings.Display));
			imageButton4.ClickAction = delegate
			{
				SettingsWindow settingsWindow = mainWindow.SettingsWindow;
				if (settingsWindow != null)
				{
					((WindowBase2)settingsWindow).ToggleWindow();
				}
			};
			_displaySettingsButton = imageButton4;
			ImageButton imageButton5 = new ImageButton();
			((Control)imageButton5).set_Parent((Container)(object)_buttonPanel);
			imageButton5.Texture = AsyncTexture2D.FromAssetId(605018);
			((Control)imageButton5).set_Size(new Point(25, 25));
			imageButton5.Color = Colors.ColonialWhite;
			imageButton5.ColorHovered = Color.get_White();
			imageButton5.SetLocalizedTooltip = () => string.Format(strings.Toggle, "Side Menu");
			imageButton5.ClickAction = delegate
			{
				mainWindow.ShowAttached(((Control)mainWindow.SideMenu).get_Visible() ? null : mainWindow.SideMenu);
			};
			_toggleSideMenuButton = imageButton5;
			CharacterEdit characterEdit = new CharacterEdit(textureManager, togglePotrait, accountImagePath, tags, settings, delegate
			{
				mainWindow.PerformFiltering();
			});
			((Control)characterEdit).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			characterEdit.Anchor = (Control)(object)this;
			characterEdit.AnchorPosition = AnchoredContainer.AnchorPos.AutoHorizontal;
			characterEdit.RelativePosition = new RectangleDimensions(0, 45, 0, 0);
			((Control)characterEdit).set_Visible(false);
			characterEdit.FadeOut = !_settings.PinSideMenus.get_Value();
			CharacterEdit = characterEdit;
			SideMenu sideMenu = new SideMenu(toggleOCR, togglePotrait, refreshAPI, textureManager, settings, characterSorting);
			((Control)sideMenu).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			sideMenu.Anchor = (Control)(object)this;
			sideMenu.AnchorPosition = AnchoredContainer.AnchorPos.AutoHorizontal;
			sideMenu.RelativePosition = new RectangleDimensions(0, 45, 0, 0);
			((Control)sideMenu).set_Visible(false);
			sideMenu.FadeOut = !_settings.PinSideMenus.get_Value();
			SideMenu = sideMenu;
			AttachContainer(CharacterEdit);
			AttachContainer(SideMenu);
			CreateCharacterControls(_characterModels);
			_created = true;
			_settings.PinSideMenus.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)PinSideMenus_SettingChanged);
			_settings.ShowRandomButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowRandomButton_SettingChanged);
			_settings.ShowLastButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowLastButton_SettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}

		private void FilterDelay_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_filterBox.FilteringDelay = e.get_NewValue();
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			_currentCharacter?.Invoke()?.UpdateCharacter();
			PerformFiltering();
		}

		private void Texture_TextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_lastButton.Texture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Texture_TextureSwapped);
			_lastButton.Texture = AsyncTexture2D.op_Implicit(e.get_NewValue().ToGrayScaledPalettable());
		}

		private void ShowLastButton_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			((Control)_lastButton).set_Visible(e.get_NewValue());
			((Control)_buttonPanel).Invalidate();
		}

		private void PinSideMenus_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			CharacterEdit.FadeOut = !e.get_NewValue();
			SideMenu.FadeOut = !e.get_NewValue();
		}

		private void ShowRandomButton_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			((Control)_randomButton).set_Visible(e.get_NewValue());
			((Control)_buttonPanel).Invalidate();
		}

		private void ButtonPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			((Control)_filterBox).set_Width(((Control)_dropdownPanel).get_Width() - ((Control)_buttonPanel).get_Width() - 2);
			ImageButton clearButton = _clearButton;
			Rectangle localBounds = ((Control)_filterBox).get_LocalBounds();
			int num = ((Rectangle)(ref localBounds)).get_Right() - 25;
			localBounds = ((Control)_filterBox).get_LocalBounds();
			((Control)clearButton).set_Location(new Point(num, ((Rectangle)(ref localBounds)).get_Top() + 5));
		}

		public void FilterCharacters(object sender = null, EventArgs e = null)
		{
			_filterCharacters = true;
		}

		public void PerformFiltering()
		{
			Regex regex = (_settings.FilterAsOne.get_Value() ? new Regex("^(?!\\s*$).+") : new Regex("\\w+|\"[\\w\\s]*\""));
			List<Match> strings = regex.Matches(((TextInputBase)_filterBox).get_Text().Trim().ToLower()).Cast<Match>().ToList();
			new List<string>();
			List<KeyValuePair<string, SearchFilter<Character_Model>>> stringFilters = new List<KeyValuePair<string, SearchFilter<Character_Model>>>();
			_003C_003Ec__DisplayClass61_0 CS_0024_003C_003E8__locals0;
			foreach (Match match in strings)
			{
				string string_text = SearchableString(match.ToString().Replace("\"", ""));
				if (_settings.DisplayToggles.get_Value()["Name"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Name", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.Name).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.get_Value()["Profession"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Specialization", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.SpecializationName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.get_Value()["Profession"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Profession", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.ProfessionName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.get_Value()["Level"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Level", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.Level.ToString()).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.get_Value()["Race"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Race", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.RaceName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.get_Value()["Map"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Map", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.MapName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.get_Value()["Gender"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Gender", new SearchFilter<Character_Model>(delegate(Character_Model c)
					{
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						//IL_000c: Unknown result type (might be due to invalid IL or missing references)
						_003C_003Ec__DisplayClass61_0 _003C_003Ec__DisplayClass61_ = CS_0024_003C_003E8__locals0;
						Gender gender = c.Gender;
						return SearchableString(((object)(Gender)(ref gender)).ToString()).Contains(string_text);
					}, enabled: true)));
				}
				if (_settings.DisplayToggles.get_Value()["CraftingProfession"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("CraftingProfession", new SearchFilter<Character_Model>(delegate(Character_Model c)
					{
						foreach (KeyValuePair<int, Data.CraftingProfession> current2 in c.CraftingDisciplines)
						{
							if ((!_settings.DisplayToggles.get_Value()["OnlyMaxCrafting"].Check || current2.Key == current2.Value.MaxRating) && SearchableString(current2.Value.Name).Contains(string_text))
							{
								return true;
							}
						}
						return false;
					}, enabled: true)));
				}
				if (!_settings.DisplayToggles.get_Value()["Tags"].Check)
				{
					continue;
				}
				stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Tags", new SearchFilter<Character_Model>(delegate(Character_Model c)
				{
					foreach (string current in c.Tags)
					{
						if (SearchableString(current).Contains(string_text))
						{
							return true;
						}
					}
					return false;
				}, enabled: true)));
			}
			bool matchAny = _settings.ResultMatchingBehavior.get_Value() == SettingsModel.MatchingBehavior.MatchAny;
			bool matchAll = _settings.ResultMatchingBehavior.get_Value() == SettingsModel.MatchingBehavior.MatchAll;
			bool include = _settings.ResultFilterBehavior.get_Value() == SettingsModel.FilterBehavior.Include;
			List<KeyValuePair<string, SearchFilter<Character_Model>>> toggleFilters = _searchFilters.Where((KeyValuePair<string, SearchFilter<Character_Model>> e) => e.Value.IsEnabled).ToList();
			List<KeyValuePair<string, SearchFilter<Character_Model>>> tagFilters = _tagFilters.Where((KeyValuePair<string, SearchFilter<Character_Model>> e) => e.Value.IsEnabled).ToList();
			foreach (var (ctrl2, toggleResult, stringsResult, tagsResult) in from CharacterCard ctrl in (IEnumerable)((Container)CharactersPanel).get_Children()
				let c = ctrl.Character
				where c != null
				let toggleResult = toggleFilters.Count == 0 || include == FilterResult(c)
				let stringsResult = stringFilters.Count == 0 || include == StringFilterResult(c)
				let tagsResult = tagFilters.Count == 0 || include == TagResult(c)
				select (ctrl, toggleResult, stringsResult, tagsResult))
			{
				((Control)ctrl2).set_Visible(toggleResult && stringsResult && tagsResult);
			}
			SortCharacters();
			((Control)CharactersPanel).Invalidate();
			((Control)_clearButton).set_Visible(stringFilters.Count > 0 || toggleFilters.Count > 0 || tagFilters.Count > 0);
			bool FilterResult(Character_Model c)
			{
				List<bool> results3 = new List<bool>();
				foreach (KeyValuePair<string, SearchFilter<Character_Model>> item in toggleFilters)
				{
					bool result2 = item.Value.CheckForMatch(c);
					results3.Add(result2);
					if (result2 && matchAny)
					{
						return true;
					}
				}
				if (matchAll)
				{
					return results3.Count((bool e) => e) == toggleFilters.Count;
				}
				return false;
			}
			string SearchableString(string s)
			{
				return (_settings.FilterDiacriticsInsensitive.get_Value() ? s.RemoveDiacritics() : s).ToLower();
			}
			bool StringFilterResult(Character_Model c)
			{
				List<bool> results2 = new List<bool>();
				foreach (KeyValuePair<string, SearchFilter<Character_Model>> item2 in stringFilters)
				{
					bool matched = item2.Value.CheckForMatch(c);
					if (matched && matchAny)
					{
						return true;
					}
					if (matched)
					{
						results2.Add(matched);
					}
				}
				if (matchAll)
				{
					return results2.Count((bool e) => e) >= strings.Count;
				}
				return false;
			}
			bool TagResult(Character_Model c)
			{
				List<bool> results = new List<bool>();
				foreach (KeyValuePair<string, SearchFilter<Character_Model>> item3 in tagFilters)
				{
					bool result = item3.Value.CheckForMatch(c);
					results.Add(result);
					if (result && matchAny)
					{
						return true;
					}
				}
				if (matchAll)
				{
					return results.Count((bool e) => e) == tagFilters.Count;
				}
				return false;
			}
		}

		public void CreateCharacterControls(IEnumerable<Character_Model> models)
		{
			foreach (Character_Model c in models)
			{
				if (CharacterCards.Find((CharacterCard e) => e.Character.Name == c.Name) == null)
				{
					List<CharacterCard> characterCards = CharacterCards;
					CharacterCard obj = new CharacterCard(_currentCharacter, _textureManager, _data, this, _settings)
					{
						Character = c
					};
					((Control)obj).set_Parent((Container)(object)CharactersPanel);
					obj.AttachedCards = CharacterCards;
					characterCards.Add(obj);
				}
			}
			FilterCharacters();
			CharacterCards.FirstOrDefault()?.UniformWithAttached();
		}

		public void SortCharacters()
		{
			SettingsModel.SortBy sortby = _settings.SortType.get_Value();
			bool asc = _settings.SortOrder.get_Value() == SettingsModel.SortDirection.Ascending;
			bool isNum = sortby == SettingsModel.SortBy.Level || sortby == SettingsModel.SortBy.TimeSinceLogin || sortby == SettingsModel.SortBy.Map;
			if (_settings.SortType.get_Value() != SettingsModel.SortBy.Custom)
			{
				((FlowPanel)CharactersPanel).SortChildren<CharacterCard>((Comparison<CharacterCard>)delegate(CharacterCard a, CharacterCard b)
				{
					if (isNum)
					{
						int num = (asc ? getValue(a.Character).CompareTo(getValue(b.Character)) : getValue(b.Character).CompareTo(getValue(a.Character)));
						int num2 = (asc ? a.Character.Position.CompareTo(b.Character.Position) : b.Character.Position.CompareTo(a.Character.Position));
						if (num != 0)
						{
							return num;
						}
						return num - num2;
					}
					int num3 = (asc ? getString(a.Character).CompareTo(getString(b.Character)) : getString(b.Character).CompareTo(getString(a.Character)));
					int num4 = (asc ? a.Character.Position.CompareTo(b.Character.Position) : b.Character.Position.CompareTo(a.Character.Position));
					return (num3 != 0) ? num3 : (num3 - num4);
				});
				return;
			}
			((FlowPanel)CharactersPanel).SortChildren<CharacterCard>((Comparison<CharacterCard>)((CharacterCard a, CharacterCard b) => a.Index.CompareTo(b.Index)));
			int i = 0;
			foreach (CharacterCard item in ((IEnumerable)((Container)CharactersPanel).get_Children()).Cast<CharacterCard>())
			{
				item.Index = i;
				i++;
			}
			string getString(Character_Model c)
			{
				return Common.GetPropertyValueAsString(c, $"{_settings.SortType.get_Value()}");
			}
			int getValue(Character_Model c)
			{
				return Common.GetPropertyValue<int>(c, $"{_settings.SortType.get_Value()}");
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			((Control)this).set_BasicTooltipText((((WindowBase2)this).get_MouseOverTitleBar() && Version != (Version)null) ? $"v. {Version}" : null);
			if (_filterCharacters && gameTime.get_TotalGameTime().TotalMilliseconds - _filterTick > (double)_settings.FilterDelay.get_Value())
			{
				_filterTick = gameTime.get_TotalGameTime().TotalMilliseconds;
				_filterCharacters = false;
				PerformFiltering();
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).RecalculateLayout();
			_emblemRectangle = new Rectangle(-43, -58, 128, 128);
			_titleFont = GameService.Content.get_DefaultFont32();
			RectangleF titleBounds = _titleFont.GetStringRectangle(BaseModule<Characters, MainWindow, SettingsModel>.ModuleName);
			if (titleBounds.Width > (float)(((Control)this).get_LocalBounds().Width - (_emblemRectangle.Width - 15)))
			{
				_titleFont = GameService.Content.get_DefaultFont18();
				titleBounds = _titleFont.GetStringRectangle(BaseModule<Characters, MainWindow, SettingsModel>.ModuleName);
			}
			_titleRectangle = new Rectangle(65, 5, (int)titleBounds.Width, Math.Max(30, (int)titleBounds.Height));
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_windowEmblem), _emblemRectangle, (Rectangle?)_windowEmblem.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			if (_titleRectangle.Width < bounds.Width - (_emblemRectangle.Width - 20))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, BaseModule<Characters, MainWindow, SettingsModel>.ModuleName ?? "", _titleFont, _titleRectangle, Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)2);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			if (_settings.FocusSearchOnShow.get_Value())
			{
				((TextInputBase)_filterBox).set_Focused(true);
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			CharacterCards?.ForEach(delegate(CharacterCard c)
			{
				c.HideTooltips();
			});
			_filterBox?.ResetText();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).OnResized(e);
			if (_created)
			{
				Rectangle val;
				if (ContentPanel != null)
				{
					FlowPanel contentPanel = ContentPanel;
					val = ((Container)this).get_ContentRegion();
					int x = ((Rectangle)(ref val)).get_Size().X;
					val = ((Container)this).get_ContentRegion();
					((Control)contentPanel).set_Size(new Point(x, ((Rectangle)(ref val)).get_Size().Y - 35));
				}
				if (_dropdownPanel != null)
				{
					((Control)_filterBox).set_Width(((Control)_dropdownPanel).get_Width() - ((Control)_buttonPanel).get_Width() - 2);
					ImageButton clearButton = _clearButton;
					val = ((Control)_filterBox).get_LocalBounds();
					int num = ((Rectangle)(ref val)).get_Right() - 23;
					val = ((Control)_filterBox).get_LocalBounds();
					((Control)clearButton).set_Location(new Point(num, ((Rectangle)(ref val)).get_Top() + 6));
				}
				if (e.get_CurrentSize().Y < 135)
				{
					((Control)this).set_Size(new Point(((Control)this).get_Size().X, 135));
				}
				_settings.WindowSize.set_Value(((Control)this).get_Size());
			}
		}

		protected override void DisposeControl()
		{
			((WindowBase2)this).DisposeControl();
			_settings.PinSideMenus.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)PinSideMenus_SettingChanged);
			_settings.ShowRandomButton.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowRandomButton_SettingChanged);
			_settings.ShowLastButton.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowLastButton_SettingChanged);
			_settings.FilterDelay.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)FilterDelay_SettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
			((Control)DraggingControl).remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)DraggingControl_LeftMouseButtonReleased);
			((Control)_buttonPanel).remove_Resized((EventHandler<ResizedEventArgs>)ButtonPanel_Resized);
			if (CharacterCards.Count > 0)
			{
				((IEnumerable<IDisposable>)CharacterCards)?.DisposeAll();
			}
			((IEnumerable<IDisposable>)ContentPanel)?.DisposeAll();
			FlowPanel charactersPanel = CharactersPanel;
			if (charactersPanel != null)
			{
				((Control)charactersPanel).Dispose();
			}
			DraggingControl draggingControl = DraggingControl;
			if (draggingControl != null)
			{
				((Control)draggingControl).Dispose();
			}
			CharacterEdit characterEdit = CharacterEdit;
			if (characterEdit != null)
			{
				((Control)characterEdit).Dispose();
			}
			FlowPanel dropdownPanel = _dropdownPanel;
			if (dropdownPanel != null)
			{
				((Control)dropdownPanel).Dispose();
			}
			ImageButton displaySettingsButton = _displaySettingsButton;
			if (displaySettingsButton != null)
			{
				((Control)displaySettingsButton).Dispose();
			}
			FilterBox filterBox = _filterBox;
			if (filterBox != null)
			{
				((Control)filterBox).Dispose();
			}
			SideMenu sideMenu = SideMenu;
			if (sideMenu != null)
			{
				((Control)sideMenu).Dispose();
			}
		}

		private async void FilterBox_EnterPressed(string t)
		{
			if (_settings.EnterToLogin.get_Value())
			{
				_filterBox.ForceFilter();
				CharacterCard c = (CharacterCard)(object)((IEnumerable<Control>)((Container)CharactersPanel).get_Children()).Where((Control e) => e.get_Visible()).FirstOrDefault();
				((Control)_filterBox).UnsetFocus();
				await Task.Delay(5);
				if (await ExtendedInputService.WaitForNoKeyPressed())
				{
					c?.Character.Swap();
				}
			}
		}

		private void FilterBox_Click()
		{
			if (_settings.OpenSidemenuOnSearch.get_Value())
			{
				ShowAttached(SideMenu);
			}
		}

		private void DraggingControl_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			SetNewIndex(DraggingControl.CharacterControl);
			DraggingControl.CharacterControl = null;
		}

		private void SetNewIndex(CharacterCard characterControl)
		{
			characterControl.Index = GetHoveredIndex(characterControl);
			SortCharacters();
		}

		private double GetHoveredIndex(CharacterCard characterControl)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			MouseHandler j = Control.get_Input().get_Mouse();
			CharacterCard lastControl = characterControl;
			int i = 0;
			foreach (CharacterCard item in ((IEnumerable)((Container)CharactersPanel).get_Children()).Cast<CharacterCard>())
			{
				item.Index = i;
				i++;
			}
			Rectangle absoluteBounds;
			foreach (CharacterCard c in ((IEnumerable)((Container)CharactersPanel).get_Children()).Cast<CharacterCard>())
			{
				absoluteBounds = ((Control)c).get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).Contains(j.get_Position()))
				{
					return (characterControl.Index > c.Index) ? (c.Index - 0.1) : (c.Index + 0.1);
				}
				lastControl = c;
			}
			absoluteBounds = ((Control)lastControl).get_AbsoluteBounds();
			if (((Rectangle)(ref absoluteBounds)).get_Bottom() >= j.get_Position().Y)
			{
				absoluteBounds = ((Control)lastControl).get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).get_Top() < j.get_Position().Y)
				{
					absoluteBounds = ((Control)lastControl).get_AbsoluteBounds();
					if (((Rectangle)(ref absoluteBounds)).get_Right() < j.get_Position().X)
					{
						goto IL_0130;
					}
				}
				return characterControl.Index;
			}
			goto IL_0130;
			IL_0130:
			return CharacterCards.Count + 1;
		}
	}
}
