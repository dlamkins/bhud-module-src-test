using GuildWars2.Hero.Equipment.Outfits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Outfits
{
	public sealed class OutfitEntityTypeConfiguration : IEntityTypeConfiguration<Outfit>
	{
		public void Configure(EntityTypeBuilder<Outfit> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("Outfits");
			builder.HasKey((Outfit outfit) => outfit.Id);
			builder.HasIndex((Outfit outfit) => outfit.Name);
			builder.Property((Outfit outfit) => outfit.UnlockItemIds).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<int>());
		}
	}
}
