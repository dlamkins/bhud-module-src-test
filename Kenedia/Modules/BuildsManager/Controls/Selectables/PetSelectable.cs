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
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			base.Tooltip = new PetTooltip();
		}

		public override void RecalculateLayout()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int pad = 16;
			_textureBounds = new Rectangle(-pad, -pad, base.Width + pad * 2, base.Height + pad * 2);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(this, base.IsSelected ? base.Data!.SelectedIcon : base.Data!.Icon, _textureBounds, base.TextureRegion, Color.get_White());
			if (base.MouseOver)
			{
				spriteBatch.DrawOnCtrl(this, _highlight.Texture, _textureBounds, base.TextureRegion, Color.get_White());
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
