using GuildWars2.Hero.Equipment.Wardrobe;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Wardrobe
{
	public sealed class WeaponSkinEntityTypeConfiguration : IEntityTypeConfiguration<WeaponSkin>
	{
		public void Configure(EntityTypeBuilder<WeaponSkin> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((WeaponSkin skin) => skin.DamageType).HasConversion(new ExtensibleEnumConverter<DamageType>());
		}
	}
}
