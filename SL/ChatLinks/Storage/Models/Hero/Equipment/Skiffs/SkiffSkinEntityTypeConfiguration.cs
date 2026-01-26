using GuildWars2.Hero.Equipment.Skiffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Skiffs
{
	public sealed class SkiffSkinEntityTypeConfiguration : IEntityTypeConfiguration<SkiffSkin>
	{
		public void Configure(EntityTypeBuilder<SkiffSkin> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("SkiffSkins");
			builder.HasKey((SkiffSkin skiffSkin) => skiffSkin.Id);
			builder.HasIndex((SkiffSkin skiffSkin) => skiffSkin.Name);
			builder.Property((SkiffSkin skiffSkin) => skiffSkin.DyeSlots).HasJsonValueConversion();
		}
	}
}
