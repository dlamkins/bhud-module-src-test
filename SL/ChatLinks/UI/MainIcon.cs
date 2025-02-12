using System;
using System.Collections.Generic;
using System.ComponentModel;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace SL.ChatLinks.UI
{
	public sealed class MainIcon : CornerIcon
	{
		public MainIconViewModel ViewModel { get; }

		public MainIcon(MainIconViewModel viewModel)
			: this(viewModel.Texture, viewModel.HoverTexture, viewModel.Name)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((CornerIcon)this).set_Priority(viewModel.Priority);
			ViewModel = viewModel;
			viewModel.Initialize();
			((Control)this).add_PropertyChanged((PropertyChangedEventHandler)ViewPropertyChanged);
			((Control)this).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)ViewModel.ContextMenuItems));
		}

		private void ViewPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			string propertyName = args.PropertyName;
			if (!(propertyName == "LoadingMessage"))
			{
				if (propertyName == "BasicTooltipText")
				{
					ViewModel.TooltipText = ((Control)this).get_BasicTooltipText();
				}
			}
			else
			{
				ViewModel.LoadingMessage = ((CornerIcon)this).get_LoadingMessage();
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			ViewModel.ClickCommand.Execute();
			((CornerIcon)this).OnClick(e);
		}
	}
}
