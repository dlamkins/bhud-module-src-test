using Blish_HUD.Controls;

namespace TargetYourFeet.Utils
{
	public static class ContainerExtensions
	{
		public static Container AddControl(this Container container, Control control, out Control generatedControl)
		{
			control.set_Parent(container);
			container.AddChild(control);
			generatedControl = control;
			return container;
		}

		public static Container AddControl(this Container container, Control control)
		{
			control.set_Parent(container);
			container.AddChild(control);
			return container;
		}
	}
}
