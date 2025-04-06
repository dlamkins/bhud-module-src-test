using GuildWars2.Hero.Equipment.Novelties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Novelties
{
	public sealed class NoveltyEntityTypeConfiguration : IEntityTypeConfiguration<Novelty>
	{
		public void Configure(EntityTypeBuilder<Novelty> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("Novelties");
			builder.HasKey((Novelty novelty) => novelty.Id);
			builder.HasIndex((Novelty novelty) => novelty.Name);
			builder.Property((Novelty novelty) => novelty.Slot).HasConversion(new ExtensibleEnumConverter<NoveltyKind>());
			builder.Property((Novelty novelty) => novelty.UnlockItemIds).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<int>());
		}
	}
}
