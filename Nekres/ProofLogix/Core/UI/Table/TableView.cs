using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
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
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Expected O, but got Unknown
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Expected O, but got Unknown
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Expected O, but got Unknown
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Expected O, but got Unknown
			//IL_0461: Expected O, but got Unknown
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Expected O, but got Unknown
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Expected O, but got Unknown
			//IL_05c6: Expected O, but got Unknown
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Expected O, but got Unknown
			//IL_05e9: Expected O, but got Unknown
			//IL_063d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_06af: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cb: Expected O, but got Unknown
			//IL_06cd: Expected O, but got Unknown
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ef: Expected O, but got Unknown
			//IL_06f1: Expected O, but got Unknown
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0713: Unknown result type (might be due to invalid IL or missing references)
			//IL_0720: Unknown result type (might be due to invalid IL or missing references)
			//IL_0721: Unknown result type (might be due to invalid IL or missing references)
			//IL_072b: Expected O, but got Unknown
			//IL_072d: Expected O, but got Unknown
			//IL_074a: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_075d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0767: Expected O, but got Unknown
			//IL_0769: Expected O, but got Unknown
			//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c3: Expected O, but got Unknown
			//IL_07c5: Expected O, but got Unknown
			//IL_083d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0842: Unknown result type (might be due to invalid IL or missing references)
			//IL_084f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0850: Unknown result type (might be due to invalid IL or missing references)
			//IL_085a: Expected O, but got Unknown
			//IL_085c: Expected O, but got Unknown
			Image val = new Image(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155052));
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
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			foreach (Resource resource in resources)
			{
				ContextMenuStripItem val = new ContextMenuStripItem(resource.Name);
				((Control)val).set_Parent((Container)(object)parent.get_Submenu());
				val.set_CanCheck(true);
				val.set_Checked(((IEnumerable<int>)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().TokenIds).Any((int id) => id == resource.Id));
				val.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
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
