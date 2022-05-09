using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Windows
{
	public class AchievementTrackWindow : WindowBase2
	{
		private const int CONTROL_PADDING_LEFT = 10;

		private const int CONTROL_PADDING_TOP = 10;

		private readonly ContentsManager contentsManager;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IAchievementControlProvider achievementControlProvider;

		private readonly IAchievementService achievementService;

		private readonly IAchievementDetailsWindowManager achievementDetailsWindowManager;

		private readonly IAchievementControlManager achievementControlManager;

		private readonly Texture2D texture;

		private readonly Dictionary<int, Panel> trackedAchievements = new Dictionary<int, Panel>();

		private FlowPanel flowPanel;

		public AchievementTrackWindow(ContentsManager contentsManager, IAchievementTrackerService achievementTrackerService, IAchievementControlProvider achievementControlProvider, IAchievementService achievementService, IAchievementDetailsWindowManager achievementDetailsWindowManager, IAchievementControlManager achievementControlManager)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementTrackerService = achievementTrackerService;
			this.achievementControlProvider = achievementControlProvider;
			this.achievementService = achievementService;
			this.achievementDetailsWindowManager = achievementDetailsWindowManager;
			this.achievementControlManager = achievementControlManager;
			texture = this.contentsManager.GetTexture("background.png");
			this.achievementTrackerService.AchievementTracked += AchievementTrackerService_AchievementTracked;
			this.achievementTrackerService.AchievementUntracked += delegate(int achievement)
			{
				if (trackedAchievements.TryGetValue(achievement, out var value))
				{
					trackedAchievements.Remove(achievement);
					this.achievementControlManager.RemoveParent(achievement);
					((Control)value).Dispose();
				}
			};
			this.achievementDetailsWindowManager.WindowHidden += delegate(int achievementId)
			{
				CreatePanel(achievementId);
			};
			BuildWindow();
			foreach (int item in this.achievementTrackerService.ActiveAchievements)
			{
				if (!this.achievementDetailsWindowManager.WindowExists(item))
				{
					AchievementTrackerService_AchievementTracked(item);
				}
			}
		}

		private void CreatePanel(int achievementId)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Expected O, but got Unknown
			AchievementTableEntry achievement = achievementService.Achievements.First((AchievementTableEntry x) => x.Id == achievementId);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)flowPanel);
			val.set_CanCollapse(true);
			val.set_Title(achievement.Name);
			((Control)val).set_Width(((Container)flowPanel).get_ContentRegion().Width - 16);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel panel = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)panel);
			((Control)val2).set_Width(32);
			((Control)val2).set_Height(32);
			val2.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("605019.png")));
			Image trackButton = val2;
			((Control)trackButton).set_Location(new Point(((Container)panel).get_ContentRegion().Width - ((Control)trackButton).get_Width(), 0));
			((Control)trackButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				achievementTrackerService.RemoveAchievement(achievementId);
			});
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)panel);
			((Control)val3).set_Location(new Point(((Control)trackButton).get_Location().X, ((Control)trackButton).get_Location().Y + ((Control)trackButton).get_Size().Y));
			((Control)val3).set_Width(32);
			((Control)val3).set_Height(32);
			val3.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("pop_out.png")));
			Image detachButton = val3;
			if (achievement.HasLink)
			{
				Image val4 = new Image();
				((Control)val4).set_Parent((Container)(object)panel);
				((Control)val4).set_Location(new Point(((Control)detachButton).get_Location().X, ((Control)detachButton).get_Location().Y + ((Control)detachButton).get_Size().Y));
				((Control)val4).set_Width(32);
				((Control)val4).set_Height(32);
				val4.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("wiki.png")));
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start("https://wiki.guildwars2.com" + achievement.Link);
				});
			}
			if (!achievementControlManager.ControlExists(achievementId))
			{
				achievementControlManager.InitializeControl(achievementId, achievement, achievement.Description);
			}
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)panel);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Control)val5).set_Width(((Container)panel).get_ContentRegion().Width - ((Control)trackButton).get_Width() - 10);
			((Control)val5).set_Location(new Point(10, 10));
			Panel controlPanel = val5;
			achievementControlManager.ChangeParent(achievementId, (Container)(object)controlPanel);
			((Control)detachButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				achievementDetailsWindowManager.CreateWindow(achievement);
				if (trackedAchievements.TryGetValue(achievementId, out var value))
				{
					trackedAchievements.Remove(achievementId);
					((Control)value).Dispose();
				}
			});
			trackedAchievements.Add(achievementId, panel);
		}

		private void AchievementTrackerService_AchievementTracked(int achievementId)
		{
			achievementService.Achievements.First((AchievementTableEntry x) => x.Id == achievementId);
			if (!trackedAchievements.ContainsKey(achievementId))
			{
				CreatePanel(achievementId);
			}
		}

		private void BuildWindow()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			((WindowBase2)this).set_Title("Tracked");
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("605019.png"));
			((WindowBase2)this).ConstructWindow(texture, new Rectangle(0, 0, 350, 600), new Rectangle(0, 30, 350, 570));
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_ControlPadding(new Vector2(7f));
			flowPanel = val;
		}

		protected override void DisposeControl()
		{
			foreach (KeyValuePair<int, Panel> trackedAchievement in trackedAchievements)
			{
				((Control)trackedAchievement.Value).Dispose();
			}
			trackedAchievements.Clear();
			((Control)flowPanel).Dispose();
			((WindowBase2)this).DisposeControl();
		}
	}
}
