using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class TextArea : TextInputBase
	{
		public override int GetCursorIndexFromPosition(int x, int y)
		{
			throw new NotImplementedException();
		}

		protected override void MoveLine(int delta)
		{
			throw new NotImplementedException();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			throw new NotImplementedException();
		}

		protected override void UpdateScrolling()
		{
			throw new NotImplementedException();
		}

		public TextArea()
			: this()
		{
		}
	}
}
