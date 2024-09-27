using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	public class InfoPanel : Container
	{
		private readonly TextureCache _textureCache;

		private readonly Helper _helper;

		private bool _dragging;

		private Point _dragStart = Point.get_Zero();

		private Image infoPanelInfo;

		private Image rangedThing;

		private Image rangedIndicator;

		private Image outOfCombatQueuingThing;

		private Image outOfCombatQueueingIndicator;

		private Image laterActivationThing;

		private Image laterActivationIndicator;

		public InfoPanel(TextureCache textureCache, Helper helper)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_textureCache = textureCache;
			_helper = helper;
			_helper.RangedThingUpdated += RangedThingUpdated;
			_helper.StoredThingForLaterUpdated += StoredThingForLaterUpdated;
			foreach (Thing thing in Module._things)
			{
				thing.QueuedTimestampUpdated += QueuedTimestampUpdated;
			}
			Draw();
		}

		private void QueuedTimestampUpdated(object sender, ValueChangedEventArgs e)
		{
			if (outOfCombatQueuingThing != null && outOfCombatQueueingIndicator != null && Module._settingDisplayMountQueueing.get_Value())
			{
				Thing thing = _helper.GetQueuedThing();
				if (thing != null)
				{
					SetThingOnImage(thing, outOfCombatQueuingThing);
					((Control)outOfCombatQueuingThing).set_ZIndex(2);
					((Control)outOfCombatQueueingIndicator).set_Visible(true);
					((Control)outOfCombatQueueingIndicator).set_ZIndex(3);
				}
				else
				{
					((Control)outOfCombatQueuingThing).set_Visible(false);
					((Control)outOfCombatQueueingIndicator).set_Visible(false);
				}
			}
		}

		private void SetThingOnImage(Thing thing, Image image)
		{
			Texture2D img = _textureCache.GetThingImgFile(thing);
			image.set_Texture(AsyncTexture2D.op_Implicit(img));
			((Control)image).set_Visible(true);
		}

		private void RangedThingUpdated(object sender, ValueChangedEventArgs<Thing> e)
		{
			Thing currentRangedThing = e.get_NewValue();
			if (currentRangedThing != null && Module._settingDisplayGroundTargetingAction.get_Value())
			{
				SetThingOnImage(currentRangedThing, rangedThing);
				((Control)rangedThing).set_ZIndex(2);
				((Control)rangedIndicator).set_Visible(true);
				((Control)rangedIndicator).set_ZIndex(3);
			}
			else
			{
				((Control)rangedThing).set_Visible(false);
				((Control)rangedIndicator).set_Visible(false);
			}
		}

		private void StoredThingForLaterUpdated(object sender, ValueChangedEventArgs<Dictionary<string, Thing>> e)
		{
			if (laterActivationThing != null && laterActivationIndicator != null && Module._settingDisplayLaterActivation.get_Value())
			{
				e.get_NewValue().TryGetValue(GameService.Gw2Mumble.get_PlayerCharacter().get_Name(), out var thing);
				if (thing != null)
				{
					SetThingOnImage(thing, laterActivationThing);
					((Control)laterActivationThing).set_ZIndex(2);
					((Control)laterActivationIndicator).set_Visible(true);
					((Control)laterActivationIndicator).set_ZIndex(3);
				}
				else
				{
					((Control)laterActivationThing).set_Visible(false);
					((Control)laterActivationIndicator).set_Visible(false);
				}
			}
		}

		public void Update()
		{
		}

		private void Draw()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected O, but got Unknown
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Expected O, but got Unknown
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Expected O, but got Unknown
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Expected O, but got Unknown
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(Module._settingInfoPanelLocation.get_Value());
			((Control)this).set_Width(64);
			((Control)this).set_Height(64);
			Texture2D img = _textureCache.GetImgFile(TextureCache.ModuleLogoTextureName);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Texture(AsyncTexture2D.op_Implicit(img));
			((Control)val).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height()));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Visible(Module._settingDragInfoPanel.get_Value());
			((Control)val).set_ZIndex(1);
			((Control)val).set_BasicTooltipText("Mounts & More info panel\nDisplays out of combat queueing, ground target action and tap action.\nSee settings and documentation for more info.");
			infoPanelInfo = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Visible(false);
			((Control)val2).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height()));
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_BasicTooltipText("Action will be performed when out of combat");
			outOfCombatQueuingThing = val2;
			Texture2D imgSword = _textureCache.GetImgFile(TextureCache.InCombatTextureName);
			Image val3 = new Image();
			val3.set_Texture(AsyncTexture2D.op_Implicit(imgSword));
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Visible(false);
			((Control)val3).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height()));
			((Control)val3).set_Location(new Point(0, 0));
			outOfCombatQueueingIndicator = val3;
			Image val4 = new Image();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Visible(false);
			((Control)val4).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height()));
			((Control)val4).set_Location(new Point(0, 0));
			((Control)val4).set_BasicTooltipText("Ranged action, left click to use action");
			rangedThing = val4;
			Texture2D imgRanged = _textureCache.GetImgFile(TextureCache.RangeIndicatorTextureName);
			Image val5 = new Image();
			val5.set_Texture(AsyncTexture2D.op_Implicit(imgRanged));
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Visible(false);
			((Control)val5).set_Size(new Point(((Control)this).get_Width() / 3, ((Control)this).get_Height() / 3));
			((Control)val5).set_Location(new Point(((Control)this).get_Width() / 2 - ((Control)this).get_Width() / 3 / 2, ((Control)this).get_Height() - ((Control)this).get_Height() / 3));
			rangedIndicator = val5;
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Visible(false);
			((Control)val6).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height()));
			((Control)val6).set_Location(new Point(0, 0));
			((Control)val6).set_BasicTooltipText("Action will be performed later");
			laterActivationThing = val6;
			Texture2D imgLater = _textureCache.GetImgFile(TextureCache.LaterActivationTextureName);
			Image val7 = new Image();
			val7.set_Texture(AsyncTexture2D.op_Implicit(imgLater));
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Visible(false);
			((Control)val7).set_Size(new Point(((Control)this).get_Width() / 3, ((Control)this).get_Height() / 3));
			((Control)val7).set_Location(new Point(((Control)this).get_Width() / 2 - ((Control)this).get_Width() / 3 / 2, ((Control)this).get_Height() - ((Control)this).get_Height() / 3));
			laterActivationIndicator = val7;
			if (Module._settingDragInfoPanel.get_Value())
			{
				Panel val8 = new Panel();
				((Control)val8).set_Parent((Container)(object)this);
				((Control)val8).set_Location(new Point(0, 0));
				((Control)val8).set_Size(new Point(25, 25));
				((Control)val8).set_BackgroundColor(Color.get_White());
				((Control)val8).set_BasicTooltipText("Drag info panel");
				((Control)val8).set_ZIndex(100);
				((Control)val8).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					_dragging = true;
					_dragStart = Control.get_Input().get_Mouse().get_Position();
				});
				((Control)val8).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					_dragging = false;
					Module._settingInfoPanelLocation.set_Value(((Control)this).get_Location());
				});
			}
		}

		protected override CaptureType CapturesInput()
		{
			if (Module._settingDragInfoPanel.get_Value())
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
