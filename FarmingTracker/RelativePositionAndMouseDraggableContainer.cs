using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class RelativePositionAndMouseDraggableContainer : Container
	{
		private Point _mousePressedLocationInsideContainer = Point.get_Zero();

		private bool _containerIsDraggedByMouse;

		private readonly SettingService _settingService;

		public RelativePositionAndMouseDraggableContainer(SettingService settingService)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_settingService = settingService;
			SetLocationFromWindowAnchorLocationSettings();
			GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			settingService.WindowAnchorSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<WindowAnchor>>)WindowAnchorSettingChanged);
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			_settingService.WindowAnchorSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<WindowAnchor>>)WindowAnchorSettingChanged);
			((Container)this).DisposeControl();
		}

		public override void RecalculateLayout()
		{
			((Control)this).RecalculateLayout();
			SetLocationFromWindowAnchorLocationSettings();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (_settingService.DragProfitWindowWithMouseIsEnabledSetting.get_Value() && _containerIsDraggedByMouse)
			{
				Point newLocation = Control.get_Input().get_Mouse().get_Position() - _mousePressedLocationInsideContainer;
				AdjustAndSetLocationToKeepWindowAnchorInsideScreenBoundaries(newLocation);
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			if (_settingService.DragProfitWindowWithMouseIsEnabledSetting.get_Value())
			{
				_containerIsDraggedByMouse = true;
				_mousePressedLocationInsideContainer = Control.get_Input().get_Mouse().get_Position() - ((Control)this).get_Location();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		private void OnLeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			_containerIsDraggedByMouse = false;
			if (_settingService.DragProfitWindowWithMouseIsEnabledSetting.get_Value())
			{
				SaveWindowAnchorLocationInSettings(((Control)this).get_Location());
			}
		}

		private void WindowAnchorSettingChanged(object sender, ValueChangedEventArgs<WindowAnchor> e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			AdjustAndSetLocationToKeepWindowAnchorInsideScreenBoundaries(((Control)this).get_Location());
		}

		private void OnSpriteScreenResized(object sender, ResizedEventArgs resizedEventArgs)
		{
			SetLocationFromWindowAnchorLocationSettings();
		}

		private void AdjustAndSetLocationToKeepWindowAnchorInsideScreenBoundaries(Point location)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			Point adjustedLocation = WindowAnchorService.ConvertBetweenControlAndWindowAnchorLocation(ScreenBoundariesService.AdjustLocationToKeepControlInsideScreenBoundaries(WindowAnchorService.ConvertBetweenControlAndWindowAnchorLocation(location, ((Control)this).get_Size(), ConvertLocation.ToWindowAnchorLocation, _settingService.WindowAnchorSetting.get_Value()), ((Control)this).get_Size(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Size(), _settingService.WindowAnchorSetting.get_Value()), ((Control)this).get_Size(), ConvertLocation.ToControlLocation, _settingService.WindowAnchorSetting.get_Value());
			SaveWindowAnchorLocationInSettings(adjustedLocation);
			((Control)this).set_Location(adjustedLocation);
		}

		private void SetLocationFromWindowAnchorLocationSettings()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Point windowAnchorLocation = ConvertCoordinatesService.ConvertRelativeToAbsoluteCoordinates(_settingService.ProfitWindowRelativeWindowAnchorLocationSetting.get_Value(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
			((Control)this).set_Location(WindowAnchorService.ConvertBetweenControlAndWindowAnchorLocation(windowAnchorLocation, ((Control)this).get_Size(), ConvertLocation.ToControlLocation, _settingService.WindowAnchorSetting.get_Value()));
		}

		private void SaveWindowAnchorLocationInSettings(Point location)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			FloatPoint newLocation = ConvertCoordinatesService.ConvertAbsoluteToRelativeCoordinates(WindowAnchorService.ConvertBetweenControlAndWindowAnchorLocation(location, ((Control)this).get_Size(), ConvertLocation.ToWindowAnchorLocation, _settingService.WindowAnchorSetting.get_Value()), ((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
			FloatPoint oldLocation = _settingService.ProfitWindowRelativeWindowAnchorLocationSetting.get_Value();
			if (newLocation.X != oldLocation.X || newLocation.Y != oldLocation.Y)
			{
				_settingService.ProfitWindowRelativeWindowAnchorLocationSetting.set_Value(newLocation);
			}
		}
	}
}
