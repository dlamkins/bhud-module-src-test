using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace lk0001.CurrentTemplates.Controls
{
	internal class DrawTemplates : Container
	{
		public FontSize Font_Size = (FontSize)16;

		public HorizontalAlignment Align;

		public bool Drag;

		public bool BuildPad;

		public string BuildPadConfigPath = "";

		public string buildName = "";

		public string equipmentName = "";

		private static BitmapFont _font;

		private Point _dragStart = Point.get_Zero();

		private bool _dragging;

		private LoadingSpinner spinner;

		public DrawTemplates()
			: this()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			((Control)this).set_Location(new Point(0, 0));
			((Control)this).set_Size(new Point(0, 0));
			((Control)this).set_Visible(true);
			((Control)this).set_Padding(Thickness.Zero);
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Location(new Point(1, 1));
			((Control)val).set_Visible(true);
			((Control)val).set_Parent((Container)(object)this);
			spinner = val;
			((Container)this).AddChild((Control)(object)spinner);
		}

		protected override CaptureType CapturesInput()
		{
			if (!Drag)
			{
				return (CaptureType)1;
			}
			return (CaptureType)4;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (Drag)
			{
				_dragging = true;
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (Drag)
			{
				_dragging = false;
				Module._settingLoc.set_Value(((Control)this).get_Location());
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
		}

		public void ShowSpinner()
		{
			((Control)spinner).Show();
		}

		public void HideSpinner()
		{
			((Control)spinner).Hide();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			string templates = "";
			_font = GameService.Content.GetFont((FontFace)0, Font_Size, (FontStyle)0);
			templates = templates + " " + buildName + " \n";
			templates = templates + " " + equipmentName + " \n";
			int templatesWidth = (int)_font.MeasureString(templates).Width;
			int spinnerSize = (int)_font.MeasureString("C").Height * 2;
			((Control)this).set_Size(new Point(templatesWidth + spinnerSize, (int)_font.MeasureString(templates).Height));
			((Control)spinner).set_Location(new Point(templatesWidth, 1));
			((Control)spinner).set_Size(new Point(spinnerSize, spinnerSize));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, templates, _font, new Rectangle(0, 0, ((Control)this).get_Size().X, ((Control)this).get_Size().Y), Color.get_White(), false, true, 1, Align, (VerticalAlignment)0);
		}
	}
}
