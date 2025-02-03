using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
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
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Characters.Views
{
	public class MainWindow : Kenedia.Modules.Core.Views.StandardWindow
	{
		private readonly Settings _settings;

		private readonly TextureManager _textureManager;

		private readonly ObservableCollection<Character_Model> _rawCharacterModels;

		private readonly SearchFilterCollection _searchFilters;

		private readonly SearchFilterCollection _tagFilters;

		private readonly Action _toggleOCR;

		private readonly Func<Character_Model> _currentCharacter;

		private readonly Data _data;

		private readonly AsyncTexture2D _windowEmblem = AsyncTexture2D.FromAssetId(156015);

		private readonly ImageButton _toggleSideMenuButton;

		private readonly CollapseContainer _collapseWrapper;

		private readonly ImageButton _displaySettingsButton;

		private readonly ImageButton _randomButton;

		private readonly ImageButton _lastButton;

		private readonly ImageButton _clearButton;

		private NotificationPanel _notifications;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _dropdownPanel;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _buttonPanel;

		private readonly FilterBox _filterBox;

		private readonly bool _created;

		private bool _filterCharacters;

		private double _filterTick;

		private APITimeoutNotification _apiTimeoutNotification;

		private APIPermissionNotification _apiPermissionNotification;

		private TesseractFailedNotification _tesseractFailedNotification;

		private double lastUniform;

		public List<Character_Model> LoadedModels { get; } = new List<Character_Model>();


		private List<Character_Model> CharacterModels => _rawCharacterModels.ToList();

		public SettingsWindow SettingsWindow { get; set; }

		public SideMenu SideMenu { get; }

		public List<CharacterCard> CharacterCards { get; } = new List<CharacterCard>();


		public CharacterEdit CharacterEdit { get; set; }

		public DraggingControl DraggingControl { get; set; } = new DraggingControl();


		public Kenedia.Modules.Core.Controls.FlowPanel CharactersPanel { get; private set; }

		public Kenedia.Modules.Core.Controls.FlowPanel ContentPanel { get; private set; }

		public MainWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Settings settings, TextureManager textureManager, ObservableCollection<Character_Model> characterModels, SearchFilterCollection searchFilters, SearchFilterCollection tagFilters, Action toggleOCR, Action togglePotrait, Action refreshAPI, Func<string> accountImagePath, TagList tags, Func<Character_Model> currentCharacter, Data data, CharacterSorting characterSorting)
			: base((AsyncTexture2D)background, windowRegion, contentRegion)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0618: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_textureManager = textureManager;
			_rawCharacterModels = characterModels;
			_searchFilters = searchFilters;
			_tagFilters = tagFilters;
			_toggleOCR = toggleOCR;
			_currentCharacter = currentCharacter;
			_data = data;
			ContentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Location = new Point(0, 0),
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				AutoSizePadding = new Point(5, 5),
				ControlPadding = new Vector2(0f, 5f)
			};
			DraggingControl.LeftMouseButtonReleased += DraggingControl_LeftMouseButtonReleased;
			_collapseWrapper = new CollapseContainer
			{
				Parent = ContentPanel,
				WidthSizingMode = SizingMode.Fill,
				SetLocalizedTitle = () => strings.Notifications,
				TitleIcon = AsyncTexture2D.FromAssetId(222246),
				TitleBarHeight = 24,
				Height = 24,
				ContentPadding = new RectangleDimensions(5),
				MaxSize = new Point(-1, 350),
				Collapsed = true,
				Visible = false
			};
			_collapseWrapper.Hidden += CollapseWrapper_Hidden;
			_notifications = new NotificationPanel(_rawCharacterModels)
			{
				Parent = _collapseWrapper,
				WidthSizingMode = SizingMode.Fill,
				MaxSize = new Point(-1, 310)
			};
			_collapseWrapper.SizeDeterminingChild = _notifications;
			_dropdownPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = ContentPanel,
				Location = new Point(0, 2),
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(5f, 0f)
			};
			_filterBox = new FilterBox
			{
				Parent = _dropdownPanel,
				PlaceholderText = strings.Search,
				Width = 100,
				FilteringDelay = _settings.FilterDelay.Value,
				EnterPressedAction = new Action<string>(FilterBox_EnterPressed),
				ClickAction = new Action(FilterBox_Click),
				PerformFiltering = delegate
				{
					PerformFiltering();
				},
				FilteringOnTextChange = true
			};
			_settings.FilterDelay.SettingChanged += FilterDelay_SettingChanged;
			_clearButton = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(2175783),
				HoveredTexture = AsyncTexture2D.FromAssetId(2175782),
				ClickedTexture = AsyncTexture2D.FromAssetId(2175784),
				Size = new Point(20, 20),
				SetLocalizedTooltip = () => strings.ClearFilters,
				Visible = false,
				ClickAction = delegate
				{
					_filterBox.Text = null;
					_filterCharacters = true;
					SideMenu.ResetToggles();
				}
			};
			_buttonPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = _dropdownPanel,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				Padding = new Thickness(15f)
			};
			_buttonPanel.Resized += ButtonPanel_Resized;
			_lastButton = new ImageButton
			{
				Parent = _buttonPanel,
				Texture = AsyncTexture2D.FromAssetId(1078535),
				Size = new Point(25, 25),
				ImageColor = ContentService.Colors.ColonialWhite,
				ColorHovered = Color.get_White(),
				SetLocalizedTooltip = () => strings.LastButton_Tooltip,
				Visible = _settings.ShowLastButton.Value,
				ClickAction = delegate
				{
					if (CharacterModels.Count > 1)
					{
						CharacterModels.Aggregate((Character_Model a, Character_Model b) => (!(b.LastLogin > a.LastLogin)) ? b : a)?.Swap(ignoreOCR: true);
					}
					else if (CharacterModels.Count == 1)
					{
						CharacterModels?.FirstOrDefault()?.Swap(ignoreOCR: true);
					}
				}
			};
			_lastButton.Texture.TextureSwapped += Texture_TextureSwapped;
			_randomButton = new ImageButton
			{
				Parent = _buttonPanel,
				Size = new Point(25, 25),
				SetLocalizedTooltip = () => strings.RandomButton_Tooltip,
				Texture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Dice),
				HoveredTexture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Dice_Hovered),
				Visible = _settings.ShowRandomButton.Value,
				ClickAction = delegate
				{
					List<Control> list = CharactersPanel.Children.Where((Control e) => e.Visible).ToList();
					int num = RandomService.Rnd.Next(list.Count);
					if (list.Count > num)
					{
						((CharacterCard)list[num])?.Character.Swap();
					}
				}
			};
			_displaySettingsButton = new ImageButton
			{
				Parent = _buttonPanel,
				Texture = AsyncTexture2D.FromAssetId(155052),
				HoveredTexture = AsyncTexture2D.FromAssetId(157110),
				Size = new Point(25, 25),
				SetLocalizedTooltip = () => string.Format(strings.ShowItem, string.Format(strings.ItemSettings, strings.Display)),
				ClickAction = delegate
				{
					SettingsWindow?.ToggleWindow();
				}
			};
			_toggleSideMenuButton = new ImageButton
			{
				Parent = _buttonPanel,
				Texture = AsyncTexture2D.FromAssetId(605018),
				Size = new Point(25, 25),
				ImageColor = ContentService.Colors.ColonialWhite,
				ColorHovered = Color.get_White(),
				SetLocalizedTooltip = () => string.Format(strings.Toggle, "Side Menu"),
				ClickAction = delegate
				{
					ShowAttached(SideMenu.Visible ? null : SideMenu);
				}
			};
			CharactersPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = ContentPanel,
				Size = base.Size,
				ControlPadding = new Vector2(2f, 4f),
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				CanScroll = true
			};
			CharacterEdit = new CharacterEdit(textureManager, togglePotrait, accountImagePath, tags, settings, new Action(PerformFiltering))
			{
				Parent = GameService.Graphics.SpriteScreen,
				Anchor = this,
				AnchorPosition = AnchoredContainer.AnchorPos.AutoHorizontal,
				RelativePosition = new RectangleDimensions(0, 45, 0, 0),
				Visible = false,
				FadeOut = !_settings.PinSideMenus.Value
			};
			SideMenu = new SideMenu(toggleOCR, togglePotrait, refreshAPI, textureManager, settings, characterSorting)
			{
				Parent = GameService.Graphics.SpriteScreen,
				Anchor = this,
				AnchorPosition = AnchoredContainer.AnchorPos.AutoHorizontal,
				RelativePosition = new RectangleDimensions(0, 45, 0, 0),
				Visible = false,
				FadeOut = !_settings.PinSideMenus.Value
			};
			AttachContainer(CharacterEdit);
			AttachContainer(SideMenu);
			UpdateMissingNotification();
			CheckOCRRegion();
			_created = true;
			_settings.PinSideMenus.SettingChanged += PinSideMenus_SettingChanged;
			_settings.ShowRandomButton.SettingChanged += ShowRandomButton_SettingChanged;
			_settings.ShowLastButton.SettingChanged += ShowLastButton_SettingChanged;
			_settings.IncludeBetaCharacters.SettingChanged += IncludeBetaCharacters_SettingChanged;
			GameService.Gw2Mumble.CurrentMap.MapChanged += CurrentMap_MapChanged;
		}

		private void IncludeBetaCharacters_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
		}

		private void CollapseWrapper_Hidden(object sender, EventArgs e)
		{
			ContentPanel.Invalidate();
		}

		private void FilterDelay_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<int> e)
		{
			_filterBox.FilteringDelay = e.NewValue;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			_currentCharacter?.Invoke()?.UpdateCharacter();
			PerformFiltering();
		}

		private void Texture_TextureSwapped(object sender, Blish_HUD.ValueChangedEventArgs<Texture2D> e)
		{
			_lastButton.Texture.TextureSwapped -= Texture_TextureSwapped;
			_lastButton.Texture = (AsyncTexture2D)e.NewValue.ToGrayScaledPalettable();
		}

		private void ShowLastButton_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			_lastButton.Visible = e.NewValue;
			_buttonPanel.Invalidate();
		}

		private void PinSideMenus_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			CharacterEdit.FadeOut = !e.NewValue;
			SideMenu.FadeOut = !e.NewValue;
		}

		private void ShowRandomButton_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			_randomButton.Visible = e.NewValue;
			_buttonPanel.Invalidate();
		}

		private void ButtonPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			_filterBox.Width = _dropdownPanel.Width - _buttonPanel.Width - 2;
			ImageButton clearButton = _clearButton;
			Rectangle localBounds = _filterBox.LocalBounds;
			int num = ((Rectangle)(ref localBounds)).get_Right() - 25;
			localBounds = _filterBox.LocalBounds;
			clearButton.Location = new Point(num, ((Rectangle)(ref localBounds)).get_Top() + 8);
		}

		public void FilterCharacters(object sender = null, EventArgs e = null)
		{
			_filterCharacters = true;
		}

		public void PerformFiltering()
		{
			Regex regex = (_settings.FilterAsOne.Value ? new Regex("^(?!\\s*$).+") : new Regex("\\w+|\"[\\w\\s]*\""));
			List<Match> strings = regex.Matches(_filterBox.Text.Trim().ToLower()).Cast<Match>().ToList();
			new List<string>();
			List<KeyValuePair<string, SearchFilter<Character_Model>>> stringFilters = new List<KeyValuePair<string, SearchFilter<Character_Model>>>();
			_003C_003Ec__DisplayClass68_0 CS_0024_003C_003E8__locals0;
			foreach (Match match in strings)
			{
				string string_text = SearchableString(match.ToString().Replace("\"", ""));
				if (_settings.DisplayToggles.Value["Name"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Name", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.Name).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.Value["Profession"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Specialization", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.SpecializationName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.Value["Profession"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Profession", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.ProfessionName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.Value["Level"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Level", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.Level.ToString()).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.Value["Race"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Race", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.RaceName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.Value["Map"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Map", new SearchFilter<Character_Model>((Character_Model c) => SearchableString(c.MapName).Contains(string_text), enabled: true)));
				}
				if (_settings.DisplayToggles.Value["Gender"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Gender", new SearchFilter<Character_Model>(delegate(Character_Model c)
					{
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						//IL_000c: Unknown result type (might be due to invalid IL or missing references)
						_003C_003Ec__DisplayClass68_0 _003C_003Ec__DisplayClass68_ = CS_0024_003C_003E8__locals0;
						Gender gender = c.Gender;
						return SearchableString(((object)(Gender)(ref gender)).ToString()).Contains(string_text);
					}, enabled: true)));
				}
				if (_settings.DisplayToggles.Value["CraftingProfession"].Check)
				{
					stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("CraftingProfession", new SearchFilter<Character_Model>(delegate(Character_Model c)
					{
						foreach (KeyValuePair<int, Data.CraftingProfession> current3 in c.CraftingDisciplines)
						{
							if ((!_settings.DisplayToggles.Value["OnlyMaxCrafting"].Check || current3.Key == current3.Value.MaxRating) && SearchableString(current3.Value.Name).Contains(string_text))
							{
								return true;
							}
						}
						return false;
					}, enabled: true)));
				}
				if (!_settings.DisplayToggles.Value["Tags"].Check)
				{
					continue;
				}
				stringFilters.Add(new KeyValuePair<string, SearchFilter<Character_Model>>("Tags", new SearchFilter<Character_Model>(delegate(Character_Model c)
				{
					foreach (string current2 in c.Tags)
					{
						if (SearchableString(current2).Contains(string_text))
						{
							return true;
						}
					}
					return false;
				}, enabled: true)));
			}
			bool matchAny = _settings.ResultMatchingBehavior.Value == Settings.MatchingBehavior.MatchAny;
			bool matchAll = _settings.ResultMatchingBehavior.Value == Settings.MatchingBehavior.MatchAll;
			bool include = _settings.ResultFilterBehavior.Value == Settings.FilterBehavior.Include;
			List<KeyValuePair<string, SearchFilter<Character_Model>>> toggleFilters = _searchFilters.Where((KeyValuePair<string, SearchFilter<Character_Model>> e) => e.Value.IsEnabled).ToList();
			List<KeyValuePair<string, SearchFilter<Character_Model>>> tagFilters = _tagFilters.Where((KeyValuePair<string, SearchFilter<Character_Model>> e) => e.Value.IsEnabled).ToList();
			foreach (var item in from CharacterCard ctrl in CharactersPanel.Children
				let c = ctrl.Character
				where c != null
				let toggleResult = toggleFilters.Count == 0 || include == FilterResult(c)
				let stringsResult = stringFilters.Count == 0 || include == StringFilterResult(c)
				let tagsResult = tagFilters.Count == 0 || include == TagResult(c)
				let showResult = (from e in toggleFilters
					where e.Value.IsEnabled
					select e.Key).Contains("Hidden") || c.Show
				let betaResult = (_data.StaticInfo.IsBeta && c.Beta) || (from e in toggleFilters
					where e.Value.IsEnabled
					select e.Key).Contains("Hidden") || !c.Beta
				select (ctrl, toggleResult, stringsResult, tagsResult, showResult, betaResult))
			{
				CharacterCard ctrl2 = item.Item1;
				bool toggleResult = item.Item2;
				bool stringsResult = item.Item3;
				bool tagsResult = item.Item4;
				bool showResult = item.Item5;
				bool betaResult = item.Item6;
				ctrl2.Visible = toggleResult && stringsResult && tagsResult && showResult && betaResult;
			}
			SortCharacters();
			CharactersPanel.Invalidate();
			_clearButton.Visible = stringFilters.Count > 0 || toggleFilters.Count > 0 || tagFilters.Count > 0;
			bool FilterResult(Character_Model c)
			{
				List<bool> results3 = new List<bool>();
				foreach (KeyValuePair<string, SearchFilter<Character_Model>> item2 in toggleFilters)
				{
					bool result2 = item2.Value.CheckForMatch(c);
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
				return (_settings.FilterDiacriticsInsensitive.Value ? s.RemoveDiacritics() : s).ToLower();
			}
			bool StringFilterResult(Character_Model c)
			{
				List<bool> results2 = new List<bool>();
				foreach (KeyValuePair<string, SearchFilter<Character_Model>> item3 in stringFilters)
				{
					bool matched = item3.Value.CheckForMatch(c);
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
				foreach (KeyValuePair<string, SearchFilter<Character_Model>> item4 in tagFilters)
				{
					bool result = item4.Value.CheckForMatch(c);
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

		public void CreateCharacterControls()
		{
			if (SideMenu.Tabs.Count <= 1 || CharacterCards == null)
			{
				return;
			}
			foreach (Character_Model c in CharacterModels.Except<Character_Model>(LoadedModels))
			{
				LoadedModels.Add(c);
				CharacterCards.Add(new CharacterCard(_currentCharacter, _textureManager, _data, this, _settings)
				{
					Character = c,
					Parent = CharactersPanel,
					AttachedCards = CharacterCards
				});
			}
			FilterCharacters();
			CharacterCards.FirstOrDefault()?.UniformWithAttached();
		}

		public void SortCharacters()
		{
			Settings.SortBy sortby = _settings.SortType.Value;
			bool asc = _settings.SortOrder.Value == Settings.SortDirection.Ascending;
			bool isNum = ((sortby == Settings.SortBy.Level || (uint)(sortby - 4) <= 1u || (uint)(sortby - 10) <= 1u) ? true : false);
			if (_settings.SortType.Value != Settings.SortBy.Custom)
			{
				CharactersPanel.SortChildren(delegate(CharacterCard a, CharacterCard b)
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
			}
			else
			{
				CharactersPanel.SortChildren((CharacterCard a, CharacterCard b) => a.Index.CompareTo(b.Index));
			}
			string getString(Character_Model c)
			{
				string sortType = ((_settings.SortType.Value == Settings.SortBy.NextBirthday) ? "SecondsUntilNextBirthday" : _settings.SortType.Value.ToString());
				return Common.GetPropertyValueAsString(c, sortType);
			}
			int getValue(Character_Model c)
			{
				string sortType2 = ((_settings.SortType.Value == Settings.SortBy.NextBirthday) ? "SecondsUntilNextBirthday" : _settings.SortType.Value.ToString());
				return Common.GetPropertyValue<int>(c, sortType2);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			base.BasicTooltipText = ((base.MouseOverTitleBar && base.Version != null) ? $"v. {base.Version}" : null);
			if (_filterCharacters && gameTime.get_TotalGameTime().TotalMilliseconds - _filterTick > (double)_settings.FilterDelay.Value)
			{
				_filterTick = gameTime.get_TotalGameTime().TotalMilliseconds;
				_filterCharacters = false;
				PerformFiltering();
			}
			if (gameTime.get_TotalGameTime().TotalMilliseconds - lastUniform > 1000.0)
			{
				lastUniform = gameTime.get_TotalGameTime().TotalMilliseconds;
				CharacterCards.FirstOrDefault()?.UniformWithAttached();
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
		}

		protected override void OnShown(EventArgs e)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			base.OnShown(e);
			if (_settings.FocusSearchOnShow.Value)
			{
				foreach (Keys item in GameService.Input.Keyboard.KeysDown)
				{
					Keyboard.Release((VirtualKeyShort)item);
					Keyboard.Release((VirtualKeyShort)item, sendToSystem: true);
				}
				_filterBox.Focused = true;
			}
			CharacterCards.FirstOrDefault()?.UniformWithAttached();
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
			base.OnResized(e);
			if (_created)
			{
				Rectangle val;
				if (ContentPanel != null)
				{
					Kenedia.Modules.Core.Controls.FlowPanel contentPanel = ContentPanel;
					val = base.ContentRegion;
					int x = ((Rectangle)(ref val)).get_Size().X;
					val = base.ContentRegion;
					contentPanel.Size = new Point(x, ((Rectangle)(ref val)).get_Size().Y - 35);
				}
				if (_dropdownPanel != null)
				{
					_filterBox.Width = _dropdownPanel.Width - _buttonPanel.Width - 2;
					ImageButton clearButton = _clearButton;
					val = _filterBox.LocalBounds;
					int num = ((Rectangle)(ref val)).get_Right() - 23;
					val = _filterBox.LocalBounds;
					clearButton.Location = new Point(num, ((Rectangle)(ref val)).get_Top() + 8);
				}
				if (e.CurrentSize.Y < 135)
				{
					base.Size = new Point(base.Size.X, 135);
				}
				_settings.WindowSize.Value = base.Size;
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_settings.PinSideMenus.SettingChanged -= PinSideMenus_SettingChanged;
			_settings.ShowRandomButton.SettingChanged -= ShowRandomButton_SettingChanged;
			_settings.ShowLastButton.SettingChanged -= ShowLastButton_SettingChanged;
			_settings.FilterDelay.SettingChanged -= FilterDelay_SettingChanged;
			GameService.Gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
			DraggingControl.LeftMouseButtonReleased -= DraggingControl_LeftMouseButtonReleased;
			_buttonPanel.Resized -= ButtonPanel_Resized;
			_collapseWrapper.Hidden -= CollapseWrapper_Hidden;
			_collapseWrapper?.Dispose();
			ContentPanel?.Dispose();
			CharactersPanel?.Dispose();
			DraggingControl?.Dispose();
			CharacterEdit?.Dispose();
			_dropdownPanel?.Dispose();
			_displaySettingsButton?.Dispose();
			_filterBox?.Dispose();
			SideMenu?.Dispose();
		}

		private async void FilterBox_EnterPressed(string t)
		{
			if (!_settings.EnterToLogin.Value)
			{
				return;
			}
			_filterBox.ForceFilter();
			CharacterCard c = (CharacterCard)CharactersPanel.Children.Where((Control e) => e.Visible).FirstOrDefault();
			_filterBox.UnsetFocus();
			await Task.Delay(5);
			if (await ExtendedInputService.WaitForNoKeyPressed())
			{
				c?.Character.Swap();
				if (c == null)
				{
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Debug("No character found to swap to.");
				}
			}
		}

		private void FilterBox_Click()
		{
			if (_settings.OpenSidemenuOnSearch.Value)
			{
				ShowAttached(SideMenu);
			}
		}

		private void DraggingControl_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			SetNewIndex(DraggingControl.CharacterControl);
			DraggingControl.EndDragging();
		}

		private void SetNewIndex(CharacterCard characterControl)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			MouseHandler i = Control.Input.Mouse;
			IEnumerable<CharacterCard> enumerable = CharactersPanel.Children.Cast<CharacterCard>();
			int newIndex = -1;
			int index = 0;
			foreach (CharacterCard c in enumerable)
			{
				Rectangle absoluteBounds = c.AbsoluteBounds;
				if (((Rectangle)(ref absoluteBounds)).Contains(i.Position) && newIndex == -1)
				{
					characterControl.Index = c.Index;
					characterControl.Character.Index = c.Index;
					newIndex = c.Index;
				}
				if (newIndex == index)
				{
					index++;
				}
				if (c != characterControl)
				{
					c.Character.SetIndex(index);
					c.Index = index;
					index++;
				}
			}
			SortCharacters();
		}

		public void SendTesseractFailedNotification(string pathToEngine)
		{
			if (_tesseractFailedNotification == null)
			{
				_tesseractFailedNotification = new TesseractFailedNotification
				{
					Parent = _notifications,
					Height = 25,
					PathToEngine = pathToEngine,
					ClickAction = _toggleOCR
				};
			}
			if (_settings.ShowNotifications.Value && _notifications.Children.Count > 0 && _notifications.Children.Any((Control e) => e.Visible))
			{
				_collapseWrapper.Show();
				ContentPanel.Invalidate();
			}
		}

		public void SendAPITimeoutNotification()
		{
			if (_apiTimeoutNotification == null)
			{
				_apiTimeoutNotification = new APITimeoutNotification
				{
					Parent = _notifications,
					Height = 25
				};
			}
			if (_settings.ShowNotifications.Value && _notifications.Children.Count > 0 && _notifications.Children.Any((Control e) => e.Visible))
			{
				_collapseWrapper.Show();
				ContentPanel.Invalidate();
			}
		}

		public void SendAPIPermissionNotification()
		{
			if (_apiPermissionNotification == null)
			{
				_apiPermissionNotification = new APIPermissionNotification
				{
					Parent = _notifications,
					Height = 25
				};
			}
			if (_settings.ShowNotifications.Value && _notifications.Children.Count > 0 && _notifications.Children.Any((Control e) => e.Visible))
			{
				_collapseWrapper.Show();
				ContentPanel.Invalidate();
			}
		}

		public void CheckOCRRegion()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			Rectangle defaultRect = default(Rectangle);
			((Rectangle)(ref defaultRect))._002Ector(50, (GameService.Graphics.Resolution.Y - 350) / 2, 530, 50);
			if (_settings.UseOCR.Value)
			{
				Rectangle activeOCRRegion = _settings.ActiveOCRRegion;
				if (((Rectangle)(ref activeOCRRegion)).Equals(defaultRect))
				{
					new OCRSetupNotification
					{
						Resolution = _settings.OCRKey,
						Parent = _notifications,
						Height = 25,
						ClickAction = delegate
						{
							_toggleOCR?.Invoke();
							SettingsWindow?.Show();
						}
					};
				}
			}
			if (_settings.ShowNotifications.Value && _notifications.Children.Count > 0 && _notifications.Children.Any((Control e) => e.Visible))
			{
				_collapseWrapper.Show();
				ContentPanel.Invalidate();
			}
			if (_collapseWrapper.Collapsed)
			{
				_collapseWrapper.Height = _collapseWrapper.TitleBarHeight;
			}
		}

		public void UpdateMissingNotification()
		{
			_notifications.UpdateCharacters();
			if (_settings.ShowNotifications.Value && _notifications.Children.Count > 0 && _notifications.Children.Any((Control e) => e.Visible))
			{
				_collapseWrapper.Show();
				ContentPanel.Invalidate();
			}
			if (_collapseWrapper.Collapsed)
			{
				_collapseWrapper.Height = _collapseWrapper.TitleBarHeight;
			}
		}

		public void AdjustForBeta()
		{
			LoadedModels?.Clear();
			CharactersPanel?.ClearChildren();
			CharacterCards?.Clear();
			CreateCharacterControls();
		}
	}
}
