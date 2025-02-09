using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class BackItemEntityTypeConfiguration : IEntityTypeConfiguration<Backpack>
	{
		[CompilerGenerated]
		private ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> _003CattributesConverter_003EP;

		[CompilerGenerated]
		private ValueComparer<IDictionary<Extensible<AttributeName>, int>> _003CattributesComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<Buff> _003CbuffComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<IReadOnlyList<InfusionSlot>> _003CinfusionSlotsComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<IReadOnlyCollection<InfusionSlotUpgradeSource>> _003CupgradeSourceComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<IReadOnlyCollection<InfusionSlotUpgradePath>> _003CupgradePathComparer_003EP;

		public BackItemEntityTypeConfiguration(ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> attributesConverter, ValueComparer<IDictionary<Extensible<AttributeName>, int>> attributesComparer, ValueComparer<Buff> buffComparer, ValueComparer<IReadOnlyList<InfusionSlot>> infusionSlotsComparer, ValueComparer<IReadOnlyCollection<InfusionSlotUpgradeSource>> upgradeSourceComparer, ValueComparer<IReadOnlyCollection<InfusionSlotUpgradePath>> upgradePathComparer)
		{
			_003CattributesConverter_003EP = attributesConverter;
			_003CattributesComparer_003EP = attributesComparer;
			_003CbuffComparer_003EP = buffComparer;
			_003CinfusionSlotsComparer_003EP = infusionSlotsComparer;
			_003CupgradeSourceComparer_003EP = upgradeSourceComparer;
			_003CupgradePathComparer_003EP = upgradePathComparer;
			base._002Ector();
		}

		public void Configure(EntityTypeBuilder<Backpack> builder)
		{
			builder.Property((Backpack backpack) => backpack.DefaultSkinId).HasColumnName("DefaultSkinId");
			builder.Property((Backpack backpack) => backpack.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((Backpack backpack) => backpack.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((Backpack backpack) => backpack.Attributes).HasColumnName("Attributes").HasConversion(_003CattributesConverter_003EP)
				.Metadata.SetValueComparer(_003CattributesComparer_003EP);
			builder.Property((Backpack backpack) => backpack.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((Backpack backpack) => backpack.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			builder.Property((Backpack backpack) => backpack.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CinfusionSlotsComparer_003EP);
			builder.Property((Backpack backpack) => backpack.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CbuffComparer_003EP);
			builder.Property((Backpack backpack) => backpack.UpgradesFrom).HasColumnName("UpgradesFrom").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CupgradeSourceComparer_003EP);
			builder.Property((Backpack backpack) => backpack.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CupgradePathComparer_003EP);
		}
	}
}
