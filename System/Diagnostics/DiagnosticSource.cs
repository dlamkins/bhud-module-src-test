using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics
{
	internal abstract class DiagnosticSource
	{
		internal const string WriteRequiresUnreferencedCode = "The type of object being written to DiagnosticSource cannot be discovered statically.";

		internal const string WriteOfTRequiresUnreferencedCode = "Only the properties of the T type will be preserved. Properties of referenced types and properties of derived types may be trimmed.";

		[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresUnreferencedCode("The type of object being written to DiagnosticSource cannot be discovered statically.")]
		public abstract void Write(string name, object? value);

		[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresUnreferencedCode("Only the properties of the T type will be preserved. Properties of referenced types and properties of derived types may be trimmed.")]
		public void Write<[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMembers(_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMemberTypes.PublicProperties)] T>(string name, T value)
		{
			Write(name, (object?)value);
		}

		public abstract bool IsEnabled(string name);

		public virtual bool IsEnabled(string name, object? arg1, object? arg2 = null)
		{
			return IsEnabled(name);
		}

		[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresUnreferencedCode("The type of object being written to DiagnosticSource cannot be discovered statically.")]
		public Activity StartActivity(Activity activity, object? args)
		{
			activity.Start();
			Write(activity.OperationName + ".Start", args);
			return activity;
		}

		[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresUnreferencedCode("Only the properties of the T type will be preserved. Properties of referenced types and properties of derived types may be trimmed.")]
		public Activity StartActivity<[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMembers(_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMemberTypes.PublicProperties)] T>(Activity activity, T args)
		{
			return StartActivity(activity, (object?)args);
		}

		[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresUnreferencedCode("The type of object being written to DiagnosticSource cannot be discovered statically.")]
		public void StopActivity(Activity activity, object? args)
		{
			if (activity.Duration == TimeSpan.Zero)
			{
				activity.SetEndTime(Activity.GetUtcNow());
			}
			Write(activity.OperationName + ".Stop", args);
			activity.Stop();
		}

		[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ERequiresUnreferencedCode("Only the properties of the T type will be preserved. Properties of referenced types and properties of derived types may be trimmed.")]
		public void StopActivity<[_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMembers(_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EDynamicallyAccessedMemberTypes.PublicProperties)] T>(Activity activity, T args)
		{
			StopActivity(activity, (object?)args);
		}

		public virtual void OnActivityImport(Activity activity, object? payload)
		{
		}

		public virtual void OnActivityExport(Activity activity, object? payload)
		{
		}
	}
}
