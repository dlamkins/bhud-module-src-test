using GuildWars2.Hero.Crafting.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Hero.Crafting
{
	public sealed class GuildConsumableRecipeEntityTypeConfiguration : IEntityTypeConfiguration<GuildConsumableRecipe>
	{
		public void Configure(EntityTypeBuilder<GuildConsumableRecipe> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((GuildConsumableRecipe recipe) => recipe.GuildIngredients).HasColumnName("GuildIngredients").HasJsonValueConversion();
			builder.Property((GuildConsumableRecipe recipe) => recipe.OutputUpgradeId).HasColumnName("OutputUpgradeId");
		}
	}
}
