using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public class Speedometer : Control
	{
		private readonly Dictionary<MountType, AnchoredRect?> _meters = new Dictionary<MountType, AnchoredRect>();

		private readonly DisposableCollection _dc = new DisposableCollection();

		private readonly MeasurerRealtime _measurer;

		private readonly AnchoredRect _meterContainer;

		private AnchoredRect? _currentMeter;

		private float AnchorY
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				AnchoredRect meterContainer = _meterContainer;
				AnchoredRect meterContainer2 = _meterContainer;
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(0.5f, value);
				meterContainer2.Pivot = val;
				meterContainer.Anchor = val;
			}
		}

		public bool Debug { get; set; }

		public Speedometer(MeasurerRealtime measurer)
			: this()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			_measurer = measurer;
			((Control)this).set_ClipsBounds(false);
			_meterContainer = new AnchoredRect
			{
				SizeDelta = new Vector2(400f, 100f),
				Anchor = new Vector2(0.5f, 0.5f)
			};
			_dc.Add(RacingModule.Settings.SpeedometerAnchorY.OnChangedAndNow(delegate(float v)
			{
				AnchorY = v;
			}));
			_dc.Add(RacingModule.Settings.ShowSpeedometer.OnChangedAndNow(delegate(bool v)
			{
				((Control)this).set_Visible(v);
			}));
			_dc.Add(RacingModule.Settings.ToggleDebug.OnActivated(delegate
			{
				Debug = !Debug;
			}));
			Dictionary<MountType, AnchoredRect> meters = new Dictionary<MountType, AnchoredRect>
			{
				[(MountType)4] = SkimmerMeter.Construct(measurer),
				[(MountType)2] = GriffonMeter.Construct(measurer),
				[(MountType)6] = BeetleMeter.Construct(measurer, new Func<bool?>(RacingModule.Settings.IsDriftKeyDown)),
				[(MountType)9] = SkiffMeter.Construct(measurer)
			};
			foreach (KeyValuePair<MountType, GenericSetting<bool>> meter2 in RacingModule.Settings.Meters)
			{
				var (mountType, setting) = meter2;
				if (meters.TryGetValue(mountType, out var meter))
				{
					_dc.Add(setting.OnChangedAndNow(delegate(bool v)
					{
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						SetMeter(mountType, v ? meter : null);
					}));
				}
			}
			GameService.Gw2Mumble.get_PlayerCharacter().add_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)MountChanged);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public void SetMeter(MountType mount, AnchoredRect? meter)
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

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			_meterContainer.Update(gameTime);
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
					DrawDebug(spriteBatch, rect);
				}
			}
		}

		private void DrawDebug(SpriteBatch spriteBatch, RectangleF rect)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatch spriteBatch2 = spriteBatch;
			BitmapFont font = Control.get_Content().get_DefaultFont16();
			ShapeExtensions.DrawRectangle(spriteBatch2, rect, Color.get_Black(), 1f, 0f);
			string toDraw = "";
			add<int>("misses", _measurer.MissedTicks);
			add<float>("ddiff", _measurer.DoubledDiff, shown: true, "G");
			add<int>("doublings", _measurer.Doublings);
			add<float>("slope", _measurer.Speed.SlopeAngle, shown: false);
			add<float>("camera", _measurer.Speed.CamMovementYaw, shown: false);
			add<float>("drift", _measurer.Speed.FwdMovementYaw, shown: false);
			add<float>("ips", _measurer.Speed.Speed2D);
			add<float>("3ips", _measurer.Speed.Speed3D);
			add<float>("vips", _measurer.Speed.UpSpeed);
			add<float>("ips2", _measurer.Accel.Accel2D);
			add<float>("3ips2", _measurer.Accel.Accel3D);
			add<float>("vips2", _measurer.Accel.UpAccel);
			if (_currentMeter != null)
			{
				foreach (var (name2, getter) in _currentMeter!.DebugData)
				{
					add<string>(name2, getter());
				}
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
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				Size2 size = font.MeasureString(text);
				Rectangle textRect = default(Rectangle);
				((Rectangle)(ref textRect))._002Ector(20, 20, (int)size.Width, (int)size.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch2, (Control)(object)this, text, font, textRect, Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().remove_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)MountChanged);
			_dc.Dispose();
			((Control)this).DisposeControl();
		}
	}
}
