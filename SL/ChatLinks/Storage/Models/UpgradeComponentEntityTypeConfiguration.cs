using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public class UpgradeComponentEntityTypeConfiguration : IEntityTypeConfiguration<UpgradeComponent>
	{
		[CompilerGenerated]
		private ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> _003CattributesConverter_003EP;

		[CompilerGenerated]
		private ValueComparer<IDictionary<Extensible<AttributeName>, int>> _003CattributesComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<Buff> _003CbuffComparer_003EP;

		public UpgradeComponentEntityTypeConfiguration(ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> attributesConverter, ValueComparer<IDictionary<Extensible<AttributeName>, int>> attributesComparer, ValueComparer<Buff> buffComparer)
		{
			_003CattributesConverter_003EP = attributesConverter;
			_003CattributesComparer_003EP = attributesComparer;
			_003CbuffComparer_003EP = buffComparer;
			base._002Ector();
		}

		public void Configure(EntityTypeBuilder<UpgradeComponent> builder)
		{
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.Attributes).HasColumnName("Attributes").HasConversion(_003CattributesConverter_003EP)
				.Metadata.SetValueComparer(_003CattributesComparer_003EP);
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.UpgradeComponentFlags).HasConversion(new JsonValueConverter<UpgradeComponentFlags>());
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.InfusionUpgradeFlags).HasConversion(new JsonValueConverter<InfusionSlotFlags>());
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CbuffComparer_003EP);
		}
	}
}
