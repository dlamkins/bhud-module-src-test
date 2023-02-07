using System;

namespace Kenedia.Modules.Characters.Models
{
	public class SearchFilter<T>
	{
		public Func<T, bool> Check;

		public bool IsEnabled { get; set; }

		public SearchFilter()
		{
		}

		public SearchFilter(Func<T, bool> check, bool enabled = false)
			: this()
		{
			Check = check;
			IsEnabled = enabled;
		}

		public bool CheckForMatch(T target)
		{
			if (IsEnabled)
			{
				return Check(target);
			}
			return true;
		}
	}
}
