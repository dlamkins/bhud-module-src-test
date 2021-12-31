using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Tooltips
{
	public class AchievementTooltipView : View, ITooltipView, IView
	{
		private Achievement _achievement;

		private AchievementCategory _achievementCategory;

		private Image _categoryIconImage;

		private Label _achievementNameLabel;

		private Label _achievementDescriptionLabel;

		private Label _achievementRequirementLabel;

		public Achievement Achievement
		{
			get
			{
				return _achievement;
			}
			set
			{
				if (_achievement != value)
				{
					_achievement = value;
					UpdateAchievementView();
				}
			}
		}

		public AchievementCategory AchievementCategory
		{
			get
			{
				return _achievementCategory;
			}
			set
			{
				if (_achievementCategory != value)
				{
					_achievementCategory = value;
					UpdateCategoryView();
				}
			}
		}

		public AchievementTooltipView()
			: this()
		{
		}

		public AchievementTooltipView(int achievementId)
			: this()
		{
			((View)this).WithPresenter((IPresenter)(object)new AchievementPresenter(this, achievementId));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Expected O, but got Unknown
			buildPanel.set_HeightSizingMode((SizingMode)1);
			buildPanel.set_WidthSizingMode((SizingMode)1);
			Image val = new Image();
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Location(new Point(8, 8));
			((Control)val).set_Parent(buildPanel);
			_categoryIconImage = val;
			Label val2 = new Label();
			val2.set_AutoSizeHeight(false);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)_categoryIconImage).get_Right() + 8, ((Control)_categoryIconImage).get_Top()));
			((Control)val2).set_Height(((Control)_categoryIconImage).get_Height() / 2);
			((Control)val2).set_Padding(new Thickness(0f, 8f, 0f, 0f));
			val2.set_HorizontalAlignment((HorizontalAlignment)0);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_ShowShadow(true);
			((Control)val2).set_Parent(buildPanel);
			_achievementNameLabel = val2;
			Label val3 = new Label();
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(false);
			((Control)val3).set_Location(new Point(((Control)_achievementNameLabel).get_Left(), ((Control)_categoryIconImage).get_Top() + ((Control)_categoryIconImage).get_Height() / 2));
			((Control)val3).set_Width(Math.Max(((Control)_achievementNameLabel).get_Width(), 200));
			((Control)val3).set_Padding(new Thickness(0f, 8f, 0f, 0f));
			val3.set_HorizontalAlignment((HorizontalAlignment)0);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_TextColor(StandardColors.get_DisabledText());
			val3.set_ShowShadow(true);
			val3.set_WrapText(true);
			((Control)val3).set_Parent(buildPanel);
			_achievementDescriptionLabel = val3;
			Label val4 = new Label();
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(false);
			((Control)val4).set_Location(new Point(((Control)_categoryIconImage).get_Left(), ((Control)_achievementDescriptionLabel).get_Bottom() + 8));
			val4.set_ShowShadow(true);
			val4.set_WrapText(true);
			((Control)val4).set_Parent(buildPanel);
			_achievementRequirementLabel = val4;
		}

		private void UpdateCategoryView()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (_achievementCategory != null)
			{
				_categoryIconImage.set_Texture(GameService.Content.GetRenderServiceTexture(RenderUrl.op_Implicit(_achievementCategory.get_Icon())));
			}
		}

		private void UpdateAchievementView()
		{
			if (_achievement != null)
			{
				_achievementNameLabel.set_Text(_achievement.get_Name());
				_achievementDescriptionLabel.set_Text(_achievement.get_Description());
				_achievementRequirementLabel.set_Text(_achievement.get_Requirement());
				((Control)_achievementNameLabel).set_Height(string.IsNullOrEmpty(_achievement.get_Description()) ? ((Control)_categoryIconImage).get_Height() : (((Control)_categoryIconImage).get_Height() / 2));
				((Control)_achievementDescriptionLabel).set_Width(Math.Max(((Control)_achievementNameLabel).get_Width(), 200));
				((Control)_achievementRequirementLabel).set_Width(new int[3]
				{
					((Control)_achievementNameLabel).get_Right() + 8,
					((Control)_achievementDescriptionLabel).get_Right() + 8,
					300
				}.Max());
				((Control)_achievementRequirementLabel).set_Top(Math.Max(((Control)_achievementDescriptionLabel).get_Bottom() + 8, ((Control)_categoryIconImage).get_Bottom() + 8));
			}
		}
	}
}