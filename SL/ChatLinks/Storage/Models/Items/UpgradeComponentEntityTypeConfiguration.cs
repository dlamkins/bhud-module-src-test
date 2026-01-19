using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public class UpgradeComponentEntityTypeConfiguration : IEntityTypeConfiguration<UpgradeComponent>
	{
		public void Configure(EntityTypeBuilder<UpgradeComponent> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.Attributes).HasColumnName("Attributes").HasImmutableValueDictionaryConverter((string key) => new Extensible<AttributeName>(key), (Extensible<AttributeName> ext) => ext.ToString());
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.UpgradeComponentFlags).HasConversion(new JsonValueConverter<UpgradeComponentFlags>());
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.InfusionUpgradeFlags).HasConversion(new JsonValueConverter<InfusionSlotFlags>());
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.Buff).HasColumnName("Buff").HasJsonValueConversion();
			builder.Property((UpgradeComponent upgradeComponent) => upgradeComponent.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion();
		}
	}
}
