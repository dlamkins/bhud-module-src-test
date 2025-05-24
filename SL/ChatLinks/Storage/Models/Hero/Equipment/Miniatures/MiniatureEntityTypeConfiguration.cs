using GuildWars2.Hero.Equipment.Miniatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Miniatures
{
	public sealed class MiniatureEntityTypeConfiguration : IEntityTypeConfiguration<Miniature>
	{
		public void Configure(EntityTypeBuilder<Miniature> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("Miniatures");
			builder.HasKey((Miniature mini) => mini.Id);
			builder.HasIndex((Miniature mini) => mini.Name);
			builder.HasIndex((Miniature mini) => mini.ItemId);
			builder.Ignore((Miniature mini) => mini.IconHref);
		}
	}
}
