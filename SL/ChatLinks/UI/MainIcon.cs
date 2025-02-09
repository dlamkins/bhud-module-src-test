using System;
using System.ComponentModel;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using SL.Common.Controls;

namespace SL.ChatLinks.UI
{
	public sealed class MainIcon : CornerIcon
	{
		public MainIconViewModel ViewModel { get; }

		public MainIcon(MainIconViewModel viewModel)
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			MainIconViewModel viewModel2 = viewModel;
			((CornerIcon)this)._002Ector(viewModel2.Texture, viewModel2.HoverTexture, viewModel2.Name);
			MainIcon mainIcon = this;
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((CornerIcon)this).set_Priority(viewModel2.Priority);
			ViewModel = viewModel2;
			viewModel2.Initialize();
			((Control)this).add_PropertyChanged((PropertyChangedEventHandler)ViewPropertyChanged);
			((Control)this).set_Menu(new ContextMenuStrip());
			ContextMenuStripItem bananaModeItem = ((Control)this).get_Menu().AddMenuItem(ViewModel.BananaModeLabel);
			bananaModeItem.set_CanCheck(true);
			bananaModeItem.set_Checked(viewModel2.BananaMode);
			bananaModeItem.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent args)
			{
				mainIcon.ViewModel.BananaMode = args.get_Checked();
			});
			ContextMenuStripItem raiseStackSizeItem = ((Control)this).get_Menu().AddMenuItem(ViewModel.RaiseStackSizeLabel);
			raiseStackSizeItem.set_CanCheck(true);
			raiseStackSizeItem.set_Checked(viewModel2.RaiseStackSize);
			raiseStackSizeItem.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent args)
			{
				mainIcon.ViewModel.RaiseStackSize = args.get_Checked();
			});
			ContextMenuStripItem syncItem = ViewModel.SyncCommand.ToMenuItem(() => mainIcon.ViewModel.SyncLabel);
			((Control)this).get_Menu().AddMenuItem(syncItem);
			ContextMenuStripItem koFiItem = ViewModel.KoFiCommand.ToMenuItem(() => mainIcon.ViewModel.KoFiLabel);
			((Control)this).get_Menu().AddMenuItem(koFiItem);
			viewModel2.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
			{
				string propertyName = args.PropertyName;
				if (propertyName != null)
				{
					switch (propertyName.Length)
					{
					case 14:
						switch (propertyName[0])
						{
						case 'R':
							if (propertyName == "RaiseStackSize")
							{
								raiseStackSizeItem.set_Checked(viewModel2.RaiseStackSize);
							}
							break;
						case 'L':
							if (propertyName == "LoadingMessage")
							{
								((CornerIcon)mainIcon).set_LoadingMessage(mainIcon.ViewModel.LoadingMessage);
							}
							break;
						}
						break;
					case 9:
						switch (propertyName[0])
						{
						case 'S':
							if (propertyName == "SyncLabel")
							{
								syncItem.set_Text(viewModel2.SyncLabel);
							}
							break;
						case 'K':
							if (propertyName == "KoFiLabel")
							{
								koFiItem.set_Text(viewModel2.KoFiLabel);
							}
							break;
						}
						break;
					case 4:
						if (propertyName == "Name")
						{
							((CornerIcon)mainIcon).set_IconName(viewModel2.Name);
						}
						break;
					case 15:
						if (propertyName == "BananaModeLabel")
						{
							bananaModeItem.set_Text(viewModel2.BananaModeLabel);
						}
						break;
					case 10:
						if (propertyName == "BananaMode")
						{
							bananaModeItem.set_Checked(viewModel2.BananaMode);
						}
						break;
					case 19:
						if (propertyName == "RaiseStackSizeLabel")
						{
							raiseStackSizeItem.set_Text(viewModel2.RaiseStackSizeLabel);
						}
						break;
					case 11:
						if (propertyName == "TooltipText")
						{
							((Control)mainIcon).set_BasicTooltipText(mainIcon.ViewModel.TooltipText);
						}
						break;
					}
				}
			};
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
