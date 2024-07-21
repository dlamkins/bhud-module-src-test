using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class NumberBoxTooltipView : View, ITooltipView, IView
	{
		private Label _textLabel;

		private Label _titleLabel;

		public string Text
		{
			get
			{
				Label textLabel = _textLabel;
				if (textLabel == null)
				{
					return null;
				}
				return textLabel.get_Text();
			}
			set
			{
				_textLabel.set_Text(value);
			}
		}

		public string Title
		{
			get
			{
				Label titleLabel = _titleLabel;
				if (titleLabel == null)
				{
					return null;
				}
				return titleLabel.get_Text();
			}
			set
			{
				_titleLabel.set_Text(value);
			}
		}

		public NumberBoxTooltipView()
			: this()
		{
			BuildControls();
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_textLabel).set_Parent(buildPanel);
			((Control)_titleLabel).set_Parent(buildPanel);
		}

		public void BuildControls()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(GetMaxCountText(Title));
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_ShowShadow(true);
			val.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val).set_Location(new Point(0, 0));
			_titleLabel = val;
			Label val2 = new Label();
			val2.set_Text(Text);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_ShowShadow(true);
			val2.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val2).set_Location(new Point(0, 30));
			_textLabel = val2;
		}

		private string GetMaxCountText(string maxCount)
		{
			return maxCount ?? "";
		}

		protected override void Unload()
		{
			Label textLabel = _textLabel;
			if (textLabel != null)
			{
				((Control)textLabel).Dispose();
			}
			Label titleLabel = _titleLabel;
			if (titleLabel != null)
			{
				((Control)titleLabel).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
