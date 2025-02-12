using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class RecipeSheetEntityTypeConfiguration : IEntityTypeConfiguration<RecipeSheet>
	{
		public void Configure(EntityTypeBuilder<RecipeSheet> builder)
		{
			builder.Property((RecipeSheet recipeSheet) => recipeSheet.ExtraRecipeIds).HasJsonValueConversion().Metadata.SetValueComparer(new CollectionComparer<int>());
		}
	}
}
