using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mounts.Settings;

namespace Manlaan.Mounts.Controls
{
	public class DrawOutOfCombat : Container
	{
		private bool _dragging;

		private Point _dragStart = Point.get_Zero();

		private LoadingSpinner _spinner;

		public DrawOutOfCombat()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Draw();
		}

		public void HideSpinner()
		{
			if (_spinner != null)
			{
				((Control)_spinner).set_Visible(false);
			}
		}

		public void ShowSpinner()
		{
			if (_spinner != null)
			{
				((Control)_spinner).set_Visible(true);
			}
		}

		private void Draw()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			if (!Module._settingDisplayMountQueueing.get_Value() && !Module._settingDragMountQueueing.get_Value())
			{
				return;
			}
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(Module._settingDisplayMountQueueingLocation.get_Value());
			((Control)this).set_Width(100);
			((Control)this).set_Height(100);
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Visible(Module._settingDragMountQueueing.get_Value());
			_spinner = val;
			if (Module._settingDragMountQueueing.get_Value())
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)this);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Size(new Point(((Control)_spinner).get_Width() / 2, ((Control)_spinner).get_Width() / 2));
				((Control)val2).set_BackgroundColor(Color.get_White());
				((Control)val2).set_ZIndex(10);
				((Control)val2).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					_dragging = true;
					_dragStart = Control.get_Input().get_Mouse().get_Position();
				});
				((Control)val2).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					_dragging = false;
					Module._settingDisplayMountQueueingLocation.set_Value(((Control)this).get_Location());
				});
			}
		}

		private void IconSettingsUpdated(object sender, SettingsUpdatedEvent e)
		{
			Draw();
		}

		protected override CaptureType CapturesInput()
		{
			if (Module._settingDragMountQueueing.get_Value())
			{
				return (CaptureType)4;
			}
			return (CaptureType)0;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
