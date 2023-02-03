using System;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class ProfessionSelection : IDisposable
	{
		private bool disposed;

		public API.Profession Profession;

		public Rectangle Bounds;

		public bool Hovered;

		public int Index;

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				Profession = null;
			}
		}
	}
}
