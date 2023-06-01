using System;

namespace Manlaan.Mounts
{
	public class MountImageFile : IEquatable<MountImageFile>, IComparable<MountImageFile>
	{
		public string Name { get; set; }

		public MountImageFile()
		{
			Name = "";
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			MountImageFile objAsCls = obj as MountImageFile;
			if (objAsCls == null)
			{
				return false;
			}
			return Equals(objAsCls);
		}

		public bool Equals(MountImageFile other)
		{
			if (other == null)
			{
				return false;
			}
			return Name.Equals(other.Name);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public int SortByNameAscending(string name1, string name2)
		{
			return name1.CompareTo(name2);
		}

		public int CompareTo(MountImageFile compare)
		{
			if (compare == null)
			{
				return 1;
			}
			return Name.CompareTo(compare.Name);
		}
	}
}
