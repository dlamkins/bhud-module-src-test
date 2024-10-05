using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Extensions;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class Rune : BaseItem
	{
		[DataMember]
		public List<BonusStat> Bonuses { get; set; } = new List<BonusStat>();


		[DataMember]
		public RuneBonuses BonusDescriptions { get; set; } = new RuneBonuses();


		public string Bonus => BonusDescriptions.Bonuses.Select((string e) => e.InterpretItemDescription()).ToList().Enumerate(Environment.NewLine, "({0}): ") ?? strings.MissingInfoFromAPI;

		public Rune()
		{
			base.TemplateSlot = TemplateSlotType.None;
		}

		public override void Apply(Item item)
		{
			base.Apply(item);
			if (item.Type == ItemType.UpgradeComponent)
			{
				ItemUpgradeComponent upgrade = (ItemUpgradeComponent)item;
				if (upgrade.Details.Bonuses != null)
				{
					BonusDescriptions.AddBonuses(upgrade.Details.Bonuses);
				}
			}
			object obj = GameService.Overlay.UserLocale.Value switch
			{
				Locale.German => new string[2] { "Überlegene Rune des", "Überlegene Rune der" }, 
				Locale.French => new string[2] { "Rune des", "supérieure" }, 
				Locale.Spanish => new string[1] { "Runa superior de" }, 
				_ => new string[2] { "Superior Rune of the", "Superior Rune of" }, 
			};
			string displayText = item.Name;
			string[] array = (string[])obj;
			foreach (string s in array)
			{
				displayText = displayText.Replace(s, string.Empty);
			}
			base.DisplayText = displayText.Trim();
		}
	}
}
