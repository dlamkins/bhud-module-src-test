using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Quarry.Interfaces;
using Quarry.UserInterface.Windows;
using Quarry.WikiData.Achievement;

namespace Quarry.Services
{
	public class InspectorWindowManager : IInspectorWindowManager, IDisposable
	{
		private readonly GraphicsService graphicsService;

		private readonly ContentsManager contentsManager;

		private readonly IAchievementService achievementService;

		private readonly IWikiSubpageDataService wikiSubpageDataService;

		private readonly IBitAlignmentService bitAlignmentService;

		private readonly IHuntService huntService;

		private readonly INearestObjectiveService nearestObjectiveService;

		private readonly ICurrentMapService currentMapService;

		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		private readonly IExternalImageService externalImageService;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly List<InspectorWindow> pinnedWindows = new List<InspectorWindow>();

		private readonly HashSet<InspectorWindow> forciblyHidden = new HashSet<InspectorWindow>();

		private InspectorWindow defaultWindow;

		public InspectorWindowManager(GraphicsService graphicsService, ContentsManager contentsManager, IAchievementService achievementService, IWikiSubpageDataService wikiSubpageDataService, IBitAlignmentService bitAlignmentService, IHuntService huntService, INearestObjectiveService nearestObjectiveService, ICurrentMapService currentMapService, IFormattedLabelHtmlService formattedLabelHtmlService, IExternalImageService externalImageService, IAchievementTrackerService achievementTrackerService)
		{
			this.graphicsService = graphicsService;
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.wikiSubpageDataService = wikiSubpageDataService;
			this.bitAlignmentService = bitAlignmentService;
			this.huntService = huntService;
			this.nearestObjectiveService = nearestObjectiveService;
			this.currentMapService = currentMapService;
			this.formattedLabelHtmlService = formattedLabelHtmlService;
			this.externalImageService = externalImageService;
			this.achievementTrackerService = achievementTrackerService;
		}

		public void ShowAchievement(AchievementTableEntry achievement)
		{
			InspectorWindow orCreateDefaultWindow = GetOrCreateDefaultWindow();
			orCreateDefaultWindow.SetAchievement(achievement);
			((Control)orCreateDefaultWindow).Show();
		}

		public void NotifyPinned(InspectorWindow window)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			pinnedWindows.Add(window);
			if (defaultWindow == window)
			{
				defaultWindow = null;
			}
			((Control)window).set_Location(new Point(((Control)window).get_Location().X + 30, ((Control)window).get_Location().Y + 30));
			((Control)window).add_Hidden((EventHandler<EventArgs>)delegate
			{
				if (!forciblyHidden.Contains(window))
				{
					pinnedWindows.Remove(window);
					((Control)window).Dispose();
				}
			});
		}

		private InspectorWindow GetOrCreateDefaultWindow()
		{
			if (defaultWindow != null)
			{
				return defaultWindow;
			}
			InspectorWindow inspectorWindow = new InspectorWindow(contentsManager, achievementService, wikiSubpageDataService, bitAlignmentService, huntService, nearestObjectiveService, currentMapService, formattedLabelHtmlService, externalImageService, achievementTrackerService, this, isDefault: true);
			((Control)inspectorWindow).set_Parent((Container)(object)graphicsService.get_SpriteScreen());
			InspectorWindow window = inspectorWindow;
			((Control)window).add_Hidden((EventHandler<EventArgs>)delegate
			{
				if (!forciblyHidden.Contains(window))
				{
					if (defaultWindow == window)
					{
						defaultWindow = null;
					}
					((Control)window).Dispose();
				}
			});
			defaultWindow = window;
			return window;
		}

		public void Update()
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			IEnumerable<InspectorWindow> liveWindows = ((defaultWindow != null) ? pinnedWindows.Append(defaultWindow) : pinnedWindows.AsEnumerable());
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				foreach (InspectorWindow window in liveWindows)
				{
					if (((Control)window).get_Visible() && forciblyHidden.Add(window))
					{
						((Control)window).Hide();
					}
				}
			}
			else
			{
				if (forciblyHidden.Count <= 0)
				{
					return;
				}
				foreach (InspectorWindow item in forciblyHidden)
				{
					((Control)item).Show();
				}
				forciblyHidden.Clear();
			}
		}

		public void Dispose()
		{
			InspectorWindow inspectorWindow = defaultWindow;
			if (inspectorWindow != null)
			{
				((Control)inspectorWindow).Dispose();
			}
			defaultWindow = null;
			foreach (InspectorWindow pinnedWindow in pinnedWindows)
			{
				((Control)pinnedWindow).Dispose();
			}
			pinnedWindows.Clear();
		}
	}
}
