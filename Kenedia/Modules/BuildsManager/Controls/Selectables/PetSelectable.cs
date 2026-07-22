using Blish_HUD;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class PetSelectable : Selectable<Pet>
	{
		private readonly DetailedTexture _highlight = new DetailedTexture(156844)
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		private Rectangle _textureBounds;

		public PetSelectable()
		{
			base.Tooltip = new PetTooltip();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int pad = 16;
			_textureBounds = new Rectangle(-pad, -pad, base.Width + pad * 2, base.Height + pad * 2);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.DrawOnCtrl(this, base.IsSelected ? base.Data!.SelectedIcon : base.Data!.Icon, _textureBounds, base.TextureRegion, Color.White);
			if (base.MouseOver)
			{
				spriteBatch.DrawOnCtrl(this, _highlight.Texture, _textureBounds, base.TextureRegion, Color.White);
			}
		}

		protected override void ApplyData(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Pet?> e)
		{
			base.ApplyData(sender, e);
			PetTooltip petTooltip = base.Tooltip as PetTooltip;
			if (petTooltip != null)
			{
				petTooltip.Pet = e.NewValue;
			}
		}
	}
}
