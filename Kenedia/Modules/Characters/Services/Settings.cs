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
			Custom
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

		private readonly SettingCollection _settings;

		public SettingCollection AccountSettings { get; private set; }

		public SettingEntry<Dictionary<string, ShowCheckPair>> DisplayToggles { get; set; }

		public SettingEntry<bool> PinSideMenus { get; set; }

		public SettingEntry<bool> CloseWindowOnSwap { get; set; }

		public SettingEntry<bool> FilterDiacriticsInsensitive { get; set; }

		public SettingEntry<bool> ShowRandomButton { get; set; }

		public SettingEntry<bool> ShowLastButton { get; set; }

		public SettingEntry<bool> ShowCornerIcon { get; set; }

		public Point CurrentWindowSize => WindowSize.get_Value();

		public SettingEntry<Point> WindowSize { get; set; }

		public SettingEntry<RectangleDimensions> WindowOffset { get; set; }

		public SettingEntry<bool> ShowStatusWindow { get; set; }

		public SettingEntry<bool> ShowChoyaSpinner { get; set; }

		public SettingEntry<bool> AutoSortCharacters { get; set; }

		public SettingEntry<bool> CancelOnlyOnESC { get; set; }

		public SettingEntry<bool> AutomaticCharacterDelete { get; }

		public SettingEntry<bool> ShowNotifications { get; }

		public SettingEntry<bool> UseOCR { get; set; }

		public SettingEntry<bool> WindowedMode { get; set; }

		public string OCRKey
		{
			get
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				Point res = GameService.Graphics.get_Resolution();
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
				Point res = GameService.Graphics.get_Resolution();
				Dictionary<string, Rectangle> regions = OCRRegions.get_Value();
				if (!regions.ContainsKey(OCRKey))
				{
					return new Rectangle(50, (res.Y - 350) / 2, 530, 50);
				}
				return regions[OCRKey];
			}
		}

		public SettingEntry<Rectangle> OCRRegion { get; set; }

		public SettingEntry<Dictionary<string, Rectangle>> OCRRegions { get; set; }

		public SettingEntry<bool> EnableRadialMenu { get; set; }

		public SettingEntry<bool> Radial_UseProfessionIconsColor { get; set; }

		public SettingEntry<bool> Radial_UseProfessionIcons { get; set; }

		public SettingEntry<bool> Radial_ShowAdvancedTooltip { get; set; }

		public SettingEntry<bool> Radial_UseProfessionColor { get; set; }

		public SettingEntry<bool> UseCharacterIconsOnRadial { get; set; }

		public SettingEntry<float> Radial_Scale { get; set; }

		public SettingEntry<Color> Radial_IdleColor { get; set; }

		public SettingEntry<Color> Radial_IdleBorderColor { get; set; }

		public SettingEntry<Color> Radial_HoveredBorderColor { get; set; }

		public SettingEntry<Color> Radial_HoveredColor { get; set; }

		public SettingEntry<bool> UseBetaGamestate { get; set; }

		public SettingEntry<bool> CharacterPanelFixedWidth { get; set; }

		public SettingEntry<int> CharacterPanelWidth { get; set; }

		public SettingEntry<int> OCRNoPixelColumns { get; set; }

		public SettingEntry<int> CustomCharacterIconSize { get; set; }

		public SettingEntry<int> CustomCharacterFontSize { get; set; }

		public SettingEntry<int> CustomCharacterNameFontSize { get; set; }

		public SettingEntry<PanelSizes> PanelSize { get; set; }

		public SettingEntry<CharacterPanelLayout> PanelLayout { get; set; }

		public SettingEntry<bool> ShowDetailedTooltip { get; set; }

		public SettingEntry<MatchingBehavior> ResultMatchingBehavior { get; set; }

		public SettingEntry<FilterBehavior> ResultFilterBehavior { get; set; }

		public SettingEntry<SortBy> SortType { get; set; }

		public SettingEntry<SortDirection> SortOrder { get; set; }

		public SettingEntry<KeyBinding> LogoutKey { get; set; }

		public SettingEntry<KeyBinding> ShortcutKey { get; set; }

		public SettingEntry<KeyBinding> RadialKey { get; }

		public SettingEntry<KeyBinding> InventoryKey { get; }

		public SettingEntry<KeyBinding> MailKey { get; }

		public SettingEntry<bool> OnlyEnterOnExact { get; set; }

		public SettingEntry<bool> EnterOnSwap { get; set; }

		public SettingEntry<bool> OpenInventoryOnEnter { get; set; }

		public SettingEntry<bool> DoubleClickToEnter { get; set; }

		public SettingEntry<bool> EnterToLogin { get; set; }

		public SettingEntry<int> CheckDistance { get; set; }

		public SettingEntry<int> SwapDelay { get; set; }

		public SettingEntry<int> KeyDelay { get; set; }

		public SettingEntry<int> FilterDelay { get; set; }

		public SettingEntry<int> OCR_ColorThreshold { get; set; }

		public SettingEntry<bool> FilterAsOne { get; set; }

		public SettingEntry<bool> LoadCachedAccounts { get; set; }

		public SettingEntry<bool> OpenSidemenuOnSearch { get; set; }

		public SettingEntry<Version> Version { get; set; }

		public SettingEntry<Version> ImportVersion { get; set; }

		public SettingEntry<bool> FocusSearchOnShow { get; private set; }

		public event EventHandler AppearanceSettingChanged;

		public Settings(SettingCollection settings)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			SettingCollection internalSettings = settings.AddSubCollection("Internal", false, false);
			Version = internalSettings.DefineSetting<Version>("Version", new Version("0.0.0", false), (Func<string>)null, (Func<string>)null);
			LogoutKey = internalSettings.DefineSetting<KeyBinding>("LogoutKey", new KeyBinding((Keys)123), (Func<string>)null, (Func<string>)null);
			ShortcutKey = internalSettings.DefineSetting<KeyBinding>("ShortcutKey", new KeyBinding((ModifierKeys)4, (Keys)67), (Func<string>)null, (Func<string>)null);
			RadialKey = internalSettings.DefineSetting<KeyBinding>("RadialKey", new KeyBinding((Keys)0), (Func<string>)null, (Func<string>)null);
			InventoryKey = internalSettings.DefineSetting<KeyBinding>("InventoryKey", new KeyBinding((Keys)73), (Func<string>)null, (Func<string>)null);
			MailKey = internalSettings.DefineSetting<KeyBinding>("MailKey", new KeyBinding((Keys)0), (Func<string>)null, (Func<string>)null);
			ShowCornerIcon = internalSettings.DefineSetting<bool>("ShowCornerIcon", true, (Func<string>)null, (Func<string>)null);
			CloseWindowOnSwap = internalSettings.DefineSetting<bool>("CloseWindowOnSwap", false, (Func<string>)null, (Func<string>)null);
			FilterDiacriticsInsensitive = internalSettings.DefineSetting<bool>("FilterDiacriticsInsensitive", false, (Func<string>)null, (Func<string>)null);
			CancelOnlyOnESC = internalSettings.DefineSetting<bool>("CancelOnlyOnESC", false, (Func<string>)null, (Func<string>)null);
			AutomaticCharacterDelete = internalSettings.DefineSetting<bool>("AutomaticCharacterDelete", false, (Func<string>)null, (Func<string>)null);
			ShowNotifications = internalSettings.DefineSetting<bool>("ShowNotifications", true, (Func<string>)null, (Func<string>)null);
			FilterAsOne = internalSettings.DefineSetting<bool>("FilterAsOne", false, (Func<string>)null, (Func<string>)null);
			UseBetaGamestate = internalSettings.DefineSetting<bool>("UseBetaGamestate", true, (Func<string>)null, (Func<string>)null);
			EnableRadialMenu = internalSettings.DefineSetting<bool>("EnableRadialMenu", true, (Func<string>)null, (Func<string>)null);
			Radial_Scale = internalSettings.DefineSetting<float>("Radial_Scale", 0.66f, (Func<string>)null, (Func<string>)null);
			Radial_HoveredBorderColor = internalSettings.DefineSetting<Color>("Radial_HoveredBorderColor", Colors.ColonialWhite, (Func<string>)null, (Func<string>)null);
			Radial_HoveredColor = internalSettings.DefineSetting<Color>("Radial_HoveredBorderColor", Colors.ColonialWhite * 0.8f, (Func<string>)null, (Func<string>)null);
			Radial_IdleColor = internalSettings.DefineSetting<Color>("Radial_IdleColor", Color.get_Black() * 0.8f, (Func<string>)null, (Func<string>)null);
			Radial_IdleBorderColor = internalSettings.DefineSetting<Color>("Radial_IdleBorderColor", Colors.ColonialWhite, (Func<string>)null, (Func<string>)null);
			Radial_UseProfessionColor = internalSettings.DefineSetting<bool>("Radial_UseProfessionColor", false, (Func<string>)null, (Func<string>)null);
			Radial_UseProfessionIcons = internalSettings.DefineSetting<bool>("Radial_UseProfessionIcons", false, (Func<string>)null, (Func<string>)null);
			Radial_UseProfessionIconsColor = internalSettings.DefineSetting<bool>("Radial_UseProfessionIconsColor", false, (Func<string>)null, (Func<string>)null);
			Radial_ShowAdvancedTooltip = internalSettings.DefineSetting<bool>("Radial_ShowAdvancedTooltip", true, (Func<string>)null, (Func<string>)null);
			LoadCachedAccounts = internalSettings.DefineSetting<bool>("LoadCachedAccounts", true, (Func<string>)null, (Func<string>)null);
			OpenSidemenuOnSearch = internalSettings.DefineSetting<bool>("OpenSidemenuOnSearch", true, (Func<string>)null, (Func<string>)null);
			FocusSearchOnShow = internalSettings.DefineSetting<bool>("FocusSearchOnShow", false, (Func<string>)null, (Func<string>)null);
			ShowRandomButton = internalSettings.DefineSetting<bool>("ShowRandomButton", true, (Func<string>)null, (Func<string>)null);
			ShowLastButton = internalSettings.DefineSetting<bool>("ShowLastButton", false, (Func<string>)null, (Func<string>)null);
			ShowStatusWindow = internalSettings.DefineSetting<bool>("ShowStatusWindow", true, (Func<string>)null, (Func<string>)null);
			ShowChoyaSpinner = internalSettings.DefineSetting<bool>("ShowChoyaSpinner", true, (Func<string>)null, (Func<string>)null);
			EnterOnSwap = internalSettings.DefineSetting<bool>("EnterOnSwap", true, (Func<string>)null, (Func<string>)null);
			OnlyEnterOnExact = internalSettings.DefineSetting<bool>("OnlyEnterOnExact", false, (Func<string>)null, (Func<string>)null);
			OpenInventoryOnEnter = internalSettings.DefineSetting<bool>("OpenInventoryOnEnter", false, (Func<string>)null, (Func<string>)null);
			DoubleClickToEnter = internalSettings.DefineSetting<bool>("DoubleClickToEnter", false, (Func<string>)null, (Func<string>)null);
			EnterToLogin = internalSettings.DefineSetting<bool>("EnterToLogin", false, (Func<string>)null, (Func<string>)null);
			CheckDistance = internalSettings.DefineSetting<int>("CheckDistance", 5, (Func<string>)null, (Func<string>)null);
			SwapDelay = internalSettings.DefineSetting<int>("SwapDelay", 250, (Func<string>)null, (Func<string>)null);
			KeyDelay = internalSettings.DefineSetting<int>("KeyDelay", 10, (Func<string>)null, (Func<string>)null);
			FilterDelay = internalSettings.DefineSetting<int>("FilterDelay", 0, (Func<string>)null, (Func<string>)null);
			WindowSize = internalSettings.DefineSetting<Point>("CurrentWindowSize", new Point(385, 920), (Func<string>)null, (Func<string>)null);
			WindowOffset = internalSettings.DefineSetting<RectangleDimensions>("WindowOffset", new RectangleDimensions(8, 31, -8, -8), (Func<string>)null, (Func<string>)null);
			DisplayToggles = internalSettings.DefineSetting<Dictionary<string, ShowCheckPair>>("DisplayToggles", new Dictionary<string, ShowCheckPair>(), (Func<string>)null, (Func<string>)null);
			GameService.Graphics.get_Resolution();
			PinSideMenus = internalSettings.DefineSetting<bool>("PinSideMenus", false, (Func<string>)null, (Func<string>)null);
			UseOCR = internalSettings.DefineSetting<bool>("UseOCR", false, (Func<string>)null, (Func<string>)null);
			AutoSortCharacters = internalSettings.DefineSetting<bool>("AutoSortCharacters", false, (Func<string>)null, (Func<string>)null);
			OCRRegion = internalSettings.DefineSetting<Rectangle>("OCRRegion", new Rectangle(50, 550, 530, 50), (Func<string>)null, (Func<string>)null);
			OCRRegions = internalSettings.DefineSetting<Dictionary<string, Rectangle>>("OCRRegions", new Dictionary<string, Rectangle>(), (Func<string>)null, (Func<string>)null);
			OCRNoPixelColumns = internalSettings.DefineSetting<int>("OCRNoPixelColumns", 20, (Func<string>)null, (Func<string>)null);
			OCR_ColorThreshold = internalSettings.DefineSetting<int>("OCR_ColorThreshold", 181, (Func<string>)null, (Func<string>)null);
			PanelSize = internalSettings.DefineSetting<PanelSizes>("PanelSize", PanelSizes.Normal, (Func<string>)null, (Func<string>)null);
			CustomCharacterIconSize = internalSettings.DefineSetting<int>("CustomCharacterIconSize", 128, (Func<string>)null, (Func<string>)null);
			CustomCharacterFontSize = internalSettings.DefineSetting<int>("CustomCharacterFontSize", 16, (Func<string>)null, (Func<string>)null);
			CustomCharacterNameFontSize = internalSettings.DefineSetting<int>("CustomCharacterNameFontSize", 18, (Func<string>)null, (Func<string>)null);
			PanelLayout = internalSettings.DefineSetting<CharacterPanelLayout>("PanelLayout", CharacterPanelLayout.IconAndText, (Func<string>)null, (Func<string>)null);
			CharacterPanelFixedWidth = internalSettings.DefineSetting<bool>("CharacterPanelFixedWidth", false, (Func<string>)null, (Func<string>)null);
			CharacterPanelWidth = internalSettings.DefineSetting<int>("CharacterPanelWidth", 300, (Func<string>)null, (Func<string>)null);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)DisplayToggles);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)PanelSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)CustomCharacterIconSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)CustomCharacterFontSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)CustomCharacterNameFontSize);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)PanelLayout);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)CharacterPanelFixedWidth);
			((Collection<SettingEntry>)(object)_appearanceSettings).Add((SettingEntry)(object)CharacterPanelWidth);
			ShowDetailedTooltip = internalSettings.DefineSetting<bool>("ShowDetailedTooltip", true, (Func<string>)null, (Func<string>)null);
			ResultMatchingBehavior = internalSettings.DefineSetting<MatchingBehavior>("ResultMatchingBehavior", MatchingBehavior.MatchAny, (Func<string>)null, (Func<string>)null);
			ResultFilterBehavior = internalSettings.DefineSetting<FilterBehavior>("ResultFilterBehavior", FilterBehavior.Include, (Func<string>)null, (Func<string>)null);
			SortType = internalSettings.DefineSetting<SortBy>("SortType", SortBy.TimeSinceLogin, (Func<string>)null, (Func<string>)null);
			SortOrder = internalSettings.DefineSetting<SortDirection>("SortOrder", SortDirection.Ascending, (Func<string>)null, (Func<string>)null);
			foreach (SettingEntry item in (Collection<SettingEntry>)(object)_appearanceSettings)
			{
				item.add_PropertyChanged((PropertyChangedEventHandler)OnAppearanceSettingChanged);
			}
			_appearanceSettings.add_ItemAdded((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemAdded);
			_appearanceSettings.add_ItemRemoved((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemRemoved);
			_settings = settings;
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			foreach (SettingEntry item in (Collection<SettingEntry>)(object)_appearanceSettings)
			{
				item.remove_PropertyChanged((PropertyChangedEventHandler)OnAppearanceSettingChanged);
			}
			_appearanceSettings.remove_ItemAdded((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemAdded);
			_appearanceSettings.remove_ItemRemoved((EventHandler<ItemEventArgs<SettingEntry>>)AppearanceSettings_ItemRemoved);
		}

		private void AppearanceSettings_ItemRemoved(object sender, ItemEventArgs<SettingEntry> e)
		{
			e.get_Item().remove_PropertyChanged((PropertyChangedEventHandler)OnAppearanceSettingChanged);
		}

		private void AppearanceSettings_ItemAdded(object sender, ItemEventArgs<SettingEntry> e)
		{
			e.get_Item().add_PropertyChanged((PropertyChangedEventHandler)OnAppearanceSettingChanged);
		}

		private void OnAppearanceSettingChanged(object sender, PropertyChangedEventArgs e)
		{
			this.AppearanceSettingChanged?.Invoke(sender, e);
		}

		public void LoadAccountSettings(string accountName)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			AccountSettings = _settings.AddSubCollection(accountName, false, false);
			ImportVersion = AccountSettings.DefineSetting<Version>("ImportVersion", new Version("0.0.0", false), (Func<string>)null, (Func<string>)null);
		}
	}
}
