using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Events_Module
{
	public class ScreenRegion
	{
		private Rectangle? _bounds;

		private readonly SettingEntry<Point> _location;

		private readonly SettingEntry<Point> _size;

		public Rectangle Bounds => (_bounds ?? (_bounds = new Rectangle(Location, Size))).Value;

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
