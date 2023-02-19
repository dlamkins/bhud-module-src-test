namespace Kenedia.Modules.Core.DataModels
{
	public class FlipSkill
	{
		public int? Default;

		public int? Activated;

		public int?[] Values => new int?[2] { Default, Activated };

		public void Clear()
		{
			Default = null;
			Activated = null;
		}
	}
}
