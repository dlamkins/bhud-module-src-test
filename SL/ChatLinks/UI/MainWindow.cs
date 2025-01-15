using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace SL.ChatLinks.UI
{
	public sealed class MainWindow : TabbedWindow2
	{
		private readonly AsyncEmblem _emblem;

		public MainWindow(MainWindowViewModel vm)
			: this(vm.BackgroundTexture, new Rectangle(0, 26, 953, 691), new Rectangle(70, 35, 880, 650))
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			_emblem = AsyncEmblem.Attach((WindowBase2)(object)this, vm.EmblemTexture);
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((WindowBase2)this).set_Id(vm.Id);
			((WindowBase2)this).set_Title(vm.Title);
			((Control)this).set_Location(new Point(300, 300));
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			foreach (Tab tab in vm.Tabs())
			{
				((TabbedWindow2)this).get_Tabs().Add(tab);
			}
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> args)
		{
			((WindowBase2)this).set_Subtitle(args.get_NewValue().get_Name());
		}

		protected override void DisposeControl()
		{
			((TabbedWindow2)this).remove_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			_emblem.Dispose();
			((WindowBase2)this).DisposeControl();
		}
	}
}
