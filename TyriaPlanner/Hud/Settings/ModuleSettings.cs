using System;
using Blish_HUD;
using Blish_HUD.Settings;
using MonoGame.Extended.BitmapFonts;

namespace TyriaPlanner.Hud.Settings
{
	public sealed class ModuleSettings
	{
		public SettingCollection Root { get; }

		public SettingEntry<string> ApiBaseUrl { get; }

		public SettingEntry<string> Gw2ApiKey { get; }

		public SettingEntry<string> CachedBearer { get; }

		public SettingEntry<bool> NotifyOwnSignups { get; }

		public SettingEntry<bool> NotifyNewGuildEvents { get; }

		public SettingEntry<bool> NotifyGuildAnnouncements { get; }

		public SettingEntry<int> PollIntervalSeconds { get; }

		public SettingEntry<FontSizePreference> FontSize { get; }

		public SettingEntry<ToastPositionPreference> ToastPosition { get; }

		public SettingEntry<bool> PlaySoundOnToast { get; }

		public SettingEntry<bool> NotifyWeeklyReset { get; }

		public SettingEntry<bool> PauseInCombat { get; }

		public SettingEntry<ColorThemePreference> ColorTheme { get; }

		public ModuleSettings(SettingCollection root)
		{
			Root = root;
			ApiBaseUrl = root.DefineSetting<string>("ApiBaseUrl", "https://tyriaplanner.com", (Func<string>)(() => "API base URL"), (Func<string>)(() => "Leave the default unless you're testing against a dev server."));
			Gw2ApiKey = root.DefineSetting<string>("Gw2ApiKey", string.Empty, (Func<string>)(() => "GW2 API key"), (Func<string>)(() => "Must be the SAME GW2 API key you already saved on your tyriaplanner.com profile · the server only recognises keys it has on file. Required ArenaNet permissions: account, characters, progression. Optional: builds, wallet, inventories (for the full website experience). Generate at https://account.arena.net/applications. The addon sends this key to Tyria Planner exactly once · after that, every poll uses a scoped, revocable bearer."));
			CachedBearer = root.DefineSetting<string>("CachedBearer", string.Empty, (Func<string>)null, (Func<string>)null);
			NotifyOwnSignups = root.DefineSetting<bool>("NotifyOwnSignups", true, (Func<string>)(() => "Notify upcoming signups"), (Func<string>)(() => "Pops a toast for events you signed up to as they approach."));
			NotifyNewGuildEvents = root.DefineSetting<bool>("NotifyNewGuildEvents", true, (Func<string>)(() => "Notify new guild events"), (Func<string>)(() => "Pops a toast when one of your guilds posts a new event."));
			NotifyGuildAnnouncements = root.DefineSetting<bool>("NotifyGuildAnnouncements", true, (Func<string>)(() => "Notify guild announcements"), (Func<string>)(() => "Pops a toast when an owner or officer of one of your guilds posts an announcement."));
			PollIntervalSeconds = root.DefineSetting<int>("PollIntervalSeconds", 45, (Func<string>)(() => "Poll interval (seconds)"), (Func<string>)(() => "How often the module checks for updates. 30-120 is sane; under 30 may rate-limit."));
			SettingComplianceExtensions.SetRange(PollIntervalSeconds, 30, 300);
			FontSize = root.DefineSetting<FontSizePreference>("FontSize", FontSizePreference.Medium, (Func<string>)(() => "Font size"), (Func<string>)(() => "Bumps all text in the menu and toasts by one or two notches. Useful on 4K or for far-away seating."));
			ToastPosition = root.DefineSetting<ToastPositionPreference>("ToastPosition", ToastPositionPreference.TopCenter, (Func<string>)(() => "Toast position"), (Func<string>)(() => "Where notification toasts pile up on screen."));
			PlaySoundOnToast = root.DefineSetting<bool>("PlaySoundOnToast", true, (Func<string>)(() => "Play sound on toast"), (Func<string>)(() => "Short chime when a check-in or starting reminder appears."));
			NotifyWeeklyReset = root.DefineSetting<bool>("NotifyWeeklyReset", true, (Func<string>)(() => "Weekly raid reset reminder"), (Func<string>)(() => "One toast on Monday 07:30 UTC reminding you the raid week reset (refresh your kill-proofs)."));
			PauseInCombat = root.DefineSetting<bool>("PauseInCombat", false, (Func<string>)(() => "Hold notifications in combat"), (Func<string>)(() => "Defers toasts while you're in combat (Mumble flag). They fire as soon as combat ends so you don't lose them."));
			ColorTheme = root.DefineSetting<ColorThemePreference>("ColorTheme", ColorThemePreference.Default, (Func<string>)(() => "Color theme"), (Func<string>)(() => "Recolours the per-type accent (raid/strike/fractal/wvw/open-world) in menu rows and toasts."));
		}

		public BitmapFont TitleFont()
		{
			return (BitmapFont)(FontSize.get_Value() switch
			{
				FontSizePreference.Small => GameService.Content.get_DefaultFont14(), 
				FontSizePreference.Large => GameService.Content.get_DefaultFont18(), 
				_ => GameService.Content.get_DefaultFont16(), 
			});
		}

		public BitmapFont BodyFont()
		{
			return (BitmapFont)(FontSize.get_Value() switch
			{
				FontSizePreference.Small => GameService.Content.get_DefaultFont12(), 
				FontSizePreference.Large => GameService.Content.get_DefaultFont16(), 
				_ => GameService.Content.get_DefaultFont14(), 
			});
		}
	}
}
