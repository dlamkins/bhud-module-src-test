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

		private readonly IMeasurer _measurer;

		private readonly AnchoredRect _mainContainer;

		private AnchoredRect? _currentMeter;

		private readonly AnchoredRect _simpleContainer;

		private float MainAnchorX
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _mainContainer.Pivot.X;
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				AnchoredRect mainContainer = _mainContainer;
				AnchoredRect mainContainer2 = _mainContainer;
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(value, MainAnchorY);
				mainContainer2.Pivot = val;
				mainContainer.Anchor = val;
			}
		}

		private float MainAnchorY
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _mainContainer.Pivot.Y;
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				AnchoredRect mainContainer = _mainContainer;
				AnchoredRect mainContainer2 = _mainContainer;
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(MainAnchorX, value);
				mainContainer2.Pivot = val;
				mainContainer.Anchor = val;
			}
		}

		private float SimpleAnchorX
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _simpleContainer.Pivot.X;
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				AnchoredRect simpleContainer = _simpleContainer;
				AnchoredRect simpleContainer2 = _simpleContainer;
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(value, SimpleAnchorY);
				simpleContainer2.Pivot = val;
				simpleContainer.Anchor = val;
			}
		}

		private float SimpleAnchorY
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _simpleContainer.Pivot.Y;
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				AnchoredRect simpleContainer = _simpleContainer;
				AnchoredRect simpleContainer2 = _simpleContainer;
				Vector2 val = default(Vector2);
				((Vector2)(ref val))._002Ector(SimpleAnchorX, value);
				simpleContainer2.Pivot = val;
				simpleContainer.Anchor = val;
			}
		}

		public bool Debug { get; set; }

		public Speedometer(IMeasurer measurer)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			IMeasurer measurer2 = measurer;
			((Control)this)._002Ector();
			Speedometer speedometer = this;
			_measurer = measurer2;
			((Control)this).set_ClipsBounds(false);
			_mainContainer = new AnchoredRect
			{
				SizeDelta = new Vector2(400f, 100f)
			};
			_simpleContainer = new AnchoredRect
			{
				SizeDelta = new Vector2(100f, 100f)
			};
			_dc.Add(RacingModule.Settings.SpeedometerAnchorX.OnChangedAndNow(delegate(float v)
			{
				speedometer.MainAnchorX = v;
			}));
			_dc.Add(RacingModule.Settings.SpeedometerAnchorY.OnChangedAndNow(delegate(float v)
			{
				speedometer.MainAnchorY = v;
			}));
			_dc.Add(RacingModule.Settings.ShowSpeedometer.OnChangedAndNow(delegate(bool v)
			{
				((Control)speedometer).set_Visible(v);
			}));
			_dc.Add(RacingModule.Settings.ToggleDebug.OnActivated(delegate
			{
				speedometer.Debug = !speedometer.Debug;
			}));
			Dictionary<MountType, AnchoredRect> meters = new Dictionary<MountType, AnchoredRect>
			{
				[(MountType)4] = SkimmerMeter.Construct(measurer2),
				[(MountType)2] = GriffonMeter.Construct(measurer2),
				[(MountType)6] = BeetleMeter.Construct(measurer2, new Func<bool?>(RacingModule.Settings.IsDriftKeyDown)),
				[(MountType)7] = WarclawMeter.Construct(measurer2),
				[(MountType)9] = SkiffMeter.Construct(measurer2),
				[(MountType)10] = SiegeTurtleMeter.Construct(measurer2)
			};
			foreach (KeyValuePair<MountType, GenericSetting<bool>> meter2 in RacingModule.Settings.Meters)
			{
				var (mountType, setting) = meter2;
				if (meters.TryGetValue(mountType, out var meter))
				{
					_dc.Add(setting.OnChangedAndNow(delegate(bool v)
					{
						//IL_000c: Unknown result type (might be due to invalid IL or missing references)
						speedometer.SetMeter(mountType, v ? meter : null);
					}));
				}
			}
			GameService.Gw2Mumble.get_PlayerCharacter().add_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)MountChanged);
			_dc.Add(RacingModule.Settings.SimpleSpeedometerAnchorX.OnChangedAndNow(delegate(float v)
			{
				speedometer.SimpleAnchorX = v;
			}));
			_dc.Add(RacingModule.Settings.SimpleSpeedometerAnchorY.OnChangedAndNow(delegate(float v)
			{
				speedometer.SimpleAnchorY = v;
			}));
			_dc.Add(RacingModule.Settings.EnableSimpleSpeedometer.OnChangedAndNow(delegate(bool v)
			{
				speedometer._simpleContainer.Visible = v;
			}));
			_simpleContainer.AddChild(new SizedTextLabel
			{
				AnchorMinX = 0.5f,
				AnchorMax = Vector2.get_One(),
				Font = GameService.Content.get_DefaultFont32(),
				Color = Color.get_White(),
				Stroke = true,
				StrokeDistance = 1
			}).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"{Math.Round(measurer2.Speed.Speed3D)}";
			});
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
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
			_mainContainer.ClearChildren();
			_currentMeter = null;
			if (_meters.TryGetValue(GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount(), out _currentMeter) && _currentMeter != null)
			{
				_mainContainer.AddChild(_currentMeter);
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			_simpleContainer.Update(gameTime);
			_mainContainer.Update(gameTime);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				RectangleF rect2 = _simpleContainer.Target(RectangleF.op_Implicit(((Control)Control.get_Graphics().get_SpriteScreen()).get_AbsoluteBounds()));
				_simpleContainer.Draw(spriteBatch, (Control)(object)this, rect2);
				if (Debug)
				{
					ShapeExtensions.DrawRectangle(spriteBatch, rect2, Color.get_Black(), 1f, 0f);
				}
				RectangleF rect = _mainContainer.Target(RectangleF.op_Implicit(((Control)Control.get_Graphics().get_SpriteScreen()).get_AbsoluteBounds()));
				_mainContainer.Draw(spriteBatch, (Control)(object)this, rect);
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
			MeasurerRealtime measurer = _measurer as MeasurerRealtime;
			if (measurer != null)
			{
				add<int>("misses", measurer.MissedTicks);
				add<float>("ddiff", measurer.DoubledDiff, shown: true, "G");
				add<int>("doublings", measurer.Doublings);
			}
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
