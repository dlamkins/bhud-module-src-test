using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Windows
{
	public class AchievementDetailsWindow : WindowBase2
	{
		private const int PADDING = 15;

		private readonly ContentsManager contentsManager;

		private readonly IAchievementService achievementService;

		private readonly IAchievementControlManager achievementControlManager;

		private readonly AchievementTableEntry achievement;

		private readonly Texture2D texture;

		public int AchievementId => achievement.Id;

		public AchievementDetailsWindow(ContentsManager contentsManager, AchievementTableEntry achievement, IAchievementService achievementService, IAchievementControlManager achievementControlManager)
			: this()
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.achievementControlManager = achievementControlManager;
			this.achievement = achievement;
			texture = this.contentsManager.GetTexture("achievement_details_background.png");
			BuildWindow();
		}

		private void BuildWindow()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Expected O, but got Unknown
			((WindowBase2)this).set_Title("Details");
			((WindowBase2)this).ConstructWindow(texture, new Rectangle(0, 0, 275, 400), new Rectangle(0, 30, 275, 370));
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 30);
			((Control)val).set_Location(new Point(15, 0));
			((Control)val).set_Height(((Container)this).get_ContentRegion().Height);
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Parent((Container)(object)this);
			val.set_ControlPadding(new Vector2(10f));
			FlowPanel flowPanel = val;
			Label val2 = new Label();
			val2.set_Text(achievement.Name);
			((Control)val2).set_Parent((Container)(object)flowPanel);
			val2.set_AutoSizeHeight(true);
			val2.set_WrapText(true);
			((Control)val2).set_Width(((Container)flowPanel).get_ContentRegion().Width);
			val2.set_Font(Control.get_Content().get_DefaultFont18());
			((Control)val2).set_Padding(new Thickness(0f, 0f, 20f, 0f));
			Panel val3 = new Panel();
			((Control)val3).set_Width(((Container)flowPanel).get_ContentRegion().Width);
			((Control)val3).set_Parent((Container)(object)flowPanel);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			Panel panel = val3;
			if (!achievementControlManager.ControlExists(achievement.Id))
			{
				achievementControlManager.InitializeControl(achievement.Id, achievement, achievement.Description);
			}
			achievementControlManager.ChangeParent(achievement.Id, (Container)(object)panel);
		}
	}
}
