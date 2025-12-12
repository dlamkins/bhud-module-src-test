using System.Collections.Generic;
using Blish_HUD;

namespace Kenedia.Modules.Characters.Models
{
	public class SearchFilterCollection : Dictionary<string, SearchFilter<Character_Model>>
	{
		public void AddOrUpdate(string key, SearchFilter<Character_Model> value)
		{
			if (!ContainsKey(key))
			{
				Add(key, value);
				return;
			}
			Logger.GetLogger(typeof(SearchFilterCollection)).Debug("Key " + key + " exists already. Updating filter '" + key + "' instead.");
			base[key] = value;
		}
	}
}
