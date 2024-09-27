using System;
using Manlaan.Mounts.Things;

namespace Manlaan.Mounts
{
	public class RangedThingUpdatedEvent : EventArgs
	{
		public Thing NewThing { get; set; }

		public RangedThingUpdatedEvent(Thing newThing)
		{
			NewThing = newThing;
		}
	}
}
