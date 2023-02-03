using System;
using Kenedia.Modules.BuildsManager.Enums;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TemplateItem : TemplateItem_json, IDisposable
	{
		private bool disposed;

		public EquipmentSlots Slot = EquipmentSlots.Unkown;

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
