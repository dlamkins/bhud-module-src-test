using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Services.Practice;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Maestro.UI.Practice
{
	public class SectionLoopBar : Control
	{
		private const int MinLoopMs = 500;

		private readonly PracticeSession _session;

		public int? LoopStartMs { get; private set; }

		public int? LoopEndMs { get; private set; }

		public event Action LoopChanged;

		public SectionLoopBar(PracticeSession session)
			: this()
		{
			_session = session;
			((Control)this).set_Height(36);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			int ms = PixelToMs(((Control)this).get_RelativeMousePosition().X);
			ModifierKeys mods = GameService.Input.get_Keyboard().get_ActiveModifiers();
			if (((Enum)mods).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				LoopEndMs = ms;
				NormalizeLoop();
				this.LoopChanged?.Invoke();
			}
			else if (((Enum)mods).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				LoopStartMs = ms;
				NormalizeLoop();
				this.LoopChanged?.Invoke();
			}
			else
			{
				_session.SeekTo(ms);
			}
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnRightMouseButtonPressed(e);
			LoopStartMs = null;
			LoopEndMs = null;
			this.LoopChanged?.Invoke();
		}

		private void NormalizeLoop()
		{
			if (!LoopStartMs.HasValue || !LoopEndMs.HasValue)
			{
				return;
			}
			if (LoopEndMs.Value < LoopStartMs.Value)
			{
				int tmp = LoopStartMs.Value;
				LoopStartMs = LoopEndMs.Value;
				LoopEndMs = tmp;
			}
			if (LoopEndMs.Value - LoopStartMs.Value < 500)
			{
				LoopEndMs = LoopStartMs.Value + 500;
			}
			int total = _session.Timeline.TotalDurationMs;
			if (total > 0 && LoopEndMs.Value > total)
			{
				LoopEndMs = total;
				if (LoopEndMs.Value - LoopStartMs.Value < 500)
				{
					LoopStartMs = Math.Max(0, LoopEndMs.Value - 500);
				}
			}
		}

		private int PixelToMs(int x)
		{
			if (((Control)this).get_Width() <= 0 || _session.Timeline.TotalDurationMs <= 0)
			{
				return 0;
			}
			return (int)((float)x / (float)((Control)this).get_Width() * (float)_session.Timeline.TotalDurationMs);
		}

		private int MsToPixel(int ms)
		{
			if (_session.Timeline.TotalDurationMs <= 0)
			{
				return 0;
			}
			return (int)((float)ms / (float)_session.Timeline.TotalDurationMs * (float)((Control)this).get_Width());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, new Color(20, 20, 26));
			if (LoopStartMs.HasValue && LoopEndMs.HasValue)
			{
				int x1 = ((Rectangle)(ref bounds)).get_Left() + MsToPixel(LoopStartMs.Value);
				int x2 = ((Rectangle)(ref bounds)).get_Left() + MsToPixel(LoopEndMs.Value);
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(x1, ((Rectangle)(ref bounds)).get_Top(), Math.Max(1, x2 - x1), bounds.Height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), rect, new Color(255, 220, 80) * 0.25f);
			}
			int playheadX = ((Rectangle)(ref bounds)).get_Left() + MsToPixel(Math.Max(0, _session.Clock.CurrentMs));
			Rectangle playhead = default(Rectangle);
			((Rectangle)(ref playhead))._002Ector(playheadX - 1, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), playhead, new Color(255, 220, 80));
		}

		public void ApplyLoopIfNeeded()
		{
			if (LoopStartMs.HasValue && LoopEndMs.HasValue && _session.Clock.CurrentMs >= LoopEndMs.Value)
			{
				_session.LoopSeek(LoopStartMs.Value, LoopEndMs.Value);
			}
		}
	}
}
