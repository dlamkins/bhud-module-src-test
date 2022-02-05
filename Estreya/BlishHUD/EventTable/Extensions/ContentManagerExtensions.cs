using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.Extensions
{
	public static class ContentManagerExtensions
	{
		private static Dictionary<string, AsyncTexture2D> IconCache { get; set; } = new Dictionary<string, AsyncTexture2D>();


		public static AsyncTexture2D GetIcon(this ContentsManager manager, string identifier, bool checkRenderAPI = true)
		{
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			if (string.IsNullOrWhiteSpace(identifier))
			{
				return null;
			}
			lock (IconCache)
			{
				if (IconCache.ContainsKey(identifier))
				{
					return IconCache[identifier];
				}
				AsyncTexture2D icon = null;
				if (!string.IsNullOrWhiteSpace(identifier))
				{
					if (checkRenderAPI && identifier.Contains("/"))
					{
						icon = GameService.Content.GetRenderServiceTexture(identifier);
					}
					else
					{
						Texture2D texture = EventTableModule.ModuleInstance.ContentsManager.GetTexture(identifier);
						if (texture == Textures.get_Error())
						{
							texture = GameService.Content.GetTexture(identifier);
						}
						icon = new AsyncTexture2D(texture);
					}
				}
				IconCache.Add(identifier, icon);
				return icon;
			}
		}
	}
}
