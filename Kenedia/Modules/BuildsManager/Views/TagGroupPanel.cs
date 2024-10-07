using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagGroupPanel : FlowPanel
	{
		public static int ControlPaddingY = 4;

		public static int OuterControlPaddingY = 30;

		public static int MaxTags = 6;

		private Rectangle _textBorder;

		private Rectangle _tagBorder;

		private Rectangle _bgBorder;

		private BitmapFont _textFont = Control.Content.DefaultFont12;

		public TagGroup TagGroup { get; }

		public TagGroupPanel(TagGroup tagGroup, Container container)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			TagGroup = tagGroup;
			base.FlowDirection = ControlFlowDirection.LeftToRight;
			WidthSizingMode = SizingMode.Fill;
			base.Parent = container;
			base.OuterControlPadding = new Vector2(4f, (float)OuterControlPaddingY);
			base.ControlPadding = new Vector2((float)ControlPaddingY, (float)ControlPaddingY);
			TagGroup.PropertyChanged += new PropertyAndValueChangedEventHandler(TagGroup_PropertyChanged);
		}

		private void TagGroup_PropertyChanged(object sender, PropertyAndValueChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				_ = e.NewValue is string;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			base.OuterControlPadding = new Vector2(4f, 22f);
			_textBorder = new Rectangle(0, 0, base.Width, _textFont.get_LineHeight());
			_tagBorder = new Rectangle(0, ((Rectangle)(ref _textBorder)).get_Bottom() + 5, base.Width, base.Height - ((Rectangle)(ref _textBorder)).get_Bottom());
			_bgBorder = new Rectangle(base.AbsoluteBounds.X + _tagBorder.X, base.AbsoluteBounds.Y + _tagBorder.Y, _tagBorder.Width, _tagBorder.Height);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			base.OnMoved(e);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, TagGroup?.Name, _textFont, _textBorder, ContentService.Colors.DullColor);
			_bgBorder = new Rectangle(base.AbsoluteBounds.X + _tagBorder.X, base.AbsoluteBounds.Y + _tagBorder.Y, _tagBorder.Width, _tagBorder.Height);
			ShapeExtensions.FillRectangle(spriteBatch, RectangleF.op_Implicit(_bgBorder), Color.get_Black() * 0.4f, 0f);
			spriteBatch.DrawFrame(this, _tagBorder, Color.get_Black(), 2);
		}
	}
}
