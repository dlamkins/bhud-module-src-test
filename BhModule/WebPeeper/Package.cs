using System;
using System.Collections.Generic;
using System.Linq;

namespace BhModule.WebPeeper
{
	internal class Package
	{
		public readonly string Name;

		public readonly Version Version;

		public readonly string[] Files;

		public readonly List<string> PendingFiles;

		public Package(string name, Version version, string[] files)
		{
			Name = name;
			Version = version;
			Files = files;
			PendingFiles = files.ToList();
			base._002Ector();
		}
	}
}
