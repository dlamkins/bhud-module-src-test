using Estreya.BlishHUD.Shared.Attributes;

namespace Estreya.BlishHUD.EventTable.Models
{
	public enum MenuEventSortMode
	{
		[Translation("menuEventSortMode-default", "Default")]
		Default,
		[Translation("menuEventSortMode-alphabetical", "Alphabetical (A-Z)")]
		Alphabetical,
		[Translation("menuEventSortMode-alphabeticalDesc", "Alphabetical (Z-A)")]
		AlphabeticalDesc
	}
}
