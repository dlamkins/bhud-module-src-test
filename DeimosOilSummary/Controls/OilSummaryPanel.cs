using System;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using DeimosOilSummary.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeimosOilSummary.Controls
{
	public sealed class OilSummaryPanel : Panel
	{
		private sealed class DragHandle : Control
		{
			public DragHandle()
				: this()
			{
				((Control)this).set_ClipsBounds(false);
			}

			protected override CaptureType CapturesInput()
			{
				return (CaptureType)4;
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
			}
		}

		private const int PanelWidth = 400;

		private const int PanelHeight = 420;

		private const int FooterHeight = 44;

		private const int StatusTop = 6;

		private const int BodyTop = 40;

		private readonly Label _statusLabel;

		private readonly Panel _bodyHost;

		private readonly Label _bodyLabel;

		private readonly StandardButton _latestButton;

		private readonly StandardButton _dismissButton;

		private readonly Control _dragHandle;

		private bool _dragging;

		private Point _dragStart = Point.get_Zero();

		public event EventHandler ParseLatestClicked;

		public OilSummaryPanel()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Expected O, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Expected O, but got Unknown
			((Control)this).set_Width(400);
			((Control)this).set_Height(420);
			((Panel)this).set_ShowBorder(true);
			((Control)this).set_BackgroundColor(Color.get_Black() * 0.6f);
			((Panel)this).set_Title("Who Touched Oils");
			DragHandle dragHandle = new DragHandle();
			((Control)dragHandle).set_Parent((Container)(object)this);
			((Control)dragHandle).set_Location(new Point(0, -36));
			((Control)dragHandle).set_Size(new Point(400, 36));
			((Control)dragHandle).set_ZIndex(536870911);
			_dragHandle = (Control)(object)dragHandle;
			_dragHandle.add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				_dragging = true;
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			});
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
			Label val = new Label();
			val.set_Text("Waiting for a Deimos log…");
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(Color.get_LightGray());
			val.set_ShowShadow(true);
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Location(new Point(10, 6));
			((Control)val).set_Parent((Container)(object)this);
			_statusLabel = val;
			Panel val2 = new Panel();
			val2.set_ShowBorder(false);
			val2.set_CanScroll(true);
			((Control)val2).set_BackgroundColor(Color.get_Transparent());
			((Control)val2).set_Parent((Container)(object)this);
			_bodyHost = val2;
			Label val3 = new Label();
			val3.set_Text("After a Deimos pull, oil hits are listed here.");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(Color.get_White());
			val3.set_ShowShadow(true);
			val3.set_WrapText(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(4, 2));
			((Control)val3).set_Parent((Container)(object)_bodyHost);
			_bodyLabel = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Parse latest Deimos log");
			((Control)val4).set_Width(200);
			((Control)val4).set_Height(28);
			((Control)val4).set_Parent((Container)(object)this);
			_latestButton = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Hide");
			((Control)val5).set_Width(80);
			((Control)val5).set_Height(28);
			((Control)val5).set_Parent((Container)(object)this);
			_dismissButton = val5;
			((Control)_latestButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ParseLatestClicked?.Invoke(this, EventArgs.Empty);
			});
			((Control)_dismissButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).set_Visible(false);
			});
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				LayoutChildren();
			});
			LayoutChildren();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			((Container)this).UpdateContainer(gameTime);
		}

		private void OnGlobalMouseRelease(object sender, MouseEventArgs e)
		{
			_dragging = false;
		}

		public void SetStatus(string status)
		{
			_statusLabel.set_Text(status ?? string.Empty);
			LayoutChildren();
		}

		public void Apply(OilHitSummary summary)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			if (summary == null)
			{
				return;
			}
			if (!summary.Success)
			{
				_statusLabel.set_Text(summary.ErrorMessage ?? "Parse failed.");
				_statusLabel.set_TextColor(Color.get_Orange());
				LayoutChildren();
				return;
			}
			int firstCount = summary.FirstTouches?.Count ?? 0;
			_statusLabel.set_TextColor(Color.get_LightGreen());
			_statusLabel.set_Text($"{summary.LogTime:g}  ·  {firstCount} oil(s)  ·  {summary.TotalHits} hit(s)");
			if ((summary.Players == null || summary.Players.Count == 0) && (summary.FirstTouches == null || summary.FirstTouches.Count == 0))
			{
				_bodyLabel.set_Text("Nobody stepped in oils this pull.");
				_bodyLabel.set_TextColor(Color.get_LightGreen());
				((Control)this).set_Visible(true);
				LayoutChildren();
				return;
			}
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("First to touch each oil:");
			if (summary.FirstTouches == null || summary.FirstTouches.Count == 0)
			{
				sb.AppendLine("  (none detected)");
			}
			else
			{
				foreach (OilFirstTouch touch in summary.FirstTouches)
				{
					sb.AppendLine($"  Oil {touch.OilNumber}: {touch.DisplayName}");
				}
			}
			sb.AppendLine();
			sb.AppendLine("Total oil hits:");
			if (summary.Players == null || summary.Players.Count == 0)
			{
				sb.AppendLine("  (none)");
			}
			else
			{
				int rank = 1;
				foreach (PlayerOilCount player in summary.Players)
				{
					string times = ((player.HitCount == 1) ? "1 time" : $"{player.HitCount} times");
					sb.AppendLine($"  {rank}. {player.DisplayName} — {times}");
					rank++;
				}
			}
			_bodyLabel.set_Text(sb.ToString().TrimEnd());
			_bodyLabel.set_TextColor(Color.get_White());
			((Control)this).set_Visible(true);
			LayoutChildren();
		}

		private void LayoutChildren()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			int contentW = Math.Max(100, ((Container)this).get_ContentRegion().Width);
			int num = Math.Max(120, ((Container)this).get_ContentRegion().Height);
			_dragHandle.set_Location(new Point(0, -36));
			_dragHandle.set_Width(Math.Max(400, contentW + ((Container)this).get_ContentRegion().X));
			_dragHandle.set_Height(36);
			((Control)_statusLabel).set_Width(contentW - 20);
			int bodyBottom = num - 44;
			((Control)_bodyHost).set_Location(new Point(6, 40));
			((Control)_bodyHost).set_Size(new Point(contentW - 12, Math.Max(60, bodyBottom - 40)));
			((Control)_bodyLabel).set_Width(Math.Max(80, ((Container)_bodyHost).get_ContentRegion().Width - 16));
			((Control)_bodyLabel).RecalculateLayout();
			int buttonY = num - 34;
			((Control)_latestButton).set_Location(new Point(10, buttonY));
			((Control)_dismissButton).set_Location(new Point(220, buttonY));
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseRelease);
			((Panel)this).DisposeControl();
		}
	}
}
