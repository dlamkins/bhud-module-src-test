using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class Pointer : Control
	{
		private readonly DetailedTexture _pointerArrow = new DetailedTexture(784266)
		{
			TextureRegion = new Rectangle(16, 16, 32, 32)
		};

		private float _animationStart;

		private Rectangle _anchorDrawBounds;

		private Control _anchor;

		private WindowBase2 _container;

		public Control Anchor
		{
			get
			{
				return _anchor;
			}
			set
			{
				Common.SetProperty(ref _anchor, value, new ValueChangedEventHandler<Control>(SetAnchor));
			}
		}

		public float BounceDistance { get; set; } = 0.25f;


		public Pointer()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(32);
			base.Parent = Control.Graphics.SpriteScreen;
			base.ClipsBounds = false;
		}

		private static Control GetAncestorsParent(Control control)
		{
			if (control != null)
			{
				if (control != Control.Graphics.SpriteScreen && control.Visible)
				{
					return GetAncestorsParent(control.Parent);
				}
				return control;
			}
			return null;
		}

		private static bool IsDrawn(Control c, Rectangle b)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (c.Parent != null && c.Parent.Visible)
			{
				Rectangle absoluteBounds = c.Parent.AbsoluteBounds;
				if (((Rectangle)(ref absoluteBounds)).Contains(((Rectangle)(ref b)).get_Center()))
				{
					if (c.Parent != Control.Graphics.SpriteScreen)
					{
						return IsDrawn(c.Parent, b);
					}
					return true;
				}
			}
			return false;
		}

		protected override CaptureType CapturesInput()
		{
			return CaptureType.None;
		}

		private void SetAnchor(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Control> e)
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (Anchor != null && Anchor.Visible && IsDrawn(Anchor, base.AbsoluteBounds))
			{
				_pointerArrow.Draw(this, spriteBatch);
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			base.DoUpdate(gameTime);
			if (Anchor != null && Anchor.Visible)
			{
				_animationStart += (float)gameTime.get_ElapsedGameTime().TotalSeconds;
				int size = Math.Min(base.Width, base.Height);
				Rectangle absoluteBounds = Anchor.AbsoluteBounds;
				int num = ((Rectangle)(ref absoluteBounds)).get_Left() - size / 2;
				absoluteBounds = Anchor.AbsoluteBounds;
				base.Location = new Point(num, ((Rectangle)(ref absoluteBounds)).get_Center().Y - size / 2);
				_anchorDrawBounds = new Rectangle(1, 0, size, size);
				BounceDistance = 15f;
				float duration = 0.75f;
				int animationOffset = (int)Tweening.Quartic.EaseOut(_animationStart, 0f - BounceDistance, BounceDistance, duration);
				_pointerArrow.Bounds = _anchorDrawBounds.Add(animationOffset, 0, 0, 0);
				if ((float)animationOffset < 0f - BounceDistance)
				{
					_animationStart -= duration * 2f;
				}
			}
		}
	}
}
