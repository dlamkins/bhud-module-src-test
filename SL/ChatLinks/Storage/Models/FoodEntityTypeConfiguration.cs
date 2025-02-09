using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class FoodEntityTypeConfiguration : IEntityTypeConfiguration<Food>
	{
		public void Configure(EntityTypeBuilder<Food> builder)
		{
			builder.Property((Food food) => food.Effect).HasColumnName("Effect").HasJsonValueConversion();
		}
	}
}
