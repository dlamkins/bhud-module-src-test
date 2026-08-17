using System;
using Blish_HUD.Settings;

namespace rp.spark.Services
{
	internal sealed class ChatSplitterSettings
	{
		private const string SettingsKey = "chat-splitter";

		private const string BreakOnBlankLinesKey = "BreakOnBlankLines";

		private const string ShortenChatCommandsKey = "ShortenChatCommands";

		private const string RepeatChatCommandKey = "RepeatChatCommand";

		private const string UseContinuationMarkersKey = "UseContinuationMarkers";

		private const string EndMarkerKey = "EndMarker";

		private const string MarkContinuationStartsKey = "MarkContinuationStarts";

		private const string StartMarkerKey = "StartMarker";

		public SettingEntry<bool> BreakOnBlankLines { get; }

		public SettingEntry<bool> ShortenChatCommands { get; }

		public SettingEntry<bool> RepeatChatCommand { get; }

		public SettingEntry<bool> UseMarkers { get; }

		public SettingEntry<string> EndMarker { get; }

		public SettingEntry<bool> UseStartMarkers { get; }

		public SettingEntry<string> StartMarker { get; }

		public ChatSplitterSettings(SettingCollection moduleSettings)
		{
			SettingCollection settings = moduleSettings.AddSubCollection("chat-splitter", true, (Func<string>)(() => "Chat Splitter"));
			BreakOnBlankLines = settings.DefineSetting<bool>("BreakOnBlankLines", true, (Func<string>)(() => "Blank lines start new messages"), (Func<string>)(() => "When disabled, blank lines are treated as spaces. /split will still start a new message."));
			ShortenChatCommands = settings.DefineSetting<bool>("ShortenChatCommands", true, (Func<string>)(() => "Shorten recognized chat commands"), (Func<string>)(() => "Changes commands such as /me and /party to /e and /p."));
			RepeatChatCommand = settings.DefineSetting<bool>("RepeatChatCommand", true, (Func<string>)(() => "Repeat the starting command"), (Func<string>)(() => "Adds the detected starting command to every generated message."));
			UseMarkers = settings.DefineSetting<bool>("UseContinuationMarkers", false, (Func<string>)(() => "Use continuation markers"), (Func<string>)(() => "Marks messages that continue into another generated message."));
			EndMarker = settings.DefineSetting<string>("EndMarker", ">", (Func<string>)(() => "End marker"), (Func<string>)(() => "Added to the end of messages that continue."));
			UseStartMarkers = settings.DefineSetting<bool>("MarkContinuationStarts", false, (Func<string>)(() => "Mark continuation starts"), (Func<string>)(() => "Marks generated messages that continue from a previous message."));
			StartMarker = settings.DefineSetting<string>("StartMarker", ">", (Func<string>)(() => "Start marker"), (Func<string>)(() => "Added to the beginning of continued messages."));
		}
	}
}
