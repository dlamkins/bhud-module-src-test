using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Mistwar.Entities
{
	public class WvwObjectiveEntity
	{
		private DateTime _lastFlipped = DateTime.MinValue;

		private WvwOwner _owner = (WvwOwner)1;

		private Guid _claimedBy = Guid.Empty;

		private int _yaksDelivered;

		private IReadOnlyList<int> _guildUpgrades;

		private float _opacity;

		private readonly WvwObjective _internalObjective;

		public DateTime LastModified { get; private set; }

		public DateTime LastFlipped
		{
			get
			{
				return _lastFlipped;
			}
			set
			{
				if (!object.Equals(value, _lastFlipped))
				{
					_lastFlipped = value;
					LastModified = DateTime.UtcNow;
				}
			}
		}

		public WvwOwner Owner
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _owner;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if (!object.Equals(value, _owner))
				{
					_owner = value;
					LastModified = DateTime.UtcNow;
				}
			}
		}

		public Guid ClaimedBy
		{
			get
			{
				return _claimedBy;
			}
			set
			{
				if (!object.Equals(value, _claimedBy))
				{
					_claimedBy = value;
					LastModified = DateTime.UtcNow;
				}
			}
		}

		public int YaksDelivered
		{
			get
			{
				return _yaksDelivered;
			}
			set
			{
				if (_yaksDelivered != value)
				{
					_yaksDelivered = value;
					LastModified = DateTime.UtcNow;
				}
			}
		}

		public IReadOnlyList<int> GuildUpgrades
		{
			get
			{
				return _guildUpgrades;
			}
			set
			{
				if (value != null && _guildUpgrades != null && !value.OrderBy((int x) => x).SequenceEqual(_guildUpgrades.OrderBy((int x) => x)))
				{
					_guildUpgrades = value;
					LastModified = DateTime.UtcNow;
				}
				else if (!object.Equals(value, _guildUpgrades))
				{
					_guildUpgrades = value;
				}
			}
		}

		public Color TeamColor => MistwarModule.ModuleInstance.Resources.GetTeamColor(Owner);

		public Texture2D Icon { get; }

		public TimeSpan BuffDuration { get; }

		public Texture2D UpgradeTexture => MistwarModule.ModuleInstance.Resources.GetUpgradeTierTexture(GetTier());

		public Texture2D ClaimedTexture => MistwarModule.ModuleInstance.Resources.GetClaimedTexture(ClaimedBy);

		public Texture2D BuffTexture => MistwarModule.ModuleInstance.Resources.GetBuffTexture();

		public IEnumerable<Point> Bounds { get; }

		public Point Center { get; }

		public Vector3 WorldPosition { get; }

		public float Opacity => GetOpacity();

		public List<ContinentFloorRegionMapPoi> WayPoints { get; }

		public string Id => _internalObjective.get_Id();

		public string Name => _internalObjective.get_Name();

		public WvwObjectiveType Type => ApiEnum<WvwObjectiveType>.op_Implicit(_internalObjective.get_Type());

		public int MapId { get; }

		public WvwObjectiveEntity(WvwObjective objective, ContinentFloorRegionMap map)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			_internalObjective = objective;
			ContinentFloorRegionMapSector internalSector = map.get_Sectors()[objective.get_SectorId()];
			_opacity = 1f;
			Icon = MistwarModule.ModuleInstance.Resources.GetObjectiveTexture(ApiEnum<WvwObjectiveType>.op_Implicit(objective.get_Type()), objective.get_Id());
			MapId = map.get_Id();
			Bounds = internalSector.get_Bounds().Select(delegate(Coordinates2 coord)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				Rectangle continentRect2 = map.get_ContinentRect();
				return MapUtil.Refit(coord, ((Rectangle)(ref continentRect2)).get_TopLeft());
			});
			Coordinates2 coord2 = internalSector.get_Coord();
			Rectangle continentRect = map.get_ContinentRect();
			Center = MapUtil.Refit(coord2, ((Rectangle)(ref continentRect)).get_TopLeft());
			BuffDuration = new TimeSpan(0, 5, 0);
			WorldPosition = CalculateWorldPosition(map);
			WayPoints = map.get_PointsOfInterest().Values.Where((ContinentFloorRegionMapPoi x) => x.get_Type() == ApiEnum<PoiType>.op_Implicit((PoiType)2)).Where(delegate(ContinentFloorRegionMapPoi y)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				Coordinates2 coord4 = y.get_Coord();
				float num = (float)((Coordinates2)(ref coord4)).get_X();
				coord4 = y.get_Coord();
				return PolygonUtil.InBounds(new Vector2(num, (float)((Coordinates2)(ref coord4)).get_Y()), ((IEnumerable<Coordinates2>)internalSector.get_Bounds()).Select((Func<Coordinates2, Vector2>)((Coordinates2 z) => new Vector2((float)((Coordinates2)(ref z)).get_X(), (float)((Coordinates2)(ref z)).get_Y()))).ToList());
			}).ToList();
			foreach (ContinentFloorRegionMapPoi wayPoint in WayPoints)
			{
				Coordinates2 coord3 = wayPoint.get_Coord();
				continentRect = map.get_ContinentRect();
				Point fit = MapUtil.Refit(coord3, ((Rectangle)(ref continentRect)).get_TopLeft());
				wayPoint.set_Coord(new Coordinates2((double)fit.X, (double)fit.Y));
			}
		}

		public bool IsOwned()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			WvwOwner owner = Owner;
			return (int)owner == 2 || (int)owner == 4;
		}

		public bool IsClaimed()
		{
			return !ClaimedBy.Equals(Guid.Empty);
		}

		public bool HasGuildUpgrades()
		{
			return !GuildUpgrades.IsNullOrEmpty();
		}

		public bool HasUpgraded()
		{
			return YaksDelivered >= 20;
		}

		public bool HasEmergencyWaypoint()
		{
			if (HasGuildUpgrades())
			{
				return GuildUpgrades.Contains(178);
			}
			return false;
		}

		public bool HasRegularWaypoint()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			if (!IsSpawn())
			{
				if (GetTier() == WvwObjectiveTier.Fortified)
				{
					WvwObjectiveType type = Type;
					return (int)type == 3 || (int)type == 2;
				}
				return false;
			}
			return true;
		}

		public bool IsSpawn()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			return (int)Type == 9;
		}

		public WvwObjectiveTier GetTier()
		{
			if (YaksDelivered < 140)
			{
				if (YaksDelivered < 60)
				{
					if (YaksDelivered < 20)
					{
						return WvwObjectiveTier.Supported;
					}
					return WvwObjectiveTier.Secured;
				}
				return WvwObjectiveTier.Reinforced;
			}
			return WvwObjectiveTier.Fortified;
		}

		public bool HasBuff(out TimeSpan remainingTime)
		{
			TimeSpan buffTime = DateTime.UtcNow.Subtract(LastFlipped);
			remainingTime = BuffDuration.Subtract(buffTime);
			return remainingTime.Ticks > 0;
		}

		public float GetDistance()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return WorldPosition.Distance(GameService.Gw2Mumble.get_PlayerCamera().get_Position());
		}

		private Vector3 CalculateWorldPosition(ContinentFloorRegionMap map)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			Coordinates3 v = _internalObjective.get_Coord();
			if (_internalObjective.get_Id().Equals("38-15") && Math.Abs(((Coordinates3)(ref v)).get_X() - 11766.3) < 1.0 && Math.Abs(((Coordinates3)(ref v)).get_Y() - 14793.5) < 1.0 && Math.Abs(((Coordinates3)(ref v)).get_Z() - -2133.39) < 1.0)
			{
				Coordinates3 coord = _internalObjective.get_Coord();
				((Coordinates3)(ref v))._002Ector(11462.5, 15490.0, ((Coordinates3)(ref coord)).get_Z() - 500.0);
			}
			Rectangle r = map.get_ContinentRect();
			Coordinates2 val = ((Rectangle)(ref r)).get_TopLeft();
			double x = ((Coordinates2)(ref val)).get_X();
			val = ((Rectangle)(ref r)).get_BottomRight();
			float num = (float)((x + ((Coordinates2)(ref val)).get_X()) / 2.0);
			val = ((Rectangle)(ref r)).get_TopLeft();
			double y = ((Coordinates2)(ref val)).get_Y();
			val = ((Rectangle)(ref r)).get_BottomRight();
			Vector3 offset = default(Vector3);
			((Vector3)(ref offset))._002Ector(num, 0f, (float)((y + ((Coordinates2)(ref val)).get_Y()) / 2.0));
			return new Vector3(WorldUtil.GameToWorldCoord((float)((((Coordinates3)(ref v)).get_X() - (double)offset.X) * 24.0)), WorldUtil.GameToWorldCoord((float)((0.0 - (((Coordinates3)(ref v)).get_Y() - (double)offset.Z)) * 24.0)), WorldUtil.GameToWorldCoord((float)(0.0 - ((Coordinates3)(ref v)).get_Z())));
		}

		private float GetOpacity()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = GameService.Gw2Mumble.get_PlayerCamera().get_Position() - WorldPosition;
			_opacity = MathUtil.Clamp(MathUtil.Map(((Vector3)(ref val)).Length(), MistwarModule.ModuleInstance.MaxViewDistanceSetting.get_Value() * 50f, _opacity, 0f, 1f), 0f, 1f);
			return _opacity;
		}
	}
}
