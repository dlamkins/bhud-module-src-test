using System;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NpcFinder.Models;

namespace NpcFinder.Util
{
	public class BigMapOverlayControl : Control
	{
		private static readonly bool DEBUG_LOGS = false;

		private Texture2D _pixel;

		public Func<NpcTarget> TargetProvider;

		public Func<int> CurrentContinentIdProvider;

		private static readonly Logger Log = Logger.GetLogger<BigMapOverlayControl>();

		private int _dbgEvery;

		private static DateTime _lastLog = DateTime.MinValue;

		public BigMapOverlayControl()
			: this()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(0, 0));
			((Control)this).set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
			((Control)this).set_Visible(true);
			((Control)this).set_Opacity(1f);
			((Control)this).set_ZIndex(10000);
			((Control)this).set_ClipsBounds(false);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		private static float GetUiScaleMultiplier()
		{
			try
			{
				object gfx = GameService.Graphics;
				if (gfx == null)
				{
					return 1f;
				}
				Type t = gfx.GetType();
				PropertyInfo p = t.GetProperty("UIScaleMultiplier") ?? t.GetProperty("UiScaleMultiplier") ?? t.GetProperty("ScaleMultiplier") ?? t.GetProperty("UIScale") ?? t.GetProperty("UiScale");
				if (p == null)
				{
					return 1f;
				}
				object v = p.GetValue(gfx);
				if (v == null)
				{
					return 1f;
				}
				if (v is float)
				{
					return (float)v;
				}
				if (v is double)
				{
					double d = (double)v;
					return (float)d;
				}
				return Convert.ToSingle(v);
			}
			catch
			{
				return 1f;
			}
		}

