using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Quarry.Interfaces;
using Quarry.UserInterface.Controls;
using Quarry.WikiData.Achievement;

namespace Quarry.UserInterface.Views
{
	public class AchievementItemOverview : View
	{
		private const string SortByName = "Name";

		private const string SortByNearestToDone = "Nearest to done";

		private static string currentSortMode = "Name";

		private readonly IAchievementService achievementService;

		private readonly IEnumerable<(AchievementCategory Category, AchievementTableEntry Achievement)> achievements;

		private readonly IAchievementCardFactory achievementCardFactory;

		private readonly IBitAlignmentService bitAlignmentService;

		private readonly string title;

		private CardGrid panel;

		private bool apiDataRequested;

		private bool panelDisposed;

		public AchievementItemOverview(IEnumerable<(AchievementCategory, AchievementTableEntry)> achievements, string title, IAchievementService achievementService, IAchievementCardFactory achievementCardFactory, IBitAlignmentService bitAlignmentService)
			: this()
		{
			this.achievements = achievements;
			this.title = title;
			this.achievementService = achievementService;
			this.achievementCardFactory = achievementCardFactory;
			this.bitAlignmentService = bitAlignmentService;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			Dropdown val = new Dropdown();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(160);
			((Control)val).set_Height(30);
			Dropdown sortDropdown = val;
			sortDropdown.get_Items().Add("Name");
			sortDropdown.get_Items().Add("Nearest to done");
			sortDropdown.set_SelectedItem(currentSortMode);
			CardGrid cardGrid = new CardGrid();
			((Panel)cardGrid).set_Title(title);
			((Panel)cardGrid).set_ShowBorder(true);
			((Control)cardGrid).set_Parent(buildPanel);
			((Control)cardGrid).set_Location(new Point(0, ((Control)sortDropdown).get_Height()));
			((Container)cardGrid).set_WidthSizingMode((SizingMode)2);
			((Container)cardGrid).set_HeightSizingMode((SizingMode)2);
			((Panel)cardGrid).set_CanScroll(true);
			panel = cardGrid;
			((Control)panel).add_Disposed((EventHandler<EventArgs>)delegate
			{
				panelDisposed = true;
			});
			Populate();
			EnsureApiDataForSort();
			sortDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
			{
				currentSortMode = e.get_CurrentValue();
				Populate();
				EnsureApiDataForSort();
			});
		}

		private void Populate()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			panel.ClearItems();
			foreach (var achievement in GetOrderedAchievements())
			{
				AchievementCard card = achievementCardFactory.Create(achievement.Achievement, RenderUrl.op_Implicit(achievement.Category.get_Icon()));
				((Control)card).set_Size(panel.CardSize);
				panel.Add((Control)(object)card);
			}
		}

		private IEnumerable<(AchievementCategory Category, AchievementTableEntry Achievement)> GetOrderedAchievements()
		{
			IEnumerable<(bool, AchievementCategory, AchievementTableEntry)> withCompletion = achievements.Select(((AchievementCategory Category, AchievementTableEntry Achievement) x) => (achievementService.HasFinishedAchievement(x.Achievement.Id), x.Category, x.Achievement));
			if (currentSortMode == "Nearest to done")
			{
				return from x in withCompletion
					orderby x.Done, IgnoresNearlyComplete(x.Achievement.Id), GetProgressRatio(x.Achievement.Id) descending, GetAchievementPoints(x.Achievement.Id) descending, x.Achievement.Name
					select (x.Category, x.Achievement);
			}
			return from x in withCompletion
				orderby x.Done, x.Category.get_Name(), x.Achievement.Name
				select (x.Category, x.Achievement);
		}

		private bool IgnoresNearlyComplete(int achievementId)
		{
			if (bitAlignmentService.TryGetCachedAchievement(achievementId, out var apiAchievement))
			{
				return ((IEnumerable<ApiEnum<AchievementFlag>>)apiAchievement.get_Flags()).Select((ApiEnum<AchievementFlag> f) => f.get_Value()).Contains((AchievementFlag)4);
			}
			return false;
		}

		private int GetAchievementPoints(int achievementId)
		{
			if (!bitAlignmentService.TryGetCachedAchievement(achievementId, out var apiAchievement))
			{
				return 0;
			}
			return apiAchievement.get_Tiers()?.Sum((AchievementTier t) => t.get_Points()) ?? 0;
		}

		private void EnsureApiDataForSort()
		{
			if (apiDataRequested || currentSortMode != "Nearest to done")
			{
				return;
			}
			apiDataRequested = true;
			List<int> ids = achievements.Select(((AchievementCategory Category, AchievementTableEntry Achievement) x) => x.Achievement.Id).Distinct().ToList();
			Task.Run(async delegate
			{
				await bitAlignmentService.GetAchievementsAsync(ids);
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					if (panel != null && !panelDisposed)
					{
						Populate();
					}
				});
			});
		}

		private double GetProgressRatio(int achievementId)
		{
			achievementService.PlayerAchievementsById.TryGetValue(achievementId, out var playerAchievement);
			if (playerAchievement == null || playerAchievement.get_Max() <= 0)
			{
				return 0.0;
			}
			return (double)playerAchievement.get_Current() / (double)playerAchievement.get_Max();
		}
	}
}
