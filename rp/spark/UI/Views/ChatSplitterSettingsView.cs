using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	internal sealed class ChatSplitterSettingsView : View
	{
		private const int ContentPadding = 12;

		private const int SectionPadding = 12;

		private const int ScrollbarAllowance = 12;

		private readonly ChatSplitterSettings _settings;

		public ChatSplitterSettingsView(ChatSplitterSettings settings)
			: this()
		{
			_settings = settings ?? throw new ArgumentNullException("settings");
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			int contentWidth = Math.Max(0, buildPanel.get_ContentRegion().Width - 24 - 12);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 10f));
			val.set_OuterControlPadding(new Vector2(12f, 12f));
			FlowPanel contentStack = val;
			Label val2 = new Label();
			val2.set_Text("Chat Splitter Settings");
			((Control)val2).set_Width(contentWidth);
			((Control)val2).set_Height(30);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			((Control)val2).set_Parent((Container)(object)contentStack);
			Label val3 = new Label();
			val3.set_Text("These settings are saved and apply the next time you split a response.");
			((Control)val3).set_Width(contentWidth);
			((Control)val3).set_Height(38);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(SparkViewUI.SecondaryTextColor);
			val3.set_WrapText(true);
			((Control)val3).set_Parent((Container)(object)contentStack);
			BuildMessageBreakSettings(contentStack, contentWidth);
			BuildChatCommandSettings(contentStack, contentWidth);
			BuildContinuationSettings(contentStack, contentWidth);
		}

		private void BuildMessageBreakSettings(FlowPanel parent, int contentWidth)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			int innerWidth = Math.Max(0, contentWidth - 24);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(contentWidth);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 12));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 8f));
			val.set_OuterControlPadding(new Vector2(12f, 12f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel section = val;
			Label val2 = new Label();
			val2.set_Text("Message Breaks");
			((Control)val2).set_Width(innerWidth);
			((Control)val2).set_Height(28);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			((Control)val2).set_Parent((Container)(object)section);
			Checkbox blankLinesCheckbox = SparkFormLayout.AddCheckbox((Container)(object)section, "Blank lines start new messages", _settings.BreakOnBlankLines.get_Value(), innerWidth);
			((Control)blankLinesCheckbox).set_BasicTooltipText("When enabled, a blank line starts a new message. When disabled, blank lines are treated as spaces. /split always starts a new message.");
			blankLinesCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.BreakOnBlankLines.get_Value() != blankLinesCheckbox.get_Checked())
				{
					_settings.BreakOnBlankLines.set_Value(blankLinesCheckbox.get_Checked());
				}
			});
		}

		private void BuildChatCommandSettings(FlowPanel parent, int contentWidth)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			int innerWidth = Math.Max(0, contentWidth - 24);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(contentWidth);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 12));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 8f));
			val.set_OuterControlPadding(new Vector2(12f, 12f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel section = val;
			Label val2 = new Label();
			val2.set_Text("Chat Commands");
			((Control)val2).set_Width(innerWidth);
			((Control)val2).set_Height(28);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			((Control)val2).set_Parent((Container)(object)section);
			Checkbox shortenCheckbox = SparkFormLayout.AddCheckbox((Container)(object)section, "Shorten recognized chat commands", _settings.ShortenChatCommands.get_Value(), innerWidth);
			((Control)shortenCheckbox).set_BasicTooltipText("Changes chat prefixes to shorter versions in messages. For example, /me becomes /e and /party becomes /p. Disable this to keep the command exactly as typed.");
			shortenCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.ShortenChatCommands.set_Value(shortenCheckbox.get_Checked());
			});
			Checkbox repeatCheckbox = SparkFormLayout.AddCheckbox((Container)(object)section, "Repeat command on every message", _settings.RepeatChatCommand.get_Value(), innerWidth);
			((Control)repeatCheckbox).set_BasicTooltipText("The first message always includes the detected starting command. When enabled, every later message also includes it. When disabled, later messages use whichever chat channel is currently selected in GW2.");
			repeatCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.RepeatChatCommand.set_Value(repeatCheckbox.get_Checked());
			});
		}

		private void BuildContinuationSettings(FlowPanel parent, int contentWidth)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			int innerWidth = Math.Max(0, contentWidth - 24);
			int labelWidth = Math.Max(0, innerWidth - 80 - 10);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(contentWidth);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 12));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 8f));
			val.set_OuterControlPadding(new Vector2(12f, 12f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel section = val;
			Label val2 = new Label();
			val2.set_Text("Continuation Markers");
			((Control)val2).set_Width(innerWidth);
			((Control)val2).set_Height(28);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			((Control)val2).set_Parent((Container)(object)section);
			Checkbox useMarkersCheckbox = SparkFormLayout.AddCheckbox((Container)(object)section, "Use continuation markers", _settings.UseMarkers.get_Value(), innerWidth);
			((Control)useMarkersCheckbox).set_BasicTooltipText("Adds the end marker to every message that continues into another generated message. The final message is left unmarked.");
			FlowPanel endMarkerRow = SparkFormLayout.AddRow((Container)(object)section, innerWidth, 35);
			SparkFormLayout.AddLabel((Container)(object)endMarkerRow, "End marker", labelWidth, 35, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor);
			TextBox endMarkerBox = SparkFormLayout.AddTextBox((Container)(object)endMarkerRow, _settings.EndMarker.get_Value(), ">", 80, 35, 3);
			((Control)endMarkerBox).set_BasicTooltipText("Enter 1 to 3 characters. Spaces are added automatically. Whitespace and / cannot be used.");
			Checkbox markStartsCheckbox = SparkFormLayout.AddCheckbox((Container)(object)section, "Mark the start of continued messages", _settings.UseStartMarkers.get_Value(), innerWidth);
			((Control)markStartsCheckbox).set_BasicTooltipText("Adds the start marker to every generated message after the first. When a chat command is repeated, the marker appears after it.");
			FlowPanel startMarkerRow = SparkFormLayout.AddRow((Container)(object)section, innerWidth, 35);
			SparkFormLayout.AddLabel((Container)(object)startMarkerRow, "Start marker", labelWidth, 35, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor);
			TextBox startMarkerBox = SparkFormLayout.AddTextBox((Container)(object)startMarkerRow, _settings.StartMarker.get_Value(), ">", 80, 35, 3);
			((Control)startMarkerBox).set_BasicTooltipText("Enter 1 to 3 characters. This marker appears at the beginning of messages. Whitespace and / cannot be used.");
			useMarkersCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.UseMarkers.set_Value(useMarkersCheckbox.get_Checked());
				if (useMarkersCheckbox.get_Checked() && string.IsNullOrEmpty(((TextInputBase)endMarkerBox).get_Text()))
				{
					((TextInputBase)endMarkerBox).set_Text(">");
				}
				updateEnabledStates();
			});
			markStartsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.UseStartMarkers.set_Value(markStartsCheckbox.get_Checked());
				if (markStartsCheckbox.get_Checked() && string.IsNullOrEmpty(((TextInputBase)startMarkerBox).get_Text()))
				{
					((TextInputBase)startMarkerBox).set_Text(">");
				}
				updateEnabledStates();
			});
			((TextInputBase)endMarkerBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				string text2 = CleanMarker(((TextInputBase)endMarkerBox).get_Text());
				if (((TextInputBase)endMarkerBox).get_Text() != text2)
				{
					((TextInputBase)endMarkerBox).set_Text(text2);
				}
				_settings.EndMarker.set_Value(text2);
			});
			((TextInputBase)startMarkerBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				string text = CleanMarker(((TextInputBase)startMarkerBox).get_Text());
				if (((TextInputBase)startMarkerBox).get_Text() != text)
				{
					((TextInputBase)startMarkerBox).set_Text(text);
				}
				_settings.StartMarker.set_Value(text);
			});
			updateEnabledStates();
			void updateEnabledStates()
			{
				((Control)endMarkerBox).set_Enabled(useMarkersCheckbox.get_Checked());
				((Control)markStartsCheckbox).set_Enabled(useMarkersCheckbox.get_Checked());
				((Control)startMarkerBox).set_Enabled(useMarkersCheckbox.get_Checked() && markStartsCheckbox.get_Checked());
			}
		}

		private static string CleanMarker(string value)
		{
			string marker = string.Empty;
			string text = value ?? string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				char character = text[i];
				if (!char.IsWhiteSpace(character) && character != '/')
				{
					marker += character;
					if (marker.Length == 3)
					{
						break;
					}
				}
			}
			return marker;
		}
	}
}
