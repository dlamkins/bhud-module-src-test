using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.Models;
using Denrage.AchievementTrackerModule.Models.Persistance;
using Denrage.AchievementTrackerModule.UserInterface.Windows;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.Services
{
	public class ItemDetailWindowManager : IItemDetailWindowManager, IDisposable
	{
		private readonly IItemDetailWindowFactory itemDetailWindowFactory;

		private readonly IAchievementService achievementService;

		private readonly Logger logger;

		private readonly List<ItemDetailWindowInformation> hiddenWindows = new List<ItemDetailWindowInformation>();

		internal Dictionary<string, ItemDetailWindowInformation> Windows { get; } = new Dictionary<string, ItemDetailWindowInformation>();


		public ItemDetailWindowManager(IItemDetailWindowFactory itemDetailWindowFactory, IAchievementService achievementService, Logger logger)
		{
			this.itemDetailWindowFactory = itemDetailWindowFactory;
			this.achievementService = achievementService;
			this.logger = logger;
		}

		public bool ShowWindow(string name)
		{
			if (Windows.TryGetValue(name, out var window))
			{
				if (GameService.Gw2Mumble.get_IsAvailable() && (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen()) && !hiddenWindows.Contains(window))
				{
					hiddenWindows.Add(window);
				}
				else
				{
					((Control)window.Window).Show();
					((WindowBase2)window.Window).BringWindowToFront();
				}
				return true;
			}
			return false;
		}

		public void Load(IPersistanceService persistanceService)
		{
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				foreach (KeyValuePair<int, Dictionary<int, ItemInformation>> achievement in persistanceService.Get().ItemInformation)
				{
					CollectionAchievementTable achievementDetail = achievementService.AchievementDetails.FirstOrDefault((CollectionAchievementTable x) => x.Id == achievement.Key);
					foreach (KeyValuePair<int, ItemInformation> item in achievement.Value)
					{
						CreateAndShowWindow(item.Value.Name, achievementDetail.ColumnNames, achievementDetail.Entries[item.Value.Index], achievementDetail.Link, item.Value.AchievementId, item.Value.Index);
						((Control)Windows[item.Value.Name].Window).set_Location(new Point(item.Value.PositionX, item.Value.PositionY));
					}
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception occured on restoring window positions");
			}
		}

		public void CreateAndShowWindow(string name, string[] columns, List<CollectionAchievementTable.CollectionAchievementTableEntry> item, string achievementLink, int achievementId, int itemIndex)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if (!ShowWindow(name))
			{
				ItemDetailWindow window = itemDetailWindowFactory.Create(name, columns, item, achievementLink);
				((Control)window).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)window).set_Location(((Control)GameService.Graphics.get_SpriteScreen()).get_Size() / new Point(2) - new Point(256, 178) / new Point(2));
				Windows[name] = new ItemDetailWindowInformation
				{
					Window = window,
					AchievementId = achievementId,
					ItemIndex = itemIndex,
					Name = name
				};
				ShowWindow(name);
			}
		}

		public void Update()
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				foreach (KeyValuePair<string, ItemDetailWindowInformation> item in Windows.Where((KeyValuePair<string, ItemDetailWindowInformation> x) => ((Control)x.Value.Window).get_Visible()))
				{
					if (!hiddenWindows.Contains(item.Value))
					{
						((Control)item.Value.Window).Hide();
						hiddenWindows.Add(item.Value);
					}
				}
			}
			else
			{
				if (!hiddenWindows.Any())
				{
					return;
				}
				foreach (ItemDetailWindowInformation hiddenWindow in hiddenWindows)
				{
					((Control)hiddenWindow.Window).Show();
				}
				hiddenWindows.Clear();
			}
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, ItemDetailWindowInformation> window in Windows)
			{
				((Control)window.Value.Window).Dispose();
			}
			Windows.Clear();
		}
	}
}
