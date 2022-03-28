using Blish_HUD.Controls;

namespace Kenedia.Modules.Characters
{
	public class CharacterDetailWindow : BasicContainer
	{
		public TextBox tag_TextBox;

		public Image addTag_Button;

		public Label name_Label;

		public Image spec_Image;

		public Label spec_Label;

		public Image separator_Image;

		public FlowPanel customTags_Panel;

		public Character assignedCharacter;

		public void setCharacter(Character c)
		{
			assignedCharacter = c;
			name_Label.Text = c.Name;
			spec_Label.Text = DataManager.getProfessionName(c._Profession);
			spec_Image.Texture = Textures.Professions[c._Profession];
			customTags_Panel.ClearChildren();
			foreach (string tag in c.Tags)
			{
				new TagEntry(tag, c, customTags_Panel);
			}
		}
	}
}
