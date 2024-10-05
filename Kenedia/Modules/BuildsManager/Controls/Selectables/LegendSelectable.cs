using Gw2Sharp;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class LegendSelectable : Selectable<Legend>
	{
		private readonly DetailedTexture _noAquaticFlagTexture = new DetailedTexture(157145)
		{
			TextureRegion = new Rectangle(16, 16, 96, 96)
		};

		public SkillTooltip SkillTooltip { get; }

		public LegendSlotType LegendSlot { get; set; } = LegendSlotType.TerrestrialActive;


		public LegendSelectable()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			base.Tooltip = (SkillTooltip = new SkillTooltip());
		}

		protected override void ApplyData(object sender, ValueChangedEventArgs<Legend?> e)
		{
			base.ApplyData(sender, e);
			SkillTooltip.Skill = e.NewValue!.Swap;
		}

		public override void RecalculateLayout()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_noAquaticFlagTexture.Bounds = new Rectangle(0, 0, base.Width, base.Height);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			base.Paint(spriteBatch, bounds);
			LegendSlotType legendSlot = LegendSlot;
			if (((uint)legendSlot <= 1u) ? true : false)
			{
				Legend? data = base.Data;
				if (data != null && data!.Swap.Flags.HasFlag(SkillFlag.NoUnderwater))
				{
					_noAquaticFlagTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.get_White());
				}
			}
		}
	}
}
