using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GuildWars2;
using GuildWars2.Hero;
using GuildWars2.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SL.ChatLinks.Storage.Comparers;
using SL.ChatLinks.Storage.Converters;

namespace SL.ChatLinks.Storage.Models
{
	public sealed class TrinketEntityTypeConfiguration : IEntityTypeConfiguration<Trinket>
	{
		[CompilerGenerated]
		private ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> _003CattributesConverter_003EP;

		[CompilerGenerated]
		private ValueComparer<IDictionary<Extensible<AttributeName>, int>> _003CattributesComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<Buff> _003CbuffComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<IReadOnlyList<InfusionSlot>> _003CinfusionSlotsComparer_003EP;

		public TrinketEntityTypeConfiguration(ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> attributesConverter, ValueComparer<IDictionary<Extensible<AttributeName>, int>> attributesComparer, ValueComparer<Buff> buffComparer, ValueComparer<IReadOnlyList<InfusionSlot>> infusionSlotsComparer)
		{
			_003CattributesConverter_003EP = attributesConverter;
			_003CattributesComparer_003EP = attributesComparer;
			_003CbuffComparer_003EP = buffComparer;
			_003CinfusionSlotsComparer_003EP = infusionSlotsComparer;
			base._002Ector();
		}

		public void Configure(EntityTypeBuilder<Trinket> builder)
		{
			builder.Property((Trinket trinket) => trinket.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((Trinket trinket) => trinket.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((Trinket trinket) => trinket.Attributes).HasColumnName("Attributes").HasConversion(_003CattributesConverter_003EP)
				.Metadata.SetValueComparer(_003CattributesComparer_003EP);
			builder.Property((Trinket trinket) => trinket.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((Trinket trinket) => trinket.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			builder.Property((Trinket trinket) => trinket.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CinfusionSlotsComparer_003EP);
			builder.Property((Trinket trinket) => trinket.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CbuffComparer_003EP);
		}
	}
}
