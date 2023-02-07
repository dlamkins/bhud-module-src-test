namespace Kenedia.Modules.Characters.Models
{
	public class ShowCheckPair
	{
		public bool Show { get; set; }

		public bool Check { get; set; }

		public ShowCheckPair(bool show, bool check)
		{
			Check = check;
			Show = show;
		}
	}
}
