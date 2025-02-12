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
	public sealed class ArmorEntityTypeConfiguration : IEntityTypeConfiguration<Armor>
	{
		[CompilerGenerated]
		private ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> _003CattributesConverter_003EP;

		[CompilerGenerated]
		private ValueComparer<IDictionary<Extensible<AttributeName>, int>> _003CattributesComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<Buff> _003CbuffComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<IReadOnlyList<InfusionSlot>> _003CinfusionSlotsComparer_003EP;

		public ArmorEntityTypeConfiguration(ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> attributesConverter, ValueComparer<IDictionary<Extensible<AttributeName>, int>> attributesComparer, ValueComparer<Buff> buffComparer, ValueComparer<IReadOnlyList<InfusionSlot>> infusionSlotsComparer)
		{
			_003CattributesConverter_003EP = attributesConverter;
			_003CattributesComparer_003EP = attributesComparer;
			_003CbuffComparer_003EP = buffComparer;
			_003CinfusionSlotsComparer_003EP = infusionSlotsComparer;
			base._002Ector();
		}

		public void Configure(EntityTypeBuilder<Armor> builder)
		{
			builder.Property((Armor armor) => armor.WeightClass).HasConversion(new ExtensibleEnumConverter<WeightClass>());
			builder.Property((Armor armor) => armor.Defense).HasColumnName("Defense");
			builder.Property((Armor armor) => armor.DefaultSkinId).HasColumnName("DefaultSkinId");
			builder.Property((Armor armor) => armor.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((Armor armor) => armor.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((Armor armor) => armor.Attributes).HasColumnName("Attributes").HasConversion(_003CattributesConverter_003EP)
				.Metadata.SetValueComparer(_003CattributesComparer_003EP);
			builder.Property((Armor armor) => armor.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((Armor armor) => armor.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			builder.Property((Armor armor) => armor.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CinfusionSlotsComparer_003EP);
			builder.Property((Armor armor) => armor.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CbuffComparer_003EP);
		}
	}
}
