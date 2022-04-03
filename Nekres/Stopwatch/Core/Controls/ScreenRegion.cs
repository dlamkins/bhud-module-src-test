using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Nekres.Stopwatch.Core.Controls
{
	public class ScreenRegion
	{
		private Rectangle? _bounds;

		private readonly SettingEntry<Point> _location;

		private readonly SettingEntry<Point> _size;

		public Rectangle Bounds
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				Rectangle valueOrDefault = _bounds.GetValueOrDefault();
				if (!_bounds.HasValue)
				{
					((Rectangle)(ref valueOrDefault))._002Ector(Location, Size);
					_bounds = valueOrDefault;
					return valueOrDefault;
				}
				return valueOrDefault;
			}
		}

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
