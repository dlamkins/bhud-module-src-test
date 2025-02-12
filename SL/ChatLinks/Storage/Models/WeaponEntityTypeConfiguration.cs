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
	public sealed class WeaponEntityTypeConfiguration : IEntityTypeConfiguration<Weapon>
	{
		[CompilerGenerated]
		private ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> _003CattributesConverter_003EP;

		[CompilerGenerated]
		private ValueComparer<IDictionary<Extensible<AttributeName>, int>> _003CattributesComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<Buff> _003CbuffComparer_003EP;

		[CompilerGenerated]
		private ValueComparer<IReadOnlyList<InfusionSlot>> _003CinfusionSlotsComparer_003EP;

		public WeaponEntityTypeConfiguration(ValueConverter<IDictionary<Extensible<AttributeName>, int>, string> attributesConverter, ValueComparer<IDictionary<Extensible<AttributeName>, int>> attributesComparer, ValueComparer<Buff> buffComparer, ValueComparer<IReadOnlyList<InfusionSlot>> infusionSlotsComparer)
		{
			_003CattributesConverter_003EP = attributesConverter;
			_003CattributesComparer_003EP = attributesComparer;
			_003CbuffComparer_003EP = buffComparer;
			_003CinfusionSlotsComparer_003EP = infusionSlotsComparer;
			base._002Ector();
		}

		public void Configure(EntityTypeBuilder<Weapon> builder)
		{
			builder.Property((Weapon weapon) => weapon.DamageType).HasConversion(new ExtensibleEnumConverter<DamageType>());
			builder.Property((Weapon weapon) => weapon.Defense).HasColumnName("Defense");
			builder.Property((Weapon weapon) => weapon.DefaultSkinId).HasColumnName("DefaultSkinId");
			builder.Property((Weapon weapon) => weapon.SuffixItemId).HasColumnName("SuffixItemId");
			builder.Property((Weapon weapon) => weapon.SecondarySuffixItemId).HasColumnName("SecondarySuffixItemId");
			builder.Property((Weapon weapon) => weapon.AttributeCombinationId).HasColumnName("AttributeCombinationId");
			builder.Property((Weapon weapon) => weapon.Attributes).HasColumnName("Attributes").HasConversion(_003CattributesConverter_003EP)
				.Metadata.SetValueComparer(_003CattributesComparer_003EP);
			builder.Property((Weapon weapon) => weapon.AttributeAdjustment).HasColumnName("AttributeAdjustment");
			builder.Property((Weapon weapon) => weapon.StatChoices).HasColumnName("StatChoices").HasJsonValueConversion()
				.Metadata.SetValueComparer(new ListComparer<int>());
			builder.Property((Weapon weapon) => weapon.InfusionSlots).HasColumnName("InfusionSlots").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CinfusionSlotsComparer_003EP);
			builder.Property((Weapon weapon) => weapon.Buff).HasColumnName("Buff").HasJsonValueConversion()
				.Metadata.SetValueComparer(_003CbuffComparer_003EP);
		}
	}
}
