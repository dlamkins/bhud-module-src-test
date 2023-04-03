using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Windows
{
	public class AchievementTrackWindow : WindowBase2
	{
		public class TestPanel : Panel
		{
			public TestPanel()
				: this()
			{
			}
		}

		private const int CONTROL_PADDING_LEFT = 10;

		private const int CONTROL_PADDING_TOP = 10;

		private readonly ContentsManager contentsManager;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IAchievementControlProvider achievementControlProvider;

		private readonly IAchievementService achievementService;

		private readonly IAchievementDetailsWindowManager achievementDetailsWindowManager;

		private readonly IAchievementControlManager achievementControlManager;

		private readonly ISubPageInformationWindowManager subPageInformationWindowManager;

		private readonly OverlayService overlayService;

		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		private readonly Func<IView> achievementOverviewView;

		private readonly Texture2D texture;

		private readonly Dictionary<int, Panel> trackedAchievements = new Dictionary<int, Panel>();

		private FlowPanel flowPanel;

		private Label noAchievementsLabel;

		public AchievementTrackWindow(ContentsManager contentsManager, IAchievementTrackerService achievementTrackerService, IAchievementControlProvider achievementControlProvider, IAchievementService achievementService, IAchievementDetailsWindowManager achievementDetailsWindowManager, IAchievementControlManager achievementControlManager, ISubPageInformationWindowManager subPageInformationWindowManager, OverlayService overlayService, IFormattedLabelHtmlService formattedLabelHtmlService, Func<IView> achievementOverviewView)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementTrackerService = achievementTrackerService;
			this.achievementControlProvider = achievementControlProvider;
			this.achievementService = achievementService;
			this.achievementDetailsWindowManager = achievementDetailsWindowManager;
			this.achievementControlManager = achievementControlManager;
			this.subPageInformationWindowManager = subPageInformationWindowManager;
			this.overlayService = overlayService;
			this.formattedLabelHtmlService = formattedLabelHtmlService;
			this.achievementOverviewView = achievementOverviewView;
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
				if (trackedAchievements.Count == 0)
				{
					((Control)noAchievementsLabel).set_Visible(true);
				}
			};
			this.achievementDetailsWindowManager.WindowHidden += delegate(int achievementId)
			{
				CreatePanel(achievementId);
			};
			BuildWindow();
			Task.Run(delegate
			{
				foreach (int current in this.achievementTrackerService.ActiveAchievements)
				{
					if (!this.achievementDetailsWindowManager.WindowExists(current))
					{
						AchievementTrackerService_AchievementTracked(current);
					}
				}
			});
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
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected O, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Expected O, but got Unknown
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
			val2.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("track_enabled.png")));
			Image trackButton = val2;
			((Control)trackButton).set_Location(new Point(((Container)panel).get_ContentRegion().Width - ((Control)trackButton).get_Width(), 0));
			((Control)trackButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				achievementTrackerService.RemoveAchievement(achievementId);
			});
			((Control)trackButton).set_BasicTooltipText("Untrack this achievement");
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)panel);
			((Control)val3).set_Location(new Point(((Control)trackButton).get_Location().X, ((Control)trackButton).get_Location().Y + ((Control)trackButton).get_Size().Y));
			((Control)val3).set_Width(32);
			((Control)val3).set_Height(32);
			val3.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("pop_out.png")));
			Image detachButton = val3;
			((Control)detachButton).set_BasicTooltipText("Detach into own window");
			if (achievement.HasLink)
			{
				Image val4 = new Image();
				((Control)val4).set_Parent((Container)(object)panel);
				((Control)val4).set_Location(new Point(((Control)detachButton).get_Location().X, ((Control)detachButton).get_Location().Y + ((Control)detachButton).get_Size().Y));
				((Control)val4).set_Width(32);
				((Control)val4).set_Height(32);
				val4.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("link.png")));
				Image linkButton = val4;
				((Control)linkButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					bool flag = false;
					foreach (SubPageInformation current in achievementService.Subpages)
					{
						if (current.Link == "https://wiki.guildwars2.com" + achievement.Link)
						{
							flag = true;
							subPageInformationWindowManager.Create(current);
						}
					}
					if (!flag)
					{
						Process.Start("https://wiki.guildwars2.com" + achievement.Link);
					}
				});
				((Control)linkButton).set_BasicTooltipText("Open achievement in subpages or wiki");
				Image val5 = new Image();
				((Control)val5).set_Parent((Container)(object)panel);
				((Control)val5).set_Location(new Point(((Control)linkButton).get_Location().X, ((Control)linkButton).get_Location().Y + ((Control)linkButton).get_Size().Y));
				((Control)val5).set_Width(32);
				((Control)val5).set_Height(32);
				val5.set_Texture(AsyncTexture2D.op_Implicit(contentsManager.GetTexture("wiki.png")));
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Process.Start("https://wiki.guildwars2.com" + achievement.Link);
				});
				((Control)val5).set_BasicTooltipText("Open achievement in wiki");
			}
			if (!achievementControlManager.ControlExists(achievementId))
			{
				achievementControlManager.InitializeControl(achievementId, achievement, achievement.Description);
			}
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)panel);
			((Container)val6).set_HeightSizingMode((SizingMode)1);
			((Control)val6).set_Width(((Container)panel).get_ContentRegion().Width - ((Control)trackButton).get_Width() - 10);
			((Control)val6).set_Location(new Point(10, 10));
			Panel controlPanel = val6;
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
			if (((Control)noAchievementsLabel).get_Visible())
			{
				((Control)noAchievementsLabel).set_Visible(false);
			}
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
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Expected O, but got Unknown
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Expected O, but got Unknown
			((WindowBase2)this).set_Title("Tracked");
			((WindowBase2)this).set_Emblem(contentsManager.GetTexture("track_enabled.png"));
			((WindowBase2)this).ConstructWindow(texture, new Rectangle(0, 0, 350, 600), new Rectangle(0, 30, 350, 570));
			StandardButton val = new StandardButton();
			val.set_Text("Collapse All");
			((Control)val).set_Height(30);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val).set_Parent((Container)(object)this);
			StandardButton collapseAll = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Open Achievement Panel");
			((Control)val2).set_Height(30);
			((Control)val2).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val2).set_Parent((Container)(object)this);
			StandardButton openAchievementPanelButton = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Close all Subpages");
			((Control)val3).set_Height(30);
			((Control)val3).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val3).set_Parent((Container)(object)this);
			StandardButton closeSubPagesButton = val3;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)this);
			((Panel)val4).set_CanScroll(true);
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Control)val4).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val4).set_Height(((Container)this).get_ContentRegion().Height - ((Control)openAchievementPanelButton).get_Height() - ((Control)closeSubPagesButton).get_Height() - ((Control)collapseAll).get_Height());
			((Control)val4).set_Location(new Point(0, ((Control)collapseAll).get_Height()));
			val4.set_ControlPadding(new Vector2(7f));
			flowPanel = val4;
			((Control)openAchievementPanelButton).set_Location(new Point(0, ((Control)flowPanel).get_Height() + ((Control)flowPanel).get_Location().Y));
			((Control)closeSubPagesButton).set_Location(new Point(0, ((Control)openAchievementPanelButton).get_Height() + ((Control)openAchievementPanelButton).get_Location().Y));
			((Control)openAchievementPanelButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!((Control)overlayService.get_BlishHudWindow()).get_Visible())
				{
					((Control)overlayService.get_BlishHudWindow()).Show();
				}
				overlayService.get_BlishHudWindow().Navigate(achievementOverviewView(), true);
			});
			((Control)closeSubPagesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				subPageInformationWindowManager.CloseWindows();
			});
			((Control)collapseAll).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (Panel value in trackedAchievements.Values)
				{
					value.Collapse();
				}
			});
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)flowPanel);
			((Control)val5).set_Height(((Container)flowPanel).get_ContentRegion().Height);
			((Control)val5).set_Width(((Container)flowPanel).get_ContentRegion().Width);
			val5.set_Text("You currently don't track any achievements.\n To open the achievement overview, either press\n the button below or open the\n blishhud window and navigate to the achievement tab.");
			((Control)val5).set_Visible(true);
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			val5.set_VerticalAlignment((VerticalAlignment)1);
			noAchievementsLabel = val5;
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
