using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Eclipse1807.BlishHUD.FishingBuddy.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class Clock : Container
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(Clock));

		private string _timePhase = string.Empty;

		public bool HideLabel;

		public bool Drag;

		public FontSize Font_Size = (FontSize)14;

		public VerticalAlignment LabelVerticalAlignment = (VerticalAlignment)2;

		private static BitmapFont _font;

		private Point _dragStart = Point.get_Zero();

		private bool _dragging;

		internal ClickThroughImage _dawn;

		internal ClickThroughImage _day;

		internal ClickThroughImage _dusk;

		internal ClickThroughImage _night;

		internal ClickThroughImage _currentTime;

		public string TimePhase
		{
			get
			{
				return _timePhase;
			}
			set
			{
				if (!object.Equals(TimePhase, value))
				{
					Logger.Debug("Time of day changed " + TimePhase + " -> " + value);
					OnTimeOfDayChanged(new ValueChangedEventArgs<string>(TimePhase, value));
				}
			}
		}

		public event EventHandler<ValueChangedEventArgs<string>> TimeOfDayChanged;

		public Clock()
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(50));
			((Control)this).set_Visible(true);
			((Control)this).set_Padding(Thickness.Zero);
			_font = GameService.Content.GetFont((FontFace)0, Font_Size, (FontStyle)0);
			ClickThroughImage clickThroughImage = new ClickThroughImage();
			((Control)clickThroughImage).set_Parent((Container)(object)this);
			((Image)clickThroughImage).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgDawn));
			((Control)clickThroughImage).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage).set_Location(new Point(0, _font.get_LineHeight()));
			((Control)clickThroughImage).set_Opacity(1f);
			((Control)clickThroughImage).set_BasicTooltipText(Strings.Dawn);
			((Control)clickThroughImage).set_Visible(TimePhase == Strings.Dawn);
			clickThroughImage.Capture = Drag;
			_dawn = clickThroughImage;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dawn).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			});
			ClickThroughImage clickThroughImage2 = new ClickThroughImage();
			((Control)clickThroughImage2).set_Parent((Container)(object)this);
			((Image)clickThroughImage2).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgDay));
			((Control)clickThroughImage2).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage2).set_Location(new Point(0, _font.get_LineHeight()));
			((Control)clickThroughImage2).set_Opacity(1f);
			((Control)clickThroughImage2).set_BasicTooltipText(Strings.Day);
			((Control)clickThroughImage2).set_Visible(TimePhase == Strings.Day);
			clickThroughImage2.Capture = Drag;
			_day = clickThroughImage2;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				((Control)_day).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			});
			ClickThroughImage clickThroughImage3 = new ClickThroughImage();
			((Control)clickThroughImage3).set_Parent((Container)(object)this);
			((Image)clickThroughImage3).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgDusk));
			((Control)clickThroughImage3).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage3).set_Location(new Point(0, _font.get_LineHeight()));
			((Control)clickThroughImage3).set_Opacity(1f);
			((Control)clickThroughImage3).set_BasicTooltipText(Strings.Dusk);
			((Control)clickThroughImage3).set_Visible(TimePhase == Strings.Dusk);
			clickThroughImage3.Capture = Drag;
			_dusk = clickThroughImage3;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dusk).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			});
			ClickThroughImage clickThroughImage4 = new ClickThroughImage();
			((Control)clickThroughImage4).set_Parent((Container)(object)this);
			((Image)clickThroughImage4).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgNight));
			((Control)clickThroughImage4).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage4).set_Location(new Point(0, _font.get_LineHeight()));
			((Control)clickThroughImage4).set_Opacity(1f);
			((Control)clickThroughImage4).set_BasicTooltipText(Strings.Night);
			((Control)clickThroughImage4).set_Visible(TimePhase == Strings.Night);
			clickThroughImage4.Capture = Drag;
			_night = clickThroughImage4;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				((Control)_night).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			});
			_currentTime = _day;
		}

		protected override CaptureType CapturesInput()
		{
			if (Drag)
			{
				return (CaptureType)4;
			}
			return (CaptureType)1;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (Drag)
			{
				_dragging = true;
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			((Control)this).OnLeftMouseButtonPressed(e);
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (Drag)
			{
				_dragging = false;
				FishingBuddyModule._timeOfDayPanelLoc.set_Value(((Control)this).get_Location());
			}
			((Control)this).OnLeftMouseButtonReleased(e);
		}

		private bool IsPointInBounds(Point point)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Point windowSize = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			if (point.X > 0 && point.Y > 0 && point.X < windowSize.X)
			{
				return point.Y < windowSize.Y;
			}
			return false;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				_dawn.Capture = Drag;
				_day.Capture = Drag;
				_dusk.Capture = Drag;
				_night.Capture = Drag;
				if (IsPointInBounds(Control.get_Input().get_Mouse().get_Position()))
				{
					Point nOffset = Control.get_Input().get_Mouse().get_Position() - _dragStart;
					((Control)this).set_Location(((Control)this).get_Location() + nOffset);
				}
				else
				{
					_dragging = false;
					FishingBuddyModule._timeOfDayPanelLoc.set_Value(((Control)this).get_Location());
				}
				_dragStart = Control.get_Input().get_Mouse().get_Position();
			}
			else
			{
				_dawn.Capture = Drag;
				_day.Capture = Drag;
				_dusk.Capture = Drag;
				_night.Capture = Drag;
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			if (!HideLabel)
			{
				TimeSpan timeTilNextPhase = TyriaTime.TimeTilNextPhase(FishingBuddyModule._currentMap);
				string timeStr = ((timeTilNextPhase.Hours > 0) ? timeTilNextPhase.ToString("h\\:mm\\:ss") : timeTilNextPhase.ToString("mm\\:ss"));
				((Control)this).set_Size(new Point(Math.Max((int)_font.MeasureString(timeStr).Width, FishingBuddyModule._timeOfDayImgSize.get_Value()), FishingBuddyModule._timeOfDayImgSize.get_Value() + _font.get_LineHeight() * 2));
				if (!(timeTilNextPhase <= TimeSpan.Zero))
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, timeStr, _font, new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()), Color.get_White(), false, true, 1, (HorizontalAlignment)1, LabelVerticalAlignment);
				}
			}
		}

		protected virtual void OnTimeOfDayChanged(ValueChangedEventArgs<string> e)
		{
			_timePhase = e.get_NewValue();
			if (TimePhase == Strings.Dawn)
			{
				((Control)_currentTime).set_Visible(false);
				_currentTime = _dawn;
				((Control)_currentTime).set_Visible(true);
			}
			else if (TimePhase == Strings.Day)
			{
				((Control)_currentTime).set_Visible(false);
				_currentTime = _day;
				((Control)_currentTime).set_Visible(true);
			}
			else if (TimePhase == Strings.Dusk)
			{
				((Control)_currentTime).set_Visible(false);
				_currentTime = _dusk;
				((Control)_currentTime).set_Visible(true);
			}
			else if (TimePhase == Strings.Night)
			{
				((Control)_currentTime).set_Visible(false);
				_currentTime = _night;
				((Control)_currentTime).set_Visible(true);
			}
			this.TimeOfDayChanged?.Invoke(this, e);
		}
	}
}
