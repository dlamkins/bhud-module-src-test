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
		private static readonly Texture2D TextureFortified = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324351.png");

		private static readonly Texture2D TextureReinforced = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324350.png");

		private static readonly Texture2D TextureSecured = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324349.png");

		private static readonly Texture2D TextureClaimed = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304078.png");

		private static readonly Texture2D TextureClaimedRepGuild = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304077.png");

		private static readonly Texture2D TextureBuff = MistwarModule.ModuleInstance.ContentsManager.GetTexture("righteous_indignation.png");

		private static readonly Texture2D TextureRuinEstate = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_estate.png");

		private static readonly Texture2D TextureRuinTemple = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_temple.png");

		private static readonly Texture2D TextureRuinOverlook = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_overlook.png");

		private static readonly Texture2D TextureRuinHollow = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_hollow.png");

		private static readonly Texture2D TextureRuinAscent = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_ascent.png");

		private static readonly Texture2D TextureRuinOther = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_other.png");

		private static readonly Texture2D TextureWayPoint = MistwarModule.ModuleInstance.ContentsManager.GetTexture("157353.png");

		private static readonly Texture2D TextureWayPointHover = MistwarModule.ModuleInstance.ContentsManager.GetTexture("60970.png");

		private static readonly Texture2D TextureWayPointContested = MistwarModule.ModuleInstance.ContentsManager.GetTexture("102349.png");

		private static readonly IReadOnlyDictionary<string, Texture2D> _ruinsTexLookUp = new Dictionary<string, Texture2D>
		{
			{ "95-62", TextureRuinTemple },
			{ "96-62", TextureRuinTemple },
			{ "1099-121", TextureRuinOther },
			{ "96-66", TextureRuinAscent },
			{ "95-66", TextureRuinAscent },
			{ "1099-118", TextureRuinOther },
			{ "96-63", TextureRuinHollow },
			{ "95-63", TextureRuinHollow },
			{ "1099-119", TextureRuinOther },
			{ "96-65", TextureRuinOverlook },
			{ "95-65", TextureRuinOverlook },
			{ "1099-120", TextureRuinOther },
			{ "96-64", TextureRuinEstate },
			{ "95-64", TextureRuinEstate },
			{ "1099-122", TextureRuinOther }
		};

		private static readonly Color ColorRed = new Color(213, 71, 67);

		private static readonly Color ColorGreen = new Color(73, 190, 111);

		private static readonly Color ColorBlue = new Color(100, 164, 228);

		private static readonly Color ColorNeutral = Color.get_DimGray();

		public static readonly Color BrightGold = new Color(223, 194, 149, 255);

		private readonly WvwObjective _internalObjective;

		private readonly ContinentFloorRegionMapSector _internalSector;

		private float _opacity;

		public int MapId { get; }

		public string Id => _internalObjective.get_Id();

		public string Name => _internalObjective.get_Name();

		public WvwObjectiveType Type => ApiEnum<WvwObjectiveType>.op_Implicit(_internalObjective.get_Type());

		public IEnumerable<Point> Bounds { get; }

		public Point Center { get; }

		public Vector3 WorldPosition { get; }

		public DateTime LastFlipped { get; set; }

		public WvwOwner Owner { get; set; }

		public Color TeamColor => GetColor();

		public Guid ClaimedBy { get; set; }

		public IReadOnlyList<int> GuildUpgrades { get; set; }

		public int YaksDelivered { get; set; }

		public Texture2D Icon { get; }

		public TimeSpan BuffDuration { get; }

		public Texture2D UpgradeTexture => GetUpgradeTierTexture();

		public Texture2D ClaimedTexture
		{
			get
			{
				if (!ClaimedBy.Equals(MistwarModule.ModuleInstance.WvwService.CurrentGuild))
				{
					return TextureClaimed;
				}
				return TextureClaimedRepGuild;
			}
		}

		public Texture2D BuffTexture => TextureBuff;

		public float Opacity => GetOpacity();

		public List<ContinentFloorRegionMapPoi> WayPoints { get; }

		public WvwObjectiveEntity(WvwObjective objective, ContinentFloorRegionMap map)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			WvwObjectiveEntity wvwObjectiveEntity = this;
			_internalObjective = objective;
			_internalSector = map.get_Sectors()[objective.get_SectorId()];
			_opacity = 1f;
			Icon = GetTexture(ApiEnum<WvwObjectiveType>.op_Implicit(objective.get_Type()));
			MapId = map.get_Id();
			Bounds = _internalSector.get_Bounds().Select(delegate(Coordinates2 coord)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				Rectangle continentRect2 = map.get_ContinentRect();
				return MapUtil.Refit(coord, ((Rectangle)(ref continentRect2)).get_TopLeft());
			});
			Coordinates2 coord2 = _internalSector.get_Coord();
			Rectangle continentRect = map.get_ContinentRect();
			Center = MapUtil.Refit(coord2, ((Rectangle)(ref continentRect)).get_TopLeft());
			LastFlipped = DateTime.MinValue.ToUniversalTime();
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
				return PolygonUtil.InBounds(new Vector2(num, (float)((Coordinates2)(ref coord4)).get_Y()), ((IEnumerable<Coordinates2>)wvwObjectiveEntity._internalSector.get_Bounds()).Select((Func<Coordinates2, Vector2>)((Coordinates2 z) => new Vector2((float)((Coordinates2)(ref z)).get_X(), (float)((Coordinates2)(ref z)).get_Y()))).ToList());
			}).ToList();
			foreach (ContinentFloorRegionMapPoi wayPoint in WayPoints)
			{
				Coordinates2 coord3 = wayPoint.get_Coord();
				continentRect = map.get_ContinentRect();
				Point fit = MapUtil.Refit(coord3, ((Rectangle)(ref continentRect)).get_TopLeft());
				wayPoint.set_Coord(new Coordinates2((double)fit.X, (double)fit.Y));
			}
		}

		public Texture2D GetWayPointIcon(bool hover)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (Owner != MistwarModule.ModuleInstance.WvwService.CurrentTeam)
			{
				return TextureWayPointContested;
			}
			if (!hover)
			{
				return TextureWayPoint;
			}
			return TextureWayPointHover;
		}

		public bool IsOwned()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)Owner <= 1;
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
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			if (!IsSpawn())
			{
				if (GetTier() == WvwObjectiveTier.Fortified)
				{
					if ((int)Type != 3)
					{
						return (int)Type == 2;
					}
					return true;
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

		private Texture2D GetTexture(WvwObjectiveType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected I4, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			switch (type - 1)
			{
			case 0:
			case 1:
			case 2:
			case 4:
				return MistwarModule.ModuleInstance.ContentsManager.GetTexture($"{type}.png");
			case 5:
			{
				if (!_ruinsTexLookUp.TryGetValue(Id, out var tex))
				{
					return null;
				}
				return tex;
			}
			default:
				return null;
			}
		}

		private Color GetColor()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected I4, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			WvwOwner owner = Owner;
			return (Color)((owner - 2) switch
			{
				0 => ColorRed, 
				1 => ColorBlue, 
				2 => ColorGreen, 
				_ => ColorNeutral, 
			});
		}

		private Texture2D GetUpgradeTierTexture()
		{
			return (Texture2D)(GetTier() switch
			{
				WvwObjectiveTier.Fortified => TextureFortified, 
				WvwObjectiveTier.Reinforced => TextureReinforced, 
				WvwObjectiveTier.Secured => TextureSecured, 
				_ => Textures.get_TransparentPixel(), 
			});
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
