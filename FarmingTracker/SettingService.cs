using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Input;

namespace FarmingTracker
{
	public class SettingService
	{
		private const string DRAG_WITH_MOUSE_LABEL_TEXT = "drag with mouse";

		private const string SETTINGS_VERSION_SETTING_KEY = "settings version";

		public SettingEntry<string> DrfTokenSetting { get; }

		public SettingEntry<AutomaticReset> AutomaticResetSetting { get; }

		public SettingEntry<int> MinutesUntilResetAfterModuleShutdownSetting { get; }

		public SettingEntry<KeyBinding> WindowVisibilityKeyBindingSetting { get; }

		public SettingEntry<bool> IsFakeDrfServerUsedSetting { get; }

		public SettingEntry<int> SettingsVersionSetting { get; private set; }

		public SettingEntry<DateTime> NextResetDateTimeUtcSetting { get; private set; }

		public SettingEntry<TimeSpan> FarmingDurationTimeSpanSetting { get; private set; }

		public SettingEntry<List<SortByWithDirection>> SortByWithDirectionListSetting { get; private set; }

		public SettingEntry<List<CountFilter>> CountFilterSetting { get; private set; }

		public SettingEntry<List<SellMethodFilter>> SellMethodFilterSetting { get; private set; }

		public SettingEntry<List<ItemRarity>> RarityStatsFilterSetting { get; private set; }

		public SettingEntry<List<ItemType>> TypeStatsFilterSetting { get; private set; }

		public SettingEntry<List<ItemFlag>> FlagStatsFilterSetting { get; private set; }

		public SettingEntry<List<CurrencyFilter>> CurrencyFilterSetting { get; private set; }

		public SettingEntry<List<KnownByApiFilter>> KnownByApiFilterSetting { get; private set; }

		public SettingEntry<bool> RarityIconBorderIsVisibleSetting { get; private set; }

		public SettingEntry<int> NegativeCountIconOpacitySetting { get; private set; }

		public SettingEntry<StatIconSize> StatIconSizeSetting { get; private set; }

		public SettingEntry<int> CountBackgroundOpacitySetting { get; private set; }

		public SettingEntry<ColorType> CountBackgroundColorSetting { get; private set; }

		public SettingEntry<ColorType> PositiveCountTextColorSetting { get; private set; }

		public SettingEntry<ColorType> NegativeCountTextColorSetting { get; private set; }

		public SettingEntry<FontSize> CountFontSizeSetting { get; private set; }

		public SettingEntry<HorizontalAlignment> CountHoritzontalAlignmentSetting { get; private set; }

		public SettingEntry<bool> IsProfitWindowVisibleSetting { get; private set; }

		public SettingEntry<bool> DragProfitWindowWithMouseIsEnabledSetting { get; private set; }

		public SettingEntry<WindowAnchor> WindowAnchorSetting { get; private set; }

		public SettingEntry<bool> ProfitWindowCanBeClickedThroughSetting { get; private set; }

		public SettingEntry<int> ProfitWindowBackgroundOpacitySetting { get; private set; }

		public SettingEntry<string> ProfitPerHourLabelTextSetting { get; private set; }

		public SettingEntry<string> ProfitLabelTextSetting { get; private set; }

		public SettingEntry<ProfitWindowDisplayMode> ProfitWindowDisplayModeSetting { get; private set; }

		public SettingEntry<FloatPoint> ProfitWindowRelativeWindowAnchorLocationSetting { get; private set; }

		public SettingService(SettingCollection settings)
		{
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			DrfTokenSetting = settings.DefineSetting<string>("drf token", string.Empty, (Func<string>)(() => "DRF Token/Key"), (Func<string>)(() => "Create an account on https://drf.rs/ and link it with your GW2 Account(s).\nOpen the drf.rs settings page. Click on 'Regenerate Token'.\nCopy the token. Paste the token with CTRL + V into this input."));
			AutomaticResetSetting = settings.DefineSetting<AutomaticReset>("automatic reset", AutomaticReset.OnModuleStart, (Func<string>)(() => "automatic reset"), (Func<string>)(() => "Change when all farmed items are reset. 'Never' means you have to use the 'Reset' button.\nUsing something else than 'on module start' helps to not lose your farming data when gw2, blish or your pc crashes."));
			MinutesUntilResetAfterModuleShutdownSetting = settings.DefineSetting<int>("number of minutes until automatic reset after module shutdown", 30, (Func<string>)(() => "minutes until automatic reset after module shutdown"), (Func<string>)(() => "Change number of minutes the module will wait after module shutdown before doing an automatic reset. This setting can be usefull to not lose tracking progress due to PC / gw2 / blish crashes, but still have automatic resets similar to 'On module start' option. Or if you prefer to play in short sessions spread throughout the day"));
			WindowVisibilityKeyBindingSetting = settings.DefineSetting<KeyBinding>("window visibility key binding", new KeyBinding((ModifierKeys)7, (Keys)70), (Func<string>)(() => "show/hide window"), (Func<string>)(() => "Double-click to change the key binding. Will show or hide the farming tracker window."));
			IsFakeDrfServerUsedSetting = settings.DefineSetting<bool>("is fake drf server used", false, (Func<string>)(() => "fake drf server"), (Func<string>)(() => "fake drf server"));
			SettingCollection internalSettings = DefineHiddenSettings(settings);
			DefineCountSettings(settings);
			DefineIconSettings(settings);
			DefineProfitSettings(settings);
			DefineProfitWindowSettings(settings, internalSettings);
			DefineSortAndFilterSettings(internalSettings);
			MigrateBlishSettingsService.MigrateSettings(SettingsVersionSetting, CurrencyFilterSetting, SellMethodFilterSetting);
		}

