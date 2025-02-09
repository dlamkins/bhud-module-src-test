using GuildWars2.Hero.Crafting;
using GuildWars2.Hero.Crafting.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class GuildConsumableRecipeEntityTypeConfiguration : IEntityTypeConfiguration<GuildConsumableRecipe>
	{
		public void Configure(EntityTypeBuilder<GuildConsumableRecipe> builder)
		{
			builder.Property((GuildConsumableRecipe recipe) => recipe.GuildIngredients).HasColumnName("GuildIngredients").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<GuildIngredient>());
			builder.Property((GuildConsumableRecipe recipe) => recipe.OutputUpgradeId).HasColumnName("OutputUpgradeId");
		}
	}
}
