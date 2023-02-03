using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Controls;

namespace Ideka.RacingMeter
{
	public static class UIPanelExtensions
	{
		public static void SoftChild(this IUIPanel panel, Control child, Action<bool>? act = null)
		{
			Control child2 = child;
			Action<bool> act2 = act;
			IUIPanel panel2 = panel;
			inner(((Control)panel2.Panel).get_Parent() != null);
			((Control)panel2.Panel).add_PropertyChanged((PropertyChangedEventHandler)delegate(object _, PropertyChangedEventArgs e)
			{
				if (!(e.PropertyName != "Parent"))
				{
					inner(((Control)panel2.Panel).get_Parent() != null);
				}
			});
			void inner(bool enabled)
			{
				child2.set_Parent((Container)(object)(enabled ? GameService.Graphics.get_SpriteScreen() : null));
				act2?.Invoke(enabled);
			}
		}
	}
}
