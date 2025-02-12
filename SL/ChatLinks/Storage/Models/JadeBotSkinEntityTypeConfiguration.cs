using GuildWars2.Hero.Equipment.JadeBots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class JadeBotSkinEntityTypeConfiguration : IEntityTypeConfiguration<JadeBotSkin>
	{
		public void Configure(EntityTypeBuilder<JadeBotSkin> builder)
		{
			builder.ToTable("JadeBots");
			builder.HasKey((JadeBotSkin jadeBot) => jadeBot.Id);
			builder.HasIndex((JadeBotSkin jadeBot) => jadeBot.Name);
			builder.HasIndex((JadeBotSkin jadeBot) => jadeBot.UnlockItemId);
		}
	}
}
