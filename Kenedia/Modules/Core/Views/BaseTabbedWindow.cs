using System;
using System.Collections.Generic;
using System.Linq;
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

		private readonly Texture2D _textureBlackFade = Control.get_Content().GetTexture("fade-down-46");

		private readonly Texture2D _textureTabActive = Control.get_Content().GetTexture("window-tab-active");

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

		private int _hoveredTabIndex;

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
				if (((Control)this).SetProperty<int>(ref _selectedTabIndex, value, true, "SelectedTabIndex"))
				{
					OnTabChanged();
				}
			}
		}

		private int HoveredTabIndex
		{
			get
			{
				return _hoveredTabIndex;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _hoveredTabIndex, value, false, "HoveredTabIndex");
			}
		}

		public BaseTabbedWindow()
			: this()
		{
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			_standardTabBounds = new Rectangle(_tabSectionWidth, 24, _tabWidth, _tabHeight);
			_background = Texture2DExtension.GetRegion(_rawbackground.get_Texture(), 0, 0, _rawbackground.get_Width(), _rawbackground.get_Height());
			_tabBarBackground = Texture2DExtension.SetRegion(Texture2DExtension.Duplicate(_background), 0, 0, 64, _background.get_Height(), Color.get_Transparent());
			((WindowBase)this).ConstructWindow((Texture2D)null, new Vector2(0f), (Rectangle?)new Rectangle(0, 0, _windowContentWidth + 64, _windowContentHeight + 30), new Thickness(30f, 75f, 45f, 25f), 40, true);
			((Container)this)._contentRegion = new Rectangle(_tabWidth / 2, 48, _windowContentWidth, _windowContentHeight);
			_rawbackground.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Background_TextureSwapped);
		}

		private void Background_TextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			ApplyBackground();
		}

		private void ApplyBackground()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			_background = Texture2DExtension.GetRegion(_rawbackground.get_Texture(), 0, 0, _rawbackground.get_Width(), _rawbackground.get_Height());
			_tabBarBackground = Texture2DExtension.SetRegion(Texture2DExtension.Duplicate(_background), 0, 0, 64, _background.get_Height(), Color.get_Transparent());
		}

		public override void RecalculateLayout()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase)this).RecalculateLayout();
			if (_tabs.Count != 0)
			{
				Rectangle firstTabBounds = TabBoundsFromIndex(0);
				Rectangle selectedTabBounds = _tabRegions[SelectedTab];
				Rectangle lastTabBounds = TabBoundsFromIndex(_tabRegions.Count - 1);
				_layoutTopTabBarBounds = new Rectangle(0, 0, _tabSectionWidth, ((Rectangle)(ref firstTabBounds)).get_Top());
				_layoutBottomTabBarBounds = new Rectangle(0, ((Rectangle)(ref lastTabBounds)).get_Bottom(), _tabSectionWidth, ((Control)this)._size.Y - ((Rectangle)(ref lastTabBounds)).get_Bottom());
				int top = ((Rectangle)(ref selectedTabBounds)).get_Top();
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				int topSplitHeight = top - ((Rectangle)(ref contentRegion)).get_Top();
				contentRegion = ((Container)this).get_ContentRegion();
				int bottomSplitHeight = ((Rectangle)(ref contentRegion)).get_Bottom() - ((Rectangle)(ref selectedTabBounds)).get_Bottom();
				_layoutTopSplitLineBounds = new Rectangle(((Container)this).get_ContentRegion().X - _textureSplitLine.get_Width() + 1, ((Container)this).get_ContentRegion().Y, _textureSplitLine.get_Width(), topSplitHeight);
				_layoutTopSplitLineSourceBounds = new Rectangle(0, 0, _textureSplitLine.get_Width(), topSplitHeight);
				_layoutBottomSplitLineBounds = new Rectangle(((Container)this).get_ContentRegion().X - _textureSplitLine.get_Width() + 1, ((Rectangle)(ref selectedTabBounds)).get_Bottom(), _textureSplitLine.get_Width(), bottomSplitHeight);
				_layoutBottomSplitLineSourceBounds = new Rectangle(0, _textureSplitLine.get_Height() - bottomSplitHeight, _textureSplitLine.get_Width(), bottomSplitHeight);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _tabBarBackground, RectangleExtension.Add(_tabBarBackground.get_Bounds(), -20, 5, 0, 0), (Rectangle?)_tabBarBackground.get_Bounds());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _layoutTopTabBarBounds, Color.get_Black());
			((WindowBase)this).PaintBeforeChildren(spriteBatch, bounds);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureBlackFade, _layoutBottomTabBarBounds);
			int i = 0;
			Rectangle subBounds = default(Rectangle);
			foreach (BaseTab tab in _tabs)
			{
				bool active = i == SelectedTabIndex;
				bool hovered = i == HoveredTabIndex;
				Rectangle tabBounds = _tabRegions[tab];
				((Rectangle)(ref subBounds))._002Ector(tabBounds.X + tabBounds.Width / 2, tabBounds.Y, _tabWidth / 2, tabBounds.Height);
				if (active)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _background, tabBounds, (Rectangle?)RectangleExtension.Add(RectangleExtension.Add(RectangleExtension.OffsetBy(RectangleExtension.OffsetBy(tabBounds, ((Vector2)(ref base._windowBackgroundOrigin)).ToPoint()), 1, -5), 0, 0, 0, 0), tabBounds.Width / 3 + 20, 0, -tabBounds.Width / 3, 0), Color.get_White());
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureTabActive, tabBounds);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, tabBounds.Y, _tabSectionWidth, tabBounds.Height), Color.get_Black());
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(tab.Icon), RectangleExtension.OffsetBy(new Rectangle(_tabWidth / 4 - _tabIconSize / 2 + 2, _tabHeight / 2 - _tabIconSize / 2, _tabIconSize, _tabIconSize), ((Rectangle)(ref subBounds)).get_Location()), (active || hovered) ? Color.get_White() : Colors.DullColor);
				i++;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureSplitLine), _layoutTopSplitLineBounds, (Rectangle?)_layoutTopSplitLineSourceBounds);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureSplitLine), _layoutBottomSplitLineBounds, (Rectangle?)_layoutBottomSplitLineSourceBounds);
		}

		public void AddTab(BaseTab tab)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			if (tab != null)
			{
				BaseTab prevTab = ((_tabs.Count > 0) ? _tabs[SelectedTabIndex] : tab);
				tab.CreateLayout((Container)(object)this, _windowContentWidth - 20);
				_tabs.Add(tab);
				_tabRegions.Add(tab, TabBoundsFromIndex(_tabRegions.Count));
				_tabs = _tabs.OrderBy((BaseTab t) => t.Priority).ToList();
				for (int i = 0; i < _tabs.Count; i++)
				{
					_tabRegions[_tabs[i]] = TabBoundsFromIndex(i);
				}
				SwitchTab(prevTab);
				((Control)this).Invalidate();
			}
		}

		public void RemoveTab(BaseTab tab)
		{
			_tabs.Remove(tab);
		}

		public void SwitchTab(BaseTab tab)
		{
			_selectedTabIndex = _tabs.IndexOf(tab);
			base._subtitle = tab.Name;
			((Control)this).RecalculateLayout();
			((Control)this).Show();
		}

		protected override void PaintWindowBackground(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			int x = ((Control)this).get_RelativeMousePosition().X;
			Rectangle val = _standardTabBounds;
			if (x < ((Rectangle)(ref val)).get_Right() && ((Control)this).get_RelativeMousePosition().Y > _standardTabBounds.Y)
			{
				List<BaseTab> tabList = _tabs.ToList();
				for (int tabIndex = 0; tabIndex < _tabs.Count; tabIndex++)
				{
					BaseTab tab = tabList[tabIndex];
					val = _tabRegions[tab];
					if (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()))
					{
						SwitchTab(tab);
						break;
					}
				}
				tabList.Clear();
			}
			((WindowBase)this).OnLeftMouseButtonPressed(e);
		}

		protected virtual void OnTabChanged(ValueChangedEventArgs<BaseTab> tab)
		{
		}

		private void OnTabChanged()
		{
		}

		private Rectangle TabBoundsFromIndex(int index)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			return RectangleExtension.OffsetBy(_standardTabBounds, -_tabWidth, ((Container)this).get_ContentRegion().Y + index * _tabHeight);
		}
	}
}
