using Gw2Sharp;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class SkillSelectable : Selectable<Skill>
	{
		private readonly DetailedTexture _noAquaticFlagTexture = new DetailedTexture(157145)
		{
			TextureRegion = new Rectangle(16, 16, 96, 96)
		};

		public Enviroment Enviroment { get; set; }

		public SkillSelectable()
		{
			base.Tooltip = new SkillTooltip();
		}

		protected override void ApplyData(object sender, ValueChangedEventArgs<Skill> e)
		{
			base.ApplyData(sender, e);
			SkillTooltip skillTooltip = base.Tooltip as SkillTooltip;
			if (skillTooltip != null)
			{
				skillTooltip.Skill = e.NewValue;
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_noAquaticFlagTexture.Bounds = new Rectangle(0, 0, base.Width, base.Height);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.Paint(spriteBatch, bounds);
			Skill skill = base.Data;
			if (skill != null && skill.Flags.HasFlag(SkillFlag.NoUnderwater) && Enviroment == Enviroment.Aquatic)
			{
				_noAquaticFlagTexture.Draw(this, spriteBatch);
			}
		}
	}
}
