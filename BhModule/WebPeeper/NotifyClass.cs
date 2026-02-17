using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.WebPeeper
{
	internal class NotifyClass : Control
	{
		private float _duration = 3000f;

		private string _message;

		private bool _waitingForPaint = true;

		private DateTime _msgStartTime = DateTime.Now;

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(((Control)((Control)this).get_Parent()).get_Size().X, 200));
			((Control)this).set_Location(new Point(0, ((Control)((Control)this).get_Parent()).get_Size().Y / 10 * 2));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			if (_message != null)
			{
				if (_waitingForPaint)
				{
					_msgStartTime = DateTime.Now;
				}
				float num = (float)(DateTime.Now - _msgStartTime).TotalMilliseconds;
				float num2 = _duration - num;
				float num3 = ((num2 > 1000f) ? 1f : (num2 / 1000f));
				if (num3 < 0f)
				{
					Clear();
					return;
				}
				Color val = Color.get_Yellow() * num3;
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _message, GameService.Content.get_DefaultFont32(), new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()), val, false, false, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
			}
			_waitingForPaint = false;
		}

		public void Clear()
		{
			((Control)this).set_Parent((Container)null);
			_message = null;
		}

		public void Show(string text, float duration = 3000f)
		{
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_msgStartTime = DateTime.Now;
			_message = text;
			_duration = duration;
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public NotifyClass()
			: this()
		{
		}
	}
}
