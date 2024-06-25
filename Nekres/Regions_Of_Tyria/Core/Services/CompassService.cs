using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Regions_Of_Tyria.Core.Services
{
	internal class CompassService : IDisposable
	{
		private class CompassRegionDisplay : Control
		{
			public string Text;

			public BitmapFont Font;

			private Texture2D _bgTex;

			public CompassRegionDisplay()
				: this()
			{
				_bgTex = GameService.Content.GetTexture("fade-down-46");
			}

			protected override CaptureType CapturesInput()
			{
				return (CaptureType)22;
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				if (string.IsNullOrWhiteSpace(Text))
				{
					return;
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _bgTex, new Rectangle(bounds.X, bounds.Y, bounds.Width, 30), (Rectangle?)_bgTex.get_Bounds(), Color.get_White() * 0.7f);
				int height = 5;
				foreach (string line in Text.Split("<br>"))
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, line, Font, new Rectangle(0, height, bounds.Width, bounds.Height), Color.get_White(), false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)0);
					height += Font.get_LineHeight();
				}
			}
		}

		private const string BREAKRULE = "<br>";

		private const int MAPWIDTH_MAX = 362;

		private const int MAPHEIGHT_MAX = 338;

		private const int MAPWIDTH_MIN = 170;

		private const int MAPHEIGHT_MIN = 170;

		private const int MAPOFFSET_MIN = 19;

		private CompassRegionDisplay _label;

		private Rectangle _compass;

		private bool _isMouseOver;

		private HashSet<int> _noCompassMaps = new HashSet<int>(new int[3] { 935, 895, 934 });

		public bool IsMouseOver
		{
			get
			{
				return _isMouseOver;
			}
			set
			{
				if (value != _isMouseOver)
				{
					_isMouseOver = value;
					if (_label != null && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
					{
						((TweenerImpl)GameService.Animation.get_Tweener()).Tween<CompassRegionDisplay>(_label, (object)(value ? new
						{
							Opacity = 0f
						} : new
						{
							Opacity = 1f
						}), 0.15f, 0f, true);
					}
				}
			}
		}

		public CompassService()
		{
			GameService.Gw2Mumble.get_UI().add_CompassSizeChanged((EventHandler<ValueEventArgs<Size>>)OnCompassSizeChanged);
			GameService.Gw2Mumble.get_UI().add_IsCompassTopRightChanged((EventHandler<ValueEventArgs<bool>>)OnCompassTopRightChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnMouseMoved);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
		}

		private void OnMapChanged(object sender, ValueEventArgs<int> e)
		{
			if (!HasCompass())
			{
				CompassRegionDisplay label = _label;
				if (label != null)
				{
					((Control)label).Dispose();
				}
				_label = null;
			}
		}

		private void OnMouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			IsMouseOver = ((Rectangle)(ref _compass)).Contains(e.get_MousePosition());
		}

		private void OnIsInGameChanged(object sender, ValueEventArgs<bool> e)
		{
			if (_label != null)
			{
				((Control)_label).set_Visible(e.get_Value());
			}
		}

		private bool HasCompass()
		{
			return !_noCompassMaps.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id());
		}

		public void Show(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				CompassRegionDisplay label = _label;
				if (label != null)
				{
					((Control)label).Dispose();
				}
				_label = null;
			}
			else if (HasCompass())
			{
				if (_label == null)
				{
					CompassRegionDisplay obj = new CompassRegionDisplay
					{
						Font = GameService.Content.get_DefaultFont16()
					};
					((Control)obj).set_Height(20);
					((Control)obj).set_ZIndex(30);
					_label = obj;
				}
				((Control)_label).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				_label.Text = text;
				UpdateCompass();
			}
		}

		private void OnCompassSizeChanged(object sender, ValueEventArgs<Size> e)
		{
			UpdateCompass();
		}

		private void OnCompassTopRightChanged(object sender, ValueEventArgs<bool> e)
		{
			UpdateCompass();
		}

		private void OnMapOpenChanged(object sender, ValueEventArgs<bool> e)
		{
			if (_label != null)
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<CompassRegionDisplay>(_label, (object)(e.get_Value() ? new
				{
					Opacity = 0f
				} : new
				{
					Opacity = 1f
				}), 0.35f, 0f, true);
			}
		}

		private int GetOffset(float curr, float max, float min, float val)
		{
			return (int)Math.Round((curr - min) / (max - min) * (val - 19f) + 19f, 0);
		}

		private void UpdateCompass()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			Size compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			int offsetWidth = GetOffset(((Size)(ref compassSize)).get_Width(), 362f, 170f, 40f);
			compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			int offsetHeight = GetOffset(((Size)(ref compassSize)).get_Height(), 338f, 170f, 40f);
			compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			int width = ((Size)(ref compassSize)).get_Width() + offsetWidth;
			compassSize = GameService.Gw2Mumble.get_UI().get_CompassSize();
			int height = ((Size)(ref compassSize)).get_Height() + offsetHeight;
			int x = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Width - width;
			int y = 0;
			if (!GameService.Gw2Mumble.get_UI().get_IsCompassTopRight())
			{
				y += ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion().Height - height - 40;
			}
			_compass = new Rectangle(x, y, width, height);
			if (_label != null)
			{
				((Control)_label).set_Left(((Rectangle)(ref _compass)).get_Left());
				((Control)_label).set_Top(((Rectangle)(ref _compass)).get_Top());
				((Control)_label).set_Width(_compass.Width);
				((Control)_label).set_Height(_compass.Height);
			}
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_UI().remove_CompassSizeChanged((EventHandler<ValueEventArgs<Size>>)OnCompassSizeChanged);
			GameService.Gw2Mumble.get_UI().remove_IsCompassTopRightChanged((EventHandler<ValueEventArgs<bool>>)OnCompassTopRightChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnMouseMoved);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			CompassRegionDisplay label = _label;
			if (label != null)
			{
				((Control)label).Dispose();
			}
			_label = null;
		}
	}
}
