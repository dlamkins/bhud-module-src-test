using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class UtilityEntityTypeConfiguration : IEntityTypeConfiguration<Utility>
	{
		public void Configure(EntityTypeBuilder<Utility> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Utility utility) => utility.Effect).HasColumnName("Effect").HasJsonValueConversion();
		}
	}
}
