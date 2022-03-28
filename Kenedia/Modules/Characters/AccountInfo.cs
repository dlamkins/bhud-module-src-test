using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters
{
	public class AccountInfo
	{
		public string Name;

		public DateTimeOffset LastModified;

		public DateTimeOffset LastBlishUpdate;

		public void Save()
		{
			if (Module.API_Account != null)
			{
				string json = JsonConvert.SerializeObject(new List<AccountInfo> { Module.userAccount }.ToArray());
				File.WriteAllText(Module.AccountPath, json);
			}
		}

		public bool CharacterUpdateNeeded()
		{
			double lastModified = DateTimeOffset.UtcNow.Subtract(LastModified).TotalSeconds;
			double lastUpdate = DateTimeOffset.UtcNow.Subtract(LastBlishUpdate).TotalSeconds;
			if (!(lastModified > 800.0))
			{
				return lastUpdate > lastModified;
			}
			return true;
		}
	}
}
