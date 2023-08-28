namespace Kenedia.Modules.Core.Controls
{
	public class ButtonDefinition
	{
		public string Title { get; set; }

		public DialogResult Result { get; set; }

		public ButtonDefinition(string title, DialogResult result)
		{
			Title = title;
			Result = result;
		}
	}
}