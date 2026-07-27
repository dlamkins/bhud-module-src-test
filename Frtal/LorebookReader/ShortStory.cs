namespace Frtal.LorebookReader
{
	public sealed class ShortStory
	{
		public string Title;

		public string Page;

		public string Writer;

		public string Published;

		public string Timeline;

		public string Url => "https://wiki.guildwars2.com/wiki/" + Page;
	}
}
