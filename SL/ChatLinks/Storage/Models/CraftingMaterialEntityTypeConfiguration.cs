using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class CraftingMaterialEntityTypeConfiguration : IEntityTypeConfiguration<CraftingMaterial>
	{
		[CompilerGenerated]
		private ValueComparer<IReadOnlyCollection<InfusionSlotUpgradePath>> _003CupgradePathComparer_003EP;

		public CraftingMaterialEntityTypeConfiguration(ValueComparer<IReadOnlyCollection<InfusionSlotUpgradePath>> upgradePathComparer)
		{
			_003CupgradePathComparer_003EP = upgradePathComparer;
			base._002Ector();
		}

		public void Configure(EntityTypeBuilder<CraftingMaterial> builder)
		{
			builder.Property((CraftingMaterial craftingMaterial) => craftingMaterial.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CupgradePathComparer_003EP);
		}
	}
}
