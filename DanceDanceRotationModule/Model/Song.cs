using System;
using System.Collections.Generic;

namespace DanceDanceRotationModule.Model
{
	public class Song
	{
		public struct ID : IEquatable<ID>
		{
			public string Name { get; set; }

			public ID(string name)
			{
				Name = name;
			}

			public bool IsValid()
			{
				return Name.Length > 0;
			}

			public bool Equals(ID other)
			{
				return Name == other.Name;
			}

			public override bool Equals(object obj)
			{
				if (obj is ID)
				{
					ID other = (ID)obj;
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				if (Name == null)
				{
					return 0;
				}
				return Name.GetHashCode();
			}
		}

		public ID Id { get; set; }

		public string Description { get; set; }

		public string BuildUrl { get; set; }

		public string BuildTemplateCode { get; set; }

		public PaletteId Utility1 { get; set; }

		public PaletteId Utility2 { get; set; }

		public PaletteId Utility3 { get; set; }

		public Profession Profession { get; set; }

		public string EliteName { get; set; }

		public List<Note> Notes { get; set; }

		public string Name => Id.Name;
	}
}
