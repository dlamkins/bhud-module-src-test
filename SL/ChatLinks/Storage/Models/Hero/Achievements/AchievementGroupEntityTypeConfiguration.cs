using GuildWars2.Hero.Achievements.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Achievements
{
	public sealed class AchievementGroupEntityTypeConfiguration : IEntityTypeConfiguration<AchievementGroup>
	{
		public void Configure(EntityTypeBuilder<AchievementGroup> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((AchievementGroup achievementGroup) => achievementGroup.Id).ValueGeneratedNever();
			builder.Property((AchievementGroup achievementGroup) => achievementGroup.Categories).HasJsonValueConversion();
			builder.HasIndex((AchievementGroup achievementGroup) => achievementGroup.Name);
			builder.HasIndex((AchievementGroup achievementGroup) => achievementGroup.Order);
		}
	}
}
