using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Windows;

namespace Denrage.AchievementTrackerModule.Services
{
	public class SubPageInformationWindowManager : ISubPageInformationWindowManager, IDisposable
	{
		private readonly Dictionary<SubPageInformation, WindowBase2> subPageWindows = new Dictionary<SubPageInformation, WindowBase2>();

		private readonly GraphicsService graphicsService;

		private readonly ContentsManager contentsManager;

		private readonly IAchievementService achievementService;

		private readonly Func<IFormattedLabelHtmlService> getFormattedLabelHtmlSerice;

		private readonly IExternalImageService externalImageService;

		private IFormattedLabelHtmlService formattedLabelHtmlService;

		public SubPageInformationWindowManager(GraphicsService graphicsService, ContentsManager contentsManager, IAchievementService achievementService, Func<IFormattedLabelHtmlService> getFormattedLabelHtmlSerice, IExternalImageService externalImageService)
		{
			this.graphicsService = graphicsService;
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.getFormattedLabelHtmlSerice = getFormattedLabelHtmlSerice;
			this.externalImageService = externalImageService;
		}

		public void Create(SubPageInformation subPageInformation)
		{
			if (subPageWindows.TryGetValue(subPageInformation, out var window))
			{
				window.BringWindowToFront();
				return;
			}
			if (formattedLabelHtmlService == null)
			{
				formattedLabelHtmlService = getFormattedLabelHtmlSerice();
			}
			SubPageInformationWindow subPageInformationWindow = new SubPageInformationWindow(contentsManager, achievementService, formattedLabelHtmlService, subPageInformation, externalImageService);
			((Control)subPageInformationWindow).set_Parent((Container)(object)graphicsService.get_SpriteScreen());
			window = (WindowBase2)(object)subPageInformationWindow;
			((Control)window).add_Hidden((EventHandler<EventArgs>)delegate
			{
				subPageWindows.Remove(subPageInformation);
				((Control)window).Dispose();
			});
			subPageWindows[subPageInformation] = window;
			((Control)window).Show();
		}

		public void CloseWindows()
		{
			foreach (KeyValuePair<SubPageInformation, WindowBase2> subPageWindow in subPageWindows)
			{
				((Control)subPageWindow.Value).Dispose();
			}
			subPageWindows.Clear();
		}

		public void Dispose()
		{
			CloseWindows();
		}
	}
}
