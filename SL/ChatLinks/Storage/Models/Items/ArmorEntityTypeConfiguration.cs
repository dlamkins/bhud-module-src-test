using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class ArmorEntityTypeConfiguration : IEntityTypeConfiguration<Armor>
	{
		public void Configure(EntityTypeBuilder<Armor> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Armor armor) => armor.WeightClass).HasConversion(new ExtensibleEnumConverter<WeightClass>());
			builder.Property((Armor armor) => armor.Defense).HasColumnName("Defense");
			builder.Property((Armor armor) => armor.DefaultSkinId).HasColumnName("DefaultSkinId");
			builder.Property((Armor armor) => armor.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((Armor armor) => armor.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((Armor armor) => armor.Attributes).HasColumnName("Attributes").HasImmutableValueDictionaryConverter((string key) => new Extensible<AttributeName>(key), (Extensible<AttributeName> ext) => ext.ToString());
			builder.Property((Armor armor) => armor.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((Armor armor) => armor.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion();
			builder.Property((Armor armor) => armor.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion();
			builder.Property((Armor armor) => armor.Buff).HasColumnName("Buff").HasJsonValueConversion();
		}
	}
}
