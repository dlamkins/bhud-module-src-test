using GuildWars2.Hero.Equipment.Gliders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Gliders
{
	public sealed class GliderSkinEntityTypeConfiguration : IEntityTypeConfiguration<GliderSkin>
	{
		public void Configure(EntityTypeBuilder<GliderSkin> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("Gliders");
			builder.HasKey((GliderSkin glider) => glider.Id);
			builder.HasIndex((GliderSkin glider) => glider.Name);
			builder.HasIndex((GliderSkin glider) => glider.Order);
			builder.Property((GliderSkin glider) => glider.UnlockItemIds).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<int>());
			builder.Property((GliderSkin glider) => glider.DefaultDyeColorIds).HasJsonValueConversion().Metadata.SetValueComparer(new ListComparer<int>());
			builder.Ignore((GliderSkin glider) => glider.IconHref);
		}
	}
}
