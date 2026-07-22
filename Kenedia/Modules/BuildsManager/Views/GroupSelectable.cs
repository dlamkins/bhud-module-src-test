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
	public class GroupSelectable : Selectable<TagGroup>
	{
		protected Rectangle PriorityTextBounds;

		public TagGroup Group => base.Item;

		public TagGroups TagGroups { get; }

		public GroupSelectable(TagGroup tagGroup, Container parent, TagGroups tagGroups)
			: base(tagGroup, parent)
		{
			TagGroups = tagGroups;
			base.Menu = new ContextMenuStrip();
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings.Delete, delegate
			{
				RemoveTag(Group);
			}));
		}

		protected override void DrawItem(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.DrawStringOnCtrl(this, string.Format("{1}", Group.Priority, Group.Name), Control.Content.DefaultFont14, TextBounds, Color.White);
			spriteBatch.DrawStringOnCtrl(this, $"{Group.Priority}", Control.Content.DefaultFont12, PriorityTextBounds, Color.Gray, wrap: false, HorizontalAlignment.Right);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int padding = base.Height - (Control.Content.DefaultFont14.LineHeight + 3 + Control.Content.DefaultFont12.LineHeight);
			ContentBounds = new Rectangle(5, padding / 2, base.Width - 30, base.Height - padding);
			PriorityTextBounds = new Rectangle(base.Width - 5, ContentBounds.Top + 5, Control.Content.DefaultFont12.LetterSpacing * 2, Control.Content.DefaultFont12.LineHeight);
			TextBounds = new Rectangle(ContentBounds.Left, ContentBounds.Top, ContentBounds.Width - 5 - PriorityTextBounds.Width, ContentBounds.Height);
		}

		private void RemoveTag(TagGroup group)
		{
			TagGroups.Remove(group);
		}
	}
}
