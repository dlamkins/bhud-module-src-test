using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class BackItemEntityTypeConfiguration : IEntityTypeConfiguration<BackItem>
	{
		public void Configure(EntityTypeBuilder<BackItem> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((BackItem back) => back.DefaultSkinId).HasColumnName("DefaultSkinId");
			builder.Property((BackItem back) => back.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((BackItem back) => back.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((BackItem back) => back.Attributes).HasColumnName("Attributes").HasImmutableValueDictionaryConverter((string key) => new Extensible<AttributeName>(key), (Extensible<AttributeName> ext) => ext.ToString());
			builder.Property((BackItem back) => back.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((BackItem back) => back.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion();
			builder.Property((BackItem back) => back.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion();
			builder.Property((BackItem back) => back.Buff).HasColumnName("Buff").HasJsonValueConversion();
			builder.Property((BackItem back) => back.UpgradesFrom).HasColumnName("UpgradesFrom").HasJsonValueConversion();
			builder.Property((BackItem back) => back.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion();
		}
	}
}
