using GuildWars2.Hero.Equipment.Miniatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class MiniatureEntityTypeConfiguration : IEntityTypeConfiguration<Miniature>
	{
		public void Configure(EntityTypeBuilder<Miniature> builder)
		{
			builder.ToTable("Miniatures");
			builder.HasKey((Miniature mini) => mini.Id);
			builder.HasIndex((Miniature mini) => mini.Name);
			builder.HasIndex((Miniature mini) => mini.ItemId);
		}
	}
}
