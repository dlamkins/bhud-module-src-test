using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Windows
{
	public class SpamUsageLogWindow : StandardWindow
	{
		private const int WINDOW_WIDTH = 500;

		private const int WINDOW_HEIGHT = 400;

		private const int PADDING = 10;

		private const int ROW_HEIGHT = 24;

		private readonly SpamClient _spamClient;

		private readonly SpamSource _source;

		private readonly Guid _spamId;

		private readonly string _spamName;

		private FlowPanel _logPanel;

		private Label _loadingLabel;

		private bool _isLoading;

		public SpamUsageLogWindow(SpamClient spamClient, SpamSource source, Guid spamId, string spamName)
			: this(Textures.get_Pixel(), new Rectangle(0, 0, 500, 400), new Rectangle(10, 10, 480, 380))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			_spamClient = spamClient ?? throw new ArgumentNullException("spamClient");
			_source = source;
			_spamId = spamId;
			_spamName = spamName ?? "Unknown";
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((WindowBase2)this).set_Title("Usage Log: " + _spamName);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id($"Neokain_GW2_AllianceManager_usageLogWindow_{spamId}");
			((Control)this).set_BackgroundColor(Color.get_Black());
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((WindowBase2)this).set_CanResize(true);
			((Control)this).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 500) / 2);
			((Control)this).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 400) / 2);
			CreateControls();
		}

		private void CreateControls()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(0);
			((Control)val).set_Top(0);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val).set_Height(24);
			Panel headerPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)headerPanel);
			val2.set_Text("Account");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Left(0);
			((Control)val2).set_Width(180);
			val2.set_AutoSizeHeight(true);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)headerPanel);
			val3.set_Text("Used At");
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Left(180);
			((Control)val3).set_Width(150);
			val3.set_AutoSizeHeight(true);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)headerPanel);
			val4.set_Text("Map");
			val4.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Left(330);
			((Control)val4).set_Width(150);
			val4.set_AutoSizeHeight(true);
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Left(0);
			((Control)val5).set_Top(29);
			((Control)val5).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val5).set_Height(((Container)this).get_ContentRegion().Height - 24 - 5);
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val5).set_CanScroll(true);
			val5.set_ControlPadding(new Vector2(0f, 2f));
			_logPanel = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_logPanel);
			val6.set_Text("Loading...");
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			_loadingLabel = val6;
		}

		public override void Show()
		{
			((WindowBase2)this).Show();
			LoadLogsAsync();
		}

		private async Task LoadLogsAsync()
		{
			if (_isLoading)
			{
				return;
			}
			_isLoading = true;
			try
			{
				_loadingLabel.set_Text("Loading...");
				((Control)_loadingLabel).set_Visible(true);
				List<SpamUsageLogDto> logs = await _spamClient.GetUsageLogs(_source, _spamId, 100);
				foreach (Control child in ((Container)_logPanel).get_Children().ToList())
				{
					if (child != _loadingLabel)
					{
						child.Dispose();
					}
				}
				if (logs == null || logs.Count == 0)
				{
					_loadingLabel.set_Text("No usage logs found.");
					return;
				}
				((Control)_loadingLabel).set_Visible(false);
				foreach (SpamUsageLogDto log in logs)
				{
					CreateLogRow(log);
				}
			}
			catch (Exception ex)
			{
				_loadingLabel.set_Text("Error: " + ex.Message);
			}
			finally
			{
				_isLoading = false;
			}
		}

		private void CreateLogRow(SpamUsageLogDto log)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_logPanel);
			((Control)val).set_Width(((Control)_logPanel).get_Width() - 20);
			((Control)val).set_Height(24);
			Panel rowPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)rowPanel);
			val2.set_Text(log.UsedByAccountName ?? "Unknown");
			((Control)val2).set_Left(0);
			((Control)val2).set_Width(180);
			val2.set_AutoSizeHeight(true);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)rowPanel);
			val3.set_Text(log.UsedAt.LocalDateTime.ToString("g"));
			((Control)val3).set_Left(180);
			((Control)val3).set_Width(150);
			val3.set_AutoSizeHeight(true);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)rowPanel);
			val4.set_Text(log.MapName ?? "-");
			((Control)val4).set_Left(330);
			((Control)val4).set_Width(150);
			val4.set_AutoSizeHeight(true);
		}

		protected override void DisposeControl()
		{
			FlowPanel logPanel = _logPanel;
			if (logPanel != null)
			{
				((Control)logPanel).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
