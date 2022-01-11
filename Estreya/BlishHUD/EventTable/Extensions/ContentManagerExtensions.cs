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


		public static AsyncTexture2D GetRenderIcon(this ContentsManager manager, string identifier)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			lock (IconCache)
			{
				if (IconCache.ContainsKey(identifier))
				{
					return IconCache[identifier];
				}
				AsyncTexture2D icon = new AsyncTexture2D(Texture2DExtension.Duplicate(Textures.get_TransparentPixel()));
				if (!string.IsNullOrWhiteSpace(identifier))
				{
					if (identifier.Contains("/"))
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
						icon.SwapTexture(texture);
					}
				}
				IconCache.Add(identifier, icon);
				return icon;
			}
		}
	}
}
