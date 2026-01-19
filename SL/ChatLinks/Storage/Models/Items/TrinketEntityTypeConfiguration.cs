using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class TrinketEntityTypeConfiguration : IEntityTypeConfiguration<Trinket>
	{
		public void Configure(EntityTypeBuilder<Trinket> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Trinket trinket) => trinket.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((Trinket trinket) => trinket.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((Trinket trinket) => trinket.Attributes).HasColumnName("Attributes").HasImmutableValueDictionaryConverter((string key) => new Extensible<AttributeName>(key), (Extensible<AttributeName> ext) => ext.ToString());
			builder.Property((Trinket trinket) => trinket.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((Trinket trinket) => trinket.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion();
			builder.Property((Trinket trinket) => trinket.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion();
			builder.Property((Trinket trinket) => trinket.Buff).HasColumnName("Buff").HasJsonValueConversion();
		}
	}
}
