using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.ChatLinks.Storage.Converters;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class RecipeSheetEntityTypeConfiguration : IEntityTypeConfiguration<RecipeSheet>
	{
		public void Configure(EntityTypeBuilder<RecipeSheet> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((RecipeSheet recipeSheet) => recipeSheet.ExtraRecipeIds).HasJsonValueConversion();
		}
	}
}
