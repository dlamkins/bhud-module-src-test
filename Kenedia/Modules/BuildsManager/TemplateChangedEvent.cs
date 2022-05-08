namespace Kenedia.Modules.BuildsManager
{
	public class TemplateChangedEvent
	{
		public Template Template;

		public TemplateChangedEvent(Template template)
		{
			Template = template;
		}
	}
}
