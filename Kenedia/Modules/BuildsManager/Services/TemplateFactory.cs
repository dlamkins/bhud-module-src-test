using System;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class TemplateFactory
	{
		public Data Data { get; }

		public TemplateFactory(Data data)
		{
			Data = data;
		}

		public Template CreateTemplate(string? name = null)
		{
			Template t = new Template(Data);
			if (name != null)
			{
				t.Name = name;
			}
			t.TriggerAutoSave = true;
			t.Loaded = true;
			return t;
		}

		public Template CreateTemplate(string? buildCode, string? gearCode)
		{
			return new Template(Data, buildCode, gearCode)
			{
				TriggerAutoSave = true
			};
		}

		public Template CreateTemplate(string? name, string? buildCode, string? gearCode)
		{
			Template t = new Template(Data, buildCode, gearCode);
			if (!string.IsNullOrEmpty(name))
			{
				t.Name = name;
			}
			t.TriggerAutoSave = true;
			return t;
		}

		public Template CreateTemplate(string name, string buildCode, string gearCode, string description, UniqueObservableCollection<string> tags, Races? race, ProfessionType? profession, int elitespecId, string? lastModified)
		{
			return new Template(Data, name, buildCode, gearCode, description, tags, race, profession, elitespecId)
			{
				LastModified = (lastModified ?? DateTime.Now.ToString()),
				TriggerAutoSave = true
			};
		}
	}
}
