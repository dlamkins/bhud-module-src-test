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

		public Control Anchor
		{
			get
			{
				return _anchor;
			}
			set
			{
				Common.SetProperty(ref _anchor, value, SetAnchor);
			}
		}

		public float BounceDistance { get; set; } = 0.25f;


		public Pointer()
			: this()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(32));
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((Control)this).set_ZIndex(int.MaxValue);
			((Control)this).set_ClipsBounds(false);
		}

		private static Control GetAncestorsParent(Control control)
		{
			if (control != null)
			{
				if (control != Control.get_Graphics().get_SpriteScreen() && control.get_Visible())
				{
					return GetAncestorsParent((Control)(object)control.get_Parent());
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
			if (c.get_Parent() != null && ((Control)c.get_Parent()).get_Visible())
			{
				Rectangle absoluteBounds = ((Control)c.get_Parent()).get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).Contains(((Rectangle)(ref b)).get_Center()))
				{
					if (c.get_Parent() != Control.get_Graphics().get_SpriteScreen())
					{
						return IsDrawn((Control)(object)c.get_Parent(), b);
					}
					return true;
				}
			}
			return false;
		}

		private void SetAnchor(object sender, ValueChangedEventArgs<Control> e)
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (Anchor != null && Anchor.get_Visible() && IsDrawn(Anchor, ((Control)this).get_AbsoluteBounds()))
			{
				_pointerArrow.Draw((Control)(object)this, spriteBatch);
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
			((Control)this).DoUpdate(gameTime);
			if (Anchor != null && Anchor.get_Visible())
			{
				_animationStart += (float)gameTime.get_ElapsedGameTime().TotalSeconds;
				int size = Math.Min(((Control)this).get_Width(), ((Control)this).get_Height());
				Rectangle absoluteBounds = Anchor.get_AbsoluteBounds();
				int num = ((Rectangle)(ref absoluteBounds)).get_Left() - size / 2;
				absoluteBounds = Anchor.get_AbsoluteBounds();
				((Control)this).set_Location(new Point(num, ((Rectangle)(ref absoluteBounds)).get_Center().Y - size / 2));
				_anchorDrawBounds = new Rectangle(1, 0, size, size);
				BounceDistance = 15f;
				float duration = 0.75f;
				int animationOffset = (int)Tweening.Quartic.EaseOut(_animationStart, 0f - BounceDistance, BounceDistance, duration);
				_pointerArrow.Bounds = RectangleExtension.Add(_anchorDrawBounds, animationOffset, 0, 0, 0);
				if ((float)animationOffset < 0f - BounceDistance)
				{
					_animationStart -= duration * 2f;
				}
			}
		}
	}
}
