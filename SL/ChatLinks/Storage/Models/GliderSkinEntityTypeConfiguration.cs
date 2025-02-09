using GuildWars2.Hero.Equipment.Gliders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class GliderSkinEntityTypeConfiguration : IEntityTypeConfiguration<GliderSkin>
	{
		public void Configure(EntityTypeBuilder<GliderSkin> builder)
		{
			builder.ToTable("Gliders");
			builder.HasKey((GliderSkin glider) => glider.Id);
			builder.HasIndex((GliderSkin glider) => glider.Name);
			builder.HasIndex((GliderSkin glider) => glider.Order);
			builder.Property((GliderSkin finisher) => finisher.UnlockItemIds).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<int>());
			builder.Property((GliderSkin finisher) => finisher.DefaultDyeColorIds).HasJsonValueConversion().Metadata.SetValueComparer(new ListComparer<int>());
		}
	}
}
