using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public class RectAnchor
	{
		public struct RectTarget
		{
			public SpriteBatch SpriteBatch { get; }

			public Control Control { get; }

			public RectangleF Rect { get; }

			public Rectangle ControlRect { get; }

			public RectTarget(SpriteBatch spriteBatch, Control control, RectangleF rect)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatch = spriteBatch;
				Control = control;
				Rect = rect;
				ControlRect = ToCtrl(control, rect);
			}

			public void DrawString(string text, BitmapFont font, Color color, bool wrap = false, bool stroke = false, int strokeDistance = 0, HorizontalAlignment horizontalAlignment = 0, VerticalAlignment verticalAlignment = 1)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatchExtensions.DrawStringOnCtrl(SpriteBatch, Control, text, font, ControlRect, color, wrap, stroke, strokeDistance, horizontalAlignment, verticalAlignment);
			}
		}

		private readonly List<RectAnchor> _children = new List<RectAnchor>();

		public bool Visible { get; set; } = true;


		public virtual Vector2 AnchorMin { get; set; } = new Vector2(0f, 0f);


		public virtual Vector2 AnchorMax { get; set; } = new Vector2(1f, 1f);


		public virtual Vector2 Position { get; set; } = new Vector2(0f, 0f);


		public virtual Vector2 SizeDelta { get; set; } = new Vector2(0f, 0f);


		public virtual Vector2 Pivot { get; set; } = new Vector2(0.5f, 0.5f);


		public virtual Vector2 Anchor
		{
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_0004: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				Vector2 val3 = (AnchorMin = (AnchorMax = value));
			}
		}

		public Action<RectAnchor> Update { get; set; }

		public RectAnchor Parent { get; private set; }

		public IReadOnlyList<RectAnchor> Children => _children;

		public T AddChild<T>(T child) where T : RectAnchor
		{
			child.Parent = this;
			_children.Add(child);
			return child;
		}

		public void ClearChildren()
		{
			foreach (RectAnchor child in _children)
			{
				child.Parent = null;
			}
			_children.Clear();
		}

		public bool RemoveChild(RectAnchor child)
		{
			bool num = _children.Remove(child);
			if (num)
			{
				child.Parent = null;
			}
			return num;
		}

		public RectangleF Target(RectangleF container)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleF(Point2.op_Implicit(new Vector2(container.X + container.Width * AnchorMin.X + Position.X - SizeDelta.X * Pivot.X, container.Y + container.Height * AnchorMin.Y + Position.Y - SizeDelta.Y * Pivot.Y)), new Size2((AnchorMax.X - AnchorMin.X) * container.Width + SizeDelta.X, (AnchorMax.Y - AnchorMin.Y) * container.Height + SizeDelta.Y));
		}

		public static RectangleF ToCtrlF(Control ctrl, RectangleF target)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleF(target.X - (float)ctrl.get_Left(), target.Y - (float)ctrl.get_Top(), target.Width, target.Height);
		}

		public static Rectangle ToCtrl(Control ctrl, RectangleF target)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return RectangleExtensions.ToRectangle(ToCtrlF(ctrl, target));
		}

		public void Draw(SpriteBatch spriteBatch, Control control, RectangleF rect)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			Update?.Invoke(this);
			if (!Visible)
			{
				return;
			}
			rect = Target(rect);
			RectTarget target = new RectTarget(spriteBatch, control, rect);
			EarlyDraw(target);
			foreach (RectAnchor child in _children)
			{
				child.Draw(spriteBatch, control, rect);
			}
			LateDraw(target);
		}

		protected virtual void EarlyDraw(RectTarget target)
		{
		}

		protected virtual void LateDraw(RectTarget target)
		{
		}
	}
}
