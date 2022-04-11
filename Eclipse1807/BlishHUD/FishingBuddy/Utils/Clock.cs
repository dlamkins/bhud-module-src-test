using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class Clock : Container
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(Clock));

		private string _timePhase = "";

		public bool HideLabel = true;

		public bool Drag;

		public FontSize Font_Size = (FontSize)14;

		public VerticalAlignment LabelAlign;

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
					_timePhase = value;
					switch (TimePhase)
					{
					case "Dawn":
						((Control)_currentTime).set_Visible(false);
						_currentTime = _dawn;
						((Control)_currentTime).set_Visible(true);
						break;
					case "Day":
						((Control)_currentTime).set_Visible(false);
						_currentTime = _day;
						((Control)_currentTime).set_Visible(true);
						break;
					case "Dusk":
						((Control)_currentTime).set_Visible(false);
						_currentTime = _dusk;
						((Control)_currentTime).set_Visible(true);
						break;
					case "Night":
						((Control)_currentTime).set_Visible(false);
						_currentTime = _night;
						((Control)_currentTime).set_Visible(true);
						break;
					}
				}
			}
		}

		public Clock()
			: this()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(50, 50));
			((Control)this).set_Size(new Point(0, 0));
			((Control)this).set_Visible(true);
			((Control)this).set_Padding(Thickness.Zero);
			ClickThroughImage clickThroughImage = new ClickThroughImage();
			((Control)clickThroughImage).set_Parent((Container)(object)this);
			((Image)clickThroughImage).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgDawn));
			((Control)clickThroughImage).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage).set_Location(new Point(0));
			((Control)clickThroughImage).set_Opacity(1f);
			((Control)clickThroughImage).set_BasicTooltipText("Dawn");
			((Control)clickThroughImage).set_Visible(TimePhase == "Dawn");
			clickThroughImage.Capture = Drag;
			_dawn = clickThroughImage;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dawn).set_Size(new Point(((Control)this).get_Size().X));
			});
			ClickThroughImage clickThroughImage2 = new ClickThroughImage();
			((Control)clickThroughImage2).set_Parent((Container)(object)this);
			((Image)clickThroughImage2).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgDay));
			((Control)clickThroughImage2).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage2).set_Location(new Point(0));
			((Control)clickThroughImage2).set_Opacity(1f);
			((Control)clickThroughImage2).set_BasicTooltipText("Day");
			((Control)clickThroughImage2).set_Visible(TimePhase == "Day");
			clickThroughImage2.Capture = Drag;
			_day = clickThroughImage2;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				((Control)_day).set_Size(new Point(((Control)this).get_Size().X));
			});
			ClickThroughImage clickThroughImage3 = new ClickThroughImage();
			((Control)clickThroughImage3).set_Parent((Container)(object)this);
			((Image)clickThroughImage3).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgDusk));
			((Control)clickThroughImage3).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage3).set_Location(new Point(0));
			((Control)clickThroughImage3).set_Opacity(1f);
			((Control)clickThroughImage3).set_BasicTooltipText("Dusk");
			((Control)clickThroughImage3).set_Visible(TimePhase == "Dusk");
			clickThroughImage3.Capture = Drag;
			_dusk = clickThroughImage3;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dusk).set_Size(new Point(((Control)this).get_Size().X));
			});
			ClickThroughImage clickThroughImage4 = new ClickThroughImage();
			((Control)clickThroughImage4).set_Parent((Container)(object)this);
			((Image)clickThroughImage4).set_Texture(AsyncTexture2D.op_Implicit(FishingBuddyModule._imgNight));
			((Control)clickThroughImage4).set_Size(new Point(FishingBuddyModule._timeOfDayImgSize.get_Value()));
			((Control)clickThroughImage4).set_Location(new Point(0));
			((Control)clickThroughImage4).set_Opacity(1f);
			((Control)clickThroughImage4).set_BasicTooltipText("Night");
			((Control)clickThroughImage4).set_Visible(TimePhase == "Night");
			clickThroughImage4.Capture = Drag;
			_night = clickThroughImage4;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				((Control)_night).set_Size(new Point(((Control)this).get_Size().X));
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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!HideLabel)
			{
				_font = GameService.Content.GetFont((FontFace)0, Font_Size, (FontStyle)0);
			}
		}
	}
}
