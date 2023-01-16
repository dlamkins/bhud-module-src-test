using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class RaidFilter : Behavior<IPathingEntity>, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "raid";

		private readonly IPackState _packState;

		public List<string> Raids { get; set; }

		public RaidFilter(IEnumerable<string> raids, IPathingEntity pathingEntity, IPackState packState)
			: base(pathingEntity)
		{
			Raids = raids.Select((string raid) => raid.ToLowerInvariant()).ToList();
			_packState = packState;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, IPathingEntity pathingEntity, IPackState packState)
		{
			IAttribute idAttr;
			return new RaidFilter(attributes.TryGetAttribute("raid", out idAttr) ? idAttr.GetValueAsStrings() : Enumerable.Empty<string>(), pathingEntity, packState);
		}

		public string FilterReason()
		{
			return "Hidden because you've completed: " + string.Join(", ", Raids) + ".";
		}

		public bool IsFiltered()
		{
			if (Raids.Any())
			{
				return !_packState.RaidStates.AreRaidsComplete(Raids);
			}
			return false;
		}
	}
}
