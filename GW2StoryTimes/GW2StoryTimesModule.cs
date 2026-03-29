using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using GW2StoryTimes.Models;
using GW2StoryTimes.Services;
using GW2StoryTimes.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GW2StoryTimes
{
	[Export(typeof(Module))]
	public class GW2StoryTimesModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<GW2StoryTimesModule>();

		private CornerIcon _cornerIcon;

		private StoryTimesWidget _widget;

		private StoryTimesWindow _selectorWindow;

		private FeedbackPrompt _feedbackPrompt;

		private NextMissionPrompt _nextMissionPrompt;

		internal static GW2StoryTimesModule Instance { get; private set; }

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal StoryTimesApiClient ApiClient { get; private set; }

		internal TimerService TimerService { get; private set; }

		internal Mission ActiveMission { get; set; }

		internal SettingEntry<bool> SettingShowFeedbackPrompt { get; private set; }

		internal SettingEntry<SubmissionCategory> SettingPreferredCategory { get; private set; }

		internal SettingEntry<KeyBinding> SettingToggleWidgetHotkey { get; private set; }

		internal SettingEntry<int> SettingWidgetX { get; private set; }

		internal SettingEntry<int> SettingWidgetY { get; private set; }

		[ImportingConstructor]
		public GW2StoryTimesModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingShowFeedbackPrompt = settings.DefineSetting("ShowFeedbackPrompt", defaultValue: true, () => "Show Feedback Prompt", () => "Show a prompt to submit your completion time after finishing a mission.");
			SettingPreferredCategory = settings.DefineSetting("PreferredCategory_v2", SubmissionCategory.Full, () => "Default Submission Type", () => "Which time estimate to display and which category to default when submitting.");
			SettingToggleWidgetHotkey = settings.DefineSetting("ToggleWidgetHotkey", new KeyBinding(Keys.None), () => "Toggle Widget Hotkey", () => "Keybind to show or hide the Story Times widget overlay.");
			SettingCollection internal_ = settings.AddSubCollection("internal");
			SettingWidgetX = internal_.DefineSetting("WidgetPositionX", -1);
			SettingWidgetY = internal_.DefineSetting("WidgetPositionY", -1);
		}

		protected override async Task LoadAsync()
		{
			ApiClient = new StoryTimesApiClient();
			TimerService = new TimerService();
			SettingToggleWidgetHotkey.Value.Enabled = true;
			SettingToggleWidgetHotkey.Value.Activated += OnToggleWidgetHotkeyActivated;
			await ApiClient.PreloadSeasonsAsync();
			CreateCornerIcon();
			CreateWidget();
			CreateSelectorWindow();
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			if (SettingToggleWidgetHotkey?.Value != null)
			{
				SettingToggleWidgetHotkey.Value.Activated -= OnToggleWidgetHotkeyActivated;
			}
			_cornerIcon?.Dispose();
			_widget?.Dispose();
			_selectorWindow?.Dispose();
			_feedbackPrompt?.Dispose();
			_nextMissionPrompt?.Dispose();
			ApiClient?.Dispose();
			TimerService?.Dispose();
			Instance = null;
		}

		private void CreateCornerIcon()
		{
			AsyncTexture2D cornerIconTexture = AsyncTexture2D.FromAssetId(440023);
			_cornerIcon = new CornerIcon
			{
				IconName = "Story Times",
				Icon = cornerIconTexture,
				BasicTooltipText = "Story Times — Toggle Widget",
				Priority = 748291035,
				Parent = GameService.Graphics.SpriteScreen
			};
			_cornerIcon.Click += delegate
			{
				ToggleWidget();
			};
		}

		private void CreateWidget()
		{
			_widget = new StoryTimesWidget
			{
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false
			};
		}

		private void CreateSelectorWindow()
		{
			AsyncTexture2D windowBackgroundTexture = AsyncTexture2D.FromAssetId(155997);
			_selectorWindow = new StoryTimesWindow(windowBackgroundTexture, ApiClient)
			{
				Parent = GameService.Graphics.SpriteScreen
			};
		}

		private void ToggleWidget()
		{
			if (_widget != null)
			{
				_widget.Visible = !_widget.Visible;
			}
		}

		private void OnToggleWidgetHotkeyActivated(object sender, EventArgs e)
		{
			ToggleWidget();
		}

		internal void OpenMissionSelector()
		{
			_selectorWindow?.ToggleWindow();
		}

		internal void ShowFeedbackPrompt(Mission mission, TimeSpan elapsed)
		{
			SettingEntry<bool> settingShowFeedbackPrompt = SettingShowFeedbackPrompt;
			if (settingShowFeedbackPrompt != null && !settingShowFeedbackPrompt.Value)
			{
				SubmissionCategory cat = SettingPreferredCategory?.Value ?? SubmissionCategory.Full;
				string catStr = ((cat == SubmissionCategory.Speed) ? "speed" : "full");
				Task.Run(() => SubmitDirectly(mission, elapsed, catStr));
				return;
			}
			_feedbackPrompt?.Dispose();
			SubmissionCategory category = SettingPreferredCategory?.Value ?? SubmissionCategory.Full;
			TimeEstimate estimate = ((category != SubmissionCategory.Speed) ? mission.Times?.Full : mission.Times?.Speed);
			_feedbackPrompt = new FeedbackPrompt(mission, elapsed, estimate, category)
			{
				Parent = GameService.Graphics.SpriteScreen
			};
			_feedbackPrompt.Disposed += delegate
			{
				_widget?.ReenableSubmit();
			};
		}

		private async Task SubmitDirectly(Mission mission, TimeSpan elapsed, string category)
		{
			StoryTimesApiClient apiClient = ApiClient;
			if (apiClient == null)
			{
				_widget?.ReenableSubmit();
				return;
			}
			StoryTimesApiClient.SubmitResult result = await apiClient.SubmitTimeAsync(mission.Id, category, elapsed.TotalMinutes);
			if (result.Success)
			{
				ScreenNotification.ShowNotification("Story Times: Time submitted for " + mission.Name + "!");
				OnSubmissionCompleted(mission);
			}
			else
			{
				ScreenNotification.ShowNotification("Story Times: " + result.Error, ScreenNotification.NotificationType.Warning);
				_widget?.ReenableSubmit();
			}
		}

		internal void OnSubmissionCompleted(Mission submittedMission)
		{
			ActiveMission = null;
			TimerService?.Reset();
			_widget?.ReenableSubmit();
			Task.Run(async delegate
			{
				try
				{
					Mission nextMission = await FindNextMissionAsync(submittedMission);
					if (nextMission != null)
					{
						ShowNextMissionPrompt(nextMission);
					}
				}
				catch (Exception ex)
				{
					Logger.Warn("Failed to find next mission: " + ex.Message);
				}
			});
		}

		private async Task<Mission> FindNextMissionAsync(Mission current)
		{
			if (current == null || string.IsNullOrEmpty(current.SeasonId))
			{
				return null;
			}
			Season season = await ApiClient.GetSeasonAsync(current.SeasonId);
			if (season?.Stories == null)
			{
				return null;
			}
			string playerRace = GetPlayerRace();
			List<(Mission, string)> allMissions = new List<(Mission, string)>();
			foreach (Story story in season.Stories.OrderBy((Story s) => s.Order))
			{
				if ((story.Races != null && story.Races.Count > 0 && playerRace != null && !story.Races.Contains(playerRace)) || story.Missions == null)
				{
					continue;
				}
				foreach (Mission mission in story.Missions.OrderBy((Mission m) => m.Order))
				{
					allMissions.Add((mission, story.Name));
				}
			}
			for (int i = 0; i < allMissions.Count - 1; i++)
			{
				if (allMissions[i].Item1.Id == current.Id)
				{
					Mission next = allMissions[i + 1].Item1;
					if (string.IsNullOrEmpty(next.SeasonName))
					{
						next.SeasonName = season.Name;
					}
					if (string.IsNullOrEmpty(next.StoryName))
					{
						next.StoryName = allMissions[i + 1].Item2;
					}
					if (string.IsNullOrEmpty(next.SeasonId))
					{
						next.SeasonId = season.Id;
					}
					return next;
				}
			}
			return null;
		}

		private void ShowNextMissionPrompt(Mission nextMission)
		{
			_nextMissionPrompt?.Dispose();
			_nextMissionPrompt = new NextMissionPrompt(nextMission)
			{
				Parent = GameService.Graphics.SpriteScreen
			};
		}

		private static string GetPlayerRace()
		{
			try
			{
				return GameService.Gw2Mumble.PlayerCharacter.Race.ToString();
			}
			catch
			{
				return null;
			}
		}
	}
}
