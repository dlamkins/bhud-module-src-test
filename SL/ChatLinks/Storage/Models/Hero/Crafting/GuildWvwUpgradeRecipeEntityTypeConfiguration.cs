using GuildWars2.Hero.Crafting.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Crafting
{
	public sealed class GuildWvwUpgradeRecipeEntityTypeConfiguration : IEntityTypeConfiguration<GuildWvwUpgradeRecipe>
	{
		public void Configure(EntityTypeBuilder<GuildWvwUpgradeRecipe> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((GuildWvwUpgradeRecipe recipe) => recipe.OutputUpgradeId).HasColumnName("OutputWvwUpgradeId");
		}
	}
}
