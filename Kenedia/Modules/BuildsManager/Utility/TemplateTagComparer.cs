using System.Collections.Generic;
using System.Linq;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;

namespace Kenedia.Modules.BuildsManager.Utility
{
	public class TemplateTagComparer : IComparer<TemplateTag>
	{
		public TagGroups TagGroups { get; }

		public TemplateTagComparer(TagGroups tagGroups)
		{
			TagGroups = tagGroups;
		}

		public static int CompareGroups(TagGroup? a, TagGroup? b)
		{
			if (a != null || b != null)
			{
				if (a != null)
				{
					if (b != null)
					{
						if (a!.Priority.CompareTo(b!.Priority) != 0)
						{
							return a!.Priority.CompareTo(b!.Priority);
						}
						return a!.Name.CompareTo(b!.Name);
					}
					return -1;
				}
				return 1;
			}
			return 0;
		}

		public static int CompareTags(TemplateTag? a, TemplateTag? b)
		{
			if (a != null || b != null)
			{
				if (a != null)
				{
					if (b != null)
					{
						if (a!.Priority.CompareTo(b!.Priority) != 0)
						{
							return a!.Priority.CompareTo(b!.Priority);
						}
						return a!.Name.CompareTo(b!.Name);
					}
					return -1;
				}
				return 1;
			}
			return 0;
		}

		public int Compare(TemplateTag x, TemplateTag y)
		{
			TemplateTag x2 = x;
			TemplateTag y2 = y;
			TagGroup xGroup = TagGroups.FirstOrDefault((TagGroup group) => group.Name == x2.Group);
			TagGroup yGroup = TagGroups.FirstOrDefault((TagGroup group) => group.Name == y2.Group);
			int grp = ((xGroup != null && yGroup != null) ? CompareGroups(xGroup, yGroup) : 0);
			int tag = CompareTags(x2, y2);
			if (grp != 0)
			{
				return grp;
			}
			return tag;
		}
	}
}
