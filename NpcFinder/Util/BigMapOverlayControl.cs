using System;
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

		private Vector2? ContinentToScreen(double cx, double cy, Rectangle bounds)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (!MumbleReader.TryGetWorldMapUi(out var centerX, out var centerY, out var scale))
			{
				return null;
			}
			if (Math.Abs(scale) < 1E-06f)
			{
				return null;
			}
			Vector2 val = new Vector2((float)bounds.X + (float)bounds.Width / 2f, (float)bounds.Y + (float)bounds.Height / 2f);
			float dx = (float)cx - centerX;
			float dy = (float)cy - centerY;
			float px = val.X + dx / scale;
			float py = val.Y + dy / scale;
			return new Vector2(px, py);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
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
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			bool? obj2;
			if (gw2Mumble == null)
			{
				obj2 = null;
			}
			else
			{
				UI uI = gw2Mumble.get_UI();
				obj2 = ((uI != null) ? new bool?(uI.get_IsMapOpen()) : null);
			}
			bool? flag = obj2;
			if (!flag.GetValueOrDefault())
			{
				return;
			}
			Vector2? screenPos = ContinentToScreen(target.TargetContinentX, target.TargetContinentY, bounds);
			if (!screenPos.HasValue)
			{
				return;
			}
			Vector2 pos = screenPos.Value;
			if (MumbleReader.TryGetWorldMapUi(out var centerX, out var centerY, out var scale))
			{
				if (_dbgEvery++ % 60 == 0)
				{
					Log.Warn($"[OverlayDbg] center=({centerX},{centerY}) scale={scale} " + $"target=({target.TargetContinentX},{target.TargetContinentY}) " + $"dxdy=({(float)target.TargetContinentX - centerX},{(float)target.TargetContinentY - centerY}) " + $"screen=({pos.X},{pos.Y})");
				}
				if (_pixel == null)
				{
					_pixel = new Texture2D(((GraphicsResource)spriteBatch).get_GraphicsDevice(), 1, 1);
					_pixel.SetData<Color>((Color[])(object)new Color[1] { Color.get_White() });
				}
				DrawRing(spriteBatch, pos, 22f, 3f, Color.get_Yellow());
				DrawCross(spriteBatch, pos, 10f, 2f, Color.get_Yellow());
			}
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
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			Vector2 edge = end - start;
			float angle = (float)Math.Atan2(edge.Y, edge.X);
			float length = ((Vector2)(ref edge)).Length();
			sb.Draw(_pixel, new Rectangle((int)start.X, (int)start.Y, (int)length, (int)thickness), (Rectangle?)null, color, angle, new Vector2(0f, 0.5f), (SpriteEffects)0, 0f);
		}

		private static bool ContinentToMap(NpcTarget target, out float mapX, out float mapY)
		{
			mapX = (mapY = 0f);
			Gw2MapInfo mi = target?.MapInfo;
			if (mi == null)
			{
				return false;
			}
			Rect2D mr = mi.MapRect;
			Rect2D cr = mi.ContinentRect;
			double cMinX = Math.Min(cr.X1, cr.X2);
			double cMaxX = Math.Max(cr.X1, cr.X2);
			double cMinY = Math.Min(cr.Y1, cr.Y2);
			double cMaxY = Math.Max(cr.Y1, cr.Y2);
			double mMinX = Math.Min(mr.X1, mr.X2);
			double mMaxX = Math.Max(mr.X1, mr.X2);
			double mMinY = Math.Min(mr.Y1, mr.Y2);
			double num = Math.Max(mr.Y1, mr.Y2);
			double cW = cMaxX - cMinX;
			double cH = cMaxY - cMinY;
			double mW = mMaxX - mMinX;
			double mH = num - mMinY;
			if (cW <= 1E-06 || cH <= 1E-06 || mW <= 1E-06 || mH <= 1E-06)
			{
				return false;
			}
			double u = (target.TargetContinentX - cMinX) / cW;
			double v = (target.TargetContinentY - cMinY) / cH;
			u = Math.Max(0.0, Math.Min(1.0, u));
			v = Math.Max(0.0, Math.Min(1.0, v));
			v = 1.0 - v;
			mapX = (float)(mMinX + u * mW);
			mapY = (float)(mMinY + v * mH);
			return true;
		}

		private static void LogOncePerSecond(Logger log, string msg)
		{
			DateTime now = DateTime.UtcNow;
			if (!((now - _lastLog).TotalSeconds < 1.0))
			{
				_lastLog = now;
				log.Warn(msg);
			}
		}
	}
}
