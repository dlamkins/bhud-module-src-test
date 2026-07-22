using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Views
{
	[Obsolete]
	public class BaseTabbedWindow : WindowBase
	{
		private readonly AsyncTexture2D _rawbackground = AsyncTexture2D.FromAssetId(155997);

		private readonly AsyncTexture2D _textureSplitLine = AsyncTexture2D.FromAssetId(605024);

		private readonly Texture2D _textureBlackFade = Control.Content.GetTexture("fade-down-46");

		private readonly Texture2D _textureTabActive = Control.Content.GetTexture("window-tab-active");

		private readonly int _tabHeight = 52;

		private readonly int _tabWidth = 104;

		private readonly int _tabIconSize = 32;

		private readonly int _tabSectionWidth = 46;

		private readonly int _windowContentWidth = 500;

		private readonly int _windowContentHeight = 640;

		private readonly Rectangle _standardTabBounds;

		private readonly Dictionary<BaseTab, Rectangle> _tabRegions = new Dictionary<BaseTab, Rectangle>();

		private Rectangle _layoutTopTabBarBounds;

		private Rectangle _layoutBottomTabBarBounds;

		private Rectangle _layoutTopSplitLineBounds;

		private Rectangle _layoutBottomSplitLineBounds;

		private Rectangle _layoutTopSplitLineSourceBounds;

		private Rectangle _layoutBottomSplitLineSourceBounds;

		private List<BaseTab> _tabs = new List<BaseTab>();

		private Texture2D _background;

		private Texture2D _tabBarBackground;

		private int _selectedTabIndex = -1;

		public BaseTab SelectedTab
		{
			get
			{
				if (_tabs.Count <= _selectedTabIndex)
				{
					return null;
				}
				return _tabs[_selectedTabIndex];
			}
		}

		public int SelectedTabIndex
		{
			get
			{
				return _selectedTabIndex;
			}
			set
			{
				if (SetProperty(ref _selectedTabIndex, value, invalidateLayout: true, "SelectedTabIndex"))
				{
					OnTabChanged();
				}
			}
		}

		private int HoveredTabIndex
		{
			[CompilerGenerated]
			get
			{
				return _003CHoveredTabIndex_003Ek__BackingField;
			}
			set
			{
				SetProperty(ref _003CHoveredTabIndex_003Ek__BackingField, value, invalidateLayout: false, "HoveredTabIndex");
			}
		}

		public BaseTabbedWindow()
		{
			_standardTabBounds = new Rectangle(_tabSectionWidth, 24, _tabWidth, _tabHeight);
			_background = _rawbackground.Texture.GetRegion(0, 0, _rawbackground.Width, _rawbackground.Height);
			_tabBarBackground = _background.Duplicate().SetRegion(0, 0, 64, _background.Height, Color.Transparent);
			ConstructWindow(null, new Vector2(0f), new Rectangle(0, 0, _windowContentWidth + 64, _windowContentHeight + 30), new Thickness(30f, 75f, 45f, 25f), 40);
			_contentRegion = new Rectangle(_tabWidth / 2, 48, _windowContentWidth, _windowContentHeight);
			_rawbackground.TextureSwapped += Background_TextureSwapped;
		}

		private void Background_TextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			ApplyBackground();
		}

		private void ApplyBackground()
		{
			_background = _rawbackground.Texture.GetRegion(0, 0, _rawbackground.Width, _rawbackground.Height);
			_tabBarBackground = _background.Duplicate().SetRegion(0, 0, 64, _background.Height, Color.Transparent);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (_tabs.Count != 0)
			{
				Rectangle firstTabBounds = TabBoundsFromIndex(0);
				Rectangle selectedTabBounds = _tabRegions[SelectedTab];
				Rectangle lastTabBounds = TabBoundsFromIndex(_tabRegions.Count - 1);
				_layoutTopTabBarBounds = new Rectangle(0, 0, _tabSectionWidth, firstTabBounds.Top);
				_layoutBottomTabBarBounds = new Rectangle(0, lastTabBounds.Bottom, _tabSectionWidth, _size.Y - lastTabBounds.Bottom);
				int topSplitHeight = selectedTabBounds.Top - base.ContentRegion.Top;
				int bottomSplitHeight = base.ContentRegion.Bottom - selectedTabBounds.Bottom;
				_layoutTopSplitLineBounds = new Rectangle(base.ContentRegion.X - _textureSplitLine.Width + 1, base.ContentRegion.Y, _textureSplitLine.Width, topSplitHeight);
				_layoutTopSplitLineSourceBounds = new Rectangle(0, 0, _textureSplitLine.Width, topSplitHeight);
				_layoutBottomSplitLineBounds = new Rectangle(base.ContentRegion.X - _textureSplitLine.Width + 1, selectedTabBounds.Bottom, _textureSplitLine.Width, bottomSplitHeight);
				_layoutBottomSplitLineSourceBounds = new Rectangle(0, _textureSplitLine.Height - bottomSplitHeight, _textureSplitLine.Width, bottomSplitHeight);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.DrawOnCtrl(this, _tabBarBackground, _tabBarBackground.Bounds.Add(-20, 5, 0, 0), _tabBarBackground.Bounds);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _layoutTopTabBarBounds, Color.Black);
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawOnCtrl(this, _textureBlackFade, _layoutBottomTabBarBounds);
			int i = 0;
			foreach (BaseTab tab in _tabs)
			{
				bool active = i == SelectedTabIndex;
				bool hovered = i == HoveredTabIndex;
				Rectangle tabBounds = _tabRegions[tab];
				Rectangle subBounds = new Rectangle(tabBounds.X + tabBounds.Width / 2, tabBounds.Y, _tabWidth / 2, tabBounds.Height);
				if (active)
				{
					spriteBatch.DrawOnCtrl(this, _background, tabBounds, tabBounds.OffsetBy(_windowBackgroundOrigin.ToPoint()).OffsetBy(1, -5).Add(0, 0, 0, 0)
						.Add(tabBounds.Width / 3 + 20, 0, -tabBounds.Width / 3, 0), Color.White);
					spriteBatch.DrawOnCtrl(this, _textureTabActive, tabBounds);
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, tabBounds.Y, _tabSectionWidth, tabBounds.Height), Color.Black);
				}
				spriteBatch.DrawOnCtrl(this, tab.Icon, new Rectangle(_tabWidth / 4 - _tabIconSize / 2 + 2, _tabHeight / 2 - _tabIconSize / 2, _tabIconSize, _tabIconSize).OffsetBy(subBounds.Location), (active || hovered) ? Color.White : ContentService.Colors.DullColor);
				i++;
			}
			spriteBatch.DrawOnCtrl(this, _textureSplitLine, _layoutTopSplitLineBounds, _layoutTopSplitLineSourceBounds);
			spriteBatch.DrawOnCtrl(this, _textureSplitLine, _layoutBottomSplitLineBounds, _layoutBottomSplitLineSourceBounds);
		}

		public void AddTab(BaseTab tab)
		{
			if (tab != null)
			{
				BaseTab prevTab = ((_tabs.Count > 0) ? _tabs[SelectedTabIndex] : tab);
				tab.CreateLayout(this, _windowContentWidth - 20);
				_tabs.Add(tab);
				_tabRegions.Add(tab, TabBoundsFromIndex(_tabRegions.Count));
				_tabs = _tabs.OrderBy((BaseTab t) => t.Priority).ToList();
				for (int i = 0; i < _tabs.Count; i++)
				{
					_tabRegions[_tabs[i]] = TabBoundsFromIndex(i);
				}
				SwitchTab(prevTab);
				Invalidate();
			}
		}

		public void RemoveTab(BaseTab tab)
		{
			_tabs.Remove(tab);
		}

		public void SwitchTab(BaseTab tab)
		{
			_selectedTabIndex = _tabs.IndexOf(tab);
			_subtitle = tab.Name;
			RecalculateLayout();
			Show();
		}

		protected override void PaintWindowBackground(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			if (base.RelativeMousePosition.X < _standardTabBounds.Right && base.RelativeMousePosition.Y > _standardTabBounds.Y)
			{
				List<BaseTab> tabList = _tabs.ToList();
				for (int tabIndex = 0; tabIndex < _tabs.Count; tabIndex++)
				{
					BaseTab tab = tabList[tabIndex];
					if (_tabRegions[tab].Contains(base.RelativeMousePosition))
					{
						SwitchTab(tab);
						break;
					}
				}
				tabList.Clear();
			}
			base.OnLeftMouseButtonPressed(e);
		}

		protected virtual void OnTabChanged(ValueChangedEventArgs<BaseTab> tab)
		{
		}

		private void OnTabChanged()
		{
		}

		private Rectangle TabBoundsFromIndex(int index)
		{
			return _standardTabBounds.OffsetBy(-_tabWidth, base.ContentRegion.Y + index * _tabHeight);
		}
	}
}
