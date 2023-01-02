using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RaidClears.Raids.Model;
using RaidClears.Raids.Services;
using RaidClears.Settings;
using Settings.Enums;

namespace RaidClears.Raids.Controls
{
	internal class RaidsPanel : FlowPanel
	{
		private Logger _logger;

		private Wing[] _wings;

		private readonly SettingService _settingService;

		private readonly WingRotationService _wingRotation;

		private bool _isDraggedByMouse;

		private Point _dragStart = Point.get_Zero();

		private Color CallOfTheMistColor = new Color(243, 245, 39);

		private Color EmboldenColor = new Color(80, 80, 255);

		private Color TextColor = Color.get_White();

		private Color NotClearedColor = new Color(120, 20, 20);

		private Color ClearedColor = new Color(20, 120, 20);

		private int _cotmIndex;

		private int _emboldenIndex;

		private bool _ignoreMouseInput;

		public bool IgnoreMouseInput
		{
			get
			{
				return _ignoreMouseInput;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _ignoreMouseInput, value, true, "IgnoreMouseInput");
			}
		}

		public RaidsPanel(Logger logger, SettingService settingService, Wing[] wings, WingRotationService wingRotation)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			_logger = logger;
			_wings = wings;
			_settingService = settingService;
			_wingRotation = wingRotation;
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 2f));
			((FlowPanel)this).set_FlowDirection(GetFlowDirection());
			IgnoreMouseInput = ShouldIgnoreMouse();
			((Control)this).set_Location(settingService.RaidPanelLocationPoint.get_Value());
			((Control)this).set_Visible(settingService.RaidPanelIsVisible.get_Value());
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			(_emboldenIndex, _cotmIndex) = wingRotation.getHighlightedWingIndices();
			wings[_emboldenIndex].setEmboldened(embolden: true);
			wings[_cotmIndex].setCallOfTheMist(call: true);
			InitColors(settingService);
			CreateWings(wings);
			settingService.RaidPanelLocationPoint.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)delegate(object s, ValueChangedEventArgs<Point> e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Location(e.get_NewValue());
			});
			settingService.RaidPanelOrientationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<Orientation>>)delegate(object s, ValueChangedEventArgs<Orientation> e)
			{
				OrientationChanged(e.get_NewValue());
			});
			settingService.RaidPanelWingLabelsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<WingLabel>>)delegate(object s, ValueChangedEventArgs<WingLabel> e)
			{
				WingLabelDisplayChanged(e.get_NewValue());
			});
			settingService.RaidPanelFontSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)delegate(object s, ValueChangedEventArgs<FontSize> e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				FontSizeChanged(e.get_NewValue());
			});
			settingService.RaidPanelWingLabelOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object s, ValueChangedEventArgs<float> e)
			{
				WingLabelOpacityChanged(e.get_NewValue());
			});
			settingService.RaidPanelEncounterOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object s, ValueChangedEventArgs<float> e)
			{
				EncounterOpacityChanged(e.get_NewValue());
			});
			settingService.DragWithMouseIsEnabledSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				IgnoreMouseInput = ShouldIgnoreMouse();
			});
			settingService.AllowTooltipsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				IgnoreMouseInput = ShouldIgnoreMouse();
			});
			settingService.W1IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				WingVisibilityChanged(0, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.W2IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				WingVisibilityChanged(1, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.W3IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				WingVisibilityChanged(2, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.W4IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				WingVisibilityChanged(3, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.W5IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				WingVisibilityChanged(4, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.W6IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				WingVisibilityChanged(5, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.W7IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				WingVisibilityChanged(6, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.RaidPanelHighlightEmbolden.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				EmboldenChanged(_emboldenIndex, e.get_NewValue());
			});
			settingService.RaidPanelHighlightCotM.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				CotMChanged(_cotmIndex, e.get_NewValue());
			});
			settingService.RaidPanelColorCleared.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				ColorChanged("cleared", e.get_NewValue());
			});
			settingService.RaidPanelColorNotCleared.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				ColorChanged("notCleared", e.get_NewValue());
			});
			settingService.RaidPanelColorText.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				ColorChanged("text", e.get_NewValue());
			});
			settingService.RaidPanelColorCotm.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				ColorChanged("cotm", e.get_NewValue());
			});
			settingService.RaidPanelColorEmbolden.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				ColorChanged("embolden", e.get_NewValue());
			});
			WingLabelOpacityChanged(settingService.RaidPanelWingLabelOpacity.get_Value());
			EncounterOpacityChanged(settingService.RaidPanelEncounterOpacity.get_Value());
			EmboldenChanged(_emboldenIndex, settingService.RaidPanelHighlightEmbolden.get_Value());
			CotMChanged(_cotmIndex, settingService.RaidPanelHighlightCotM.get_Value());
			AddDragDelegates();
		}

		private void InitColors(SettingService _settings)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			ColorHelper emboldenColor = new ColorHelper(_settings.RaidPanelColorEmbolden.get_Value());
			EmboldenColor = emboldenColor.XnaColor;
			ColorHelper cotmColor = new ColorHelper(_settings.RaidPanelColorCotm.get_Value());
			CallOfTheMistColor = cotmColor.XnaColor;
			ColorHelper textColor = new ColorHelper(_settings.RaidPanelColorText.get_Value());
			TextColor = textColor.XnaColor;
			ColorHelper clearedColor = new ColorHelper(_settings.RaidPanelColorCleared.get_Value());
			ClearedColor = clearedColor.XnaColor;
			ColorHelper notClearedColor = new ColorHelper(_settings.RaidPanelColorNotCleared.get_Value());
			NotClearedColor = notClearedColor.XnaColor;
		}

		protected void ColorChanged(string type, string hexCode)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			ColorHelper _colorHelper = new ColorHelper(hexCode);
			switch (type)
			{
			case "cleared":
				ClearedColor = _colorHelper.XnaColor;
				break;
			case "notCleared":
				NotClearedColor = _colorHelper.XnaColor;
				break;
			case "text":
				TextColor = _colorHelper.XnaColor;
				break;
			case "cotm":
				CallOfTheMistColor = _colorHelper.XnaColor;
				break;
			case "embolden":
				EmboldenColor = _colorHelper.XnaColor;
				break;
			}
			for (int i = 0; i < _wings.Length; i++)
			{
				Color textColor = TextColor;
				Wing obj = _wings[i];
				if (obj.isEmboldened && _settingService.RaidPanelHighlightEmbolden.get_Value())
				{
					textColor = EmboldenColor;
				}
				if (obj.isCallOfTheMist && _settingService.RaidPanelHighlightCotM.get_Value())
				{
					textColor = CallOfTheMistColor;
				}
				obj.GetWingPanelReference().UpdateEncounterColors(ClearedColor, NotClearedColor);
				obj.GetWingPanelReference().SetHighlightColor(textColor);
				Encounter[] encounters = obj.encounters;
				for (int j = 0; j < encounters.Length; j++)
				{
					encounters[j].UpdateColors(ClearedColor, NotClearedColor);
				}
			}
		}

		protected void WingVisibilityChanged(int wingIndex, bool was, bool now)
		{
			_wings[wingIndex].GetWingPanelReference().ShowHide(now);
			((Control)this).Invalidate();
		}

		protected void EmboldenChanged(int wingIndex, bool highlight)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			_wings[wingIndex].GetWingPanelReference().SetHighlightColor(highlight ? EmboldenColor : TextColor);
		}

		protected void CotMChanged(int wingIndex, bool highlight)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			_wings[wingIndex].GetWingPanelReference().SetHighlightColor(highlight ? CallOfTheMistColor : TextColor);
		}

		protected ControlFlowDirection GetFlowDirection()
		{
			return (ControlFlowDirection)(_settingService.RaidPanelOrientationSetting.get_Value() switch
			{
				Orientation.Horizontal => 2, 
				Orientation.Vertical => 3, 
				Orientation.SingleRow => 2, 
				_ => 3, 
			});
		}

		protected void OrientationChanged(Orientation orientation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection(GetFlowDirection());
			Wing[] wings = _wings;
			for (int i = 0; i < wings.Length; i++)
			{
				wings[i].GetWingPanelReference().SetOrientation(orientation);
			}
		}

		protected void WingLabelDisplayChanged(WingLabel labelDisplay)
		{
			Wing[] wings = _wings;
			for (int i = 0; i < wings.Length; i++)
			{
				wings[i].GetWingPanelReference().SetLabelDisplay(labelDisplay);
			}
		}

		protected void FontSizeChanged(FontSize fontSize)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Wing[] wings = _wings;
			for (int i = 0; i < wings.Length; i++)
			{
				wings[i].GetWingPanelReference().SetFontSize(fontSize);
			}
		}

		protected void WingLabelOpacityChanged(float opacity)
		{
			Wing[] wings = _wings;
			for (int i = 0; i < wings.Length; i++)
			{
				wings[i].GetWingPanelReference().SetWingLabelOpacity(opacity);
			}
		}

		protected void EncounterOpacityChanged(float opacity)
		{
			Wing[] wings = _wings;
			for (int i = 0; i < wings.Length; i++)
			{
				wings[i].GetWingPanelReference().SetEncounterOpacity(opacity);
			}
		}

		protected override void DisposeControl()
		{
			((Panel)this).DisposeControl();
		}

		public virtual void DoUpdate()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (_isDraggedByMouse && _settingService.DragWithMouseIsEnabledSetting.get_Value())
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
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				if (_settingService.DragWithMouseIsEnabledSetting.get_Value())
				{
					_isDraggedByMouse = true;
					_dragStart = GameService.Input.get_Mouse().get_Position();
				}
			});
			((Control)this).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if (_settingService.DragWithMouseIsEnabledSetting.get_Value())
				{
					_isDraggedByMouse = false;
					_settingService.RaidPanelLocationPoint.set_Value(((Control)this).get_Location());
				}
			});
		}

		protected bool ShouldIgnoreMouse()
		{
			if (!_settingService.DragWithMouseIsEnabledSetting.get_Value())
			{
				return !_settingService.AllowTooltipsSetting.get_Value();
			}
			return false;
		}

		public override Control TriggerMouseInput(MouseEventType mouseEventType, MouseState ms)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (_ignoreMouseInput)
			{
				return null;
			}
			return ((Container)this).TriggerMouseInput(mouseEventType, ms);
		}

		public void ShowOrHide()
		{
			bool shouldBeVisible = _settingService.RaidPanelIsVisible.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			if (shouldBeVisible && _settingService.DragWithMouseIsEnabledSetting.get_Value())
			{
				DoUpdate();
			}
			if (!((Control)this).get_Visible() && shouldBeVisible)
			{
				((Control)this).Show();
			}
			else if (((Control)this).get_Visible() && !shouldBeVisible)
			{
				((Control)this).Hide();
			}
		}

		protected void CreateWings(Wing[] wings)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			bool[] wingVis = _settingService.GetWingVisibilitySettings();
			foreach (Wing wing in wings)
			{
				WingPanel wingPanel = new WingPanel((Container)(object)this, wing, _settingService.RaidPanelOrientationSetting.get_Value(), _settingService.RaidPanelWingLabelsSetting.get_Value(), _settingService.RaidPanelFontSizeSetting.get_Value(), ClearedColor, NotClearedColor);
				wing.SetWingPanelReference(wingPanel);
				wing.GetWingPanelReference().SetHighlightColor(TextColor);
				wingPanel.ShowHide(wingVis[wing.index - 1]);
				((Container)this).AddChild((Control)(object)wingPanel);
			}
		}

		public void UpdateClearedStatus(ApiRaids apiraids)
		{
			Wing[] wings = _wings;
			for (int i = 0; i < wings.Length; i++)
			{
				Encounter[] encounters = wings[i].encounters;
				foreach (Encounter encounter in encounters)
				{
					encounter.SetCleared(apiraids.Clears.Contains(encounter.id));
				}
			}
			((Control)this).Invalidate();
		}
	}
}
