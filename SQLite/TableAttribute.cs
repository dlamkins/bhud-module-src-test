using System;

namespace SQLite
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableAttribute : Attribute
	{
		public string Name { get; set; }

		public bool WithoutRowId { get; set; }

		public TableAttribute(string name)
		{
			Name = name;
		}
	}
}
