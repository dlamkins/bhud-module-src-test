using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mounts.Settings;

namespace Manlaan.Mounts.Controls
{
	public class DrawIcons : Container, IDisposable
	{
		private bool _disposed;

		private static readonly Logger Logger = Logger.GetLogger<DrawIcons>();

		private readonly IconThingSettings _iconThingSettings;

		private readonly Helper _helper;

		private readonly TextureCache _textureCache;

		private bool _dragging;

		private Point _dragStart = Point.get_Zero();

		public DrawIcons(IconThingSettings iconThingSettings, Helper helper, TextureCache textureCache)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			_iconThingSettings = iconThingSettings;
			_helper = helper;
			_textureCache = textureCache;
			_iconThingSettings.IconSettingsUpdated += IconSettingsUpdated;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(_iconThingSettings.Location.get_Value());
			Draw();
		}

		private void Draw()
		{
			((Container)this).ClearChildren();
			DrawManualIcons();
			DrawCornerIcons();
		}

		private void DrawCornerIcons()
		{
			if (_iconThingSettings.IsDefault)
			{
				foreach (Thing thing2 in Module._things)
				{
					thing2.DisposeCornerIcon();
				}
			}
			if (!_iconThingSettings.ShouldDisplayCornerIcons)
			{
				return;
			}
			foreach (Thing thing in _iconThingSettings.AvailableThings)
			{
				thing.CreateCornerIcon(_textureCache.GetThingImgFile(thing));
			}
		}

		private void DrawManualIcons()
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			if (!_iconThingSettings.IsEnabled.get_Value() || (!_iconThingSettings.IsEnabledInCompetitiveMaps.get_Value() && _helper.IsPlayerInWvwMap()))
			{
				return;
			}
			int curX = 0;
			int curY = 0;
			ICollection<Thing> things = _iconThingSettings.AvailableThings;
			foreach (Thing thing in things)
			{
				Texture2D img = _textureCache.GetThingImgFile(thing);
				Image val = new Image();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Texture(AsyncTexture2D.op_Implicit(img));
				((Control)val).set_Size(new Point(_iconThingSettings.Size.get_Value(), _iconThingSettings.Size.get_Value()));
				((Control)val).set_Location(new Point(curX, curY));
				((Control)val).set_Opacity(_iconThingSettings.Opacity.get_Value());
				((Control)val).set_BasicTooltipText(thing.DisplayName);
				((Control)val).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)async delegate
				{
					await thing.DoAction(unconditionallyDoAction: false, isActionComingFromMouseActionOnModuleUI: true);
				});
				if (_iconThingSettings.Orientation.get_Value() == IconOrientation.Horizontal)
				{
					curX += _iconThingSettings.Size.get_Value();
				}
				else
				{
					curY += _iconThingSettings.Size.get_Value();
				}
			}
			if (_iconThingSettings.Orientation.get_Value() == IconOrientation.Horizontal)
			{
				((Control)this).set_Size(new Point(_iconThingSettings.Size.get_Value() * things.Count(), _iconThingSettings.Size.get_Value()));
			}
			else
			{
				((Control)this).set_Size(new Point(_iconThingSettings.Size.get_Value(), _iconThingSettings.Size.get_Value() * things.Count()));
			}
			if (_iconThingSettings.IsDraggingEnabled.get_Value())
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)this);
				((Control)val2).set_Location(new Point(0, 0));
				((Control)val2).set_Size(new Point(_iconThingSettings.Size.get_Value() / 2, _iconThingSettings.Size.get_Value() / 2));
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
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					_dragging = false;
					_iconThingSettings.Location.set_Value(((Control)this).get_Location());
				});
			}
		}

		private void IconSettingsUpdated(object sender, SettingsUpdatedEvent e)
		{
			Draw();
		}

		protected override CaptureType CapturesInput()
		{
			if (_iconThingSettings.IsEnabled.get_Value())
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

		protected override void OnHidden(EventArgs e)
		{
			foreach (Thing thing in _iconThingSettings.Things)
			{
				CornerIcon cornerIcon = thing.CornerIcon;
				if (cornerIcon != null)
				{
					((Control)cornerIcon).Hide();
				}
			}
			((Control)this).OnHidden(e);
		}

		protected override void OnShown(EventArgs e)
		{
			foreach (Thing thing in _iconThingSettings.Things)
			{
				CornerIcon cornerIcon = thing.CornerIcon;
				if (cornerIcon != null)
				{
					((Control)cornerIcon).Show();
				}
			}
			((Control)this).OnHidden(e);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				foreach (Thing thing in Module._things)
				{
					thing.DisposeCornerIcon();
				}
			}
			_disposed = true;
		}
	}
}
