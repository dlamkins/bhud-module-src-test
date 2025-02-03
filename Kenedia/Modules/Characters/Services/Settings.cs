using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using SemVer;

namespace Kenedia.Modules.Characters.Services
{
	public class Settings : BaseSettingsModel
	{
		public enum SortDirection
		{
			Ascending,
			Descending
		}

		public enum SortBy
		{
			Name,
			Level,
			Tag,
			Profession,
			TimeSinceLogin,
			Map,
			Race,
			Gender,
			Specialization,
			Custom,
			NextBirthday,
			Age
		}

		public enum FilterBehavior
		{
			Include,
			Exclude
		}

		public enum MatchingBehavior
		{
			MatchAny,
			MatchAll
		}

		public enum CharacterPanelLayout
		{
			OnlyIcons,
			OnlyText,
			IconAndText
		}

		public enum PanelSizes
		{
			Small,
			Normal,
			Large,
			Custom
		}

		private readonly ObservableCollection<SettingEntry> _appearanceSettings = new ObservableCollection<SettingEntry>();

		public SettingCollection AccountSettings { get; private set; }

		public SettingEntry<Dictionary<string, ShowCheckPair>> DisplayToggles { get; private set; }

		public SettingEntry<bool> DebugMode { get; private set; }

		public SettingEntry<bool> PinSideMenus { get; private set; }

		public SettingEntry<bool> IncludeBetaCharacters { get; private set; }

		public SettingEntry<bool> CloseWindowOnSwap { get; private set; }

		public SettingEntry<bool> FilterDiacriticsInsensitive { get; private set; }

		public SettingEntry<bool> ShowRandomButton { get; private set; }

		public SettingEntry<bool> ShowLastButton { get; private set; }

		public SettingEntry<bool> ShowCornerIcon { get; private set; }

		public Point CurrentWindowSize => WindowSize.Value;

		public SettingEntry<Point> WindowSize { get; private set; }

		public SettingEntry<RectangleDimensions> WindowOffset { get; private set; }

		public SettingEntry<bool> ShowStatusWindow { get; private set; }

		public SettingEntry<bool> ShowChoyaSpinner { get; private set; }

		public SettingEntry<bool> AutoSortCharacters { get; private set; }

		public SettingEntry<bool> CancelOnlyOnESC { get; private set; }

		public SettingEntry<bool> AutomaticCharacterDelete { get; private set; }

		public SettingEntry<bool> ShowNotifications { get; private set; }

		public SettingEntry<bool> UseOCR { get; private set; }

		public SettingEntry<bool> WindowedMode { get; private set; }

		public string OCRKey
		{
			get
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				Point res = GameService.Graphics.Resolution;
				return "{" + $"X:{Math.Floor((double)res.X / 10.0) * 10.0} Y:{Math.Floor((double)res.Y / 10.0) * 10.0}" + "}";
			}
		}

