using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Contexts;

namespace Kenedia.Modules.Core.Contexts
{
	public abstract class BaseContext : Context
	{
		protected Logger Logger { get; private set; }

		public BaseContext()
		{
			Logger = Blish_HUD.Logger.GetLogger(GetType());
		}

		protected override void Load()
		{
			ConfirmReady();
		}

		protected void CheckReady()
		{
			if (base.State == ContextState.Expired)
			{
				throw new InvalidOperationException("Context has expired.");
			}
			if (base.State != ContextState.Ready)
			{
				throw new InvalidOperationException("Context is not ready.");
			}
		}

		protected Type GetCaller()
		{
			bool lastFrameWasBaseType = false;
			Type type = null;
			StackFrame[] frames = new StackTrace(fNeedFileInfo: false).GetFrames();
			for (int i = 0; i < frames.Length; i++)
			{
				Type methodType = frames[i].GetMethod().DeclaringType;
				bool currentFrameIsBaseType = methodType.BaseType == typeof(BaseContext);
				if (lastFrameWasBaseType && !currentFrameIsBaseType)
				{
					type = methodType;
				}
				lastFrameWasBaseType = currentFrameIsBaseType;
			}
			return type.DeclaringType ?? type;
		}
	}
}
