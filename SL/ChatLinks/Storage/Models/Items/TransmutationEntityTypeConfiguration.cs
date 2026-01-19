using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class TransmutationEntityTypeConfiguration : IEntityTypeConfiguration<Transmutation>
	{
		public void Configure(EntityTypeBuilder<Transmutation> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Transmutation transmutation) => transmutation.SkinIds).HasJsonValueConversion();
		}
	}
}
