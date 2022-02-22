using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts
{
	internal class Helper
	{
		private readonly ContentsManager contentsManager;

		private MapType[] warclawOnlyMaps;

		public Helper(ContentsManager contentsManager)
		{
			MapType[] array = new MapType[8];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			warclawOnlyMaps = (MapType[])(object)array;
			base._002Ector();
			this.contentsManager = contentsManager;
		}

		public Texture2D GetImgFile(string filename)
		{
			return (Texture2D)(Module._settingDisplay.get_Value() switch
			{
				"Transparent" => contentsManager.GetTexture(filename + "-trans.png"), 
				"SolidText" => contentsManager.GetTexture(filename + "-text.png"), 
				_ => contentsManager.GetTexture(filename + ".png"), 
			});
		}

		private bool IsPlayerInWvWMap()
		{
			return Array.Exists(warclawOnlyMaps, (MapType mapType) => mapType == GameService.Gw2Mumble.get_CurrentMap().get_Type());
		}

		private bool IsPlayerUnderOrCloseToWater()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z <= 0f;
		}

		private static Mount GetWaterMount()
		{
			return Module._mounts.SingleOrDefault((Mount m) => m.IsWaterMount && m.Name == Module._settingDefaultWaterMountChoice.get_Value());
		}

		internal Mount GetDefaultMount()
		{
			if (IsPlayerInWvWMap())
			{
				return Module._mounts.Single((Mount m) => m.IsWvWMount);
			}
			if (IsPlayerUnderOrCloseToWater())
			{
				return GetWaterMount();
			}
			return Module._mounts.SingleOrDefault((Mount m) => m.Name == Module._settingDefaultMountChoice.get_Value());
		}

		internal Mount GetLastUsedMount()
		{
			if (IsPlayerUnderOrCloseToWater())
			{
				return GetWaterMount();
			}
			return (from m in Module._mounts
				where m.LastUsedTimestamp.HasValue
				orderby m.LastUsedTimestamp descending
				select m).FirstOrDefault();
		}
	}
}