		private Vector2? ContinentToScreen(double cx, double cy, Rectangle bounds, float centerX, float centerY, float scale)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			if (float.IsNaN(scale) || float.IsInfinity(scale) || Math.Abs(scale) < 1E-06f)
			{
				return null;
			}
			float uiMul = GetUiScaleMultiplier();
			if (float.IsNaN(uiMul) || float.IsInfinity(uiMul) || uiMul <= 1E-06f)
			{
				uiMul = 1f;
			}
			Vector2 val = new Vector2((float)bounds.X + (float)bounds.Width / 2f, (float)bounds.Y + (float)bounds.Height / 2f);
			float dx = (float)cx - centerX;
			float dy = (float)cy - centerY;
			float px = val.X + dx / scale / uiMul;
			float py = val.Y + dy / scale / uiMul;
			if (float.IsNaN(px) || float.IsInfinity(px) || float.IsNaN(py) || float.IsInfinity(py))
			{
				return null;
			}
			return new Vector2(px, py);
		}

		private void DrawRing(SpriteBatch sb, Vector2 center, float radius, float thickness, Color color)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			Vector2 prev = default(Vector2);
			((Vector2)(ref prev))._002Ector(center.X + radius, center.Y);
			Vector2 cur = default(Vector2);
			for (int i = 1; i <= 36; i++)
			{
				float a = (float)(Math.PI * 2.0 * (double)i / 36.0);
				((Vector2)(ref cur))._002Ector(center.X + (float)Math.Cos(a) * radius, center.Y + (float)Math.Sin(a) * radius);
				DrawLine(sb, prev, cur, thickness, color);
				prev = cur;
			}
		}

		private void DrawCross(SpriteBatch sb, Vector2 c, float half, float thickness, Color color)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			DrawLine(sb, new Vector2(c.X - half, c.Y), new Vector2(c.X + half, c.Y), thickness, color);
			DrawLine(sb, new Vector2(c.X, c.Y - half), new Vector2(c.X, c.Y + half), thickness, color);
		}

		private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, float thickness, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			Vector2 edge = end - start;
			float angle = (float)Math.Atan2(edge.Y, edge.X);
			float length = ((Vector2)(ref edge)).Length();
			sb.Draw(_pixel, start, (Rectangle?)null, color, angle, new Vector2(0f, 0.5f), new Vector2(length, thickness), (SpriteEffects)0, 0f);
		}

		private static Vector2 ClampToBounds(Vector2 p, Rectangle b, float margin)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			float num = MathHelper.Clamp(p.X, (float)((Rectangle)(ref b)).get_Left() + margin, (float)((Rectangle)(ref b)).get_Right() - margin);
			float y = MathHelper.Clamp(p.Y, (float)((Rectangle)(ref b)).get_Top() + margin, (float)((Rectangle)(ref b)).get_Bottom() - margin);
			return new Vector2(num, y);
		}

		private void DrawArrow(SpriteBatch sb, Vector2 tip, Vector2 dir, float size, float thickness, Color color)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Vector2 left = Rotate(dir, 2.6f);
			Vector2 right = Rotate(dir, -2.6f);
			Vector2 a = tip - left * size;
			Vector2 b = tip - right * size;
			DrawLine(sb, a, tip, thickness, color);
			DrawLine(sb, b, tip, thickness, color);
		}

		private static Vector2 Rotate(Vector2 v, float radians)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			float c = (float)Math.Cos(radians);
			float s = (float)Math.Sin(radians);
			return new Vector2(v.X * c - v.Y * s, v.X * s + v.Y * c);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			bool? obj;
			if (gw2Mumble == null)
			{
				obj = null;
			}
			else
			{
				UI uI = gw2Mumble.get_UI();
				obj = ((uI != null) ? new bool?(uI.get_IsMapOpen()) : null);
			}
			bool? flag = obj;
			if (!flag.GetValueOrDefault())
			{
				return;
			}
			Func<NpcTarget> tp = TargetProvider;
			if (tp == null)
			{
				return;
			}
			NpcTarget target;
			try
			{
				target = tp();
			}
			catch
			{
				return;
			}
			if (target == null)
			{
				return;
			}
			if (CurrentContinentIdProvider != null)
			{
				int curCont = CurrentContinentIdProvider();
				if (curCont != 0 && target.TargetContinentId != 0 && curCont != target.TargetContinentId)
				{
					return;
				}
			}
			if (!MumbleReader.TryGetWorldMapUi(out var centerX, out var centerY, out var scale) || float.IsNaN(scale) || float.IsInfinity(scale) || Math.Abs(scale) < 1E-06f)
			{
				return;
			}
			if (_pixel == null)
			{
				_pixel = new Texture2D(((GraphicsResource)spriteBatch).get_GraphicsDevice(), 1, 1);
				_pixel.SetData<Color>((Color[])(object)new Color[1] { Color.get_White() });
			}
			Vector2? posOpt = ContinentToScreen(target.TargetContinentX, target.TargetContinentY, bounds, centerX, centerY, scale);
			if (!posOpt.HasValue)
			{
				return;
			}
			Vector2 pos = posOpt.Value;
			if (_dbgEvery++ % 60 == 0 && DEBUG_LOGS)
			{
				Log.Warn($"[OverlayDbg] center=({centerX},{centerY}) scale={scale} " + $"target=({target.TargetContinentX},{target.TargetContinentY}) " + $"dxdy=({(float)target.TargetContinentX - centerX},{(float)target.TargetContinentY - centerY}) " + $"screen=({pos.X},{pos.Y})");
			}
			if (pos.X < (float)((Rectangle)(ref bounds)).get_Left() + 18f || pos.X > (float)((Rectangle)(ref bounds)).get_Right() - 18f || pos.Y < (float)((Rectangle)(ref bounds)).get_Top() + 18f || pos.Y > (float)((Rectangle)(ref bounds)).get_Bottom() - 18f)
			{
				Vector2 clamped = ClampToBounds(pos, bounds, 18f);
				Vector2 mapCenter = default(Vector2);
				((Vector2)(ref mapCenter))._002Ector((float)bounds.X + (float)bounds.Width / 2f, (float)bounds.Y + (float)bounds.Height / 2f);
				Vector2 dir = pos - mapCenter;
				if (((Vector2)(ref dir)).LengthSquared() < 0.001f)
				{
					((Vector2)(ref dir))._002Ector(1f, 0f);
				}
				else
				{
					((Vector2)(ref dir)).Normalize();
				}
				DrawRing(spriteBatch, clamped, 18f, 3f, Color.get_Yellow());
				DrawArrow(spriteBatch, clamped, dir, 16f, 3f, Color.get_Yellow());
			}
			else
			{
				DrawRing(spriteBatch, pos, 22f, 3f, Color.get_Yellow());
				DrawCross(spriteBatch, pos, 10f, 2f, Color.get_Yellow());
			}
		}
	}
}
