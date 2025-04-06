using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.Common;

namespace SL.ChatLinks.Storage.Models.Items
{
	public sealed class GizmoEntityTypeConfiguration : IEntityTypeConfiguration<Gizmo>
	{
		public void Configure(EntityTypeBuilder<Gizmo> builder)
		{
			ThrowHelper.ThrowIfNull(builder, "builder");
			builder.Property((Gizmo gizmo) => gizmo.GuildUpgradeId).HasColumnName("GuildUpgradeId");
		}
	}
}
