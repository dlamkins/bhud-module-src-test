using System.IO;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	public class Template_json
	{
		public string Profession;

		public int Specialization;

		public string Name;

		public GearTemplate_json Gear;

		public string BuildCode;

		public Template_json(string path = null)
		{
			if (path != null && File.Exists(path))
			{
				Template_json template = JsonConvert.DeserializeObject<Template_json>(File.ReadAllText(path));
				if (template != null)
				{
					Name = template.Name;
					Gear = template.Gear;
					BuildCode = template.BuildCode;
				}
			}
			else
			{
				Gear = new GearTemplate_json();
				BuildCode = "[&DQIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=]";
			}
		}
	}
}
