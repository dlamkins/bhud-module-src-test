using GuildWars2.Hero.Equipment.Outfits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class OutfitEntityTypeConfiguration : IEntityTypeConfiguration<Outfit>
	{
		public void Configure(EntityTypeBuilder<Outfit> builder)
		{
			builder.ToTable("Outfits");
			builder.HasKey((Outfit outfit) => outfit.Id);
			builder.HasIndex((Outfit outfit) => outfit.Name);
			builder.Property((Outfit outfit) => outfit.UnlockItemIds).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<int>());
		}
	}
}
