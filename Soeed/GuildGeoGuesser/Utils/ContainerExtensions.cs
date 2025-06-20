using Blish_HUD.Controls;

namespace Soeed.GuildGeoGuesser.Utils
{
	public static class ContainerExtensions
	{
		public static Container AddControl<T>(this Container container, T control, out T generatedControl) where T : Control
		{
			((Control)control).set_Parent(container);
			container.AddChild((Control)(object)control);
			generatedControl = control;
			return container;
		}

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
