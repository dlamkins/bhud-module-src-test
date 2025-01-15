using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed record MouseLeftUpgradeSelector
	{
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(MouseLeftUpgradeSelector);
			}
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("MouseLeftUpgradeSelector");
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
			return false;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<Type>.Default.GetHashCode(EqualityContract);
		}

		[CompilerGenerated]
		public bool Equals(MouseLeftUpgradeSelector? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null)
				{
					return EqualityContract == other!.EqualityContract;
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		private MouseLeftUpgradeSelector(MouseLeftUpgradeSelector original)
		{
		}

		public MouseLeftUpgradeSelector()
		{
		}
	}
}
