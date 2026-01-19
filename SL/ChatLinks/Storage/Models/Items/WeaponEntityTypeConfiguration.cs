using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class WeaponEntityTypeConfiguration : IEntityTypeConfiguration<Weapon>
	{
		public void Configure(EntityTypeBuilder<Weapon> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Weapon weapon) => weapon.DamageType).HasConversion(new ExtensibleEnumConverter<DamageType>());
			builder.Property((Weapon weapon) => weapon.Defense).HasColumnName("Defense");
			builder.Property((Weapon weapon) => weapon.DefaultSkinId).HasColumnName("DefaultSkinId");
			builder.Property((Weapon weapon) => weapon.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((Weapon weapon) => weapon.SecondarySuffixItemId).HasColumnName("SecondarySuffixItemId");
			builder.Property((Weapon weapon) => weapon.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((Weapon weapon) => weapon.Attributes).HasColumnName("Attributes").HasImmutableValueDictionaryConverter((string key) => new Extensible<AttributeName>(key), (Extensible<AttributeName> ext) => ext.ToString());
			builder.Property((Weapon weapon) => weapon.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((Weapon weapon) => weapon.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion();
			builder.Property((Weapon weapon) => weapon.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion();
			builder.Property((Weapon weapon) => weapon.Buff).HasColumnName("Buff").HasJsonValueConversion();
		}
	}
}
