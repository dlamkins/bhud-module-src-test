using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class TabbedRegionTab
	{
		private readonly DetailedTexture _inactiveHeader = new DetailedTexture(2200567);

		private readonly DetailedTexture _activeHeader = new DetailedTexture(2200566);

		private RectangleDimensions _padding = new RectangleDimensions(5, 2);

		private Rectangle _bounds;

		private Rectangle _iconBounds;

		private Rectangle _textBounds;

		private Func<string> _header;

		private string _title;

		public Container Container { get; set; }

		public bool IsActive { get; set; }

		public Func<string> Header
		{
			get
			{
				return _header;
			}
			set
			{
				_header = value;
				_title = value?.Invoke();
			}
		}

		public Rectangle Bounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _bounds;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				_bounds = value;
				_inactiveHeader.Bounds = value;
				_activeHeader.Bounds = value;
				RecalculateLayout();
			}
		}

		public AsyncTexture2D Icon { get; set; }

		public BitmapFont Font { get; set; } = GameService.Content.DefaultFont18;


		public TabbedRegionTab(Container container)
		{
			Container = container;
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(this, null);
		}

		private void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			_title = Header?.Invoke();
		}

		public bool IsHovered(Point p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = Bounds;
			return ((Rectangle)(ref bounds)).Contains(p);
		}

		public void DrawHeader(Control ctrl, SpriteBatch spriteBatch, Point mousePos)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			Color color = (IsActive ? Color.get_White() : (Color.get_White() * (IsHovered(mousePos) ? 0.9f : 0.6f)));
			((!IsActive) ? _activeHeader : _inactiveHeader).Draw(ctrl, spriteBatch, mousePos, color);
			if (!string.IsNullOrEmpty(_title))
			{
				spriteBatch.DrawStringOnCtrl(ctrl, _title, Font, _textBounds, Color.get_White());
			}
			if (Icon != null)
			{
				spriteBatch.DrawOnCtrl(ctrl, (Texture2D)Icon, _iconBounds, Color.get_White());
			}
		}

		private void RecalculateLayout()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = Bounds;
			int num = ((Rectangle)(ref bounds)).get_Left() + _padding.Left;
			bounds = Bounds;
			_iconBounds = new Rectangle(num, ((Rectangle)(ref bounds)).get_Top() + _padding.Top, Bounds.Height - _padding.Vertical, Bounds.Height - _padding.Vertical);
			_textBounds = new Rectangle(((Rectangle)(ref _iconBounds)).get_Right() + 10, _padding.Top, Bounds.Width - _iconBounds.Width - 10 - _padding.Right, Bounds.Height - _padding.Vertical);
		}
	}
}
