using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class LabelNode : TreeNodeBase
	{
		private Image _iconControl;

		private Label _labelControl;

		private Color _textColor = Color.get_White();

		private readonly string _text;

		private readonly AsyncTexture2D _icon;

		public override string PathName => "Label";

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).SetProperty<Color>(ref _textColor, value, false, "TextColor"))
				{
					_labelControl.set_TextColor(value);
					if (_iconControl != null)
					{
						_iconControl.set_Tint(value);
					}
				}
			}
		}

		public LabelNode(string text, Container parent, AsyncTexture2D icon = null)
			: base(parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			_text = text;
			_icon = icon;
			((Control)this).set_Parent(parent);
			FrameColor = Color.get_LightYellow();
			base.ShowBackground = true;
			base.PanelHeight = 25;
			base.ShowFrame = false;
			base.ShowBackground = false;
			UpdateControls();
		}

		public void UpdateControls()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			Label labelControl = _labelControl;
			if (labelControl != null)
			{
				((Control)labelControl).Dispose();
			}
			if (_icon != null)
			{
				Image val = new Image(_icon);
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(new Point(22, 22));
				((Control)val).set_Location(new Point(10, 3));
				_iconControl = val;
			}
			int xPos = ((_iconControl == null) ? 10 : 35);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(_text);
			((Control)val2).set_Location(new Point(xPos, 5));
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			_labelControl = val2;
		}
	}
}
