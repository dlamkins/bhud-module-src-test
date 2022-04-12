using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Strings;

namespace Kenedia.Modules.Characters
{
	public class CharacterDetailWindow : BasicContainer
	{
		public DateTime lastInput;

		public TextBox tag_TextBox;

		public Image addTag_Button;

		public Label name_Label;

		public Image spec_Image;

		public Image include_Image;

		public Image border_TopRight;

		public Image border_BottomLeft;

		public Label spec_Label;

		public Image separator_Image;

		public FlowPanel customTags_Panel;

		public Checkbox loginCharacter;

		private List<string> Tags = new List<string>();

		public Character assignedCharacter;

		public CharacterDetailWindow()
		{
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_MouseMoved((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
				Module.Logger.Debug("Mouse Moved!");
			});
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_Shown((EventHandler<EventArgs>)delegate
			{
				lastInput = DateTime.Now;
				((Control)this).set_Opacity(1f);
			});
			((Control)this).add_Hidden((EventHandler<EventArgs>)delegate
			{
				((Control)this).set_Opacity(1f);
			});
		}

		public void setCharacter(Character c)
		{
			assignedCharacter = c;
			name_Label.set_Text(c.Name);
			spec_Image.set_Texture(AsyncTexture2D.op_Implicit(c.getProfessionTexture()));
			loginCharacter.set_Checked(c.loginCharacter);
			include_Image.set_Texture(AsyncTexture2D.op_Implicit(c.include ? Textures.Icons[42] : Textures.Icons[43]));
			if (!Tags.SequenceEqual(Module.Tags))
			{
				((Container)customTags_Panel).ClearChildren();
				Tags = new List<string>(Module.Tags);
				foreach (string tag2 in Module.Tags)
				{
					new TagEntry(tag2, c, customTags_Panel);
				}
			}
			foreach (TagEntry tag in (Container)customTags_Panel)
			{
				tag.Highlighted = c.Tags.Contains(tag.Text);
				tag.assignedCharacter = c;
			}
			bool defaultIcon = assignedCharacter.Icon == null || assignedCharacter.Icon == "";
			((Control)border_TopRight).set_Visible(defaultIcon);
			((Control)border_BottomLeft).set_Visible(defaultIcon);
			((Control)include_Image).set_BasicTooltipText(string.Format(common.ShowHide_Tooltip, c.Name));
		}
	}
}
