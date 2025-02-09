using System.Drawing;
using GuildWars2.Hero.Equipment.Dyes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class DyeColorEntityTypeConfiguration : IEntityTypeConfiguration<DyeColor>
	{
		public void Configure(EntityTypeBuilder<DyeColor> builder)
		{
			builder.ToTable("Colors");
			builder.HasKey((DyeColor color) => color.Id);
			builder.HasIndex((DyeColor color) => color.Name);
			builder.HasIndex((DyeColor color) => color.ItemId);
			builder.HasIndex((DyeColor color) => color.Hue);
			builder.HasIndex((DyeColor color) => color.Material);
			builder.HasIndex((DyeColor color) => color.Set);
			builder.Property((DyeColor color) => color.BaseRgb).HasConversion((Color color) => color.ToArgb(), (int argb) => Color.FromArgb(argb));
			builder.Property((DyeColor color) => color.Cloth).HasJsonValueConversion();
			builder.Property((DyeColor color) => color.Leather).HasJsonValueConversion();
			builder.Property((DyeColor color) => color.Metal).HasJsonValueConversion();
			builder.Property((DyeColor color) => color.Fur).HasJsonValueConversion();
			builder.Property((DyeColor color) => color.Hue).HasConversion(new ExtensibleEnumConverter<Hue>());
			builder.Property((DyeColor color) => color.Material).HasConversion(new ExtensibleEnumConverter<Material>());
			builder.Property((DyeColor color) => color.Set).HasConversion(new ExtensibleEnumConverter<ColorSet>());
		}
	}
}
