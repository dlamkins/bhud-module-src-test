using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.Services.PartySync.Models;
using Nekres.ProofLogix.Core.UI.Configs;

namespace Nekres.ProofLogix.Core.UI.Table
{
	public class TableView : View<TablePresenter>
	{
		public FlowPanel Table;

		private AsyncTexture2D _cogWheelIcon;

		private AsyncTexture2D _cogWheelIconHover;

		private AsyncTexture2D _cogWheelIconClick;

		private const int SCROLLBAR_WIDTH = 12;

		public TableView(TableConfig config)
		{
			base.WithPresenter(new TablePresenter(this, config));
			_cogWheelIcon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155052);
			_cogWheelIconHover = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(157110);
			_cogWheelIconClick = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(157109);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Expected O, but got Unknown
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Expected O, but got Unknown
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Expected O, but got Unknown
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Expected O, but got Unknown
			//IL_0453: Expected O, but got Unknown
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0558: Expected O, but got Unknown
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Expected O, but got Unknown
			//IL_05c5: Expected O, but got Unknown
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Expected O, but got Unknown
			//IL_05e8: Expected O, but got Unknown
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ca: Expected O, but got Unknown
			//IL_06cc: Expected O, but got Unknown
			//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Expected O, but got Unknown
			//IL_06f0: Expected O, but got Unknown
			//IL_070d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0712: Unknown result type (might be due to invalid IL or missing references)
			//IL_071f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0720: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Expected O, but got Unknown
			//IL_072c: Expected O, but got Unknown
			//IL_0749: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_075b: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0766: Expected O, but got Unknown
			//IL_0768: Expected O, but got Unknown
			//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c2: Expected O, but got Unknown
			//IL_07c4: Expected O, but got Unknown
			//IL_083c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0841: Unknown result type (might be due to invalid IL or missing references)
			//IL_084e: Unknown result type (might be due to invalid IL or missing references)
			//IL_084f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0859: Expected O, but got Unknown
			//IL_085b: Expected O, but got Unknown
			Image val = new Image(_cogWheelIcon);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(32);
			((Control)val).set_Height(32);
			Image cogWheel = val;
			((Control)cogWheel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIconHover);
			});
			((Control)cogWheel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIcon);
			});
			((Control)cogWheel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIconClick);
			});
			((Control)cogWheel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIconHover);
			});
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(200);
			((Control)val2).set_Height(32);
			((Control)val2).set_Left(((Control)cogWheel).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
			((TextInputBase)val2).set_Font(GameService.Content.get_DefaultFont18());
			((TextInputBase)val2).set_PlaceholderText("Search..");
			((Control)val2).set_BasicTooltipText("Guild Wars 2 account, character name or \nKillproof.me identifier.");
			TextBox search = val2;
			string notFound = "Not found.";
			Point size = LabelUtil.GetLabelSize(GameService.Content.get_DefaultFont18(), notFound, hasPrefix: true);
			FormattedLabel notFoundLabel = new FormattedLabelBuilder().SetWidth(size.X).SetHeight(size.Y).CreatePart(notFound, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				o.SetTextColor(Color.get_Yellow());
				o.SetPrefixImage(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("common/1444522")));
			})
				.Build();
			((Control)notFoundLabel).set_Parent(buildPanel);
			((Control)notFoundLabel).set_Left(((Control)search).get_Right() + 4);
			((Control)notFoundLabel).set_Top(((Control)search).get_Top() + (((Control)search).get_Height() - ((Control)notFoundLabel).get_Height()) / 2);
			((Control)notFoundLabel).set_Visible(false);
			LoadingSpinner val3 = new LoadingSpinner();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Width(size.Y);
			((Control)val3).set_Height(size.Y);
			((Control)val3).set_Left(((Control)search).get_Right() + 4);
			((Control)val3).set_Top(((Control)search).get_Top() + (((Control)search).get_Height() - size.Y) / 2);
			((Control)val3).set_Visible(false);
			LoadingSpinner loading = val3;
			search.add_EnterPressed((EventHandler<EventArgs>)async delegate
			{
				if (!string.IsNullOrEmpty(((TextInputBase)search).get_Text()))
				{
					((Control)loading).set_Visible(true);
					string query = (string)((TextInputBase)search).get_Text().Clone();
					Profile profile = await ProofLogix.Instance.KpWebApi.GetProfile(query);
					if (profile.NotFound)
					{
						profile = await ProofLogix.Instance.KpWebApi.GetProfileByCharacter(query);
					}
					if (profile.NotFound)
					{
						((Control)loading).set_Visible(false);
						((Control)notFoundLabel).set_Visible(((TextInputBase)search).get_Text().Equals(query));
						ScreenNotification.ShowNotification(notFound, (NotificationType)1, (Texture2D)null, 4);
						GameService.Content.PlaySoundEffectByName("error");
					}
					else
					{
						((Control)loading).set_Visible(false);
						if (((TextInputBase)search).get_Text().Equals(query))
						{
							((TextInputBase)search).set_Text(string.Empty);
						}
						ProofLogix.Instance.PartySync.AddKpProfile(profile);
					}
				}
			});
			((TextInputBase)search).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				((Control)notFoundLabel).set_Visible(false);
			});
			TableHeaderEntry tableHeaderEntry = new TableHeaderEntry();
			((Control)tableHeaderEntry).set_Parent(buildPanel);
			((Control)tableHeaderEntry).set_Top(((Control)search).get_Bottom() + 7);
			((Control)tableHeaderEntry).set_Height(32);
			TableHeaderEntry headerEntry = tableHeaderEntry;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Top(((Control)headerEntry).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			((Control)val4).set_Width(((Control)headerEntry).get_Width() + 12);
			((Control)val4).set_Height(buildPanel.get_ContentRegion().Height - ((Control)search).get_Height() - ((Control)headerEntry).get_Height() - 7 - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			((Panel)val4).set_CanScroll(true);
			val4.set_ControlPadding(new Vector2(0f, 7f));
			val4.set_FlowDirection((ControlFlowDirection)3);
			Table = val4;
			((Control)headerEntry).add_Resized((EventHandler<ResizedEventArgs>)delegate(object _, ResizedEventArgs e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				((Control)Table).set_Width(e.get_CurrentSize().X + 12);
				((Control)buildPanel).set_Width(((Control)Table).get_Width() + (((Control)buildPanel).get_Width() - buildPanel.get_ContentRegion().Width) - 6);
			});
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				((Control)Table).set_Height(e.get_CurrentRegion().Height - ((Control)search).get_Height() - ((Control)headerEntry).get_Height() - 7 - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			});
			headerEntry.ColumnClick += delegate(object _, ValueEventArgs<int> e)
			{
				ProofLogix.Instance.Resources.PlayMenuItemClick();
				((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().SelectedColumn = e.get_Value();
				((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().OrderDescending = !((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().OrderDescending;
				base.get_Presenter().SortEntries();
			};
			base.get_Presenter().CreatePlayerEntry(ProofLogix.Instance.PartySync.LocalPlayer);
			foreach (Player player in ProofLogix.Instance.PartySync.PlayerList)
			{
				base.get_Presenter().CreatePlayerEntry(player);
			}
			base.get_Presenter().SortEntries();
			ContextMenuStrip val5 = new ContextMenuStrip();
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_ClipsBounds(false);
			ContextMenuStrip menu = val5;
			ContextMenuStripItem val6 = new ContextMenuStripItem("Color Grading Mode");
			((Control)val6).set_Parent((Container)(object)menu);
			val6.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem colorGradingModeCategory = val6;
			List<ContextMenuStripItem> colorGradingModeEntries = new List<ContextMenuStripItem>();
			foreach (PartySyncService.ColorGradingMode mode in Enum.GetValues(typeof(PartySyncService.ColorGradingMode)).Cast<PartySyncService.ColorGradingMode>())
			{
				string suffixTooltip = mode switch
				{
					PartySyncService.ColorGradingMode.LocalPlayerComparison => "your own", 
					PartySyncService.ColorGradingMode.MedianComparison => "the median", 
					PartySyncService.ColorGradingMode.LargestComparison => "the largest", 
					PartySyncService.ColorGradingMode.AverageComparison => "the average", 
					_ => string.Empty, 
				};
				ContextMenuStripItem val7 = new ContextMenuStripItem(mode.ToString().SplitCamelCase());
				((Control)val7).set_Parent((Container)(object)colorGradingModeCategory.get_Submenu());
				val7.set_CanCheck(true);
				val7.set_Checked(((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().ColorGradingMode == mode);
				((Control)val7).set_BasicTooltipText("Highlight low amounts by comparison to " + suffixTooltip + ".");
				ContextMenuStripItem colorGradingMode = val7;
				colorGradingMode.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
				{
					if (!e.get_Checked())
					{
						if (colorGradingModeEntries.All((ContextMenuStripItem x) => !x.get_Checked()))
						{
							colorGradingMode.set_Checked(true);
						}
					}
					else
					{
						foreach (ContextMenuStripItem item in colorGradingModeEntries.Where((ContextMenuStripItem x) => x != o))
						{
							item.set_Checked(false);
						}
						((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().ColorGradingMode = mode;
						GameService.Content.PlaySoundEffectByName("color-change");
					}
				});
				colorGradingModeEntries.Add(colorGradingMode);
			}
			ContextMenuStripItem val8 = new ContextMenuStripItem("Columns");
			((Control)val8).set_Parent((Container)(object)menu);
			val8.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem columnsCategory = val8;
			ContextMenuStripItem val9 = new ContextMenuStripItem("Identifier");
			((Control)val9).set_Parent((Container)(object)columnsCategory.get_Submenu());
			val9.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem identityCategory = val9;
			foreach (TableConfig.Column col in Enum.GetValues(typeof(TableConfig.Column)).Cast<TableConfig.Column>())
			{
				ContextMenuStripItem val10 = new ContextMenuStripItem(col.ToString().SplitCamelCase());
				((Control)val10).set_Parent((Container)(object)identityCategory.get_Submenu());
				val10.set_CanCheck(true);
				val10.set_Checked(((IEnumerable<TableConfig.Column>)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().Columns).Any((TableConfig.Column c) => c == col));
				val10.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
				{
					if (e.get_Checked())
					{
						((Collection<TableConfig.Column>)(object)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().Columns).Add(col);
						GameService.Content.PlaySoundEffectByName("color-change");
					}
					else
					{
						((IList<TableConfig.Column>)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().Columns).RemoveAll((Enum)col);
						ProofLogix.Instance.Resources.PlayMenuItemClick();
					}
				});
			}
			ContextMenuStripItem val11 = new ContextMenuStripItem("Proofs");
			((Control)val11).set_Parent((Container)(object)columnsCategory.get_Submenu());
			val11.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem proofsCategory = val11;
			ContextMenuStripItem val12 = new ContextMenuStripItem("General");
			((Control)val12).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val12.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem generalCategory = val12;
			AddProofEntries(generalCategory, ProofLogix.Instance.Resources.GetGeneralItems());
			ContextMenuStripItem val13 = new ContextMenuStripItem("Coffers");
			((Control)val13).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val13.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem coffersCategory = val13;
			AddProofEntries(coffersCategory, ProofLogix.Instance.Resources.GetCofferItems());
			ContextMenuStripItem val14 = new ContextMenuStripItem("Raids");
			((Control)val14).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val14.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem raidsCategory = val14;
			int i = 1;
			foreach (Raid.Wing wing in ProofLogix.Instance.Resources.GetWings())
			{
				ContextMenuStripItem val15 = new ContextMenuStripItem($"Wing {i++}");
				((Control)val15).set_Parent((Container)(object)raidsCategory.get_Submenu());
				val15.set_Submenu(new ContextMenuStrip());
				ContextMenuStripItem wingEntry = val15;
				AddProofEntries(wingEntry, from ev in wing.Events
					where ev.Token != null
					select ev.Token);
			}
			ContextMenuStripItem val16 = new ContextMenuStripItem("Fractals");
			((Control)val16).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val16.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem fractalsCategory = val16;
			AddProofEntries(fractalsCategory, ProofLogix.Instance.Resources.GetItemsForFractals());
			((Control)cogWheel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				GameService.Content.PlaySoundEffectByName("button-click");
				menu.Show(GameService.Input.get_Mouse().get_Position());
			});
			base.Build(buildPanel);
		}

		private void AddProofEntries(ContextMenuStripItem parent, IEnumerable<Resource> resources)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			foreach (Resource resource in resources)
			{
				ContextMenuStripItemWithColor contextMenuStripItemWithColor = new ContextMenuStripItemWithColor(resource.Name);
				((Control)contextMenuStripItemWithColor).set_Parent((Container)(object)parent.get_Submenu());
				((ContextMenuStripItem)contextMenuStripItemWithColor).set_CanCheck(true);
				((ContextMenuStripItem)contextMenuStripItemWithColor).set_Checked(((IEnumerable<int>)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().TokenIds).Any((int id) => id == resource.Id));
				contextMenuStripItemWithColor.TextColor = ProofLogix.Instance.Resources.GetItem(resource.Id).Rarity.AsColor();
				((ContextMenuStripItem)contextMenuStripItemWithColor).add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
				{
					if (e.get_Checked())
					{
						((Collection<int>)(object)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().TokenIds).Add(resource.Id);
						GameService.Content.PlaySoundEffectByName("color-change");
					}
					else
					{
						((IList<int>)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().TokenIds).RemoveAll(resource.Id);
						ProofLogix.Instance.Resources.PlayMenuItemClick();
					}
				});
			}
		}
	}
}
