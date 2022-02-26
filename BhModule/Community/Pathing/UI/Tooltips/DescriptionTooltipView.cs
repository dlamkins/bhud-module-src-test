using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Tooltips
{
	public class DescriptionTooltipView : View, ITooltipView, IView
	{
		private readonly Label _titleLabel = new Label();

		private readonly Label _descriptionLabel = new Label();

		public string Title
		{
			get
			{
				return _titleLabel.get_Text();
			}
			set
			{
				_titleLabel.set_Text(value);
				UpdateLayout();
			}
		}

		public string Description
		{
			get
			{
				return _descriptionLabel.get_Text();
			}
			set
			{
				_descriptionLabel.set_Text(value);
				UpdateLayout();
			}
		}

		public DescriptionTooltipView()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown


		public DescriptionTooltipView(string title, string description)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Title = title;
			Description = description;
		}

		private void UpdateLayout()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			((Control)_descriptionLabel).set_Location(new Point(0, (Title != null && Description != null) ? (((Control)_titleLabel).get_Bottom() + 8) : 0));
			_titleLabel.set_AutoSizeWidth(true);
			_descriptionLabel.set_AutoSizeWidth(true);
			if (((Control)_titleLabel).get_Width() > 300 || ((Control)_descriptionLabel).get_Width() > 300)
			{
				_titleLabel.set_AutoSizeWidth(false);
				_descriptionLabel.set_AutoSizeWidth(false);
				((Control)_titleLabel).set_Width(300);
				((Control)_descriptionLabel).set_Width(300);
			}
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			buildPanel.set_HeightSizingMode((SizingMode)1);
			buildPanel.set_WidthSizingMode((SizingMode)1);
			((Control)_titleLabel).set_Location(Point.get_Zero());
			_titleLabel.set_AutoSizeHeight(true);
			((Control)_titleLabel).set_Width(300);
			_titleLabel.set_ShowShadow(true);
			_titleLabel.set_TextColor(Color.FromNonPremultiplied(255, 204, 119, 255));
			_titleLabel.set_Font(GameService.Content.get_DefaultFont16());
			((Control)_titleLabel).set_Parent(buildPanel);
			_descriptionLabel.set_AutoSizeHeight(true);
			((Control)_descriptionLabel).set_Width(300);
			_descriptionLabel.set_WrapText(true);
			_descriptionLabel.set_ShowShadow(true);
			((Control)_descriptionLabel).set_Parent(buildPanel);
			UpdateLayout();
		}
	}
}
