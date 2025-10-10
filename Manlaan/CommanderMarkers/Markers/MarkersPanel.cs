using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Library.Models;
using Manlaan.CommanderMarkers.Library.Services;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Manlaan.CommanderMarkers.Markers
{
	public class MarkersPanel : FlowPanel, IDisposable
	{
		private bool _draggable;

		private bool _isDraggedByMouse;

		private bool _mouseEventsEnabled;

		private bool _mouseIsInsidePanel;

		private bool _panelEnabled = true;

		private Point _dragStart = Point.get_Zero();

		private KeyBinding? _tmpBinding;

		private Image? _tmpButton;

		protected SettingService _settings;

		protected TextureService _textures;

		private static readonly BitmapFont _dragFont = GameService.Content.get_DefaultFont16();

		public MarkersPanel(SettingService settings, TextureService textures, bool mouseEventsEnabled = true)
			: this()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			_mouseEventsEnabled = mouseEventsEnabled;
			_settings = settings;
			_textures = textures;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(settings._settingLoc.get_Value());
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)(object)this).LayoutChange(_settings._settingOrientation);
			((FlowPanel)(object)this).OpacityChange(_settings._settingOpacity);
			int size = settings._settingImgWidth.get_Value();
			float opacity = settings._settingOpacity.get_Value();
			FlowPanel groundIcons = CreateGroupingFlowPanel();
			groundIcons.VisiblityChanged(_settings._settingGroundMarkersEnabled);
			FlowPanel objectIcons = CreateGroupingFlowPanel();
			objectIcons.VisiblityChanged(_settings._settingTargetMarkersEnabled);
			CreateMarkerButtons(groundIcons, objectIcons, size, opacity);
			if (_mouseEventsEnabled)
			{
				AddDragDelegates();
			}
			_settings._settingDrag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				_draggable = e.get_NewValue();
			});
			_draggable = _settings._settingDrag.get_Value();
			_panelEnabled = _settings._settingShowMarkersPanel.get_Value();
			_settings._settingShowMarkersPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				_panelEnabled = e.get_NewValue();
			});
			if (_mouseEventsEnabled)
			{
				GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseClick);
			}
		}

		private void CreateMarkerButtons(FlowPanel groundIcons, FlowPanel objectIcons, int size, float opacity)
		{
			MarkerDefinition[] allMarkers = MarkerDefinitionService.AllMarkers;
			for (int i = 0; i < allMarkers.Length; i++)
			{
				MarkerDefinition markerDef = allMarkers[i];
				if (markerDef.SupportsGroundTarget)
				{
					SettingEntry<KeyBinding> groundBinding = GetKeyBindingForMarker(markerDef, isGroundTarget: true);
					CreateIconButton((Container)(object)groundIcons, GetTextureForMarker(markerDef.MarkerType), size, opacity, markerDef.GetGroundTooltip(), groundBinding);
				}
				if (markerDef.SupportsObjectTarget)
				{
					SettingEntry<KeyBinding> objectBinding = GetKeyBindingForMarker(markerDef, isGroundTarget: false);
					CreateIconButton((Container)(object)objectIcons, GetTextureForMarker(markerDef.MarkerType), size, opacity, markerDef.GetObjectTooltip(), objectBinding, groundTarget: false);
				}
			}
		}

		private SettingEntry<KeyBinding> GetKeyBindingForMarker(MarkerDefinition markerDef, bool isGroundTarget)
		{
			return (SettingEntry<KeyBinding>)(markerDef.MarkerType switch
			{
				SquadMarker.Arrow => isGroundTarget ? _settings._settingArrowGndBinding : _settings._settingArrowObjBinding, 
				SquadMarker.Circle => isGroundTarget ? _settings._settingCircleGndBinding : _settings._settingCircleObjBinding, 
				SquadMarker.Heart => isGroundTarget ? _settings._settingHeartGndBinding : _settings._settingHeartObjBinding, 
				SquadMarker.Square => isGroundTarget ? _settings._settingSquareGndBinding : _settings._settingSquareObjBinding, 
				SquadMarker.Star => isGroundTarget ? _settings._settingStarGndBinding : _settings._settingStarObjBinding, 
				SquadMarker.Spiral => isGroundTarget ? _settings._settingSpiralGndBinding : _settings._settingSpiralObjBinding, 
				SquadMarker.Triangle => isGroundTarget ? _settings._settingTriangleGndBinding : _settings._settingTriangleObjBinding, 
				SquadMarker.Cross => isGroundTarget ? _settings._settingXGndBinding : _settings._settingXObjBinding, 
				SquadMarker.Clear => isGroundTarget ? _settings._settingClearGndBinding : _settings._settingClearObjBinding, 
				_ => throw new ArgumentException($"Unknown marker type: {markerDef.MarkerType}"), 
			});
		}

		private AsyncTexture2D GetTextureForMarker(SquadMarker markerType)
		{
			return AsyncTexture2D.op_Implicit((Texture2D)(markerType switch
			{
				SquadMarker.Arrow => _textures._imgArrow, 
				SquadMarker.Circle => _textures._imgCircle, 
				SquadMarker.Heart => _textures._imgHeart, 
				SquadMarker.Square => _textures._imgSquare, 
				SquadMarker.Star => _textures._imgStar, 
				SquadMarker.Spiral => _textures._imgSpiral, 
				SquadMarker.Triangle => _textures._imgTriangle, 
				SquadMarker.Cross => _textures._imgX, 
				SquadMarker.Clear => _textures._imgClear, 
				_ => _textures._imgArrow, 
			}));
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (_draggable && _mouseEventsEnabled)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()), new Color(96, 96, 96, 192));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Drag", _dragFont, new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()), Color.get_Black(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}

		protected override void DisposeControl()
		{
			if (_mouseEventsEnabled)
			{
				GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnMouseClick);
			}
		}

		public void Update(GameTime gt)
		{
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Update(gt);
			bool shouldBeVisible = _panelEnabled && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable();
			if (GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				shouldBeVisible = false;
			}
			if (_settings._settingOnlyWhenCommander.get_Value() || Service.LtMode.get_Value())
			{
				shouldBeVisible = shouldBeVisible && (GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander() || Service.LtMode.get_Value());
			}
			if (!((Control)this).get_Visible() && shouldBeVisible)
			{
				((Control)this).Show();
			}
			else if (((Control)this).get_Visible() && !shouldBeVisible)
			{
				((Control)this).Hide();
			}
			if (((Control)this).get_Visible() && _draggable && _isDraggedByMouse)
			{
				Point nOffset = GameService.Input.get_Mouse().get_Position() - _dragStart;
				((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				_dragStart = GameService.Input.get_Mouse().get_Position();
			}
		}

		private void AddDragDelegates()
		{
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				if (_draggable)
				{
					_isDraggedByMouse = true;
					_dragStart = GameService.Input.get_Mouse().get_Position();
				}
			});
			((Control)this).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				if (_draggable)
				{
					_isDraggedByMouse = false;
					_settings._settingLoc.set_Value(((Control)this).get_Location());
				}
			});
		}

		protected FlowPanel CreateGroupingFlowPanel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0045: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)2);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			FlowPanelExtensions.LayoutChange(val, _settings._settingOrientation, 1);
			FlowPanelExtensions.SizeChange(val, _settings._settingImgWidth);
			return val;
		}

		protected void CreateIconButton(Container parent, AsyncTexture2D texture, int size, float opacity, string tooltip, SettingEntry<KeyBinding> keybind, bool groundTarget = true)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			SettingEntry<KeyBinding> keybind2 = keybind;
			Image val = new Image();
			((Control)val).set_Parent(parent);
			val.set_Texture(texture);
			((Control)val).set_Size(new Point(size, size));
			((Control)val).set_Opacity(opacity);
			((Control)val).set_BasicTooltipText(tooltip);
			Image button = val;
			if (!_mouseEventsEnabled)
			{
				return;
			}
			if (groundTarget)
			{
				((Control)button).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					AddGround(button, keybind2.get_Value());
				});
				((Control)button).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					RemoveGround(keybind2.get_Value());
				});
			}
			else
			{
				((Control)button).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					DoHotKey(keybind2.get_Value());
				});
				((Control)button).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					DoHotKey(keybind2.get_Value());
				});
			}
		}

		private void OnMouseClick(object o, MouseEventArgs e)
		{
			if (!_draggable && _tmpBinding != null && !_mouseIsInsidePanel)
			{
				DoHotKey(_tmpBinding);
				ResetGroundIcon();
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			_mouseIsInsidePanel = true;
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			_mouseIsInsidePanel = false;
		}

		protected void ResetGroundIcon()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (_tmpButton != null)
			{
				((Control)_tmpButton).set_BackgroundColor(Color.get_Transparent());
				_tmpButton = null;
			}
			_tmpBinding = null;
		}

		protected void AddGround(Image btn, KeyBinding key)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (_draggable)
			{
				return;
			}
			if (_tmpBinding == key)
			{
				ResetGroundIcon();
				return;
			}
			if (_tmpBinding != null)
			{
				ResetGroundIcon();
			}
			_tmpBinding = key;
			_tmpButton = btn;
			((Control)btn).set_BackgroundColor(Color.get_Yellow());
		}

		protected void RemoveGround(KeyBinding key)
		{
			if (!_draggable)
			{
				DoHotKey(key);
				Thread.Sleep(50);
				DoHotKey(key);
			}
		}

		protected void DoHotKey(KeyBinding key)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			if (_draggable || key == null)
			{
				return;
			}
			if ((int)key.get_ModifierKeys() != 0)
			{
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Press((VirtualKeyShort)18, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Press((VirtualKeyShort)17, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Press((VirtualKeyShort)16, true);
				}
			}
			Keyboard.Press(ToVirtualKey(key.get_PrimaryKey()), true);
			Thread.Sleep(50);
			Keyboard.Release(ToVirtualKey(key.get_PrimaryKey()), true);
			if ((int)key.get_ModifierKeys() != 0)
			{
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					Keyboard.Release((VirtualKeyShort)16, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Keyboard.Release((VirtualKeyShort)17, true);
				}
				if (((Enum)key.get_ModifierKeys()).HasFlag((Enum)(object)(ModifierKeys)2))
				{
					Keyboard.Release((VirtualKeyShort)18, true);
				}
			}
		}

		private VirtualKeyShort ToVirtualKey(Keys key)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				return (VirtualKeyShort)(short)key;
			}
			catch
			{
				return (VirtualKeyShort)0;
			}
		}
	}
}
