using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class RuneEntityTypeConfiguration : IEntityTypeConfiguration<Rune>
	{
		public void Configure(EntityTypeBuilder<Rune> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Rune rune) => rune.Bonuses).HasJsonValueConversion().Metadata.SetValueComparer(new ListComparer<string>());
		}
	}
}
