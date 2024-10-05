using System.Runtime.Serialization;
using Blish_HUD;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Items
{
	[DataContract]
	public class Sigil : BaseItem
	{
		[DataMember]
		public LocalizedString Buffs { get; protected set; } = new LocalizedString();


		public string Buff
		{
			get
			{
				return Buffs.Text.InterpretItemDescription();
			}
			set
			{
				Buffs.Text = value;
			}
		}

		public Sigil()
		{
			base.TemplateSlot = TemplateSlotType.None;
		}

		public override void Apply(Item item)
		{
			base.Apply(item);
			if (item.Type == ItemType.UpgradeComponent)
			{
				ItemUpgradeComponent upgrade = (ItemUpgradeComponent)item;
				if (upgrade.Details.InfixUpgrade != null && upgrade.Details.InfixUpgrade!.Buff != null)
				{
					Buff = upgrade.Details.InfixUpgrade!.Buff.Description;
				}
				object obj = GameService.Overlay.UserLocale.Value switch
				{
					Locale.German => new string[2] { "Überlegenes Sigill des", "Überlegenes Sigill der" }, 
					Locale.French => new string[2] { "Cachet d'", "supérieur" }, 
					Locale.Spanish => new string[1] { "Sello superior de" }, 
					_ => new string[2] { "Superior Sigil of the", "Superior Sigil of" }, 
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
}
