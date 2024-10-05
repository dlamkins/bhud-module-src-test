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
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int padding = base.Height - (Control.Content.DefaultFont14.get_LineHeight() + 3 + Control.Content.DefaultFont12.get_LineHeight());
			ContentBounds = new Rectangle(5, padding / 2, base.Width - 30, base.Height - padding);
			IconBounds = new Rectangle(((Rectangle)(ref ContentBounds)).get_Left(), ((Rectangle)(ref ContentBounds)).get_Top() + (ContentBounds.Height - 25) / 2, 25, 25);
			PriorityTextBounds = new Rectangle(base.Width - 5, ((Rectangle)(ref ContentBounds)).get_Top(), Control.Content.DefaultFont12.get_LetterSpacing() * 2, Control.Content.DefaultFont12.get_LineHeight());
			TextBounds = new Rectangle(((Rectangle)(ref IconBounds)).get_Right() + 5, ((Rectangle)(ref ContentBounds)).get_Top(), ContentBounds.Width - IconBounds.Width - 5, Control.Content.DefaultFont14.get_LineHeight());
			GroupTextBounds = new Rectangle(((Rectangle)(ref IconBounds)).get_Right() + 5, ((Rectangle)(ref ContentBounds)).get_Bottom() - Control.Content.DefaultFont14.get_LineHeight(), ContentBounds.Width - IconBounds.Width - 5, Control.Content.DefaultFont12.get_LineHeight());
		}

		protected override void DrawItem(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			if (Tag.Icon.Texture != null)
			{
				spriteBatch.DrawOnCtrl((Control)this, (Texture2D)Tag.Icon.Texture, IconBounds);
			}
			spriteBatch.DrawStringOnCtrl(this, string.Format("{1}", Tag.Priority, Tag.Name), Control.Content.DefaultFont14, TextBounds, Color.get_White());
			spriteBatch.DrawStringOnCtrl(this, $"{(string.IsNullOrEmpty(Tag.Group) ? TagGroup.DefaultName : Tag.Group)}", Control.Content.DefaultFont12, GroupTextBounds, Color.get_Gray());
			spriteBatch.DrawStringOnCtrl(this, $"{Tag.Priority}", Control.Content.DefaultFont12, PriorityTextBounds, Color.get_Gray(), wrap: false, HorizontalAlignment.Right);
		}
	}
}
