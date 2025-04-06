using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace SL.ChatLinks.UI.Tabs.Items
{
	[System.Runtime.CompilerServices.RequiredMember]
	public sealed record ItemCategoryMenuItem
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ItemCategoryMenuItem);
			}
		}

		public string? Id { get; init; }

		[System.Runtime.CompilerServices.RequiredMember]
		public string Label { get; init; }

		public Collection<ItemCategoryMenuItem> Subcategories { get; init; }

		public bool CanSelect
		{
			get
			{
				if (Id != null)
				{
					return Subcategories.Count == 0;
				}
				return false;
			}
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ItemCategoryMenuItem");
			stringBuilder.Append(" { ");
			if (PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Id = ");
			builder.Append((object)Id);
			builder.Append(", Label = ");
			builder.Append((object)Label);
			builder.Append(", Subcategories = ");
			builder.Append(Subcategories);
			builder.Append(", CanSelect = ");
			builder.Append(CanSelect.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Label)) * -1521134295 + EqualityComparer<Collection<ItemCategoryMenuItem>>.Default.GetHashCode(Subcategories);
		}

		[CompilerGenerated]
		public bool Equals(ItemCategoryMenuItem? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<string>.Default.Equals(Id, other!.Id) && EqualityComparer<string>.Default.Equals(Label, other!.Label))
				{
					return EqualityComparer<Collection<ItemCategoryMenuItem>>.Default.Equals(Subcategories, other!.Subcategories);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
		private ItemCategoryMenuItem(ItemCategoryMenuItem original)
		{
			Id = original.Id;
			Label = original.Label;
			Subcategories = original.Subcategories;
		}

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[System.Runtime.CompilerServices.CompilerFeatureRequired("RequiredMembers")]
		public ItemCategoryMenuItem()
		{
			Subcategories = new Collection<ItemCategoryMenuItem>();
			base._002Ector();
		}
	}
}
