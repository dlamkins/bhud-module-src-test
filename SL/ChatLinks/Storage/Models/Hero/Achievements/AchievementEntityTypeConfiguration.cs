using GuildWars2.Hero.Achievements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Achievements
{
	public sealed class AchievementEntityTypeConfiguration : IEntityTypeConfiguration<Achievement>
	{
		public void Configure(EntityTypeBuilder<Achievement> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Achievement achievement) => achievement.Id).ValueGeneratedNever();
			builder.Property((Achievement achievement) => achievement.Flags).HasJsonValueConversion();
			builder.Property((Achievement achievement) => achievement.Tiers).HasJsonValueConversion().Metadata.SetValueComparer(new ListComparer<AchievementTier>());
			builder.Property((Achievement achievement) => achievement.Rewards).HasJsonValueConversion().Metadata.SetValueComparer(new ListComparer<AchievementReward>());
			builder.Property((Achievement achievement) => achievement.Bits).HasJsonValueConversion().Metadata.SetValueComparer(new ListComparer<AchievementBit>());
			builder.Property((Achievement achievement) => achievement.Prerequisites).HasJsonValueConversion().Metadata.SetValueComparer(new ListComparer<int>());
			builder.HasIndex((Achievement achievement) => achievement.Name);
			builder.HasDiscriminator<string>("Type").HasValue<Achievement>("achievement").HasValue<CollectionAchievement>("collection_achievement");
		}
	}
}
