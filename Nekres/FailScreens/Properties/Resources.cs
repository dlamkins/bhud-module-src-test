using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Nekres.FailScreens.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("Nekres.FailScreens.Properties.Resources", typeof(Resources).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string _0__complete => ResourceManager.GetString("{0} complete", resourceCulture);

		internal static string _0__ran_into_a_problem_and_needs_to_revive__We_re_just_collecting_some_tears__and_then_we_ll_grief_with_you_ => ResourceManager.GetString("{0} ran into a problem and needs to revive. We're just collecting some tears, and then we'll grief with you.", resourceCulture);

		internal static string Error => ResourceManager.GetString("Error", resourceCulture);

		internal static string Fail => ResourceManager.GetString("Fail", resourceCulture);

		internal static string For_more_information_about_this_issue_and_possible_fixes__bother_your_party_leader_ => ResourceManager.GetString("For more information about this issue and possible fixes, bother your party leader.", resourceCulture);

		internal static string The_process_terminated_unexpectedly_ => ResourceManager.GetString("The process terminated unexpectedly.", resourceCulture);

		internal Resources()
		{
		}
	}
}
