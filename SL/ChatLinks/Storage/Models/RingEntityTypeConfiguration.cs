using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class RingEntityTypeConfiguration : IEntityTypeConfiguration<Ring>
	{
		[CompilerGenerated]
		private ValueComparer<IReadOnlyCollection<InfusionSlotUpgradeSource>> _003CupgradeSourceComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<IReadOnlyCollection<InfusionSlotUpgradePath>> _003CupgradePathComparer_003EP;

		public RingEntityTypeConfiguration(ValueComparer<IReadOnlyCollection<InfusionSlotUpgradeSource>> upgradeSourceComparer, ValueComparer<IReadOnlyCollection<InfusionSlotUpgradePath>> upgradePathComparer)
		{
			_003CupgradeSourceComparer_003EP = upgradeSourceComparer;
			_003CupgradePathComparer_003EP = upgradePathComparer;
			base._002Ector();
		}

		public void Configure(EntityTypeBuilder<Ring> builder)
		{
			builder.Property((Ring ring) => ring.UpgradesFrom).HasColumnName("UpgradesFrom").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CupgradeSourceComparer_003EP);
			builder.Property((Ring ring) => ring.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CupgradePathComparer_003EP);
		}
	}
}
