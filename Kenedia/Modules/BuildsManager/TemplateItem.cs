using System;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager
{
	public class TemplateItem : TemplateItem_json, IDisposable
	{
		private bool disposed;

		public _EquipmentSlots Slot = _EquipmentSlots.Unkown;

		public API.Stat Stat;

		public Rectangle Bounds;

		public Rectangle StatBounds;

		public Rectangle UpgradeBounds;

		public bool Hovered;

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				Stat = null;
			}
		}
	}
}
