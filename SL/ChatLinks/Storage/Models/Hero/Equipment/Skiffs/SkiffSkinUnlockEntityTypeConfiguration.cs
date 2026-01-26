using GuildWars2.Hero.Equipment.Skiffs;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Skiffs
{
	public sealed class SkiffSkinUnlockEntityTypeConfiguration : IEntityTypeConfiguration<SkiffSkinUnlock>
	{
		public void Configure(EntityTypeBuilder<SkiffSkinUnlock> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.ToTable("SkiffSkinUnlocks");
			builder.HasKey((SkiffSkinUnlock skiffSkinUnlock) => new { skiffSkinUnlock.SkiffSkinId, skiffSkinUnlock.ItemId });
			builder.HasOne<SkiffSkin>().WithMany().HasForeignKey((SkiffSkinUnlock skiffSkinUnlock) => skiffSkinUnlock.SkiffSkinId);
			builder.HasOne<Item>().WithMany().HasForeignKey((SkiffSkinUnlock skiffSkinUnlock) => skiffSkinUnlock.ItemId);
		}
	}
}
