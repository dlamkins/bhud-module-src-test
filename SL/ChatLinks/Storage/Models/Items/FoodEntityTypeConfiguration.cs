using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class FoodEntityTypeConfiguration : IEntityTypeConfiguration<Food>
	{
		public void Configure(EntityTypeBuilder<Food> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Food food) => food.Effect).HasColumnName("Effect").HasJsonValueConversion();
		}
	}
}
