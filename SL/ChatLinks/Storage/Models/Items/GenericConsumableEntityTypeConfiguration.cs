using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class GenericConsumableEntityTypeConfiguration : IEntityTypeConfiguration<GenericConsumable>
	{
		public void Configure(EntityTypeBuilder<GenericConsumable> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((GenericConsumable genericConsumable) => genericConsumable.Effect).HasColumnName("Effect").HasJsonValueConversion();
			builder.Property((GenericConsumable genericConsumable) => genericConsumable.GuildUpgradeId).HasColumnName("GuildUpgradeId");
		}
	}
}
