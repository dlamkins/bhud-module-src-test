using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Views
{
	public class AchievementItemOverview : View
	{
		private readonly IAchievementService achievementService;

		private readonly IEnumerable<(AchievementCategory Category, AchievementTableEntry Achievement)> achievements;

		private readonly IAchievementListItemFactory achievementListItemFactory;

		private readonly string title;

		public AchievementItemOverview(IEnumerable<(AchievementCategory, AchievementTableEntry)> achievements, string title, IAchievementService achievementService, IAchievementListItemFactory achievementListItemFactory)
			: this()
		{
			this.achievements = achievements;
			this.title = title;
			this.achievementService = achievementService;
			this.achievementListItemFactory = achievementListItemFactory;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title(title);
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)0);
			FlowPanel panel = val;
			foreach (var achievement in from x in achievements
				select (achievementService.HasFinishedAchievement(x.Achievement.Id), x) into x
				orderby x.Item1, x.x.Category.get_Name(), x.x.Achievement.Name
				select x.x)
			{
				ViewContainer val2 = new ViewContainer();
				((Control)val2).set_Size(new Point(((Control)panel).get_Width() / 2, 100));
				((Panel)val2).set_ShowBorder(true);
				((Control)val2).set_Parent((Container)(object)panel);
				val2.Show((IView)(object)achievementListItemFactory.Create(achievement.Item2, RenderUrl.op_Implicit(achievement.Item1.get_Icon())));
			}
		}
	}
}
