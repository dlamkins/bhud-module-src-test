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
				if (!IsClaimedByRepresentedGuild())
				{
					return TextureClaimed;
				}
				return TextureClaimedRepGuild;
			}
		}

		public Texture2D BuffTexture => TextureBuff;

		public float Opacity => GetOpacity();

		public WvwObjectiveEntity(WvwObjective objective, Map map, ContinentFloorRegionMapSector sector)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			_internalObjective = objective;
			_opacity = 1f;
			Icon = GetTexture(ApiEnum<WvwObjectiveType>.op_Implicit(objective.get_Type()));
			MapId = map.get_Id();
			Bounds = sector.get_Bounds().Select(delegate(Coordinates2 coord)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				Rectangle continentRect2 = map.get_ContinentRect();
				return MapUtil.Refit(coord, ((Rectangle)(ref continentRect2)).get_TopLeft());
			});
			Coordinates2 coord2 = sector.get_Coord();
			Rectangle continentRect = map.get_ContinentRect();
			Center = MapUtil.Refit(coord2, ((Rectangle)(ref continentRect)).get_TopLeft());
			LastFlipped = DateTime.MinValue.ToUniversalTime();
			BuffDuration = new TimeSpan(0, 5, 0);
			Coordinates3 v = objective.get_Coord();
			if (objective.get_Id().Equals("38-15") && Math.Abs(((Coordinates3)(ref v)).get_X() - 11766.3) < 1.0 && Math.Abs(((Coordinates3)(ref v)).get_Y() - 14793.5) < 1.0 && Math.Abs(((Coordinates3)(ref v)).get_Z() - -2133.39) < 1.0)
			{
				Coordinates3 coord3 = objective.get_Coord();
				((Coordinates3)(ref v))._002Ector(11462.5, 15490.0, ((Coordinates3)(ref coord3)).get_Z() - 500.0);
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
			Vector3 pos = default(Vector3);
			((Vector3)(ref pos))._002Ector(WorldUtil.GameToWorldCoord((float)((((Coordinates3)(ref v)).get_X() - (double)offset.X) * 24.0)), WorldUtil.GameToWorldCoord((float)((0.0 - (((Coordinates3)(ref v)).get_Y() - (double)offset.Z)) * 24.0)), WorldUtil.GameToWorldCoord((float)(0.0 - ((Coordinates3)(ref v)).get_Z())));
			WorldPosition = pos;
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
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			WvwOwner owner = Owner;
			return (Color)((owner - 2) switch
			{
				0 => ColorRed, 
				1 => ColorBlue, 
				2 => ColorGreen, 
				_ => ColorNeutral, 
			});
		}

		public bool IsClaimed()
		{
			return !ClaimedBy.Equals(Guid.Empty);
		}

		public bool IsClaimedByRepresentedGuild()
		{
			return ClaimedBy.Equals(MistwarModule.ModuleInstance.WvwService.CurrentGuild);
		}

		public bool HasGuildUpgrades()
		{
			return GuildUpgrades.IsNullOrEmpty();
		}

		public bool HasUpgraded()
		{
			return YaksDelivered >= 20;
		}

		public bool HasBuff(out TimeSpan remainingTime)
		{
			TimeSpan buffTime = DateTime.UtcNow.Subtract(LastFlipped);
			remainingTime = BuffDuration.Subtract(buffTime);
			return remainingTime.Ticks > 0;
		}

		private Texture2D GetUpgradeTierTexture()
		{
			if (YaksDelivered < 140)
			{
				if (YaksDelivered < 60)
				{
					return TextureSecured;
				}
				return TextureReinforced;
			}
			return TextureFortified;
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
