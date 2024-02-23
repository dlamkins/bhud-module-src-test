using System;
using System.Diagnostics;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	public class AreasPanel : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<AreasPanel>();

		private const string KoFiUrl = "https://ko-fi.com/ideka";

		private const string DiscordUrl = "https://discord.gg/8MJnhYzbHP";

		private const int Spacing = 10;

		private AreaView? _opened;

		private AreaView? _selected;

		private readonly StandardButton _koFiButton;

		private readonly StandardButton _discordButton;

		private readonly StandardButton _backButton;

		private readonly StandardButton _openButton;

		private readonly AreasMenu _areasMenu;

		private readonly StandardButton _reloadButton;

		private readonly StandardButton _stopEditingButton;

		private readonly StandardButton _createNewButton;

		private readonly StandardButton _deleteButton;

		private readonly StandardButton _saveButton;

		private readonly AreaPanel _areaPanel;

		private AreaView? Opened
		{
			get
			{
				return _opened;
			}
			set
			{
				_opened = null;
				((Control)_backButton).set_Enabled(value != null);
				((Control)_openButton).set_Enabled(_selected != null && _selected != Opened);
				_areasMenu.SetOpened(value);
				((Panel)_areasMenu).set_Title((value != null) ? ("/ " + string.Join(" / ", from x in value.GetAncestors().Reverse()
					select x.Model.Describe)) : "Areas");
				_opened = value;
				Selected = value;
			}
		}

		public AreaView? Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				_selected = null;
				((Control)_openButton).set_Enabled(value != null && value != Opened);
				((Control)_deleteButton).set_Enabled(value != null);
				_areaPanel.Target = value;
				_selected = value;
			}
		}

		public AreasPanel()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Expected O, but got Unknown
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Ko-fi");
			((Control)val).set_BasicTooltipText("Support my work on Ko-fi.");
			val.set_Icon(AsyncTexture2D.op_Implicit(CTextModule.ContentsManager.GetTexture("KoFiIcon.png")));
			_koFiButton = val;
			((Control)_koFiButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://ko-fi.com/ideka");
			});
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Discord");
			((Control)val2).set_BasicTooltipText("Join my Discord to post questions, comments, suggestions...");
			val2.set_Icon(AsyncTexture2D.op_Implicit(CTextModule.ContentsManager.GetTexture("DiscordIcon.png")));
			_discordButton = val2;
			((Control)_discordButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://discord.gg/8MJnhYzbHP");
			});
			AreasMenu areasMenu = new AreasMenu();
			((Control)areasMenu).set_Parent((Container)(object)this);
			((Panel)areasMenu).set_Title("Areas");
			((Panel)areasMenu).set_ShowTint(true);
			_areasMenu = areasMenu;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Back");
			_backButton = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Open");
			_openButton = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Reload");
			_reloadButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Stop Editing");
			_stopEditingButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Create New");
			_createNewButton = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("Delete");
			_deleteButton = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Text("Save Changes");
			_saveButton = val9;
			AreaPanel areaPanel = new AreaPanel();
			((Control)areaPanel).set_Parent((Container)(object)this);
			_areaPanel = areaPanel;
			UpdateLayout();
			_areasMenu.ItemSelected += delegate(AreaView? area)
			{
				Selected = area;
			};
			_areasMenu.DoubleClicked += delegate(AreaView area)
			{
				Opened = area;
			};
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AreaView opened = Opened;
				Opened = Opened?.GetParent();
				AreasMenu areasMenu5 = _areasMenu;
				AreaView selectable4 = (_areasMenu.Selected = opened);
				areasMenu5.Select(selectable4);
			});
			((Control)_openButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (Selected != null)
				{
					Opened = Selected;
				}
			});
			((Control)_reloadButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (await CTextModule.ConfirmationModal.ShowAsync("Are you sure?", "This will reload your settings from disk. Any changes you haven't saved will be lost.", "Reload", "Cancel"))
				{
					CTextModule.LocalData.ReloadViews();
				}
			});
			((Control)_stopEditingButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AreasMenu areasMenu4 = _areasMenu;
				AreaView selectable3 = (_areasMenu.Selected = null);
				areasMenu4.Select(selectable3);
			});
			((Control)_createNewButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AreaView areaView2 = new AreaView(new ViewTypeContainer(new AreaModel()));
				(Opened ?? CTextModule.LocalData.AreaViewParent).AddChild(areaView2);
				_areasMenu.SetOpened(Opened);
				AreasMenu areasMenu3 = _areasMenu;
				AreaView selectable2 = (_areasMenu.Selected = areaView2);
				areasMenu3.Select(selectable2);
			});
			((Control)_deleteButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				AnchoredRect parent = Selected?.Parent;
				if (parent == null)
				{
					ScreenNotification.ShowNotification("Delete failed (null parent)", (NotificationType)2, (Texture2D)null, 4);
				}
				else if (await CTextModule.ConfirmationModal.ShowAsync("Are you sure?", "This will delete \"" + Selected!.Model.Describe + "\", including all receivers and areas contained within.\nThis action cannot be undone.", "Delete", "Cancel"))
				{
					parent.RemoveChild(Selected);
					Opened = ((Opened == Selected) ? (parent as AreaView) : Opened);
				}
			});
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					CTextModule.LocalData.SaveViews();
				}
				catch (Exception ex)
				{
					ScreenNotification.ShowNotification("Error saving: " + ex.Message, (NotificationType)0, (Texture2D)null, 4);
					Logger.Warn(ex, "Save failed");
					return;
				}
				ScreenNotification.ShowNotification("Saved", (NotificationType)0, (Texture2D)null, 4);
			});
			_areaPanel.HierarchyChanged += delegate
			{
				AreaView selected = Selected;
				Opened = ((Opened == Selected) ? Opened : Selected?.GetParent());
				AreasMenu areasMenu2 = _areasMenu;
				AreaView selectable = (Selected = selected);
				areasMenu2.Select(selectable);
			};
			_areaPanel.NameChanged += delegate(AreaView target)
			{
				_areasMenu.UpdateName(target, Opened != null && Opened != target);
			};
			CTextModule.LocalData.ViewsReloaded += new Action(ViewsReloaded);
		}

		private void ViewsReloaded()
		{
			Opened = null;
			Selected = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (_backButton != null)
			{
				((Control)_backButton).set_Location(Point.get_Zero());
				((Control)(object)_backButton).ArrangeLeftRight(10, (Control)_openButton);
				((Control)_koFiButton).set_Left(0);
				((Control)_koFiButton).set_Bottom(((Container)this).get_ContentRegion().Height);
				StandardButton koFiButton = _koFiButton;
				int height;
				((Control)_discordButton).set_Height(height = 50);
				((Control)koFiButton).set_Height(height);
				((Control)(object)_koFiButton).ArrangeLeftRight(10, (Control)_discordButton);
				((Control)(object)_koFiButton).ArrangeBottomUp(10, (Control)_saveButton, (Control)_createNewButton, (Control)_reloadButton);
				((Control)(object)_createNewButton).ArrangeLeftRight(10, (Control)_deleteButton);
				((Control)(object)_reloadButton).ArrangeLeftRight(10, (Control)_stopEditingButton);
				((Control)_saveButton).set_Width(((Control)_deleteButton).get_Right());
				((Control)(object)_backButton).ArrangeTopDown(10, (Control)_areasMenu);
				((Control)_areasMenu).set_Width(((Control)_openButton).get_Right());
				((Control)_areasMenu).set_Height(((Control)_reloadButton).get_Top() - ((Control)_areasMenu).get_Top() - 10);
				((Control)(object)_areasMenu).ArrangeLeftRight(10, (Control)_areaPanel);
				((Control)_areaPanel).set_Top(0);
				((Control)(object)_areaPanel).WidthFillRight();
				((Control)(object)_areaPanel).HeightFillDown();
			}
		}

		protected override void DisposeControl()
		{
			CTextModule.LocalData.ViewsReloaded -= new Action(ViewsReloaded);
			_koFiButton.get_Icon().Dispose();
			_discordButton.get_Icon().Dispose();
			((Container)this).DisposeControl();
		}
	}
}
