using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Core.Services
{
	public class ResourceService : IDisposable
	{
		private Texture2D _textureFortified = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324351.png");

		private Texture2D _textureReinforced = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324350.png");

		private Texture2D _textureSecured = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1324349.png");

		private Texture2D _textureClaimed = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304078.png");

		private Texture2D _textureClaimedRepGuild = MistwarModule.ModuleInstance.ContentsManager.GetTexture("1304077.png");

		private Texture2D _textureBuff = MistwarModule.ModuleInstance.ContentsManager.GetTexture("righteous_indignation.png");

		private Texture2D _textureRuinEstate = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_estate.png");

		private Texture2D _textureRuinTemple = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_temple.png");

		private Texture2D _textureRuinOverlook = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_overlook.png");

		private Texture2D _textureRuinHollow = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_hollow.png");

		private Texture2D _textureRuinAscent = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_ascent.png");

		private Texture2D _textureRuinOther = MistwarModule.ModuleInstance.ContentsManager.GetTexture("ruin_other.png");

		private Texture2D _textureWayPoint = MistwarModule.ModuleInstance.ContentsManager.GetTexture("157353.png");

		private Texture2D _textureWayPointHover = MistwarModule.ModuleInstance.ContentsManager.GetTexture("60970.png");

		private Texture2D _textureWayPointContested = MistwarModule.ModuleInstance.ContentsManager.GetTexture("102349.png");

		private IReadOnlyDictionary<string, Texture2D> _ruinsTexLookUp;

		public readonly Color ColorRed = new Color(213, 71, 67);

		public readonly Color ColorGreen = new Color(73, 190, 111);

		public readonly Color ColorBlue = new Color(100, 164, 228);

		public readonly Color ColorNeutral = Color.get_DimGray();

		public readonly Color BrightGold = new Color(223, 194, 149, 255);

		private AsyncCache<(Map, int), ContinentFloorRegionMap> _mapExpandedCache = new AsyncCache<(Map, int), ContinentFloorRegionMap>(((Map, int) p) => RequestMapExpanded(p.Item1, p.Item2));

		private AsyncCache<int, Map> _mapCache = new AsyncCache<int, Map>(RequestMap);

		public ResourceService()
		{
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
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

		public Texture2D GetClaimedTexture(Guid claimedBy)
		{
			if (!claimedBy.Equals(MistwarModule.ModuleInstance.WvW.CurrentGuild))
			{
				return _textureClaimed;
			}
			return _textureClaimedRepGuild;
		}

		public Texture2D GetBuffTexture()
		{
			return _textureBuff;
		}

		public Texture2D GetWayPointTexture(bool isHovered, bool isContested)
		{
			if (isContested)
			{
				return _textureWayPointContested;
			}
			if (isHovered)
			{
				return _textureWayPointHover;
			}
			return _textureWayPoint;
		}

		public Texture2D GetObjectiveTexture(WvwObjectiveType type, string id)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected I4, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			Texture2D tex;
			return (Texture2D)((type - 1) switch
			{
				0 => MistwarModule.ModuleInstance.ContentsManager.GetTexture($"{type}.png"), 
				1 => MistwarModule.ModuleInstance.ContentsManager.GetTexture($"{type}.png"), 
				2 => MistwarModule.ModuleInstance.ContentsManager.GetTexture($"{type}.png"), 
				4 => MistwarModule.ModuleInstance.ContentsManager.GetTexture($"{type}.png"), 
				5 => _ruinsTexLookUp.TryGetValue(id, out tex) ? tex : Textures.get_TransparentPixel(), 
				_ => Textures.get_TransparentPixel(), 
			});
		}

		public Color GetTeamColor(WvwOwner owner)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected I4, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			return (Color)((owner - 2) switch
			{
				0 => ColorRed, 
				1 => ColorBlue, 
				2 => ColorGreen, 
				_ => ColorNeutral, 
			});
		}

		public Texture2D GetUpgradeTierTexture(WvwObjectiveTier tier)
		{
			return (Texture2D)(tier switch
			{
				WvwObjectiveTier.Fortified => _textureFortified, 
				WvwObjectiveTier.Reinforced => _textureReinforced, 
				WvwObjectiveTier.Secured => _textureSecured, 
				_ => Textures.get_TransparentPixel(), 
			});
		}

		public async Task<Map> GetMap(int mapId)
		{
			return await _mapCache.GetItem(mapId);
		}

		public async Task<ContinentFloorRegionMap> GetMapExpanded(Map map, int floorId)
		{
			return await _mapExpandedCache.GetItem((map, floorId));
		}

		private static async Task<Map> RequestMap(int mapId)
		{
			return await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Map, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Maps()).GetAsync(mapId, default(CancellationToken)));
		}

		private static async Task<ContinentFloorRegionMap> RequestMapExpanded(Map map, int floorId)
		{
			return await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<ContinentFloorRegionMap, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Continents()
				.get_Item(map.get_ContinentId())
				.get_Floors()
				.get_Item(floorId)
				.get_Regions()
				.get_Item(map.get_RegionId())
				.get_Maps()).GetAsync(map.get_Id(), default(CancellationToken)));
		}

		public void Dispose()
		{
			Texture2D textureFortified = _textureFortified;
			if (textureFortified != null)
			{
				((GraphicsResource)textureFortified).Dispose();
			}
			Texture2D textureReinforced = _textureReinforced;
			if (textureReinforced != null)
			{
				((GraphicsResource)textureReinforced).Dispose();
			}
			Texture2D textureSecured = _textureSecured;
			if (textureSecured != null)
			{
				((GraphicsResource)textureSecured).Dispose();
			}
			Texture2D textureClaimed = _textureClaimed;
			if (textureClaimed != null)
			{
				((GraphicsResource)textureClaimed).Dispose();
			}
			Texture2D textureClaimedRepGuild = _textureClaimedRepGuild;
			if (textureClaimedRepGuild != null)
			{
				((GraphicsResource)textureClaimedRepGuild).Dispose();
			}
			Texture2D textureBuff = _textureBuff;
			if (textureBuff != null)
			{
				((GraphicsResource)textureBuff).Dispose();
			}
			Texture2D textureRuinEstate = _textureRuinEstate;
			if (textureRuinEstate != null)
			{
				((GraphicsResource)textureRuinEstate).Dispose();
			}
			Texture2D textureRuinTemple = _textureRuinTemple;
			if (textureRuinTemple != null)
			{
				((GraphicsResource)textureRuinTemple).Dispose();
			}
			Texture2D textureRuinOverlook = _textureRuinOverlook;
			if (textureRuinOverlook != null)
			{
				((GraphicsResource)textureRuinOverlook).Dispose();
			}
			Texture2D textureRuinHollow = _textureRuinHollow;
			if (textureRuinHollow != null)
			{
				((GraphicsResource)textureRuinHollow).Dispose();
			}
			Texture2D textureRuinAscent = _textureRuinAscent;
			if (textureRuinAscent != null)
			{
				((GraphicsResource)textureRuinAscent).Dispose();
			}
			Texture2D textureRuinOther = _textureRuinOther;
			if (textureRuinOther != null)
			{
				((GraphicsResource)textureRuinOther).Dispose();
			}
			Texture2D textureWayPoint = _textureWayPoint;
			if (textureWayPoint != null)
			{
				((GraphicsResource)textureWayPoint).Dispose();
			}
			Texture2D textureWayPointHover = _textureWayPointHover;
			if (textureWayPointHover != null)
			{
				((GraphicsResource)textureWayPointHover).Dispose();
			}
			Texture2D textureWayPointContested = _textureWayPointContested;
			if (textureWayPointContested != null)
			{
				((GraphicsResource)textureWayPointContested).Dispose();
			}
		}
	}
}
