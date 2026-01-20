using GuildWars2.Hero.Achievements.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Achievements
{
	public sealed class AchievementCategoryEntityTypeConfiguration : IEntityTypeConfiguration<AchievementCategory>
	{
		public void Configure(EntityTypeBuilder<AchievementCategory> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((AchievementCategory achievementCategory) => achievementCategory.Id).ValueGeneratedNever();
			builder.Property((AchievementCategory achievementCategory) => achievementCategory.Achievements).HasJsonValueConversion();
			builder.Property((AchievementCategory achievementCategory) => achievementCategory.Tomorrow).HasJsonValueConversion();
			builder.HasIndex((AchievementCategory achievementCategory) => achievementCategory.Name);
			builder.HasIndex((AchievementCategory achievementCategory) => achievementCategory.Order);
		}
	}
}
