using Newtonsoft.Json;

namespace Kenedia.Modules.Core.Models
{
	public class SerializerSettings
	{
		public static JsonSerializerSettings Default;

		static SerializerSettings()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_Formatting((Formatting)1);
			val.set_NullValueHandling((NullValueHandling)1);
			Default = val;
		}
	}
}
