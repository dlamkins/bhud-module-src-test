using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Windows
{
	public class MapSpamHistoryWindow : StandardWindow
	{
		private const int WINDOW_WIDTH = 400;

		private const int WINDOW_HEIGHT = 350;

		private const int PADDING = 10;

		private const int ROW_HEIGHT = 24;

		private readonly SpamClient _spamClient;

		private readonly SpamSource _source;

		private readonly Guid _spamId;

		private readonly string _spamName;

		private FlowPanel _contentPanel;

		private Label _loadingLabel;

		private Label _emptyLabel;

		private List<SpamMapHistoryDto> _mapHistory;

		public MapSpamHistoryWindow(SpamClient spamClient, SpamSource source, Guid spamId, string spamName)
			: this(Textures.get_Pixel(), new Rectangle(0, 0, 400, 350), new Rectangle(10, 10, 380, 330))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			_spamClient = spamClient ?? throw new ArgumentNullException("spamClient");
			_source = source;
			_spamId = spamId;
			_spamName = spamName ?? "Unknown";
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((WindowBase2)this).set_Title("Map History: " + _spamName);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id($"Neokain_GW2_AllianceManager_mapHistoryWindow_{_spamId}");
			((Control)this).set_Top(150);
			((Control)this).set_Left(150);
			((Control)this).set_BackgroundColor(Color.get_Black());
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((WindowBase2)this).set_CanResize(false);
			CreateControls();
		}

		private void CreateControls()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Loading...");
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Left(10);
			((Control)val).set_Top(10);
			_loadingLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("No map history available.");
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(10);
			((Control)val2).set_Visible(false);
			_emptyLabel = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Left(0);
			((Control)val3).set_Top(0);
			((Control)val3).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val3).set_Height(((Container)this).get_ContentRegion().Height);
			((Panel)val3).set_CanScroll(true);
			((Control)val3).set_Visible(false);
			_contentPanel = val3;
		}

		public override void Show()
		{
			((WindowBase2)this).Show();
			LoadMapHistoryAsync();
		}

		private async Task LoadMapHistoryAsync()
		{
			try
			{
				((Control)_loadingLabel).set_Visible(true);
				((Control)_emptyLabel).set_Visible(false);
				((Control)_contentPanel).set_Visible(false);
				ClearContentPanel();
				_mapHistory = await _spamClient.GetMapHistory(_source, _spamId);
				((Control)_loadingLabel).set_Visible(false);
				if (_mapHistory == null || _mapHistory.Count == 0)
				{
					((Control)_emptyLabel).set_Visible(true);
					return;
				}
				List<SpamMapHistoryDto> sortedHistory = _mapHistory.OrderByDescending((SpamMapHistoryDto h) => h.LastSpammed ?? DateTimeOffset.MinValue).ToList();
				CreateHistoryRows(sortedHistory);
				((Control)_contentPanel).set_Visible(true);
			}
			catch (Exception ex)
			{
				_loadingLabel.set_Text("Error: " + ex.Message);
			}
		}

		private void CreateHistoryRows(List<SpamMapHistoryDto> history)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)_contentPanel);
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Control)val).set_Height(24);
			val.set_OuterControlPadding(new Vector2(5f, 0f));
			FlowPanel headerPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)headerPanel);
			val2.set_Text("Map Name");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Width(200);
			((Control)val2).set_Height(24);
			val2.set_TextColor(Color.get_LightGray());
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)headerPanel);
			val3.set_Text("Last Spammed");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Width(150);
			((Control)val3).set_Height(24);
			val3.set_TextColor(Color.get_LightGray());
			foreach (SpamMapHistoryDto entry in history)
			{
				CreateHistoryRow(entry);
			}
		}

		private void CreateHistoryRow(SpamMapHistoryDto entry)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)_contentPanel);
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Control)val).set_Height(24);
			val.set_OuterControlPadding(new Vector2(5f, 0f));
			FlowPanel rowPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)rowPanel);
			val2.set_Text(entry.MapName ?? $"Map {entry.MapId}");
			((Control)val2).set_Width(200);
			((Control)val2).set_Height(24);
			string lastSpammedText = (entry.LastSpammed.HasValue ? FormatElapsedTime(DateTime.UtcNow - entry.LastSpammed.Value) : "Never");
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)rowPanel);
			val3.set_Text(lastSpammedText);
			((Control)val3).set_Width(150);
			((Control)val3).set_Height(24);
			Label lastSpammedLabel = val3;
			if (entry.LastSpammed.HasValue)
			{
				lastSpammedLabel.set_TextColor(((DateTime.UtcNow - entry.LastSpammed.Value).TotalMinutes >= 5.0) ? Color.get_Green() : Color.get_Red());
			}
			else
			{
				lastSpammedLabel.set_TextColor(Color.get_Green());
			}
		}

		private void ClearContentPanel()
		{
			foreach (Control item in ((Container)_contentPanel).get_Children().ToList())
			{
				item.Dispose();
			}
			((Container)_contentPanel).ClearChildren();
		}

		private string FormatElapsedTime(TimeSpan elapsed)
		{
			if (elapsed.TotalSeconds < 60.0)
			{
				return $"{(int)elapsed.TotalSeconds}s ago";
			}
			if (elapsed.TotalMinutes < 60.0)
			{
				return $"{(int)elapsed.TotalMinutes}m ago";
			}
			if (elapsed.TotalHours < 24.0)
			{
				return $"{(int)elapsed.TotalHours}h ago";
			}
			return $"{(int)elapsed.TotalDays}d ago";
		}

		protected override void DisposeControl()
		{
			ClearContentPanel();
			((WindowBase2)this).DisposeControl();
		}
	}
}
