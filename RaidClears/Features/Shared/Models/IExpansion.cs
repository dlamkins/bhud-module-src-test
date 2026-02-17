using System.Collections.Generic;

namespace RaidClears.Features.Shared.Models
{
	public interface IExpansion<TChild> where TChild : EncounterInterface
	{
		string Id { get; }

		string Name { get; }

		string Abbriviation { get; }

		string Asset { get; }

		IReadOnlyList<TChild> Children { get; }
	}
}
