using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class LabelNode : TreeNodeBase
	{
		private Image _iconControl;

		private Label _labelControl;

		private Color _textColor = Color.White;

		private readonly string _text;

		private readonly AsyncTexture2D _icon;

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
					if (_iconControl != null)
					{
						_iconControl.Tint = value;
					}
				}
			}
		}

		public LabelNode(string text, Container parent, AsyncTexture2D icon = null)
		{
			_text = text;
			_icon = icon;
			base.Parent = parent;
			FrameColor = Color.LightYellow;
			base.ShowBackground = true;
			base.PanelHeight = 25;
			base.ShowFrame = false;
			base.ShowBackground = false;
			UpdateControls();
		}

		public void UpdateControls()
		{
			_labelControl?.Dispose();
			if (_icon != null)
			{
				_iconControl = new Image(_icon)
				{
					Parent = this,
					Size = new Point(22, 22),
					Location = new Point(10, 3)
				};
			}
			int xPos = ((_iconControl == null) ? 10 : 35);
			_labelControl = new Label
			{
				Parent = this,
				Text = _text,
				Location = new Point(xPos, 5),
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				Font = GameService.Content.DefaultFont14,
				TextColor = Color.White,
				StrokeText = true
			};
		}
	}
}
