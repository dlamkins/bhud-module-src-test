using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListView : View<IItemListPresenter>
	{
		private Panel _noResultsFavoritesPanel;

		private Label _noResultsFavoritesLabel;

		private Image _noResultsFavoritesImage;

		private Panel _noResultsPanel;

		private Label _noResultsLabel;

		private Label _noResultsFiltersLabel;

		private Image _noResultsImage;

		private StandardButton _noResultsClearFilterButton;

		public EventHandler<EventArgs> ItemClicked;

		private Label _limitLabel;

		private bool _noResults;

		public ViewContainer Container { get; set; }

		public FlowPanel ResultsPanel { get; set; }

		public ItemListModel Model { get; set; }

		public bool NoResults
		{
			get
			{
				return _noResults;
			}
			set
			{
				_noResults = value;
				NoResultsChanged(_noResults);
			}
		}

		public Container BuildPanel { get; set; }

		public ItemListView(ItemListModel itemListModel)
		{
			Model = itemListModel;
			base.WithPresenter((IItemListPresenter)new ItemListPresenter(this, itemListModel));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			BuildPanel = buildPanel;
			ViewContainer val = new ViewContainer();
			val.set_FadeView(true);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height()));
			((Control)val).set_Padding(new Thickness(0f));
			((Control)val).set_Location(new Point(0, 0));
			Container = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)Container);
			((Control)val2).set_Location(new Point(0, 38));
			((Control)val2).set_Width(((Control)Container).get_Width() - 5);
			((Control)val2).set_Height(((Control)Container).get_Height() - 50);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_CanScroll(true);
			ResultsPanel = val2;
			BuildHeaderPanel(((Control)ResultsPanel).get_Width(), (Container)(object)Container);
			BuildNoResultsPanel((Container)(object)Container);
			BuildNoResultsFavoritesPanel((Container)(object)Container);
		}

		public Panel BuildHeaderPanel(int width, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(parent);
			val.set_ShowBorder(false);
			val.set_ShowTint(false);
			val.set_BackgroundTexture(AsyncTexture2D.FromAssetId(1032325));
			((Control)val).set_Size(new Point(width, 36));
			Panel headerPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)headerPanel);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White() * 0.7f);
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_Text(MysticCrafting.Module.Strings.Discovery.TableHeaderName);
			((Control)val2).set_Location(new Point(50, 8));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)headerPanel);
			val3.set_Font(GameService.Content.get_DefaultFont16());
			val3.set_TextColor(Color.get_White() * 0.7f);
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			val3.set_Text(MysticCrafting.Module.Strings.Discovery.TableHeaderType);
			((Control)val3).set_Location(new Point(340, 8));
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)headerPanel);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			val4.set_TextColor(Color.get_White() * 0.7f);
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			val4.set_Text(MysticCrafting.Module.Strings.Discovery.TableHeaderWeight);
			((Control)val4).set_Location(new Point(485, 8));
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)headerPanel);
			val5.set_Font(GameService.Content.get_DefaultFont16());
			val5.set_TextColor(Color.get_White() * 0.7f);
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			val5.set_Text(MysticCrafting.Module.Strings.Discovery.TableHeaderSkin);
			((Control)val5).set_Location(new Point(((Control)headerPanel).get_Width() - 230, 8));
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)headerPanel);
			val6.set_Font(GameService.Content.get_DefaultFont16());
			val6.set_TextColor(Color.get_White() * 0.7f);
			val6.set_AutoSizeHeight(true);
			val6.set_AutoSizeWidth(true);
			val6.set_Text(MysticCrafting.Module.Strings.Discovery.TableHeaderCount);
			((Control)val6).set_Location(new Point(((Control)headerPanel).get_Width() - 150, 8));
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)headerPanel);
			val7.set_Font(GameService.Content.get_DefaultFont16());
			val7.set_TextColor(Color.get_White() * 0.7f);
			val7.set_AutoSizeHeight(true);
			val7.set_AutoSizeWidth(true);
			val7.set_Text(MysticCrafting.Module.Strings.Discovery.TableHeaderFavorite);
			((Control)val7).set_Location(new Point(((Control)headerPanel).get_Width() - 80, 8));
			return headerPanel;
		}

		private void NoResultsChanged(bool noResults)
		{
			Panel noResultsPanel = _noResultsPanel;
			if (noResultsPanel != null)
			{
				((Control)noResultsPanel).Hide();
			}
			Panel noResultsFavoritesPanel = _noResultsFavoritesPanel;
			if (noResultsFavoritesPanel != null)
			{
				((Control)noResultsFavoritesPanel).Hide();
			}
			Panel resultsPanel = (Model.Filter.IsFavorite ? _noResultsFavoritesPanel : _noResultsPanel);
			if (noResults)
			{
				FlowPanel resultsPanel2 = ResultsPanel;
				if (resultsPanel2 != null)
				{
					((Control)resultsPanel2).Hide();
				}
				if (resultsPanel != null)
				{
					((Control)resultsPanel).Show();
				}
			}
			else
			{
				((Control)ResultsPanel).Show();
			}
		}

		public void BuildNoResultsPanel(Container parent)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Expected O, but got Unknown
			Panel noResultsPanel = _noResultsPanel;
			if (noResultsPanel != null)
			{
				((Container)noResultsPanel).ClearChildren();
			}
			Panel noResultsPanel2 = _noResultsPanel;
			if (noResultsPanel2 != null)
			{
				((Control)noResultsPanel2).Dispose();
			}
			Panel val = new Panel();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(((Control)parent).get_Size());
			((Control)val).set_Visible(false);
			_noResultsPanel = val;
			Label val2 = new Label();
			val2.set_Text(MysticCrafting.Module.Strings.Discovery.NoResults);
			((Control)val2).set_Parent((Container)(object)_noResultsPanel);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_StrokeText(true);
			((Control)val2).set_Location(new Point(250, 200));
			_noResultsLabel = val2;
			Label val3 = new Label();
			val3.set_Text(MysticCrafting.Module.Strings.Discovery.NoResultsCheckFilters);
			((Control)val3).set_Parent((Container)(object)_noResultsPanel);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_TextColor(Color.get_White() * 0.8f);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			val3.set_StrokeText(true);
			((Control)val3).set_Location(new Point(265, 245));
			_noResultsLabel = val3;
		}

		public void BuildNoResultsFavoritesPanel(Container parent)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			Panel noResultsFavoritesPanel = _noResultsFavoritesPanel;
			if (noResultsFavoritesPanel != null)
			{
				((Container)noResultsFavoritesPanel).ClearChildren();
			}
			Panel noResultsFavoritesPanel2 = _noResultsFavoritesPanel;
			if (noResultsFavoritesPanel2 != null)
			{
				((Control)noResultsFavoritesPanel2).Dispose();
			}
			Panel val = new Panel();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(((Control)parent).get_Size());
			((Control)val).set_Visible(false);
			_noResultsFavoritesPanel = val;
			Label val2 = new Label();
			val2.set_Text(Common.FavoritesEmpty);
			((Control)val2).set_Parent((Container)(object)_noResultsFavoritesPanel);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_StrokeText(true);
			((Control)val2).set_Location(new Point(120, 200));
			_noResultsFavoritesLabel = val2;
			Image val3 = new Image();
			val3.set_Texture(ServiceContainer.TextureRepository.Textures.HeartDisabled);
			((Control)val3).set_Parent((Container)(object)_noResultsFavoritesPanel);
			((Control)val3).set_Size(new Point(50, 50));
			((Control)val3).set_Location(new Point(350, 300));
			_noResultsFavoritesImage = val3;
		}

		public void SetItemRows(IList<ItemRowView> rows)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			((Container)ResultsPanel).ClearChildren();
			foreach (ItemRowView row in rows)
			{
				ViewContainer val = new ViewContainer();
				((Control)val).set_Size(new Point(((Control)ResultsPanel).get_Width() - 25, 45));
				((Control)val).set_Parent((Container)(object)ResultsPanel);
				val.Show((IView)(object)row);
			}
		}

		public void SetLimitLabel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)ResultsPanel);
			val.set_ShowBorder(false);
			val.set_ShowTint(false);
			((Control)val).set_Size(new Point(800, 60));
			Panel container = val;
			Label val2 = new Label();
			val2.set_Text(Common.MaximumResultsReached);
			((Control)val2).set_Parent((Container)(object)container);
			((Control)val2).set_Left(200);
			((Control)val2).set_Height(50);
			val2.set_AutoSizeWidth(true);
			val2.set_TextColor(Color.get_Orange());
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_StrokeText(true);
			_limitLabel = val2;
		}
	}
}
