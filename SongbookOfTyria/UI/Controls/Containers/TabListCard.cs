using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;

namespace SongbookOfTyria.UI.Controls.Containers
{
	public class TabListCard : Panel
	{
		public const int DefaultCardHeight = 60;

		private const int ThumbnailSize = 44;

		private const int ThumbnailLeft = 8;

		private const int ThumbnailTop = 8;

		private const int TextLeft = 60;

		private const int TitleTop = 10;

		private const int SubtitleTop = 32;

		private const int FavoriteIconSize = 24;

		private const int FavoriteIconRightMargin = 42;

		private readonly MusicTab _musicTab;

		private readonly TextureService _textureService;

		private readonly UserSettingsService _userSettingsService;

		private string _decodedTitle;

		private string _decodedSubtitle;

		private AsyncTexture2D _thumbnailTexture;

		private AsyncTexture2D _favoriteTexture;

		private AsyncTexture2D _privateIconTexture;

		private AsyncTexture2D _practiceModeIconTexture;

		private ScrollingHighlightEffect _scrollEffect;

		private bool _isFavorite;

		private bool _isSelected;

		private int _cachedIndex = -1;

		private Rectangle _favoriteIconBounds;

		private Rectangle _practiceModeIconBounds;

		private Rectangle _privateIconBounds;

