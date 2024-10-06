using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class TabbedRegion : Container
	{
		private readonly AsyncTexture2D _inactiveHeader = AsyncTexture2D.FromAssetId(2200566);

		private readonly AsyncTexture2D _activeHeader = AsyncTexture2D.FromAssetId(2200567);

		private readonly AsyncTexture2D _separator = AsyncTexture2D.FromAssetId(156055);

		private readonly Panel _contentPanel;

		private TabbedRegionTab _activeTab;

		private ObservableCollection<TabbedRegionTab> _tabs = new ObservableCollection<TabbedRegionTab>();

		private new Rectangle _contentRegion;

		private Rectangle _headerRegion;

		private RectangleDimensions _headerPading = new RectangleDimensions(0, 8);

		private RectangleDimensions _contentPadding = new RectangleDimensions(0, 4);

		private BitmapFont _headerFont = Control.Content.DefaultFont18;

		public TabbedRegionTab ActiveTab
		{
			get
			{
				return _activeTab;
			}
			set
			{
				SwitchTab(value);
			}
		}

		public ObservableCollection<TabbedRegionTab> Tabs => _tabs;

		public BitmapFont HeaderFont
		{
			get
			{
				return _headerFont;
			}
			set
			{
				_headerFont = value ?? Control.Content.DefaultFont18;
				foreach (TabbedRegionTab tab in _tabs)
				{
					tab.Font = _headerFont;
				}
			}
		}

		public RectangleDimensions ContentPadding
		{
			get
			{
				return _contentPadding;
			}
			set
			{
				_contentPadding = value;
			}
		}

		public Action OnTabSwitched { get; set; }

		public TabbedRegion()
		{
			_tabs.CollectionChanged += Tab_CollectionChanged;
			_contentPanel = new Panel
			{
				Parent = this
			};
		}

		private void Tab_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_activeTab == null)
			{
				SwitchTab(_tabs.FirstOrDefault());
			}
			RecalculateLayout();
		}

		public void AddTab(TabbedRegionTab tab)
		{
			tab.Container.Visible = false;
			tab.Container.Parent = _contentPanel;
			_tabs.Add(tab);
		}

		public void AddTab(Container tab)
		{
			_tabs.Add(new TabbedRegionTab(tab));
		}

		public void RemoveTab(Container tab)
		{
			Container tab2 = tab;
			_tabs.ToList().RemoveAll((TabbedRegionTab e) => e.Container == tab2);
		}

		public void SwitchTab(TabbedRegionTab tab)
		{
			if (tab != null)
			{
				tab.Container.Visible = true;
				_activeTab = tab;
				_activeTab.Container?.Invalidate();
			}
			foreach (TabbedRegionTab item in _tabs)
			{
				item.IsActive = item == _activeTab;
				item.Container.Visible = item.IsActive;
			}
			OnTabSwitched?.Invoke();
			RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_headerRegion = new Rectangle(0, 0, base.Width, HeaderFont.get_LineHeight() + _headerPading.Vertical);
			_contentRegion = new Rectangle(_contentPadding.Left, ((Rectangle)(ref _headerRegion)).get_Bottom() + _contentPadding.Top, base.Width - _contentPadding.Horizontal, base.Height - ((Rectangle)(ref _headerRegion)).get_Bottom() - _contentPadding.Vertical);
			if (_contentPanel != null)
			{
				_contentPanel.Location = ((Rectangle)(ref _contentRegion)).get_Location();
				_contentPanel.Size = ((Rectangle)(ref _contentRegion)).get_Size();
			}
			int tabHeaderWidth = base.Width / Math.Max(1, _tabs.Count);
			for (int i = 0; i < _tabs.Count; i++)
			{
				_tabs[i].Bounds = new Rectangle(((Rectangle)(ref _headerRegion)).get_Left() + i * tabHeaderWidth, ((Rectangle)(ref _headerRegion)).get_Top(), tabHeaderWidth, _headerRegion.Height);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			for (int i = 0; i < _tabs.Count; i++)
			{
				_tabs[i].DrawHeader(this, spriteBatch, base.RelativeMousePosition);
			}
			spriteBatch.DrawOnCtrl((Control)this, (Texture2D)_separator, new Rectangle(((Rectangle)(ref _headerRegion)).get_Left(), ((Rectangle)(ref _headerRegion)).get_Bottom() - 9, _headerRegion.Width, 16), Color.get_Black());
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			for (int i = 0; i < _tabs.Count; i++)
			{
				if (_tabs[i].IsHovered(base.RelativeMousePosition))
				{
					SwitchTab(_tabs[i]);
					break;
				}
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_tabs.CollectionChanged -= Tab_CollectionChanged;
		}
	}
}
