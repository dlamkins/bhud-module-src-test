using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Items.Controls;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Items.Presenters
{
	public class ButtonPresenter
	{
		public event EventHandler<CheckChangedEvent> SelectChanged;

		public TextureButton BuildFavoriteButton(int itemId, Container parent)
		{
			bool isFavorite = ServiceContainer.FavoritesRepository.IsFavorite(itemId);
			TextureButton textureButton = new TextureButton();
			textureButton.Parent = parent;
			textureButton.Active = isFavorite;
			textureButton.Size = new Point(40, 40);
			textureButton.Texture = ServiceContainer.TextureRepository.Textures.HeartDisabled;
			textureButton.Click += delegate
			{
				IFavoritesRepository favoritesRepository = ServiceContainer.FavoritesRepository;
				isFavorite = favoritesRepository.IsFavorite(itemId);
				if (!isFavorite)
				{
					favoritesRepository.SaveFavorite(itemId);
				}
				else
				{
					favoritesRepository.RemoveFavorite(itemId);
				}
			};
			return textureButton;
		}

		public TextureButton BuildWikiButton(int itemId, Container parent)
		{
			MysticWikiLink wikiLink = ServiceContainer.WikiLinkRepository.GetLink(itemId);
			if (wikiLink != null)
			{
				TextureButton textureButton = new TextureButton();
				textureButton.Texture = ServiceContainer.TextureRepository.GetRefTexture("wiki-new.png");
				textureButton.Size = new Point(30, 30);
				textureButton.Padding = new Thickness(2f, 0f);
				textureButton.Parent = parent;
				textureButton.HasActiveState = false;
				textureButton.Click += delegate
				{
					LinkHelper.OpenWiki(wikiLink);
				};
				return textureButton;
			}
			return null;
		}

		public TextureButton BuildGw2BLTCButton(int itemId, Container parent)
		{
			TextureButton textureButton = new TextureButton();
			textureButton.Texture = ServiceContainer.TextureRepository.Textures.BltcIcon;
			textureButton.Size = new Point(25, 25);
			textureButton.Padding = new Thickness(6f, 0f);
			textureButton.Parent = parent;
			textureButton.HasActiveState = false;
			textureButton.Click += delegate
			{
				LinkHelper.OpenGw2Bltc(itemId);
			};
			return textureButton;
		}

		public TextureButton BuildCopyChatLinkButton(string chatLink, Container parent)
		{
			TextureButton textureButton = new TextureButton();
			textureButton.Texture = ServiceContainer.TextureRepository.GetRefTexture("link.png");
			textureButton.Size = new Point(25, 25);
			textureButton.Padding = new Thickness(6f, 0f);
			textureButton.Parent = parent;
			textureButton.HasActiveState = false;
			textureButton.Click += delegate
			{
				_ = ClipboardUtil.WindowsClipboardService.SetTextAsync(chatLink).Result;
			};
			return textureButton;
		}
	}
}
