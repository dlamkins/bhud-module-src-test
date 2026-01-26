using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class RingEntityTypeConfiguration : IEntityTypeConfiguration<Ring>
	{
		public void Configure(EntityTypeBuilder<Ring> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Ring ring) => ring.UpgradesFrom).HasColumnName("UpgradesFrom").HasJsonValueConversion();
			builder.Property((Ring ring) => ring.UpgradesInto).HasColumnName("UpgradesInto").HasJsonValueConversion();
		}
	}
}
