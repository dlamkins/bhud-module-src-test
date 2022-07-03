using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Mistwar.Entities
{
	internal class WvwObjectiveEntity
	{
		private static readonly Texture2D TextureFortified = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324351.png");

		private static readonly Texture2D TextureReinforced = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324350.png");

		private static readonly Texture2D TextureSecured = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324349.png");

		private static readonly Texture2D TextureClaimed = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304078.png");

		private static readonly Texture2D TextureBuff = MistwarModule.ModuleInstance.ContentsManager.GetTexture("righteous_indignation.png");

		private static readonly Color ColorRed = new Color(213, 71, 67);

		private static readonly Color ColorGreen = new Color(73, 190, 111);

		private static readonly Color ColorBlue = new Color(100, 164, 228);

		private static readonly Color ColorNeutral = Color.get_DimGray();

		public static readonly Color BrightGold = new Color(223, 194, 149, 255);

		private readonly WvwObjective _internalObjective;

		public int MapId { get; }

		public string Id => _internalObjective.get_Id();

		public string Name => _internalObjective.get_Name();

		public WvwObjectiveType Type => ApiEnum<WvwObjectiveType>.op_Implicit(_internalObjective.get_Type());

		public IEnumerable<Point> Bounds { get; }

		public Point Center { get; }

		public DateTime LastFlipped { get; set; }

		public WvwOwner Owner { get; set; }

		public Color TeamColor => GetColor();

		public Guid ClaimedBy { get; set; }

		public IReadOnlyList<int> GuildUpgrades { get; set; }

		public int YaksDelivered { get; set; }

		public Texture2D Icon { get; }

		public TimeSpan BuffDuration { get; }

		public Texture2D UpgradeTexture => GetUpgradeTierTexture();

		public Texture2D ClaimedTexture => TextureClaimed;

		public Texture2D BuffTexture => TextureBuff;

		public WvwObjectiveEntity(WvwObjective objective, Map map, ContinentFloorRegionMapSector sector)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			_internalObjective = objective;
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
		}

		private Texture2D GetTexture(WvwObjectiveType type)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Invalid comparison between Unknown and I4
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (type - 1 <= 2 || (int)type == 5)
			{
				return MistwarModule.ModuleInstance.ContentsManager.GetTexture($"{type}.png");
			}
			return null;
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
	}
}
