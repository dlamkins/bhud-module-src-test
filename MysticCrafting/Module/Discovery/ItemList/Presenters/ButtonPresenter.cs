using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery.ItemList.Presenters
{
	public class ButtonPresenter
	{
		public TextureButton BuildFavoriteButton(int itemId, Container parent)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			bool isFavorite = ServiceContainer.FavoritesRepository.IsFavorite(itemId);
			TextureButton textureButton = new TextureButton();
			((Control)textureButton).set_Parent(parent);
			textureButton.Active = isFavorite;
			((Control)textureButton).set_Size(new Point(40, 40));
			textureButton.Texture = ServiceContainer.TextureRepository.Textures.HeartDisabled;
			((Control)textureButton).add_Click((EventHandler<MouseEventArgs>)delegate
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
			});
			return textureButton;
		}
	}
}
