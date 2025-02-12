using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class ServiceEntityTypeConfiguration : IEntityTypeConfiguration<Service>
	{
		public void Configure(EntityTypeBuilder<Service> builder)
		{
			builder.Property((Service service) => service.Effect).HasColumnName("Effect").HasJsonValueConversion();
			builder.Property((Service service) => service.GuildUpgradeId).HasColumnName("GuildUpgradeId");
		}
	}
}