		private void DefineSortAndFilterSettings(SettingCollection internalSettings)
		{
			SortByWithDirectionListSetting = internalSettings.DefineSetting<List<SortByWithDirection>>("sort by list", new List<SortByWithDirection>(new SortByWithDirection[2]
			{
				SortByWithDirection.PositiveAndNegativeCount_Descending,
				SortByWithDirection.ApiId_Ascending
			}), (Func<string>)null, (Func<string>)null);
			RemoveUnknownEnumValues<SortByWithDirection>(SortByWithDirectionListSetting);
			CountFilterSetting = internalSettings.DefineSetting<List<CountFilter>>("count stat filter", new List<CountFilter>(Constants.ALL_COUNTS), (Func<string>)null, (Func<string>)null);
			RemoveUnknownEnumValues<CountFilter>(CountFilterSetting);
			SellMethodFilterSetting = internalSettings.DefineSetting<List<SellMethodFilter>>("sellable item filter", new List<SellMethodFilter>(Constants.ALL_SELL_METHODS), (Func<string>)null, (Func<string>)null);
			RemoveUnknownEnumValues<SellMethodFilter>(SellMethodFilterSetting);
			RarityStatsFilterSetting = internalSettings.DefineSetting<List<ItemRarity>>("rarity item filter", new List<ItemRarity>(Constants.ALL_ITEM_RARITIES), (Func<string>)null, (Func<string>)null);
			SettingService.RemoveUnknownEnumValues<ItemRarity>(RarityStatsFilterSetting);
			TypeStatsFilterSetting = internalSettings.DefineSetting<List<ItemType>>("type item filter", new List<ItemType>(Constants.ALL_ITEM_TYPES), (Func<string>)null, (Func<string>)null);
			SettingService.RemoveUnknownEnumValues<ItemType>(TypeStatsFilterSetting);
			FlagStatsFilterSetting = internalSettings.DefineSetting<List<ItemFlag>>("flag item filter", new List<ItemFlag>(Constants.ALL_ITEM_FLAGS), (Func<string>)null, (Func<string>)null);
			SettingService.RemoveUnknownEnumValues<ItemFlag>(FlagStatsFilterSetting);
			CurrencyFilterSetting = internalSettings.DefineSetting<List<CurrencyFilter>>("currency filter", new List<CurrencyFilter>(Constants.ALL_CURRENCIES), (Func<string>)null, (Func<string>)null);
			RemoveUnknownEnumValues<CurrencyFilter>(CurrencyFilterSetting);
			KnownByApiFilterSetting = internalSettings.DefineSetting<List<KnownByApiFilter>>("known by api filter", new List<KnownByApiFilter>(Constants.ALL_KNOWN_BY_API), (Func<string>)null, (Func<string>)null);
			RemoveUnknownEnumValues<KnownByApiFilter>(KnownByApiFilterSetting);
		}

		private SettingCollection DefineHiddenSettings(SettingCollection settings)
		{
			SettingCollection internalSettings = settings.AddSubCollection("internal settings (not visible in UI)", false);
			bool num = internalSettings.get_Item("settings version") == null;
			SettingsVersionSetting = internalSettings.DefineSetting<int>("settings version", 2, (Func<string>)null, (Func<string>)null);
			if (num)
			{
				SettingsVersionSetting.set_Value(1);
			}
			NextResetDateTimeUtcSetting = internalSettings.DefineSetting<DateTime>("next reset dateTimeUtc", NextAutomaticResetCalculator.UNDEFINED_RESET_DATE_TIME, (Func<string>)null, (Func<string>)null);
			FarmingDurationTimeSpanSetting = internalSettings.DefineSetting<TimeSpan>("farming duration time span", TimeSpan.Zero, (Func<string>)null, (Func<string>)null);
			return internalSettings;
		}

		private void DefineIconSettings(SettingCollection settings)
		{
			RarityIconBorderIsVisibleSetting = settings.DefineSetting<bool>("rarity icon border is visible", true, (Func<string>)(() => "rarity colored border"), (Func<string>)(() => "Show a border in rarity color around item icons."));
			StatIconSizeSetting = settings.DefineSetting<StatIconSize>("stat icon size", StatIconSize.M, (Func<string>)(() => "size"), (Func<string>)(() => "Change item/currency icon size."));
			NegativeCountIconOpacitySetting = settings.DefineSetting<int>("negative count icon opacity", 77, (Func<string>)(() => "negative count opacity"), (Func<string>)(() => "Change item/currency icon opacity / transparency for negative counts."));
			SettingComplianceExtensions.SetRange(NegativeCountIconOpacitySetting, 0, 255);
		}

