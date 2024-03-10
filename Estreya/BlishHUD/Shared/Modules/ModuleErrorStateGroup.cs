namespace Estreya.BlishHUD.Shared.Modules
{
	public class ModuleErrorStateGroup
	{
		private string _group;

		public static ModuleErrorStateGroup BACKEND_UNAVAILABLE = new ModuleErrorStateGroup("backend-unavailable");

		public static ModuleErrorStateGroup MODULE_VALIDATION = new ModuleErrorStateGroup("module-validation");

		protected ModuleErrorStateGroup(string group)
		{
			_group = group;
		}

		public override string ToString()
		{
			return _group;
		}
	}
}
