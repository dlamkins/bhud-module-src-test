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
			TagGroup = tagGroup;
			base.FlowDirection = ControlFlowDirection.LeftToRight;
			WidthSizingMode = SizingMode.Fill;
			base.Parent = container;
			base.OuterControlPadding = new Vector2(4f, OuterControlPaddingY);
			base.ControlPadding = new Vector2(ControlPaddingY, ControlPaddingY);
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
			base.RecalculateLayout();
			base.OuterControlPadding = new Vector2(4f, 22f);
			_textBorder = new Rectangle(0, 0, base.Width, _textFont.LineHeight);
			_tagBorder = new Rectangle(0, _textBorder.Bottom + 5, base.Width, base.Height - _textBorder.Bottom);
			_bgBorder = new Rectangle(base.AbsoluteBounds.X + _tagBorder.X, base.AbsoluteBounds.Y + _tagBorder.Y, _tagBorder.Width, _tagBorder.Height);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			base.OnMoved(e);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, TagGroup?.Name, _textFont, _textBorder, ContentService.Colors.DullColor);
			_bgBorder = new Rectangle(base.AbsoluteBounds.X + _tagBorder.X, base.AbsoluteBounds.Y + _tagBorder.Y, _tagBorder.Width, _tagBorder.Height);
			spriteBatch.FillRectangle(_bgBorder, Color.Black * 0.4f);
			spriteBatch.DrawFrame(this, _tagBorder, Color.Black, 2);
		}
	}
}
