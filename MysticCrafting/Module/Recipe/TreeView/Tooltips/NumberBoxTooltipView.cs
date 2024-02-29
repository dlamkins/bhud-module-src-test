using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class NumberBoxTooltipView : View, ITooltipView, IView
	{
		private Label _textLabel;

		private Label _titleLabel;

		public string Text
		{
			get
			{
				return _textLabel?.Text;
			}
			set
			{
				_textLabel.Text = value;
			}
		}

		public string Title
		{
			get
			{
				return _titleLabel?.Text;
			}
			set
			{
				_titleLabel.Text = value;
			}
		}

		public NumberBoxTooltipView()
		{
			BuildControls();
		}

		protected override void Build(Container buildPanel)
		{
			_textLabel.Parent = buildPanel;
			_titleLabel.Parent = buildPanel;
		}

		public void BuildControls()
		{
			_titleLabel = new Label
			{
				Text = GetMaxCountText(Title),
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				ShowShadow = true,
				Font = GameService.Content.DefaultFont14,
				Location = new Point(0, 0)
			};
			_textLabel = new Label
			{
				Text = Text,
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				ShowShadow = true,
				Font = GameService.Content.DefaultFont14,
				Location = new Point(0, 30)
			};
		}

		private string GetMaxCountText(string maxCount)
		{
			return maxCount ?? "";
		}

		protected override void Unload()
		{
			_textLabel?.Dispose();
			_titleLabel?.Dispose();
			base.Unload();
		}
	}
}
