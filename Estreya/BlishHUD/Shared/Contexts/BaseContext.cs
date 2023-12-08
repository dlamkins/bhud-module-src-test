using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Contexts;

namespace Estreya.BlishHUD.Shared.Contexts
{
	public abstract class BaseContext : Context
	{
		protected Logger Logger { get; private set; }

		public BaseContext()
			: this()
		{
			Logger = Logger.GetLogger(((object)this).GetType());
		}

		protected override void Load()
		{
			((Context)this).ConfirmReady();
		}

		protected void CheckReady()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			if ((int)((Context)this).get_State() == 3)
			{
				throw new InvalidOperationException("Context has expired.");
			}
			if ((int)((Context)this).get_State() != 2)
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
