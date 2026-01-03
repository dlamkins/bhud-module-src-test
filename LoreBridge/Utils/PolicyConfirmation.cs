using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using LoreBridge.Views;
using Microsoft.Xna.Framework;

namespace LoreBridge.Utils
{
	public static class PolicyConfirmation
	{
		private static EventHandler<ResizedEventArgs> _resizeHandler;

		public static async Task<bool> ShowPolicyConfirmationAsync()
		{
			Point screenSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			Point windowSize = new Point(500, 300);
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Location(new Point((screenSize.X - windowSize.X) / 2, (screenSize.Y - windowSize.Y) / 2));
			((Control)val).set_Size(windowSize);
			((Control)val).set_ZIndex(2147483645);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FadeView(true);
			ViewContainer container = val;
			_resizeHandler = delegate(object _, ResizedEventArgs args)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				((Control)container).set_Location(new Point((args.get_CurrentSize().X - windowSize.X) / 2, (args.get_CurrentSize().Y - windowSize.Y) / 2));
			};
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized(_resizeHandler);
			PolicyConfirmationView view = new PolicyConfirmationView();
			container.Show((IView)(object)view);
			try
			{
				return await view.ResultTask.Task;
			}
			finally
			{
				((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized(_resizeHandler);
			}
		}
	}
}
