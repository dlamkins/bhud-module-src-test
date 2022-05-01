using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;

namespace Universal_Search_Module.Controls.SearchResultItems
{
	public class SkillSearchResultItem : SearchResultItem
	{
		private Skill _skill;

		public Skill Skill
		{
			get
			{
				return _skill;
			}
			set
			{
				if (((Control)this).SetProperty<Skill>(ref _skill, value, false, "Skill") && _skill != null)
				{
					_ = _skill.Icon;
					base.Icon = Control.get_Content().GetRenderServiceTexture((string)_skill.Icon);
					base.Name = _skill.Name;
					base.Description = _skill.Description;
				}
			}
		}

		protected override string ChatLink => Skill?.ChatLink;

		protected override Tooltip BuildTooltip()
		{
			return (Tooltip)(object)new SkillTooltip(Skill);
		}
	}
}
