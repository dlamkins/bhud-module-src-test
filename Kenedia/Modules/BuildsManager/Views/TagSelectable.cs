using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.Controls;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagSelectable : Selectable<TemplateTag>
	{
		protected Rectangle PriorityTextBounds;

		protected Rectangle GroupTextBounds;

		public TemplateTag Tag => base.Item;

		public TemplateTags TemplateTags { get; }

		public TagSelectable(TemplateTag tag, Container parent, TemplateTags templateTags)
			: base(tag, parent)
		{
			TemplateTags = templateTags;
			base.Height = 40;
			base.Menu = new ContextMenuStrip();
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings.Delete, delegate
			{
				RemoveTag(Tag);
			}));
		}

		private void RemoveTag(TemplateTag tag)
		{
			TemplateTags.Remove(Tag);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int padding = base.Height - (Control.Content.DefaultFont14.LineHeight + 3 + Control.Content.DefaultFont12.LineHeight);
			ContentBounds = new Rectangle(5, padding / 2, base.Width - 30, base.Height - padding);
			IconBounds = new Rectangle(ContentBounds.Left, ContentBounds.Top + (ContentBounds.Height - 25) / 2, 25, 25);
			PriorityTextBounds = new Rectangle(base.Width - 5, ContentBounds.Top, Control.Content.DefaultFont12.LetterSpacing * 2, Control.Content.DefaultFont12.LineHeight);
			TextBounds = new Rectangle(IconBounds.Right + 5, ContentBounds.Top, ContentBounds.Width - IconBounds.Width - 5, Control.Content.DefaultFont14.LineHeight);
			GroupTextBounds = new Rectangle(IconBounds.Right + 5, ContentBounds.Bottom - Control.Content.DefaultFont14.LineHeight, ContentBounds.Width - IconBounds.Width - 5, Control.Content.DefaultFont12.LineHeight);
		}

		protected override void DrawItem(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Tag.Icon.Texture != null)
			{
				spriteBatch.DrawOnCtrl(this, Tag.Icon.Texture, IconBounds, Tag.TextureRegion);
			}
			spriteBatch.DrawStringOnCtrl(this, string.Format("{1}", Tag.Priority, Tag.Name), Control.Content.DefaultFont14, TextBounds, Color.White);
			spriteBatch.DrawStringOnCtrl(this, $"{(string.IsNullOrEmpty(Tag.Group) ? TagGroup.DefaultName : Tag.Group)}", Control.Content.DefaultFont12, GroupTextBounds, Color.Gray);
			spriteBatch.DrawStringOnCtrl(this, $"{Tag.Priority}", Control.Content.DefaultFont12, PriorityTextBounds, Color.Gray, wrap: false, HorizontalAlignment.Right);
		}
	}
}