		public Rectangle ActiveOCRRegion
		{
			get
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				Point res = GameService.Graphics.Resolution;
				Dictionary<string, Rectangle> regions = OCRRegions.Value;
				if (!regions.ContainsKey(OCRKey))
				{
					return new Rectangle(50, (res.Y - 350) / 2, 530, 50);
				}
				return regions[OCRKey];
			}
		}

		public SettingEntry<Rectangle> OCRRegion { get; private set; }

		public SettingEntry<Dictionary<string, Rectangle>> OCRRegions { get; private set; }

		public SettingEntry<bool> EnableRadialMenu { get; private set; }

		public SettingEntry<bool> Radial_UseProfessionIconsColor { get; private set; }

		public SettingEntry<bool> Radial_UseProfessionIcons { get; private set; }

		public SettingEntry<bool> Radial_ShowAdvancedTooltip { get; private set; }

		public SettingEntry<bool> Radial_UseProfessionColor { get; private set; }

		public SettingEntry<bool> UseCharacterIconsOnRadial { get; private set; }

		public SettingEntry<float> Radial_Scale { get; private set; }

		public SettingEntry<Color> Radial_IdleColor { get; private set; }

		public SettingEntry<Color> Radial_IdleBorderColor { get; private set; }

		public SettingEntry<Color> Radial_HoveredBorderColor { get; private set; }

		public SettingEntry<Color> Radial_HoveredColor { get; private set; }

		public SettingEntry<bool> UseBetaGamestate { get; private set; }

		public SettingEntry<bool> CharacterPanelFixedWidth { get; private set; }

		public SettingEntry<int> CharacterPanelWidth { get; private set; }

		public SettingEntry<int> OCRNoPixelColumns { get; private set; }

		public SettingEntry<int> CustomCharacterIconSize { get; private set; }

		public SettingEntry<int> CustomCharacterFontSize { get; private set; }

		public SettingEntry<int> CustomCharacterNameFontSize { get; private set; }

		public SettingEntry<PanelSizes> PanelSize { get; private set; }

		public SettingEntry<CharacterPanelLayout> PanelLayout { get; private set; }

		public SettingEntry<bool> ShowDetailedTooltip { get; private set; }

		public SettingEntry<MatchingBehavior> ResultMatchingBehavior { get; private set; }

		public SettingEntry<FilterBehavior> ResultFilterBehavior { get; private set; }

		public SettingEntry<SortBy> SortType { get; private set; }

		public SettingEntry<SortDirection> SortOrder { get; private set; }

		public SettingEntry<KeyBinding> LogoutKey { get; private set; }

		public SettingEntry<KeyBinding> ShortcutKey { get; private set; }

		public SettingEntry<KeyBinding> RadialKey { get; private set; }

		public SettingEntry<KeyBinding> InventoryKey { get; private set; }

		public SettingEntry<KeyBinding> MailKey { get; private set; }

		public SettingEntry<bool> OnlyEnterOnExact { get; private set; }

		public SettingEntry<bool> EnterOnSwap { get; private set; }

		public SettingEntry<bool> OpenInventoryOnEnter { get; private set; }

		public SettingEntry<bool> DoubleClickToEnter { get; private set; }

		public SettingEntry<bool> EnterToLogin { get; private set; }

		public SettingEntry<int> CheckDistance { get; private set; }

		public SettingEntry<int> SwapDelay { get; private set; }

		public SettingEntry<int> KeyDelay { get; private set; }

		public SettingEntry<int> FilterDelay { get; private set; }

		public SettingEntry<int> OCR_ColorThreshold { get; private set; }

		public SettingEntry<bool> FilterAsOne { get; private set; }

		public SettingEntry<bool> LoadCachedAccounts { get; private set; }

		public SettingEntry<bool> OpenSidemenuOnSearch { get; private set; }

		public SettingEntry<Version> Version { get; private set; }

		public SettingEntry<Version> ImportVersion { get; private set; }

		public SettingEntry<bool> FocusSearchOnShow { get; private set; }

		public event EventHandler AppearanceSettingChanged;

		public Settings(SettingCollection settings)
			: base(settings)
		{
		}

		protected override void InitializeSettings(SettingCollection settings)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			base.InitializeSettings(settings);
			SettingCollection internalSettings = settings.AddSubCollection("Internal", renderInUi: false, lazyLoaded: false);
			Version = internalSettings.DefineSetting<Version>("Version", new Version("0.0.0", false));
			LogoutKey = internalSettings.DefineSetting("LogoutKey", new KeyBinding((Keys)123));
			ShortcutKey = internalSettings.DefineSetting("ShortcutKey", new KeyBinding(ModifierKeys.Shift, (Keys)67));
			RadialKey = internalSettings.DefineSetting("RadialKey", new KeyBinding((Keys)0));
			InventoryKey = internalSettings.DefineSetting("InventoryKey", new KeyBinding((Keys)73));
			MailKey = internalSettings.DefineSetting("MailKey", new KeyBinding((Keys)0));
			ShowCornerIcon = internalSettings.DefineSetting("ShowCornerIcon", defaultValue: true);
			CloseWindowOnSwap = internalSettings.DefineSetting("CloseWindowOnSwap", defaultValue: false);
			FilterDiacriticsInsensitive = internalSettings.DefineSetting("FilterDiacriticsInsensitive", defaultValue: false);
			CancelOnlyOnESC = internalSettings.DefineSetting("CancelOnlyOnESC", defaultValue: false);
			AutomaticCharacterDelete = internalSettings.DefineSetting("AutomaticCharacterDelete", defaultValue: false);
			ShowNotifications = internalSettings.DefineSetting("ShowNotifications", defaultValue: true);
			IncludeBetaCharacters = internalSettings.DefineSetting("IncludeBetaCharacters", defaultValue: true);
			FilterAsOne = internalSettings.DefineSetting("FilterAsOne", defaultValue: false);
			UseBetaGamestate = internalSettings.DefineSetting("UseBetaGamestate", defaultValue: false);
			DebugMode = internalSettings.DefineSetting("DebugMode", defaultValue: true);
			EnableRadialMenu = internalSettings.DefineSetting("EnableRadialMenu", defaultValue: true);
			Radial_Scale = internalSettings.DefineSetting("Radial_Scale", 0.66f);
			Radial_HoveredBorderColor = internalSettings.DefineSetting<Color>("Radial_HoveredBorderColor", ContentService.Colors.ColonialWhite);
			Radial_HoveredColor = internalSettings.DefineSetting<Color>("Radial_HoveredBorderColor", ContentService.Colors.ColonialWhite * 0.8f);
			Radial_IdleColor = internalSettings.DefineSetting<Color>("Radial_IdleColor", Color.get_Black() * 0.8f);
			Radial_IdleBorderColor = internalSettings.DefineSetting<Color>("Radial_IdleBorderColor", ContentService.Colors.ColonialWhite);
			Radial_UseProfessionColor = internalSettings.DefineSetting("Radial_UseProfessionColor", defaultValue: false);
			Radial_UseProfessionIcons = internalSettings.DefineSetting("Radial_UseProfessionIcons", defaultValue: false);
			Radial_UseProfessionIconsColor = internalSettings.DefineSetting("Radial_UseProfessionIconsColor", defaultValue: false);
			Radial_ShowAdvancedTooltip = internalSettings.DefineSetting("Radial_ShowAdvancedTooltip", defaultValue: true);
			LoadCachedAccounts = internalSettings.DefineSetting("LoadCachedAccounts", defaultValue: true);
			OpenSidemenuOnSearch = internalSettings.DefineSetting("OpenSidemenuOnSearch", defaultValue: true);
			FocusSearchOnShow = internalSettings.DefineSetting("FocusSearchOnShow", defaultValue: false);
			ShowRandomButton = internalSettings.DefineSetting("ShowRandomButton", defaultValue: true);
			ShowLastButton = internalSettings.DefineSetting("ShowLastButton", defaultValue: false);
			ShowStatusWindow = internalSettings.DefineSetting("ShowStatusWindow", defaultValue: true);
			ShowChoyaSpinner = internalSettings.DefineSetting("ShowChoyaSpinner", defaultValue: true);
			EnterOnSwap = internalSettings.DefineSetting("EnterOnSwap", defaultValue: true);
			OnlyEnterOnExact = internalSettings.DefineSetting("OnlyEnterOnExact", defaultValue: false);
			OpenInventoryOnEnter = internalSettings.DefineSetting("OpenInventoryOnEnter", defaultValue: false);
			DoubleClickToEnter = internalSettings.DefineSetting("DoubleClickToEnter", defaultValue: false);
			EnterToLogin = internalSettings.DefineSetting("EnterToLogin", defaultValue: false);
			CheckDistance = internalSettings.DefineSetting("CheckDistance", 5);
			SwapDelay = internalSettings.DefineSetting("SwapDelay", 500);
			KeyDelay = internalSettings.DefineSetting("KeyDelay", 10);
			FilterDelay = internalSettings.DefineSetting("FilterDelay", 0);
			WindowSize = internalSettings.DefineSetting<Point>("CurrentWindowSize", new Point(385, 920));
			WindowOffset = internalSettings.DefineSetting("WindowOffset", new RectangleDimensions(8, 31, -8, -8));
			DisplayToggles = internalSettings.DefineSetting("DisplayToggles", new Dictionary<string, ShowCheckPair>());
			_ = GameService.Graphics.Resolution;
			PinSideMenus = internalSettings.DefineSetting("PinSideMenus", defaultValue: false);
			UseOCR = internalSettings.DefineSetting("UseOCR", defaultValue: false);
			AutoSortCharacters = internalSettings.DefineSetting("AutoSortCharacters", defaultValue: false);
			OCRRegion = internalSettings.DefineSetting<Rectangle>("OCRRegion", new Rectangle(50, 550, 530, 50));
			OCRRegions = internalSettings.DefineSetting("OCRRegions", new Dictionary<string, Rectangle>());
			OCRNoPixelColumns = internalSettings.DefineSetting("OCRNoPixelColumns", 20);
			OCR_ColorThreshold = internalSettings.DefineSetting("OCR_ColorThreshold", 181);
			PanelSize = internalSettings.DefineSetting("PanelSize", PanelSizes.Normal);
			CustomCharacterIconSize = internalSettings.DefineSetting("CustomCharacterIconSize", 128);
			CustomCharacterFontSize = internalSettings.DefineSetting("CustomCharacterFontSize", 16);
			CustomCharacterNameFontSize = internalSettings.DefineSetting("CustomCharacterNameFontSize", 18);
			PanelLayout = internalSettings.DefineSetting("PanelLayout", CharacterPanelLayout.IconAndText);
			CharacterPanelFixedWidth = internalSettings.DefineSetting("CharacterPanelFixedWidth", defaultValue: false);
			CharacterPanelWidth = internalSettings.DefineSetting("CharacterPanelWidth", 300);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)DisplayToggles);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)PanelSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)CustomCharacterIconSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)CustomCharacterFontSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)CustomCharacterNameFontSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)PanelLayout);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)CharacterPanelFixedWidth);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)CharacterPanelWidth);
			ShowDetailedTooltip = internalSettings.DefineSetting("ShowDetailedTooltip", defaultValue: true);
			ResultMatchingBehavior = internalSettings.DefineSetting("ResultMatchingBehavior", MatchingBehavior.MatchAny);
			ResultFilterBehavior = internalSettings.DefineSetting("ResultFilterBehavior", FilterBehavior.Include);
			SortType = internalSettings.DefineSetting("SortType", SortBy.TimeSinceLogin);
			SortOrder = internalSettings.DefineSetting("SortOrder", SortDirection.Ascending);
			foreach (SettingEntry item in (Collection<SettingEntry>)(object)_appearanceSettings)
			{
				item.PropertyChanged += OnAppearanceSettingChanged;
			}
			_appearanceSettings.add_ItemAdded((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemAdded);
			_appearanceSettings.add_ItemRemoved((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemRemoved);
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			foreach (SettingEntry item in (Collection<SettingEntry>)(object)_appearanceSettings)
			{
				item.PropertyChanged -= OnAppearanceSettingChanged;
			}
			_appearanceSettings.remove_ItemAdded((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemAdded);
			_appearanceSettings.remove_ItemRemoved((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemRemoved);
		}

		private void AppearanceSettings_ItemRemoved(object sender, ItemEventArgs<SettingEntry> e)
		{
			e.get_Item().PropertyChanged -= OnAppearanceSettingChanged;
		}

		private void AppearanceSettings_ItemAdded(object sender, ItemEventArgs<SettingEntry> e)
		{
			e.get_Item().PropertyChanged += OnAppearanceSettingChanged;
		}

		private void OnAppearanceSettingChanged(object sender, PropertyChangedEventArgs e)
		{
			this.AppearanceSettingChanged?.Invoke(sender, e);
		}

		public void LoadAccountSettings(string accountName)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			AccountSettings = base.SettingCollection.AddSubCollection(accountName, renderInUi: false, lazyLoaded: false);
			ImportVersion = AccountSettings.DefineSetting<Version>("ImportVersion", new Version("0.0.0", false));
		}
	}
}
