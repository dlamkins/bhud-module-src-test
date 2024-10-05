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
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawStringOnCtrl(this, string.Format("{1}", Group.Priority, Group.Name), Control.Content.DefaultFont14, TextBounds, Color.get_White());
			spriteBatch.DrawStringOnCtrl(this, $"{Group.Priority}", Control.Content.DefaultFont12, PriorityTextBounds, Color.get_Gray(), wrap: false, HorizontalAlignment.Right);
		}

		public override void RecalculateLayout()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int padding = base.Height - (Control.Content.DefaultFont14.get_LineHeight() + 3 + Control.Content.DefaultFont12.get_LineHeight());
			ContentBounds = new Rectangle(5, padding / 2, base.Width - 30, base.Height - padding);
			PriorityTextBounds = new Rectangle(base.Width - 5, ((Rectangle)(ref ContentBounds)).get_Top() + 5, Control.Content.DefaultFont12.get_LetterSpacing() * 2, Control.Content.DefaultFont12.get_LineHeight());
			TextBounds = new Rectangle(((Rectangle)(ref ContentBounds)).get_Left(), ((Rectangle)(ref ContentBounds)).get_Top(), ContentBounds.Width - 5 - PriorityTextBounds.Width, ContentBounds.Height);
		}

		private void RemoveTag(TagGroup group)
		{
			TagGroups.Remove(group);
		}
	}
}
