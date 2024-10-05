using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class RuneBonuses
	{
		[DataMember]
		public Dictionary<int, LocalizedString> LocalizedBonuses { get; private set; } = new Dictionary<int, LocalizedString>();


		public string this[int key]
		{
			get
			{
				return LocalizedBonuses[key].Text;
			}
			set
			{
				if (!LocalizedBonuses.ContainsKey(key))
				{
					LocalizedBonuses[key] = new LocalizedString();
				}
				LocalizedBonuses[key].Text = value;
			}
		}

		public List<string> Bonuses => LocalizedBonuses.Select<KeyValuePair<int, LocalizedString>, string>((KeyValuePair<int, LocalizedString> e) => e.Value.Text).ToList();

		internal void AddBonuses(IReadOnlyList<string> bonuses)
		{
			for (int i = 0; i < bonuses.Count; i++)
			{
				this[i] = bonuses[i];
			}
		}
	}
}
