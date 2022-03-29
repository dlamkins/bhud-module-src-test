using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;

namespace Kenedia.Modules.Characters
{
	public class CharacterDetailWindow : BasicContainer
	{
		public TextBox tag_TextBox;

		public Image addTag_Button;

		public Label name_Label;

		public Image spec_Image;

		public Image include_Image;

		public Label spec_Label;

		public Image separator_Image;

		public FlowPanel customTags_Panel;

		public Checkbox loginCharacter;

		private List<string> Tags = new List<string>();

		public Character assignedCharacter;

		public void setCharacter(Character c)
		{
			assignedCharacter = c;
			name_Label.Text = c.Name;
			spec_Label.Text = DataManager.getProfessionName(c._Profession);
			spec_Image.Texture = Textures.Professions[c._Profession];
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
		}
	}
}
