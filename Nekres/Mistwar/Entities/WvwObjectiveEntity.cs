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
	public class WvwObjectiveEntity : IDisposable
	{
		private static Texture2D _textureFortified = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324351.png");

		private static Texture2D _textureReinforced = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324350.png");

		private static Texture2D _textureSecured = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324349.png");

		private static Texture2D _textureClaimed = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304078.png");

		private static Texture2D _textureClaimedRepGuild = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304077.png");

		private static Texture2D _textureBuff = MistwarModule.ModuleInstance.ContentsManager.GetTexture("righteous_indignation.png");

		private static Texture2D _textureRuinEstate = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_estate.png");

		private static Texture2D _textureRuinTemple = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_temple.png");

		private static Texture2D _textureRuinOverlook = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_overlook.png");

		private static Texture2D _textureRuinHollow = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_hollow.png");

		private static Texture2D _textureRuinAscent = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_ascent.png");

		private static Texture2D _textureRuinOther = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_other.png");

		private static Texture2D _textureWayPoint = MistwarModule.ModuleInstance.ContentsManager.GetTexture("157353.png");

		private static Texture2D _textureWayPointHover = MistwarModule.ModuleInstance.ContentsManager.GetTexture("60970.png");

		private static Texture2D _textureWayPointContested = MistwarModule.ModuleInstance.ContentsManager.GetTexture("102349.png");

		private static IReadOnlyDictionary<string, Texture2D> _ruinsTexLookUp = new Dictionary<string, Texture2D>
		{
			{ "95-62", _textureRuinTemple },
			{ "96-62", _textureRuinTemple },
			{ "1099-121", _textureRuinOther },
			{ "96-66", _textureRuinAscent },
			{ "95-66", _textureRuinAscent },
			{ "1099-118", _textureRuinOther },
			{ "96-63", _textureRuinHollow },
			{ "95-63", _textureRuinHollow },
			{ "1099-119", _textureRuinOther },
			{ "96-65", _textureRuinOverlook },
			{ "95-65", _textureRuinOverlook },
			{ "1099-120", _textureRuinOther },
			{ "96-64", _textureRuinEstate },
			{ "95-64", _textureRuinEstate },
			{ "1099-122", _textureRuinOther }
		};

		private static Color _colorRed = new Color(213, 71, 67);

		private static Color _colorGreen = new Color(73, 190, 111);

		private static Color _colorBlue = new Color(100, 164, 228);

		private static Color _colorNeutral = Color.get_DimGray();

		public static Color BrightGold = new Color(223, 194, 149, 255);

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
				if (_lastFlipped != value)
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
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				if (_owner != value)
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
				if (_claimedBy != value)
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
				if (_guildUpgrades?.SequenceEqual(value) ?? (_guildUpgrades != value))
				{
					_guildUpgrades = value;
					LastModified = DateTime.UtcNow;
				}
			}
		}

		public Color TeamColor => GetColor();

		public Texture2D Icon { get; }

		public TimeSpan BuffDuration { get; }

		public Texture2D UpgradeTexture => GetUpgradeTierTexture();

		public Texture2D ClaimedTexture
		{
			get
			{
				if (!ClaimedBy.Equals(MistwarModule.ModuleInstance.WvwService.CurrentGuild))
				{
					return _textureClaimed;
				}
				return _textureClaimedRepGuild;
			}
		}

		public Texture2D BuffTexture => _textureBuff;

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
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			if (_textureFortified == null)
			{
				_textureFortified = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324351.png");
			}
			if (_textureReinforced == null)
			{
				_textureReinforced = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324350.png");
			}
			if (_textureSecured == null)
			{
				_textureSecured = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324349.png");
			}
			if (_textureClaimed == null)
			{
				_textureClaimed = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304078.png");
			}
			if (_textureClaimedRepGuild == null)
			{
				_textureClaimedRepGuild = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304077.png");
			}
			if (_textureBuff == null)
			{
				_textureBuff = MistwarModule.ModuleInstance.ContentsManager.GetTexture("righteous_indignation.png");
			}
			if (_textureRuinEstate == null)
			{
				_textureRuinEstate = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_estate.png");
			}
			if (_textureRuinTemple == null)
			{
				_textureRuinTemple = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_temple.png");
			}
			if (_textureRuinOverlook == null)
			{
				_textureRuinOverlook = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_overlook.png");
			}
			if (_textureRuinHollow == null)
			{
				_textureRuinHollow = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_hollow.png");
			}
			if (_textureRuinAscent == null)
			{
				_textureRuinAscent = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_ascent.png");
			}
			if (_textureRuinOther == null)
			{
				_textureRuinOther = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_other.png");
			}
			if (_textureWayPoint == null)
			{
				_textureWayPoint = MistwarModule.ModuleInstance.ContentsManager.GetTexture("157353.png");
			}
			if (_textureWayPointHover == null)
			{
				_textureWayPointHover = MistwarModule.ModuleInstance.ContentsManager.GetTexture("60970.png");
			}
			if (_textureWayPointContested == null)
			{
				_textureWayPointContested = MistwarModule.ModuleInstance.ContentsManager.GetTexture("102349.png");
			}
			if (_ruinsTexLookUp == null)
			{
				_ruinsTexLookUp = new Dictionary<string, Texture2D>
				{
					{ "95-62", _textureRuinTemple },
					{ "96-62", _textureRuinTemple },
					{ "1099-121", _textureRuinOther },
					{ "96-66", _textureRuinAscent },
					{ "95-66", _textureRuinAscent },
					{ "1099-118", _textureRuinOther },
					{ "96-63", _textureRuinHollow },
					{ "95-63", _textureRuinHollow },
					{ "1099-119", _textureRuinOther },
					{ "96-65", _textureRuinOverlook },
					{ "95-65", _textureRuinOverlook },
					{ "1099-120", _textureRuinOther },
					{ "96-64", _textureRuinEstate },
					{ "95-64", _textureRuinEstate },
					{ "1099-122", _textureRuinOther }
				};
			}
			_internalObjective = objective;
			ContinentFloorRegionMapSector internalSector = map.get_Sectors()[objective.get_SectorId()];
			_opacity = 1f;
			Icon = GetTexture(ApiEnum<WvwObjectiveType>.op_Implicit(objective.get_Type()));
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

		public Texture2D GetWayPointIcon(bool hover)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (Owner != MistwarModule.ModuleInstance.WvwService.CurrentTeam)
			{
				return _textureWayPointContested;
			}
			if (!hover)
			{
				return _textureWayPoint;
			}
			return _textureWayPointHover;
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
				0 => _colorRed, 
				1 => _colorBlue, 
				2 => _colorGreen, 
				_ => _colorNeutral, 
			});
		}

		private Texture2D GetUpgradeTierTexture()
		{
			return (Texture2D)(GetTier() switch
			{
				WvwObjectiveTier.Fortified => _textureFortified, 
				WvwObjectiveTier.Reinforced => _textureReinforced, 
				WvwObjectiveTier.Secured => _textureSecured, 
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

		public void Dispose()
		{
			Texture2D icon = Icon;
			if (icon != null)
			{
				((GraphicsResource)icon).Dispose();
			}
			Texture2D textureFortified = _textureFortified;
			if (textureFortified != null)
			{
				((GraphicsResource)textureFortified).Dispose();
			}
			_textureFortified = null;
			Texture2D textureReinforced = _textureReinforced;
			if (textureReinforced != null)
			{
				((GraphicsResource)textureReinforced).Dispose();
			}
			_textureReinforced = null;
			Texture2D textureSecured = _textureSecured;
			if (textureSecured != null)
			{
				((GraphicsResource)textureSecured).Dispose();
			}
			_textureSecured = null;
			Texture2D textureClaimed = _textureClaimed;
			if (textureClaimed != null)
			{
				((GraphicsResource)textureClaimed).Dispose();
			}
			_textureClaimed = null;
			Texture2D textureClaimedRepGuild = _textureClaimedRepGuild;
			if (textureClaimedRepGuild != null)
			{
				((GraphicsResource)textureClaimedRepGuild).Dispose();
			}
			_textureClaimedRepGuild = null;
			Texture2D textureBuff = _textureBuff;
			if (textureBuff != null)
			{
				((GraphicsResource)textureBuff).Dispose();
			}
			_textureBuff = null;
			Texture2D textureRuinEstate = _textureRuinEstate;
			if (textureRuinEstate != null)
			{
				((GraphicsResource)textureRuinEstate).Dispose();
			}
			_textureRuinEstate = null;
			Texture2D textureRuinTemple = _textureRuinTemple;
			if (textureRuinTemple != null)
			{
				((GraphicsResource)textureRuinTemple).Dispose();
			}
			_textureRuinTemple = null;
			Texture2D textureRuinOverlook = _textureRuinOverlook;
			if (textureRuinOverlook != null)
			{
				((GraphicsResource)textureRuinOverlook).Dispose();
			}
			_textureRuinOverlook = null;
			Texture2D textureRuinHollow = _textureRuinHollow;
			if (textureRuinHollow != null)
			{
				((GraphicsResource)textureRuinHollow).Dispose();
			}
			_textureRuinHollow = null;
			Texture2D textureRuinAscent = _textureRuinAscent;
			if (textureRuinAscent != null)
			{
				((GraphicsResource)textureRuinAscent).Dispose();
			}
			_textureRuinAscent = null;
			Texture2D textureRuinOther = _textureRuinOther;
			if (textureRuinOther != null)
			{
				((GraphicsResource)textureRuinOther).Dispose();
			}
			_textureRuinOther = null;
			Texture2D textureWayPoint = _textureWayPoint;
			if (textureWayPoint != null)
			{
				((GraphicsResource)textureWayPoint).Dispose();
			}
			_textureWayPoint = null;
			Texture2D textureWayPointHover = _textureWayPointHover;
			if (textureWayPointHover != null)
			{
				((GraphicsResource)textureWayPointHover).Dispose();
			}
			_textureWayPointHover = null;
			Texture2D textureWayPointContested = _textureWayPointContested;
			if (textureWayPointContested != null)
			{
				((GraphicsResource)textureWayPointContested).Dispose();
			}
			_textureWayPointContested = null;
			_ruinsTexLookUp = null;
		}
	}
}
