using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RaidClears.Dungeons.Model;
using RaidClears.Settings;
using Settings.Enums;

namespace RaidClears.Dungeons.Controls
{
	internal class DungeonsPanel : FlowPanel
	{
		private Logger _logger;

		private Dungeon[] _dungeons;

		private readonly SettingService _settingService;

		private bool _isDraggedByMouse;

		private Point _dragStart = Point.get_Zero();

		private PathsPanel _frequenterPanel;

		private Dungeon _frequenterdungeon;

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

		public DungeonsPanel(Logger logger, SettingService settingService, Dungeon[] dungeons)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			_logger = logger;
			_dungeons = dungeons;
			_settingService = settingService;
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 2f));
			((FlowPanel)this).set_FlowDirection(GetFlowDirection());
			IgnoreMouseInput = ShouldIgnoreMouse();
			((Control)this).set_Location(settingService.DungeonPanelLocationPoint.get_Value());
			((Control)this).set_Visible(settingService.DungeonPanelIsVisible.get_Value());
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			CreateDungeons(dungeons);
			AddFrequenter();
			settingService.DungeonPanelLocationPoint.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)delegate(object s, ValueChangedEventArgs<Point> e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Location(e.get_NewValue());
			});
			settingService.DungeonPanelOrientationSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<DungeonOrientation>>)delegate(object s, ValueChangedEventArgs<DungeonOrientation> e)
			{
				OrientationChanged(e.get_NewValue());
			});
			settingService.DungeonPanelWingLabelsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<DungeonLabel>>)delegate(object s, ValueChangedEventArgs<DungeonLabel> e)
			{
				WingLabelDisplayChanged(e.get_NewValue());
			});
			settingService.DungeonPanelFontSizeSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<FontSize>>)delegate(object s, ValueChangedEventArgs<FontSize> e)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				FontSizeChanged(e.get_NewValue());
			});
			settingService.DungeonPanelWingLabelOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object s, ValueChangedEventArgs<float> e)
			{
				WingLabelOpacityChanged(e.get_NewValue());
			});
			settingService.DungeonPanelEncounterOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object s, ValueChangedEventArgs<float> e)
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
			settingService.D1IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(0, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.D2IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(1, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.D3IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(2, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.D4IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(3, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.D5IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(4, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.D6IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(5, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.D7IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(6, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.D8IsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonVisibilityChanged(7, e.get_PreviousValue(), e.get_NewValue());
			});
			settingService.DFIsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				DungeonFrequenterVisibilityChanged(e.get_NewValue());
			});
			WingLabelOpacityChanged(settingService.DungeonPanelWingLabelOpacity.get_Value());
			EncounterOpacityChanged(settingService.DungeonPanelEncounterOpacity.get_Value());
			AddDragDelegates();
		}

		protected void DungeonVisibilityChanged(int wingIndex, bool was, bool now)
		{
			_dungeons[wingIndex].GetPanelReference().ShowHide(now);
			((Control)this).Invalidate();
		}

		protected void DungeonFrequenterVisibilityChanged(bool now)
		{
			_frequenterPanel.ShowHide(now);
			((Control)this).Invalidate();
		}

		protected ControlFlowDirection GetFlowDirection()
		{
			return (ControlFlowDirection)(_settingService.DungeonPanelOrientationSetting.get_Value() switch
			{
				DungeonOrientation.Horizontal => 2, 
				DungeonOrientation.Vertical => 3, 
				DungeonOrientation.SingleRow => 2, 
				_ => 3, 
			});
		}

		protected void OrientationChanged(DungeonOrientation orientation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection(GetFlowDirection());
			Dungeon[] dungeons = _dungeons;
			for (int i = 0; i < dungeons.Length; i++)
			{
				dungeons[i].GetPanelReference().SetOrientation(orientation);
			}
			_frequenterPanel.SetOrientation(orientation);
		}

		protected void WingLabelDisplayChanged(DungeonLabel labelDisplay)
		{
			Dungeon[] dungeons = _dungeons;
			for (int i = 0; i < dungeons.Length; i++)
			{
				dungeons[i].GetPanelReference().SetLabelDisplay(labelDisplay);
			}
			_frequenterPanel.SetLabelDisplay(labelDisplay);
		}

		protected void FontSizeChanged(FontSize fontSize)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Dungeon[] dungeons = _dungeons;
			for (int i = 0; i < dungeons.Length; i++)
			{
				dungeons[i].GetPanelReference().SetFontSize(fontSize);
			}
			_frequenterPanel.SetFontSize(fontSize);
		}

		protected void WingLabelOpacityChanged(float opacity)
		{
			Dungeon[] dungeons = _dungeons;
			for (int i = 0; i < dungeons.Length; i++)
			{
				dungeons[i].GetPanelReference().SetWingLabelOpacity(opacity);
			}
			_frequenterPanel.SetWingLabelOpacity(opacity);
		}

		protected void EncounterOpacityChanged(float opacity)
		{
			Dungeon[] dungeons = _dungeons;
			for (int i = 0; i < dungeons.Length; i++)
			{
				dungeons[i].GetPanelReference().SetEncounterOpacity(opacity);
			}
			_frequenterPanel.SetEncounterOpacity(opacity);
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
					_settingService.DungeonPanelLocationPoint.set_Value(((Control)this).get_Location());
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
			bool shouldBeVisible = _settingService.DungeonsEnabled.get_Value() && _settingService.DungeonPanelIsVisible.get_Value() && GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
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

		protected void AddFrequenter()
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			_frequenterdungeon = new Dungeon(9, "Frequenter Achievement Summary", 0, 0, "Freq", new Path[1]
			{
				new Path("freq", "Frequenter Achievement Paths Finished", "0/8")
			});
			_frequenterPanel = new PathsPanel((Container)(object)this, _frequenterdungeon, _settingService.DungeonPanelOrientationSetting.get_Value(), _settingService.DungeonPanelWingLabelsSetting.get_Value(), _settingService.DungeonPanelFontSizeSetting.get_Value());
			_frequenterdungeon.SetPanelReference(_frequenterPanel);
			((Container)this).AddChild((Control)(object)_frequenterPanel);
		}

		protected void CreateDungeons(Dungeon[] dungeons)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			bool[] wingVis = _settingService.GetDungeonVisibilitySettings();
			foreach (Dungeon dungeon in dungeons)
			{
				PathsPanel wingPanel = new PathsPanel((Container)(object)this, dungeon, _settingService.DungeonPanelOrientationSetting.get_Value(), _settingService.DungeonPanelWingLabelsSetting.get_Value(), _settingService.DungeonPanelFontSizeSetting.get_Value());
				dungeon.SetPanelReference(wingPanel);
				wingPanel.ShowHide(wingVis[dungeon.index - 1]);
				((Container)this).AddChild((Control)(object)wingPanel);
			}
		}

		public void UpdateClearedStatus(ApiDungeons apidungeons)
		{
			Dungeon[] dungeons = _dungeons;
			for (int i = 0; i < dungeons.Length; i++)
			{
				Path[] paths = dungeons[i].paths;
				foreach (Path path in paths)
				{
					path.SetCleared(apidungeons.Clears.Contains(path.id));
					path.SetFrequenter(apidungeons.Frequenter.Contains(path.id));
				}
			}
			_frequenterdungeon.paths[0].GetLabelReference().set_Text($"{apidungeons.Frequenter.Count}/8");
			_frequenterdungeon.paths[0].SetFrequenter(done: true);
			((Control)this).Invalidate();
		}
	}
}
