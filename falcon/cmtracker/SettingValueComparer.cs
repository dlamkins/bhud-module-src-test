using System.Collections.Generic;

namespace falcon.cmtracker
{
	public class SettingValueComparer : IEqualityComparer<SettingValue>
	{
		public bool Equals(SettingValue x, SettingValue y)
		{
			if (x.Account.Equals(y.Account))
			{
				return x.Boss.Equals(y.Boss);
			}
			return false;
		}

		public int GetHashCode(SettingValue obj)
		{
			return obj.Account.GetHashCode() * 397 + obj.Boss.GetHashCode();
		}
	}
}
