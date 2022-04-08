using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
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
			base.Click += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.MouseMoved += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
				Module.Logger.Debug("Mouse Moved!");
			};
			base.MouseEntered += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.MouseLeft += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.Shown += delegate
			{
				lastInput = DateTime.Now;
				base.Opacity = 1f;
			};
			base.Hidden += delegate
			{
				base.Opacity = 1f;
			};
		}

		public void setCharacter(Character c)
		{
			assignedCharacter = c;
			name_Label.Text = c.Name;
			spec_Image.Texture = c.getProfessionTexture();
			loginCharacter.Checked = c.loginCharacter;
			include_Image.Texture = (c.include ? Textures.Icons[42] : Textures.Icons[43]);
			if (!Tags.SequenceEqual(Module.Tags))
			{
				customTags_Panel.ClearChildren();
				Tags = new List<string>(Module.Tags);
				foreach (string tag2 in Module.Tags)
				{
					new TagEntry(tag2, c, customTags_Panel);
				}
			}
			foreach (TagEntry tag in customTags_Panel)
			{
				tag.Highlighted = c.Tags.Contains(tag.Text);
				tag.assignedCharacter = c;
			}
			bool defaultIcon = assignedCharacter.Icon == null || assignedCharacter.Icon == "";
			border_TopRight.Visible = defaultIcon;
			border_BottomLeft.Visible = defaultIcon;
			include_Image.BasicTooltipText = string.Format(common.ShowHide_Tooltip, c.Name);
		}
	}
}
