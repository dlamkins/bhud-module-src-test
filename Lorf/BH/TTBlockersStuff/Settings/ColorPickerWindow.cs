using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTBlockersStuff.Language;

namespace Lorf.BH.TTBlockersStuff.Settings
{
	internal class ColorPickerWindow : Container, IWindow
	{
		private int textureMarginLeft;

		private int textureMarginTop;

		private int textureMarginRight;

		private int textureMarginBottom;

		private Color originalColor;

		private static readonly Texture2D _textureWindowTexture = GameService.Content.GetTexture("controls/window/502049");

		private readonly Rectangle _normalizedWindowRegion = new Rectangle(0, 0, 400, 400);

		private readonly string pickName;

		private readonly ColorBox colorBox;

		private ColorPicker picker;

		private StandardButton _acceptBttn;

		private StandardButton _cancelBttn;

		private Rectangle _windowRegion;

		public bool TopMost => false;

		public double LastInteraction => double.MaxValue;

		public bool CanClose => false;

		public bool CanCloseWithEscape => true;

		public event EventHandler<EventArgs> AssignmentAccepted;

		public event EventHandler<EventArgs> AssignmentCanceled;

		private void OnAssignmentAccepted(EventArgs e)
		{
			this.AssignmentAccepted?.Invoke(this, e);
			((Control)this).Dispose();
		}

		private void OnAssignmentCanceled(EventArgs e)
		{
			colorBox.set_Color(originalColor);
			this.AssignmentCanceled?.Invoke(this, e);
			((Control)this).Dispose();
		}

		public ColorPickerWindow(string name, ColorBox box)
			: this()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			pickName = name;
			colorBox = box;
			originalColor = colorBox.get_Color();
			((Control)this).set_BackgroundColor(Color.get_Black() * 0.3f);
			((Control)this).set_Size(new Point(_normalizedWindowRegion.Width, _normalizedWindowRegion.Height));
			((Control)this).set_ZIndex(2147483614);
			((Control)this).set_Visible(false);
			BuildChildElements();
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).Invalidate();
			((Control)this).OnShown(e);
		}

		private int doABitOfMath(float size, bool height)
		{
			float num = (height ? _textureWindowTexture.get_Height() : _textureWindowTexture.get_Width());
			float num2 = (height ? ((Control)this).get_Height() : ((Control)this).get_Width());
			return (int)(size * (num2 / num));
		}

		private void BuildChildElements()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Expected O, but got Unknown
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			textureMarginLeft = doABitOfMath(36f, height: false);
			textureMarginTop = doABitOfMath(26f, height: true);
			textureMarginRight = doABitOfMath(72f, height: false);
			textureMarginBottom = doABitOfMath(35f, height: true);
			Label val = new Label();
			val.set_Text(pickName);
			((Control)val).set_Location(new Point(textureMarginLeft + 18, textureMarginTop + 18));
			val.set_ShowShadow(true);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Parent((Container)(object)this);
			val.set_Font(GameService.Content.get_DefaultFont16());
			Label val2 = val;
			StandardButton val3 = new StandardButton();
			val3.set_Text(Translations.ColorSelectionButtonTextCancel);
			((Control)val3).set_Location(new Point(((Control)this).get_Width() - textureMarginRight - 100 - 18, ((Control)this).get_Height() - textureMarginBottom - 18 - 25));
			((Control)val3).set_Width(100);
			((Control)val3).set_Height(25);
			((Control)val3).set_Parent((Container)(object)this);
			_cancelBttn = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text(Translations.ColorSelectionButtonTextOk);
			((Control)val4).set_Width(100);
			((Control)val4).set_Height(25);
			((Control)val4).set_Parent((Container)(object)this);
			_acceptBttn = val4;
			((Control)_acceptBttn).set_Location(new Point(((Control)_cancelBttn).get_Left() - 8 - ((Control)_acceptBttn).get_Width(), ((Control)_cancelBttn).get_Top()));
			((Control)_cancelBttn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OnAssignmentCanceled(EventArgs.Empty);
			});
			((Control)_acceptBttn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OnAssignmentAccepted(EventArgs.Empty);
			});
			picker = new ColorPicker();
			((Control)picker).set_Visible(true);
			((Control)picker).set_Parent((Container)(object)this);
			((Control)picker).set_Size(new Point(((Control)this).get_Width() - ((Control)val2).get_Left() - 36, ((Control)_cancelBttn).get_Top() - ((Control)val2).get_Bottom() - 36));
			((Panel)picker).set_CanScroll(true);
			((Control)picker).set_Location(new Point(((Control)val2).get_Left(), ((Control)val2).get_Bottom() + 18));
			picker.set_AssociatedColorBox(colorBox);
			foreach (Color color in Module.Instance.Colors)
			{
				picker.get_Colors().Add(color);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)this)._size = ((Control)parent).get_Size();
				Point val = default(Point);
				((Point)(ref val))._002Ector(((Control)this)._size.X / 2 - _normalizedWindowRegion.Width / 2, ((Control)this)._size.Y / 2 - _normalizedWindowRegion.Height / 2);
				_windowRegion = RectangleExtension.OffsetBy(_normalizedWindowRegion, val);
				((Container)this).set_ContentRegion(_windowRegion);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureWindowTexture, _windowRegion);
		}

		public void BringWindowToFront()
		{
		}
	}
}
