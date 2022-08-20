using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public class Speedometer : Control
	{
		private readonly Dictionary<MountType, RectAnchor> _meters = new Dictionary<MountType, RectAnchor>();

		private readonly RectAnchor _meterContainer;

		private RectAnchor _currentMeter;

		public static List<(RectAnchor, string, Func<string>)> ExtraDebug { get; } = new List<(RectAnchor, string, Func<string>)>();


		public float AnchorY
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				RectAnchor meterContainer = _meterContainer;
				RectAnchor meterContainer2 = _meterContainer;
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(0.5f, value);
				meterContainer2.Pivot = val;
				meterContainer.Anchor = val;
			}
		}

		public bool Debug { get; set; }

		public Speedometer()
			: this()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_ClipsBounds(false);
			_meterContainer = new RectAnchor
			{
				SizeDelta = new Vector2(400f, 100f),
				Anchor = new Vector2(0.5f, 0.5f)
			};
			RacingModule.Settings.SpeedometerAnchorY.OnChangedAndNow(delegate(float v)
			{
				AnchorY = v;
			});
			RacingModule.Settings.ShowSpeedometer.OnChangedAndNow(delegate(bool v)
			{
				((Control)this).set_Visible(v);
			});
			RacingModule.Settings.ToggleDebug.OnActivated(delegate
			{
				Debug = !Debug;
			});
			foreach (KeyValuePair<MountType, (GenericSetting<bool>, RectAnchor)> meter2 in RacingModule.Settings.Meters)
			{
				meter2.Deconstruct(out var key, out var value);
				(GenericSetting<bool>, RectAnchor) tuple = value;
				MountType mountType = key;
				var (genericSetting, meter) = tuple;
				genericSetting.OnChangedAndNow(delegate(bool v)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					SetMeter(mountType, v ? meter : null);
				});
			}
			GameService.Gw2Mumble.get_PlayerCharacter().add_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)MountChanged);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void SetMeter(MountType mount, RectAnchor meter)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_meters[mount] = meter;
			UpdateMeter();
		}

		private void MountChanged(object sender, ValueEventArgs<MountType> e)
		{
			UpdateMeter();
		}

		private void UpdateMeter()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			_meterContainer.ClearChildren();
			_currentMeter = null;
			if (_meters.TryGetValue(GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount(), out _currentMeter) && _currentMeter != null)
			{
				_meterContainer.AddChild(_currentMeter);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				RectangleF rect = _meterContainer.Target(RectangleF.op_Implicit(((Control)Control.get_Graphics().get_SpriteScreen()).get_AbsoluteBounds()));
				_meterContainer.Draw(spriteBatch, (Control)(object)this, rect);
				if (Debug)
				{
					DrawDebug(spriteBatch, (Control)(object)this, rect, _currentMeter);
				}
			}
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().remove_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)MountChanged);
			ExtraDebug.Clear();
			((Control)this).DisposeControl();
		}

		private static void DrawDebug(SpriteBatch spriteBatch, Control ctrl, RectangleF rect, RectAnchor currentMeter)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = Control.get_Content().get_DefaultFont16();
			ShapeExtensions.DrawRectangle(spriteBatch, rect, Color.get_Black(), 1f, 0f);
			string toDraw = "";
			add<int>("misses", RacingModule.Measurer.MissedTicks);
			add<float>("ddiff", RacingModule.Measurer.DoubledDiff, shown: true, "G");
			add<int>("doublings", RacingModule.Measurer.Doublings);
			add<float>("slope", RacingModule.Measurer.Speed.SlopeAngle, shown: false);
			add<float>("camera", RacingModule.Measurer.Speed.CamMovementYaw, shown: false);
			add<float>("drift", RacingModule.Measurer.Speed.FwdMovementYaw, shown: false);
			add<float>("ips", RacingModule.Measurer.Speed.Speed2D);
			add<float>("3ips", RacingModule.Measurer.Speed.Speed3D);
			add<float>("vips", RacingModule.Measurer.Speed.UpSpeed);
			add<float>("ips2", RacingModule.Measurer.Accel.Accel2D);
			add<float>("3ips2", RacingModule.Measurer.Accel.Accel3D);
			add<float>("vips2", RacingModule.Measurer.Accel.UpAccel);
			foreach (var item in ExtraDebug.Where(((RectAnchor, string, Func<string>) e) => e.Item1 == currentMeter || e.Item1 == null))
			{
				string name2 = item.Item2;
				Func<string> getter = item.Item3;
				add<string>(name2, getter());
			}
			draw(toDraw);
			T add<T>(string name, T value, bool shown = true, string format = "N0")
			{
				if (shown)
				{
					toDraw += StringExtensions.Format("{0} {1:" + format + "}\n", name, value);
				}
				return value;
			}
			void draw(string text)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				Size2 size = font.MeasureString(text);
				Rectangle textRect = default(Rectangle);
				((Rectangle)(ref textRect))._002Ector(20, 20, (int)size.Width, (int)size.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, ctrl, text, font, textRect, Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
}
