using System;
using System.Linq;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class EditState
	{
		private FullRace _fullRace;

		private RacePoint _selected;

		public FullRace FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				if (_fullRace != value)
				{
					Selected = null;
					_fullRace = value;
					this.RaceLoaded?.Invoke(_fullRace);
				}
			}
		}

		public Race Race => FullRace?.Race;

		public RacePoint Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				if (value == null || Race == null || Race.RacePoints.Contains(value))
				{
					_selected = value;
					this.PointSelected?.Invoke(_selected);
				}
			}
		}

		public int SelectedPointIndex
		{
			get
			{
				return PointIndexOf(Selected);
			}
			set
			{
				if (Race != null)
				{
					Selected = Race.RacePoints[Math.Max(Math.Min(value, Race.RacePoints.Count - 1), 0)];
				}
			}
		}

		public event Action<FullRace> RaceLoaded;

		public event Action<FullRace> RaceModified;

		public event Action<RacePoint> PointSelected;

		public event Action<RacePoint> PointInserted;

		public event Action<RacePoint> PointRemoved;

		public event Action<RacePoint, bool> PointSwapped;

		public event Action<RacePoint> PointModified;

		public int PointIndexOf(RacePoint point)
		{
			return Race?.RacePoints.IndexOf(point) ?? (-1);
		}

		public bool SetRaceMap(int map)
		{
			if (map <= 0)
			{
				throw new CommandException(Strings.ExceptionInvalidRaceMap);
			}
			Race.MapId = map;
			this.RaceModified?.Invoke(FullRace);
			return true;
		}

		public bool SetRaceType(RaceType type)
		{
			if (!Enum.IsDefined(type.GetType(), type))
			{
				throw new CommandException(Strings.ExceptionInvalidRaceType);
			}
			Race.Type = type;
			this.RaceModified?.Invoke(FullRace);
			return true;
		}

		public bool RenameRace(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new CommandException(Strings.ExceptionEmptyRaceName);
			}
			if (name.Any((char c) => char.IsControl(c) || c > 'ÿ'))
			{
				throw new CommandException(Strings.ExceptionInvalidRaceName);
			}
			Race.Name = name;
			this.RaceModified?.Invoke(FullRace);
			return true;
		}

		public RacePoint InsertPoint(int at, RacePoint point)
		{
			Race.RacePoints.Insert(at, point);
			this.PointInserted?.Invoke(point);
			return point;
		}

		public int RemovePoint(RacePoint point)
		{
			if (Race.RacePoints.Count <= 2)
			{
				throw new CommandException(Strings.ExceptionMinimumCheckpoints);
			}
			int index = Race.RacePoints.IndexOf(point);
			if (index >= 0)
			{
				Race.RacePoints.RemoveAt(index);
				this.PointRemoved?.Invoke(point);
			}
			return index;
		}

		public void RemovePoint(int at)
		{
			RacePoint point = Race.RacePoints[at];
			Race.RacePoints.RemoveAt(at);
			this.PointRemoved?.Invoke(point);
		}

		public bool SwapPoint(RacePoint point, bool previous)
		{
			int index = Race.RacePoints.IndexOf(point);
			if (index < 0)
			{
				return false;
			}
			int cap = Race.RacePoints.Count;
			int newIndex = ((index + ((!previous) ? 1 : (-1))) % cap + cap) % cap;
			if (index < 0 || newIndex < 0 || newIndex >= Race.RacePoints.Count)
			{
				return false;
			}
			RacePoint temp = get(index);
			set(index, get(newIndex));
			set(newIndex, temp);
			this.PointSwapped?.Invoke(point, previous);
			return true;
			RacePoint get(int n)
			{
				return Race.RacePoints[n];
			}
			void set(int n, RacePoint r)
			{
				Race.RacePoints[n] = r;
			}
		}

		public bool MovePoint(RacePoint point, Vector3 position)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (!Race.RacePoints.Contains(point))
			{
				return false;
			}
			point.Position = position;
			this.PointModified?.Invoke(point);
			return true;
		}

		public bool ResizePoint(RacePoint point, float radius)
		{
			if (!Race.RacePoints.Contains(point))
			{
				return false;
			}
			if (radius <= 0f)
			{
				throw new CommandException(Strings.ExceptionPointRadius);
			}
			point.Radius = radius;
			this.PointModified?.Invoke(point);
			return true;
		}

		public bool SetPointType(RacePoint point, RacePointType type)
		{
			if (!Race.RacePoints.Contains(point))
			{
				return false;
			}
			if (!Enum.IsDefined(type.GetType(), type))
			{
				throw new CommandException(Strings.ExceptionInvalidPointType);
			}
			point.Type = type;
			this.PointModified?.Invoke(point);
			return true;
		}
	}
}
