using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Fractals.Models
{
	public class Fractal : Wing
	{
		public Fractal(string name, int index, string shortName, IEnumerable<BoxModel> boxes)
			: base(name, index, shortName, boxes)
		{
		}

		public virtual void Dispose()
		{
		}
	}
}
