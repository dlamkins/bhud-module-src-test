using System;
using System.Collections.Generic;
using System.Linq;
using Ideka.BHUDCommon.AnchoredRect;

namespace Ideka.CustomCombatText
{
	internal static class AreaViewExtensions
	{
		public static AreaView? GetParent(this AreaView areaView)
		{
			return areaView.Parent as AreaView;
		}

		public static IEnumerable<AreaView> GetAreaViewChildren(this AnchoredRect rect)
		{
			return from x in rect.Children
				select x as AreaView into x
				where x != null
				select x;
		}

		public static IEnumerable<AreaView> GetChildren(this AreaView areaView)
		{
			return areaView.GetAreaViewChildren();
		}

		public static IEnumerable<AreaView> GetAncestors(this AreaView areaView)
		{
			for (AreaView view = areaView; view != null; view = view.GetParent())
			{
				yield return view;
			}
		}

		public static IEnumerable<AreaView> GetSiblings(this AreaView areaView)
		{
			AnchoredRect parent = areaView.Parent;
			if (parent == null)
			{
				return Array.Empty<AreaView>();
			}
			return parent.GetAreaViewChildren();
		}
	}
}
