using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Services;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	internal sealed class ChatSplitterView : View
	{
		private const int ContentPadding = 12;

		private const int SectionPadding = 12;

		private const int CardPadding = 8;

		private const int EditorTop = 58;

		private const int EditorHeight = 236;

		private const int EditorInputHeight = 140;

		private const int ResultsHeadingTop = 310;

		private const int ResultsTop = 342;

		private const int GenerateButtonWidth = 100;

		private const int ClearButtonWidth = 75;

		private const int ControlGap = 8;

		private const int ControlHeight = 32;

		private readonly ChatSplitterSession _session;

		private readonly ChatSplitterSettings _settings;

		private readonly List<StandardButton> _copyButtons = new List<StandardButton>();

		private SparkMultiline _responseInput;

		private FlowPanel _resultsShell;

		private Label _status;

		private int _resultsContentWidth;

		public ChatSplitterView(ChatSplitterSession session, ChatSplitterSettings settings)
			: this()
		{
			_session = session ?? throw new ArgumentNullException("session");
			_settings = settings ?? throw new ArgumentNullException("settings");
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			Point contentSize = ((Rectangle)(ref contentRegion)).get_Size();
			int contentWidth = Math.Max(0, contentSize.X - 24);
			int sectionContentWidth = Math.Max(0, contentWidth - 24 - 12);
			BuildHeader(buildPanel, contentWidth);
			BuildEditor(buildPanel, contentWidth, sectionContentWidth);
			BuildResults(buildPanel, contentSize, contentWidth, sectionContentWidth);
			if (_session.GeneratedChunks.Count > 0)
			{
				RenderResults(_session.GeneratedChunks);
				if (_session.NeedsUpdate)
				{
					SetStatus("Press Split Message again to update the messages.", warning: true);
				}
				else
				{
					SetGeneratedStatus();
				}
			}
			else
			{
				RenderEmptyState("Enter a response above and select 'Split Message'.");
				SetStatus("Ready.");
			}
		}

		private static void BuildHeader(Container parent, int contentWidth)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("Write a response and split it into GW2-sized messages. Write '/split' on its own line to force a new message early.");
			((Control)val).set_Location(new Point(12, 16));
			((Control)val).set_Size(new Point(contentWidth, 38));
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(SparkViewUI.SecondaryTextColor);
			val.set_WrapText(true);
			((Control)val).set_Parent(parent);
		}

		private void BuildEditor(Container parent, int contentWidth, int sectionContentWidth)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(parent);
			((Control)val).set_Location(new Point(12, 58));
			((Control)val).set_Size(new Point(contentWidth, 236));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 6f));
			val.set_OuterControlPadding(new Vector2(12f, 12f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel editorShell = val;
			Label val2 = new Label();
			val2.set_Text("Response Editor");
			((Control)val2).set_Width(sectionContentWidth);
			((Control)val2).set_Height(22);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			((Control)val2).set_Parent((Container)(object)editorShell);
			_responseInput = SparkFormLayout.AddMultilineTextBox((Container)(object)editorShell, _session.SourceText, "Write or paste your RP response here...", sectionContentWidth, 140);
			((TextInputBase)_responseInput).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				_session.SourceText = ((TextInputBase)_responseInput).get_Text();
				ResetCopyFeedback();
				if (_session.NeedsUpdate)
				{
					SetStatus("Press Split Message again to update the messages.", warning: true);
				}
				else if (_session.GeneratedChunks.Count > 0)
				{
					SetGeneratedStatus();
				}
			});
			FlowPanel controls = SparkFormLayout.AddRow((Container)(object)editorShell, sectionContentWidth, 32, 8);
			StandardButton generateButton = SparkFormLayout.AddButton((Container)(object)controls, "Split Message", 100, 32);
			StandardButton obj = SparkFormLayout.AddButton((Container)(object)controls, "Clear", 75, 32);
			_status = SparkFormLayout.AddLabel(width: Math.Max(0, sectionContentWidth - 100 - 75 - 16), parent: (Container)(object)controls, text: string.Empty, height: 32, font: GameService.Content.get_DefaultFont14(), textColor: SparkViewUI.SecondaryTextColor);
			((Control)generateButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GenerateChunks();
			});
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearEditor();
			});
		}

		private void BuildResults(Container parent, Point contentSize, int contentWidth, int sectionContentWidth)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Split Messages");
			((Control)val).set_Location(new Point(12, 310));
			((Control)val).set_Size(new Point(contentWidth, 28));
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			((Control)val).set_Parent(parent);
			int resultsHeight = Math.Max(0, contentSize.Y - 342 - 12);
			_resultsContentWidth = sectionContentWidth;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent(parent);
			((Control)val2).set_Location(new Point(12, 342));
			((Control)val2).set_Size(new Point(contentWidth, resultsHeight));
			((Panel)val2).set_CanScroll(true);
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_ControlPadding(new Vector2(0f, 3f));
			val2.set_OuterControlPadding(new Vector2(12f, 6f));
			((Panel)val2).set_ShowBorder(true);
			_resultsShell = val2;
		}

		private void GenerateChunks()
		{
			ChatSplitterSession session = _session;
			SparkMultiline responseInput = _responseInput;
			session.SourceText = ((responseInput != null) ? ((TextInputBase)responseInput).get_Text() : null) ?? string.Empty;
			ChatSplitterOptions options = new ChatSplitterOptions
			{
				BreakOnBlankLines = _settings.BreakOnBlankLines.get_Value(),
				ShortenChatCommands = _settings.ShortenChatCommands.get_Value(),
				RepeatChatCommand = _settings.RepeatChatCommand.get_Value(),
				UseMarkers = _settings.UseMarkers.get_Value(),
				EndMarker = _settings.EndMarker.get_Value(),
				UseStartMarkers = _settings.UseStartMarkers.get_Value(),
				StartMarker = _settings.StartMarker.get_Value()
			};
			if (!ChatSplitter.TrySplit(_session.SourceText, options, out var chunks, out var error))
			{
				_session.ClearGeneratedChunks();
				RenderEmptyState(error);
				SetStatus("Check the response or splitter settings.", warning: true);
			}
			else if (chunks.Count == 0)
			{
				_session.ClearGeneratedChunks();
				RenderEmptyState("No messages were generated.");
				SetStatus("Enter a response before generating.", warning: true);
			}
			else
			{
				_session.SetGeneratedChunks(chunks, _session.SourceText);
				RenderResults(_session.GeneratedChunks);
				SetGeneratedStatus();
			}
		}

		private void SetGeneratedStatus()
		{
			int count = _session.GeneratedChunks.Count;
			SetStatus((count == 1) ? "Generated 1 message." : $"Generated {count} messages.");
		}

		private void RenderResults(IReadOnlyList<string> chunks)
		{
			ClearResults();
			for (int index = 0; index < chunks.Count; index++)
			{
				AddChunkCard(chunks[index], index + 1, chunks.Count);
			}
		}

		private void AddChunkCard(string chunk, int number, int total)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Expected O, but got Unknown
			int cardWidth = _resultsContentWidth;
			int innerWidth = Math.Max(0, cardWidth - 16);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)_resultsShell);
			((Control)val).set_Width(cardWidth);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 6));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 2f));
			val.set_OuterControlPadding(new Vector2(8f, 6f));
			((Panel)val).set_ShowBorder(false);
			((Control)val).set_BackgroundColor((number % 2 == 1) ? new Color(0, 0, 0, 65) : new Color(20, 20, 20, 55));
			FlowPanel header = SparkFormLayout.AddRow((Container)val, innerWidth, 28, 6);
			SparkFormLayout.AddLabel(width: Math.Max(0, innerWidth - 72 - 94 - 12), parent: (Container)(object)header, text: $"Message {number} of {total}", height: 28, font: GameService.Content.get_DefaultFont14(), textColor: Color.get_White(), strokeText: true);
			SparkFormLayout.AddLabel((Container)(object)header, $"{chunk.Length}/{199}", 72, 28, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor).set_HorizontalAlignment((HorizontalAlignment)2);
			int copyCount = _session.GetCopyCount(number - 1);
			StandardButton copyButton = SparkFormLayout.AddButton((Container)(object)header, (copyCount > 0) ? $"Copied ({copyCount})" : "Copy", 94, 26);
			_copyButtons.Add(copyButton);
			((Control)copyButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await CopyChunkAsync(copyButton, chunk, number - 1);
			});
			Label obj = SparkFormLayout.AddLabel((Container)val, chunk, innerWidth, 24, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor);
			obj.set_WrapText(true);
			obj.set_AutoSizeHeight(true);
		}

		private async Task CopyChunkAsync(StandardButton copyButton, string chunk, int chunkIndex)
		{
			bool copied;
			try
			{
				copied = await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(chunk ?? string.Empty);
			}
			catch
			{
				copied = false;
			}
			SparkUiThread.Queue(delegate
			{
				if (!copied)
				{
					if (((Control)copyButton).get_Parent() != null)
					{
						SetStatus("Couldn't copy that message right now.", warning: true);
					}
				}
				else
				{
					int num = _session.IncrementCopyCount(chunkIndex);
					if (((Control)copyButton).get_Parent() != null)
					{
						copyButton.set_Text($"Copied ({num})");
						SetStatus($"Copied message {chunkIndex + 1}.");
					}
				}
			});
		}

		private void ResetCopyFeedback()
		{
			_session.ResetCopyCounts();
			foreach (StandardButton copyButton in _copyButtons)
			{
				if (((Control)copyButton).get_Parent() != null)
				{
					copyButton.set_Text("Copy");
				}
			}
		}

		private void RenderEmptyState(string message)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			ClearResults();
			Label val = new Label();
			val.set_Text(message ?? string.Empty);
			((Control)val).set_Width(_resultsContentWidth);
			((Control)val).set_Height(40);
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(SparkViewUI.SecondaryTextColor);
			val.set_WrapText(true);
			((Control)val).set_Parent((Container)(object)_resultsShell);
		}

		private void ClearEditor()
		{
			_session.Clear();
			if (_responseInput != null)
			{
				((TextInputBase)_responseInput).set_Text(string.Empty);
			}
			RenderEmptyState("Enter a response above and select 'Split Message'.");
			SetStatus("Cleared.");
		}

		private void ClearResults()
		{
			if (_resultsShell != null)
			{
				_copyButtons.Clear();
				Control[] array = ((Container)_resultsShell).get_Children().ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Dispose();
				}
				((Container)_resultsShell).set_VerticalScrollOffset(0);
			}
		}

		private void SetStatus(string text, bool warning = false)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (_status != null)
			{
				_status.set_Text(text ?? string.Empty);
				_status.set_TextColor(warning ? SparkViewUI.WarningTextColor : SparkViewUI.SecondaryTextColor);
			}
		}
	}
}