		private bool _texturesLoaded;

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					UpdateSelectedState();
				}
			}
		}

		public bool IsFavorite
		{
			get
			{
				return _isFavorite;
			}
			private set
			{
				if (_isFavorite != value)
				{
					_isFavorite = value;
					if (_texturesLoaded)
					{
						UpdateFavoriteTexture();
					}
				}
			}
		}

		public event EventHandler<MusicTab> CardClicked;

		public event EventHandler<MusicTab> FavoriteToggled;

		public TabListCard(MusicTab musicTab, TextureService textureService, UserSettingsService userSettingsService, Container parent)
			: this()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			_musicTab = musicTab;
			_textureService = textureService;
			_userSettingsService = userSettingsService;
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Control)this).set_Height(60);
			((Control)this).set_Parent(parent);
			_isFavorite = _userSettingsService?.IsFavorite(_musicTab.Id) ?? false;
			_scrollEffect = new ScrollingHighlightEffect((Control)(object)this);
			((Control)this).set_EffectBehind((ControlEffect)(object)_scrollEffect);
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnCardClick);
		}

		private void EnsureTexturesLoaded()
		{
			if (!_texturesLoaded)
			{
				_texturesLoaded = true;
				_decodedTitle = WebUtility.HtmlDecode(_musicTab.Name ?? string.Empty);
				if (string.IsNullOrEmpty(_decodedTitle))
				{
					_decodedTitle = "Unknown Tab";
				}
				_decodedSubtitle = WebUtility.HtmlDecode(BuildSubtitleText()) ?? string.Empty;
				if (!string.IsNullOrEmpty(_musicTab.Thumbnail))
				{
					_thumbnailTexture = _textureService?.GetRemoteTexture(_musicTab.Thumbnail);
				}
				if (_musicTab.IsPrivate)
				{
					_privateIconTexture = AsyncTexture2D.FromAssetId(733265);
				}
				if (_musicTab.PracticeMode)
				{
					_practiceModeIconTexture = AsyncTexture2D.FromAssetId(528696);
				}
				UpdateFavoriteTexture();
			}
		}

		private string BuildSubtitleText()
		{
			StringBuilder sb = new StringBuilder(64);
			if (!string.IsNullOrEmpty(_musicTab.Genre))
			{
				sb.Append(_musicTab.Genre);
			}
			List<string> tabType = _musicTab.TabType;
			if (tabType != null && tabType.Count > 0)
			{
				if (sb.Length > 0)
				{
					sb.Append(" • ");
				}
				sb.Append(string.Join(", ", _musicTab.TabType));
			}
			if (!string.IsNullOrEmpty(_musicTab.TabbedBy))
			{
				if (sb.Length > 0)
				{
					sb.Append(" • ");
				}
				sb.Append(_musicTab.TabbedBy);
			}
			return sb.ToString();
		}

		private void UpdateFavoriteTexture()
		{
			if (_textureService != null)
			{
				_favoriteTexture = (_isFavorite ? _textureService.GetFavoriteFilledIcon() : _textureService.GetFavoriteEmptyIcon());
			}
		}

		private string TruncateText(string text, BitmapFont font, int maxWidth)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(text) || font.MeasureString(text).Width <= (float)maxWidth)
			{
				return text;
			}
			float ellipsisWidth = font.MeasureString("...").Width;
			for (int i = text.Length - 1; i > 0; i--)
			{
				string truncated = text.Substring(0, i);
				if (font.MeasureString(truncated).Width + ellipsisWidth <= (float)maxWidth)
				{
					return truncated + "...";
				}
			}
			return "...";
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			EnsureTexturesLoaded();
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (ShouldDrawDarkStripe())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.3f);
			}
			AsyncTexture2D thumbnailTexture = _thumbnailTexture;
			if (thumbnailTexture != null && thumbnailTexture.get_HasTexture())
			{
				Rectangle thumbnailBounds = default(Rectangle);
				((Rectangle)(ref thumbnailBounds))._002Ector(8, 8, 44, 44);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_thumbnailTexture), thumbnailBounds);
			}
			BitmapFont titleFont = GameService.Content.get_DefaultFont16();
			int maxTitleWidth = ((Control)this).get_Width() - 60 - 110;
			string displayTitle = TruncateText(_decodedTitle, titleFont, maxTitleWidth);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, displayTitle, titleFont, new Rectangle(60, 10, maxTitleWidth, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			int titleWidth = (int)titleFont.MeasureString(displayTitle).Width;
			int iconOffset = 60 + titleWidth + 4;
			AsyncTexture2D practiceModeIconTexture = _practiceModeIconTexture;
			if (practiceModeIconTexture != null && practiceModeIconTexture.get_HasTexture())
			{
				_practiceModeIconBounds = new Rectangle(iconOffset, 10, 20, 20);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_practiceModeIconTexture), _practiceModeIconBounds);
				iconOffset += 24;
			}
			else
			{
				_practiceModeIconBounds = Rectangle.get_Empty();
			}
			AsyncTexture2D privateIconTexture = _privateIconTexture;
			if (privateIconTexture != null && privateIconTexture.get_HasTexture())
			{
				_privateIconBounds = new Rectangle(iconOffset, 10, 20, 20);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_privateIconTexture), _privateIconBounds);
			}
			else
			{
				_privateIconBounds = Rectangle.get_Empty();
			}
			BitmapFont subtitleFont = GameService.Content.get_DefaultFont14();
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _decodedSubtitle, subtitleFont, new Rectangle(60, 32, ((Control)this).get_Width() - 60 - 80, 18), Color.get_LightGray(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			AsyncTexture2D favoriteTexture = _favoriteTexture;
			if (favoriteTexture != null && favoriteTexture.get_HasTexture())
			{
				int iconX = ((Control)this).get_Width() - 24 - 42;
				int iconY = (((Control)this).get_Height() - 24) / 2;
				_favoriteIconBounds = new Rectangle(iconX, iconY, 24, 24);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_favoriteTexture), _favoriteIconBounds);
			}
		}

		private bool ShouldDrawDarkStripe()
		{
			if (((Control)this).get_Parent() == null)
			{
				return false;
			}
			if (_cachedIndex < 0)
			{
				_cachedIndex = 0;
				foreach (Control child in ((Control)this).get_Parent().get_Children())
				{
					if (child == this)
					{
						break;
					}
					if (child is TabListCard)
					{
						_cachedIndex++;
					}
				}
			}
			return _cachedIndex % 2 == 0;
		}

		public void InvalidateStripeIndex()
		{
			_cachedIndex = -1;
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseEntered(e);
			((ControlEffect)_scrollEffect).Enable();
			UpdateTooltipForMousePosition(e.get_MousePosition());
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMoved(e);
			UpdateTooltipForMousePosition(e.get_MousePosition());
		}

		private void UpdateTooltipForMousePosition(Point mousePosition)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			Point relativePos = default(Point);
			((Point)(ref relativePos))._002Ector(mousePosition.X - ((Control)this).get_AbsoluteBounds().X, mousePosition.Y - ((Control)this).get_AbsoluteBounds().Y);
			if (_practiceModeIconBounds != Rectangle.get_Empty() && ((Rectangle)(ref _practiceModeIconBounds)).Contains(relativePos))
			{
				((Control)this).set_BasicTooltipText("Practice Mode Available");
			}
			else if (_privateIconBounds != Rectangle.get_Empty() && ((Rectangle)(ref _privateIconBounds)).Contains(relativePos))
			{
				((Control)this).set_BasicTooltipText("Private Tab");
			}
			else if (_favoriteIconBounds != Rectangle.get_Empty() && ((Rectangle)(ref _favoriteIconBounds)).Contains(relativePos))
			{
				((Control)this).set_BasicTooltipText(_isFavorite ? "Remove from Favorites" : "Add to Favorites");
			}
			else
			{
				((Control)this).set_BasicTooltipText(_musicTab.Name + "\nClick to view notation");
			}
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			if (!_isSelected)
			{
				((ControlEffect)_scrollEffect).Disable();
			}
		}

		private void UpdateSelectedState()
		{
			_scrollEffect.set_ForceActive(_isSelected);
		}

		private void OnCardClick(object sender, MouseEventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			Point relativeMousePos = default(Point);
			((Point)(ref relativeMousePos))._002Ector(e.get_MousePosition().X - ((Control)this).get_AbsoluteBounds().X, e.get_MousePosition().Y - ((Control)this).get_AbsoluteBounds().Y);
			int iconX = ((Control)this).get_Width() - 24 - 42;
			int iconY = (((Control)this).get_Height() - 24) / 2;
			Rectangle favoriteHitArea = default(Rectangle);
			((Rectangle)(ref favoriteHitArea))._002Ector(iconX, iconY, 24, 24);
			if (((Rectangle)(ref favoriteHitArea)).Contains(relativeMousePos))
			{
				_userSettingsService?.ToggleFavorite(_musicTab.Id);
				IsFavorite = _userSettingsService?.IsFavorite(_musicTab.Id) ?? false;
				this.FavoriteToggled?.Invoke(this, _musicTab);
			}
			else
			{
				this.CardClicked?.Invoke(this, _musicTab);
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)OnCardClick);
			((Control)this).set_EffectBehind((ControlEffect)null);
			_scrollEffect = null;
			_thumbnailTexture = null;
			_favoriteTexture = null;
			_privateIconTexture = null;
			_practiceModeIconTexture = null;
			((Panel)this).DisposeControl();
		}
	}
}