		private void DefineCountSettings(SettingCollection settings)
		{
			CountBackgroundColorSetting = settings.DefineSetting<ColorType>("count background color", ColorType.Black, (Func<string>)(() => "background color"), (Func<string>)(() => "Change item/currency count background color. It is not visible when count background opacity slider is set to full transparency."));
			CountBackgroundOpacitySetting = settings.DefineSetting<int>("count background opacity", 0, (Func<string>)(() => "background opacity"), (Func<string>)(() => "Change item/currency count background opacity / transparency."));
			SettingComplianceExtensions.SetRange(CountBackgroundOpacitySetting, 0, 255);
			PositiveCountTextColorSetting = settings.DefineSetting<ColorType>("positive count text color", ColorType.White, (Func<string>)(() => "positive text color"), (Func<string>)(() => "Change item/currency count text color for positive counts (>0)."));
			NegativeCountTextColorSetting = settings.DefineSetting<ColorType>("negative count text color", ColorType.White, (Func<string>)(() => "negative text color"), (Func<string>)(() => "Change item/currency count text color for negative counts (<0)."));
			CountFontSizeSetting = settings.DefineSetting<FontSize>("count font size", (FontSize)20, (Func<string>)(() => "text size"), (Func<string>)(() => "Change item/currency count font size."));
			CountHoritzontalAlignmentSetting = settings.DefineSetting<HorizontalAlignment>("count horizontal alignment", (HorizontalAlignment)2, (Func<string>)(() => "horizontal alignment"), (Func<string>)(() => "Change item/currency count horizontal alignment. Dont use 'center'. It can cut off at both ends."));
		}

		private void DefineProfitWindowSettings(SettingCollection settings, SettingCollection internalSettings)
		{
			IsProfitWindowVisibleSetting = settings.DefineSetting<bool>("profit window is visible", true, (Func<string>)(() => "show"), (Func<string>)(() => "Show profit window."));
			DragProfitWindowWithMouseIsEnabledSetting = settings.DefineSetting<bool>("dragging profit window is allowed", true, (Func<string>)(() => "drag with mouse"), (Func<string>)(() => "Allow dragging the window by moving the mouse when left mouse button is pressed inside window"));
			WindowAnchorSetting = settings.DefineSetting<WindowAnchor>("window anchor", WindowAnchor.TopLeft, (Func<string>)(() => "window anchor"), (Func<string>)(() => "When the window content grows/shrinks the window anchor is the part of the window that will not move, while the rest of the window will. For example for 'Top..,' the window top will stay in position while the window bottom expands. And for 'Bottom..,' the window bottom will stay in position while the window top expands."));
			ProfitWindowCanBeClickedThroughSetting = settings.DefineSetting<bool>("profit window is not capturing mouse clicks", true, (Func<string>)(() => "mouse clickthrough (disabled when 'drag with mouse' is checked)"), (Func<string>)(() => "This allows clicking with the mouse through the window to interact with Guild Wars 2 behind the window."));
			ProfitWindowBackgroundOpacitySetting = settings.DefineSetting<int>("profit window background opacity", 125, (Func<string>)(() => "background opacity"), (Func<string>)(() => "Change background opacity / transparency."));
			SettingComplianceExtensions.SetRange(ProfitWindowBackgroundOpacitySetting, 0, 255);
			ProfitWindowDisplayModeSetting = settings.DefineSetting<ProfitWindowDisplayMode>("profit window display mode", ProfitWindowDisplayMode.ProfitAndProfitPerHour, (Func<string>)(() => "display mode"), (Func<string>)(() => "Change what should be displayed inside the profit window."));
			ProfitWindowRelativeWindowAnchorLocationSetting = internalSettings.DefineSetting<FloatPoint>("profit window relative location", new FloatPoint(0.2f, 0.2f), (Func<string>)null, (Func<string>)null);
		}

		private void DefineProfitSettings(SettingCollection settings)
		{
			ProfitPerHourLabelTextSetting = settings.DefineSetting<string>("profit per hour label text", "Profit per hour", (Func<string>)(() => "profit per hour label"), (Func<string>)(() => "Change the label for profit per hour. This will affect profit display in profit window and summary tab. You have to click somewhere outside of this text input to see your change. This will not affect the value itself."));
			ProfitLabelTextSetting = settings.DefineSetting<string>("total profit label text", "Profit", (Func<string>)(() => "profit label"), (Func<string>)(() => "Change the label for total profit. This will affect profit display in profit window and summary tab. You have to click somewhere outside of this text input to see your change. This will not affect the value itself."));
		}

		private static void RemoveUnknownEnumValues<T>(SettingEntry<List<T>> ListSetting) where T : Enum
		{
			foreach (T element in new List<T>(ListSetting.get_Value()))
			{
				if (FilterService.IsUnknownFilterElement<T>((int)(object)element))
				{
					ListSetting.get_Value().Remove(element);
				}
			}
		}
	}
}
