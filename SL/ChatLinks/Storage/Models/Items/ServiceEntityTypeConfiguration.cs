using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class ServiceEntityTypeConfiguration : IEntityTypeConfiguration<Service>
	{
		public void Configure(EntityTypeBuilder<Service> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Service service) => service.Effect).HasColumnName("Effect").HasJsonValueConversion();
			builder.Property((Service service) => service.GuildUpgradeId).HasColumnName("GuildUpgradeId");
		}
	}
}
