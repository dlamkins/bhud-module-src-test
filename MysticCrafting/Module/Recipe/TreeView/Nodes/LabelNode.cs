using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class LabelNode : TreeNodeBase
	{
		private Label _labelControl;

		private Color _textColor = Color.White;

		private readonly string _text;

		public override string PathName => "Label";

		public Color TextColor
		{
			get
			{
				return _textColor;
			}
			set
			{
				if (SetProperty(ref _textColor, value, invalidateLayout: false, "TextColor"))
				{
					_labelControl.TextColor = value;
				}
			}
		}

		public LabelNode(string text, Container parent)
		{
			_text = text;
			base.Parent = parent;
			base.PanelHeight = 20;
			base.ShowFrame = false;
			base.ShowBackground = false;
			UpdateControls();
		}

		public void UpdateControls()
		{
			_labelControl?.Dispose();
			_labelControl = new Label
			{
				Parent = this,
				Text = _text,
				Location = new Point(0, 0),
				Width = 200,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont14,
				TextColor = Color.White,
				StrokeText = true,
				AutoSizeWidth = false
			};
		}
	}
}
