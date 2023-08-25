using System;
using Blish_HUD;
using felix.BlishEmotes.Strings;

namespace felix.BlishEmotes
{
	public static class CategoryExtensions
	{
		private static readonly Logger Logger = Logger.GetLogger<Emote>();

		public static string Label(this Category category)
		{
			switch (category)
			{
			case Category.Greeting:
				return Common.emote_categoryGreeting;
			case Category.Reaction:
				return Common.emote_categoryReaction;
			case Category.Fun:
				return Common.emote_categoryFun;
			case Category.Pose:
				return Common.emote_categoryPose;
			case Category.Dance:
				return Common.emote_categoryDance;
			case Category.Miscellaneous:
				return Common.emote_categoryMiscellaneous;
			default:
				Logger.Fatal("Missing category handling - Tried to retrieve label for " + category);
				throw new Exception("Missing category handling");
			}
		}
	}
}
