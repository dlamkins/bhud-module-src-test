using System;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal class TypeOverrideAttribute : Attribute
	{
		public Type Type { get; }

		public TypeOverrideAttribute(Type type)
		{
			Type = type;
		}
	}
}
