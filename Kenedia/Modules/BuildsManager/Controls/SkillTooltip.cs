using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillTooltip : Tooltip
	{
		private readonly SkillTooltipContentControl _skillContentControl;

		public Skill Skill
		{
			get
			{
				return _skillContentControl.Skill;
			}
			set
			{
				_skillContentControl.Skill = value;
			}
		}

		public SkillTooltip()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.AutoSizePadding = new Point(5);
			_skillContentControl = new SkillTooltipContentControl
			{
				Parent = this
			};
		}

		public SkillTooltip(Skill skill)
			: this()
		{
			_skillContentControl.Skill = skill;
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(_skillContentControl.Title))
			{
				base.Draw(spriteBatch, drawBounds, scissor);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_skillContentControl?.Dispose();
		}
	}
}
