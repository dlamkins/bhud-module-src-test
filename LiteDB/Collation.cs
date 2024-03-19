using System;
using System.Collections.Generic;
using System.Globalization;

namespace LiteDB
{
	public class Collation : IComparer<BsonValue>, IComparer<string>, IEqualityComparer<BsonValue>
	{
		private readonly CompareInfo _compareInfo;

		public static Collation Default = new Collation(LiteDB.LCID.Current, CompareOptions.IgnoreCase);

		public static Collation Binary = new Collation(127, CompareOptions.Ordinal);

		public int LCID { get; }

		public CultureInfo Culture { get; }

		public CompareOptions SortOptions { get; }

		public Collation(string collation)
		{
			string[] parts = collation.Split('/');
			string culture = parts[0];
			CompareOptions sortOptions = ((parts.Length > 1) ? ((CompareOptions)Enum.Parse(typeof(CompareOptions), parts[1])) : CompareOptions.None);
			LCID = LiteDB.LCID.GetLCID(culture);
			SortOptions = sortOptions;
			Culture = new CultureInfo(culture);
			_compareInfo = Culture.CompareInfo;
		}

		public Collation(int lcid, CompareOptions sortOptions)
		{
			LCID = lcid;
			SortOptions = sortOptions;
			Culture = LiteDB.LCID.GetCulture(lcid);
			_compareInfo = Culture.CompareInfo;
		}

		public int Compare(string left, string right)
		{
			int result = _compareInfo.Compare(left, right, SortOptions);
			if (result >= 0)
			{
				return (result > 0) ? 1 : 0;
			}
			return -1;
		}

		public int Compare(BsonValue left, BsonValue rigth)
		{
			return left.CompareTo(rigth, this);
		}

		public bool Equals(BsonValue x, BsonValue y)
		{
			return Compare(x, y) == 0;
		}

		public int GetHashCode(BsonValue obj)
		{
			return obj.GetHashCode();
		}

		public override string ToString()
		{
			return Culture.Name + "/" + SortOptions;
		}
	}
}
