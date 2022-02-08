using System;
using System.Collections.Generic;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Modules;
using Sentry;

namespace BhModule.Community.ErrorSubmissionModule
{
	public static class DsnAssocUtil
	{
		private static bool _stackFramesFailed;

		private static void SetReleaseFromId(SentryEvent sentryEvent, string id)
		{
			if (!((GameService)GameService.Module).get_Loaded())
			{
				return;
			}
			foreach (ModuleManager module in GameService.Module.get_Modules())
			{
				if (string.Equals(module.get_Manifest().get_Namespace(), id, StringComparison.InvariantCultureIgnoreCase))
				{
					sentryEvent.Release = module.get_Manifest().get_Version().ToString();
					break;
				}
			}
		}

		public static string GetDsnFromEvent(SentryEvent sentryEvent, EtmConfig config)
		{
			List<string> stackNamespaces = new List<string>();
			if (!_stackFramesFailed && sentryEvent.Exception != null)
			{
				try
				{
					StackTrace stacktrace = new StackTrace(sentryEvent.Exception);
					if (stacktrace.FrameCount > 0)
					{
						StackFrame[] frames = stacktrace.GetFrames();
						foreach (StackFrame frame in frames)
						{
							stackNamespaces.Add(frame.GetMethod().DeclaringType?.Namespace);
						}
					}
				}
				catch (Exception)
				{
					_stackFramesFailed = true;
				}
			}
			foreach (ModuleDetails moduleDetails in config.Modules)
			{
				foreach (string moduleNamespace in moduleDetails.ModuleNamespaces)
				{
					if (sentryEvent.Logger != null && sentryEvent.Logger!.StartsWith(moduleNamespace, StringComparison.InvariantCultureIgnoreCase))
					{
						SetReleaseFromId(sentryEvent, moduleDetails.Id);
						return moduleDetails.Dsn;
					}
					foreach (string item in stackNamespaces)
					{
						if (item.StartsWith(moduleNamespace, StringComparison.InvariantCultureIgnoreCase))
						{
							SetReleaseFromId(sentryEvent, moduleDetails.Id);
							return moduleDetails.Dsn;
						}
					}
				}
			}
			return config.BaseDsn;
		}
	}
}
