using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Models
{
	public class ScreenRegion
	{
		private readonly SettingEntry<Point> _location;

		private readonly SettingEntry<Point> _size;

		private Rectangle? _bounds;

		public Rectangle Bounds => (Rectangle)(((_003F?)_bounds) ?? new Rectangle(Location, Size));

		public string RegionName { get; set; }

		public Point Location
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _location.get_Value();
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_location.set_Value(value);
				_bounds = null;
			}
		}

		public Point Size
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _size.get_Value();
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_size.set_Value(value);
				_bounds = null;
			}
		}

		public ScreenRegion(string regionName, SettingEntry<Point> location, SettingEntry<Point> size)
		{
			RegionName = regionName;
			_location = location;
			_size = size;
		}
	}
}
