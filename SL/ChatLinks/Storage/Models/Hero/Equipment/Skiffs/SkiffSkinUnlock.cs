using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace SL.ChatLinks.Storage.Models.Hero.Equipment.Skiffs
{
	[System.Runtime.CompilerServices.RequiredMember]
	public sealed record SkiffSkinUnlock
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(SkiffSkinUnlock);
			}
		}

		[System.Runtime.CompilerServices.RequiredMember]
		public int SkiffSkinId { get; init; }

		[System.Runtime.CompilerServices.RequiredMember]
		public int ItemId { get; init; }

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SkiffSkinUnlock");
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
			builder.Append("SkiffSkinId = ");
			builder.Append(SkiffSkinId.ToString());
			builder.Append(", ItemId = ");
			builder.Append(ItemId.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(SkiffSkinId)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(ItemId);
		}

		[CompilerGenerated]
		public bool Equals(SkiffSkinUnlock? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(SkiffSkinId, other!.SkiffSkinId))
				{
					return EqualityComparer<int>.Default.Equals(ItemId, other!.ItemId);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
		private SkiffSkinUnlock(SkiffSkinUnlock original)
		{
			SkiffSkinId = original.SkiffSkinId;
			ItemId = original.ItemId;
		}

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[System.Runtime.CompilerServices.CompilerFeatureRequired("RequiredMembers")]
		public SkiffSkinUnlock()
		{
		}
	}
}
