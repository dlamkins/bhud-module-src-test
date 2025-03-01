using System;
using System.Runtime.CompilerServices;
using GuildWars2.Items;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	[System.Runtime.CompilerServices.RequiredMember]
	public sealed class UpgradeSelectedEventArgs : EventArgs
	{
		[System.Runtime.CompilerServices.RequiredMember]
		public UpgradeComponent Selected { get; init; }

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[System.Runtime.CompilerServices.CompilerFeatureRequired("RequiredMembers")]
		public UpgradeSelectedEventArgs()
		{
		}
	}
}
