namespace Denrage.AchievementTrackerModule
{
	public static class StringUtils
	{
		public static string SanitizeHtml(string input)
		{
			input = RemoveStyleTag(input);
			input = RemoveScriptTag(input);
			for (int currentIndex = 0; currentIndex != -1; currentIndex = 0)
			{
				currentIndex = input.IndexOf('<', currentIndex);
				if (currentIndex == -1)
				{
					break;
				}
				int endIndex = input.IndexOf('>', currentIndex);
				input = input.Remove(currentIndex, endIndex - currentIndex + 1);
			}
			return input;
		}

		private static string RemoveStyleTag(string input)
		{
			for (int currentIndex = 0; currentIndex != -1; currentIndex = 0)
			{
				currentIndex = input.IndexOf("<style", currentIndex);
				if (currentIndex == -1)
				{
					break;
				}
				int endIndex = input.IndexOf("</style>", currentIndex);
				input = input.Remove(currentIndex, endIndex - currentIndex + "</style>".Length);
			}
			return input;
		}

		private static string RemoveScriptTag(string input)
		{
			for (int currentIndex = 0; currentIndex != -1; currentIndex = 0)
			{
				currentIndex = input.IndexOf("<script", currentIndex);
				if (currentIndex == -1)
				{
					break;
				}
				int endIndex = input.IndexOf("</script>", currentIndex);
				input = input.Remove(currentIndex, endIndex - currentIndex + "</script>".Length);
			}
			return input;
		}
	}
}
