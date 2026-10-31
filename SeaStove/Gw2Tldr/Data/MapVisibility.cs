namespace SeaStove.Gw2Tldr.Data
{
	internal static class MapVisibility
	{
		public static VisibilityAction Decide(bool hasGuide, bool hideUnmapped, bool showMapped, bool visible)
		{
			if (!hasGuide && hideUnmapped && visible)
			{
				return VisibilityAction.Hide;
			}
			if (hasGuide && showMapped && !visible)
			{
				return VisibilityAction.Show;
			}
			return VisibilityAction.None;
		}
	}
}
