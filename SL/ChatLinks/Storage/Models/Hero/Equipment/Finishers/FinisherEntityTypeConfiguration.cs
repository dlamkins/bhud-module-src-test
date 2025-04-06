using GuildWars2.Hero.Equipment.Finishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Finishers
{
	public sealed class FinisherEntityTypeConfiguration : IEntityTypeConfiguration<Finisher>
	{
		public void Configure(EntityTypeBuilder<Finisher> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("Finishers");
			builder.HasKey((Finisher finisher) => finisher.Id);
			builder.HasIndex((Finisher finisher) => finisher.Name);
			builder.HasIndex((Finisher finisher) => finisher.Order);
			builder.Property((Finisher finisher) => finisher.UnlockItemIds).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<int>());
		}
	}
}
