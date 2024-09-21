using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Models;

namespace MysticCrafting.Module.Services
{
	public class TextureRepository : ITextureRepository, IDisposable
	{
		private readonly ConcurrentDictionary<string, AsyncTexture2D> _textures;

		private readonly ConcurrentDictionary<string, AsyncTexture2D> _refTextures;

		private readonly ContentsManager _contentsManager;

		private const string BaseUrl = "https://render.guildwars2.com/file";

		public TextureResources Textures { get; } = new TextureResources();


		public TextureRepository(ContentsManager contentsManager)
		{
			_textures = new ConcurrentDictionary<string, AsyncTexture2D>();
			_refTextures = new ConcurrentDictionary<string, AsyncTexture2D>();
			_contentsManager = contentsManager;
		}

		public void ClearTextures()
		{
			_textures.Clear();
		}

		public AsyncTexture2D GetTexture(string url)
		{
			if (url == null || !url.StartsWith("https://render.guildwars2.com/file"))
			{
				url = "https://render.guildwars2.com/file" + url;
			}
			AsyncTexture2D texture = _textures.FirstOrDefault((KeyValuePair<string, AsyncTexture2D> t) => t.Key.Equals(url)).Value;
			if (texture != null)
			{
				return texture;
			}
			texture = GameService.Content.GetRenderServiceTexture(url);
			if (texture != null)
			{
				_textures.AddOrUpdate(url, texture, (string key, AsyncTexture2D value) => value = texture);
			}
			return texture;
		}

		public AsyncTexture2D GetRefTexture(string fileName)
		{
			if (_contentsManager == null)
			{
				return null;
			}
			AsyncTexture2D texture = _refTextures.FirstOrDefault((KeyValuePair<string, AsyncTexture2D> t) => t.Key.Equals(fileName)).Value;
			if (texture != null)
			{
				return texture;
			}
			texture = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture(fileName));
			if (texture != null)
			{
				_refTextures.AddOrUpdate(fileName, texture, (string key, AsyncTexture2D value) => value = texture);
			}
			return texture;
		}

		public AsyncTexture2D GetVendorIconTexture(string icon)
		{
			if (string.IsNullOrEmpty(icon))
			{
				return GetRefTexture("VendorIcons/Merchant.png");
			}
			return GetRefTexture("VendorIcons/" + icon.Replace("File:", "")) ?? GetRefTexture("VendorIcons/Merchant.png");
		}

		public AsyncTexture2D GetItemSourceBackgroundTexture(IList<IItemSource> itemSource)
		{
			foreach (RecipeSource source in itemSource.OfType<RecipeSource>())
			{
				if (source.Recipe.Disciplines.Count == 1)
				{
					AsyncTexture2D texture = GetRefTexture("Disciplines/" + source.Recipe.Disciplines.FirstOrDefault().ToString().ToLower() + ".png");
					if (texture != null)
					{
						return texture;
					}
				}
			}
			return GetRefTexture("Disciplines/mystic_forge.png");
		}

		public AsyncTexture2D GetRecipeSourceIcon(IItemSource itemSource)
		{
			AsyncTexture2D icon = GetRefTexture("mystic_forge.png");
			RecipeSource recipeSource = itemSource as RecipeSource;
			if (recipeSource != null && recipeSource.Recipe.Disciplines != null)
			{
				if (recipeSource.Recipe.Disciplines.Count == 1)
				{
					icon = IconHelper.GetIcon(recipeSource.Recipe.Disciplines.FirstOrDefault());
				}
				else if (recipeSource.Recipe.Disciplines.Count > 1)
				{
					icon = Textures.CraftingIcon;
				}
			}
			return icon;
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, AsyncTexture2D> texture in _textures)
			{
				texture.Value.Dispose();
			}
			foreach (KeyValuePair<string, AsyncTexture2D> refTexture in _refTextures)
			{
				refTexture.Value.Dispose();
			}
			_textures.Clear();
			_refTextures.Clear();
		}
	}
}