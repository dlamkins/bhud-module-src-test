using System;

namespace FarmingTracker
{
	public class ExceptionService
	{
		public static string GetExceptionSummary(Exception e)
		{
			string summary = GetSingleExceptionSummary(e);
			for (Exception innerException = e.InnerException; innerException != null; innerException = innerException.InnerException)
			{
				summary = summary + " -> " + GetSingleExceptionSummary(innerException);
			}
			return summary;
		}

		private static string GetSingleExceptionSummary(Exception e)
		{
			return e.GetType().Name + ": " + e.Message;
		}
	}
}
