using GuildWars2.Hero.Equipment.MailCarriers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.MailCarriers
{
	public sealed class MailCarrierEntityTypeConfiguration : IEntityTypeConfiguration<MailCarrier>
	{
		public void Configure(EntityTypeBuilder<MailCarrier> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("MailCarriers");
			builder.HasKey((MailCarrier mailCarrier) => mailCarrier.Id);
			builder.HasIndex((MailCarrier mailCarrier) => mailCarrier.Name);
			builder.HasIndex((MailCarrier mailCarrier) => mailCarrier.Order);
			builder.Property((MailCarrier mailCarrier) => mailCarrier.UnlockItemIds).HasJsonValueConversion();
			builder.Property((MailCarrier mailCarrier) => mailCarrier.Flags).HasJsonValueConversion();
		}
	}
}
