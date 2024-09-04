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
		private AsyncTexture2D _cogWheelIcon;

		private AsyncTexture2D _cogWheelIconHover;

		private AsyncTexture2D _cogWheelIconClick;

		private const int SCROLLBAR_WIDTH = 12;

		public FlowPanel Table { get; private set; }

		public Label PlayerCountLbl { get; private set; }

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
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Expected O, but got Unknown
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Expected O, but got Unknown
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Expected O, but got Unknown
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Expected O, but got Unknown
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_0608: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Expected O, but got Unknown
			//IL_0620: Expected O, but got Unknown
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0725: Expected O, but got Unknown
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_077a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0786: Unknown result type (might be due to invalid IL or missing references)
			//IL_0787: Unknown result type (might be due to invalid IL or missing references)
			//IL_0791: Expected O, but got Unknown
			//IL_0793: Expected O, but got Unknown
			//IL_0798: Unknown result type (might be due to invalid IL or missing references)
			//IL_079d: Unknown result type (might be due to invalid IL or missing references)
			//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Expected O, but got Unknown
			//IL_07b7: Expected O, but got Unknown
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0810: Unknown result type (might be due to invalid IL or missing references)
			//IL_081d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0824: Unknown result type (might be due to invalid IL or missing references)
			//IL_087d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0882: Unknown result type (might be due to invalid IL or missing references)
			//IL_088f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0890: Unknown result type (might be due to invalid IL or missing references)
			//IL_089a: Expected O, but got Unknown
			//IL_089c: Expected O, but got Unknown
			//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08be: Expected O, but got Unknown
			//IL_08c0: Expected O, but got Unknown
			//IL_0900: Unknown result type (might be due to invalid IL or missing references)
			//IL_0905: Unknown result type (might be due to invalid IL or missing references)
			//IL_0912: Unknown result type (might be due to invalid IL or missing references)
			//IL_0913: Unknown result type (might be due to invalid IL or missing references)
			//IL_091d: Expected O, but got Unknown
			//IL_091f: Expected O, but got Unknown
			//IL_093b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0940: Unknown result type (might be due to invalid IL or missing references)
			//IL_094d: Unknown result type (might be due to invalid IL or missing references)
			//IL_094e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0958: Expected O, but got Unknown
			//IL_095a: Expected O, but got Unknown
			//IL_0984: Unknown result type (might be due to invalid IL or missing references)
			//IL_0989: Unknown result type (might be due to invalid IL or missing references)
			//IL_0996: Unknown result type (might be due to invalid IL or missing references)
			//IL_0997: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a1: Expected O, but got Unknown
			//IL_09a3: Expected O, but got Unknown
			//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a14: Expected O, but got Unknown
			//IL_0a16: Expected O, but got Unknown
			//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac2: Expected O, but got Unknown
			//IL_0ac4: Expected O, but got Unknown
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
			Label val4 = new Label();
			((Control)val4).set_Parent(buildPanel);
			val4.set_Text(string.Empty);
			((Control)val4).set_Top(buildPanel.get_ContentRegion().Height - 24);
			((Control)val4).set_Width(buildPanel.get_ContentRegion().Width - 24);
			((Control)val4).set_Height(24);
			val4.set_HorizontalAlignment((HorizontalAlignment)2);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val4).set_BasicTooltipText("Playercount");
			PlayerCountLbl = val4;
			AsyncTexture2D playerIconTex = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(733268);
			Image val5 = new Image(playerIconTex);
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_Width(24);
			((Control)val5).set_Height(24);
			((Control)val5).set_Top(((Control)PlayerCountLbl).get_Top());
			((Control)val5).set_Left(((Control)PlayerCountLbl).get_Right());
			Image playerIconImg = val5;
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent(buildPanel);
			((Control)val6).set_Top(((Control)headerEntry).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			((Control)val6).set_Width(((Control)headerEntry).get_Width() + 12);
			((Control)val6).set_Height(buildPanel.get_ContentRegion().Height - ((Control)search).get_Height() - ((Control)headerEntry).get_Height() - 7 - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y - ((Control)PlayerCountLbl).get_Height());
			((Panel)val6).set_CanScroll(true);
			val6.set_ControlPadding(new Vector2(0f, 7f));
			val6.set_FlowDirection((ControlFlowDirection)3);
			Table = val6;
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
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				((Control)Table).set_Height(e.get_CurrentRegion().Height - ((Control)search).get_Height() - ((Control)headerEntry).get_Height() - 7 - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y - ((Control)PlayerCountLbl).get_Height());
				((Control)PlayerCountLbl).set_Top(buildPanel.get_ContentRegion().Height - 24);
				((Control)PlayerCountLbl).set_Width(buildPanel.get_ContentRegion().Width - 24);
				((Control)playerIconImg).set_Top(((Control)PlayerCountLbl).get_Top());
				((Control)playerIconImg).set_Left(((Control)PlayerCountLbl).get_Right());
			});
			headerEntry.ColumnClick += delegate(object _, ValueEventArgs<int> e)
			{
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
			ContextMenuStrip val7 = new ContextMenuStrip();
			((Control)val7).set_Parent(buildPanel);
			((Control)val7).set_ClipsBounds(false);
			ContextMenuStrip menu = val7;
			ContextMenuStripItem val8 = new ContextMenuStripItem("Always Sort by Status");
			((Control)val8).set_Parent((Container)(object)menu);
			val8.set_CanCheck(true);
			val8.set_Checked(((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().AlwaysSortStatus);
			((Control)val8).set_BasicTooltipText("Sorts players by their online status\nbefore sorting by your selected column.");
			val8.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().AlwaysSortStatus = e.get_Checked();
				base.get_Presenter().SortEntries();
			});
			ContextMenuStripItem val9 = new ContextMenuStripItem("Keep Leavers");
			((Control)val9).set_Parent((Container)(object)menu);
			val9.set_CanCheck(true);
			val9.set_Checked(((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().KeepLeavers);
			((Control)val9).set_BasicTooltipText("Disables the automatic removal of players who have left your party.");
			val9.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().KeepLeavers = e.get_Checked();
			});
			ContextMenuStripItem val10 = new ContextMenuStripItem("Require Profile");
			((Control)val10).set_Parent((Container)(object)menu);
			val10.set_CanCheck(true);
			val10.set_Checked(((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().RequireProfile);
			((Control)val10).set_BasicTooltipText("Only players with a profile are added when they join your party.");
			val10.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().RequireProfile = e.get_Checked();
			});
			ContextMenuStripItem val11 = new ContextMenuStripItem("Color Grading Mode");
			((Control)val11).set_Parent((Container)(object)menu);
			val11.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem colorGradingModeCategory = val11;
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
				ContextMenuStripItem val12 = new ContextMenuStripItem(mode.ToString().SplitCamelCase());
				((Control)val12).set_Parent((Container)(object)colorGradingModeCategory.get_Submenu());
				val12.set_CanCheck(true);
				val12.set_Checked(((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().ColorGradingMode == mode);
				((Control)val12).set_BasicTooltipText("Highlight low amounts by comparison to " + suffixTooltip + ".");
				ContextMenuStripItem colorGradingMode = val12;
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
			ContextMenuStripItem val13 = new ContextMenuStripItem("Columns");
			((Control)val13).set_Parent((Container)(object)menu);
			val13.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem columnsCategory = val13;
			ContextMenuStripItem val14 = new ContextMenuStripItem("Identifier");
			((Control)val14).set_Parent((Container)(object)columnsCategory.get_Submenu());
			val14.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem identityCategory = val14;
			foreach (TableConfig.Column col in Enum.GetValues(typeof(TableConfig.Column)).Cast<TableConfig.Column>())
			{
				ContextMenuStripItem val15 = new ContextMenuStripItem(col.ToString().SplitCamelCase());
				((Control)val15).set_Parent((Container)(object)identityCategory.get_Submenu());
				val15.set_CanCheck(true);
				val15.set_Checked(((IEnumerable<TableConfig.Column>)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().Columns).Any((TableConfig.Column c) => c == col));
				val15.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
				{
					if (e.get_Checked())
					{
						((Collection<TableConfig.Column>)(object)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().Columns).Add(col);
						GameService.Content.PlaySoundEffectByName("color-change");
					}
					else
					{
						((IList<TableConfig.Column>)((Presenter<TableView, TableConfig>)base.get_Presenter()).get_Model().Columns).RemoveAll((Enum)col);
					}
				});
			}
			ContextMenuStripItem val16 = new ContextMenuStripItem("Proofs");
			((Control)val16).set_Parent((Container)(object)columnsCategory.get_Submenu());
			val16.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem proofsCategory = val16;
			ContextMenuStripItem val17 = new ContextMenuStripItem("Generic");
			((Control)val17).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val17.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem generalCategory = val17;
			AddProofEntries(generalCategory, from x in ProofLogix.Instance.Resources.GetGeneralItems()
				where x.Id != 93781 && x.Id != 88485
				select x);
			ContextMenuStripItem val18 = new ContextMenuStripItem("Fractals");
			((Control)val18).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val18.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem fractalsCategory = val18;
			AddProofEntries(fractalsCategory, ProofLogix.Instance.Resources.GetItemsForFractals());
			ContextMenuStripItem val19 = new ContextMenuStripItem("Raids");
			((Control)val19).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val19.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem raidsCategory = val19;
			AddProofEntries(raidsCategory, ProofLogix.Instance.Resources.GetItems(88485));
			ContextMenuStripItem val20 = new ContextMenuStripItem("Coffers");
			((Control)val20).set_Parent((Container)(object)raidsCategory.get_Submenu());
			val20.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem coffersCategory = val20;
			AddProofEntries(coffersCategory, ProofLogix.Instance.Resources.GetCofferItems());
			int i = 1;
			foreach (Raid.Wing wing in ProofLogix.Instance.Resources.GetWings())
			{
				ContextMenuStripItem val21 = new ContextMenuStripItem($"Wing {i++}");
				((Control)val21).set_Parent((Container)(object)raidsCategory.get_Submenu());
				val21.set_Submenu(new ContextMenuStrip());
				ContextMenuStripItem wingEntry = val21;
				AddProofEntries(wingEntry, from ev in wing.Events
					where ev.Token != null
					select ev.Token);
			}
			((Control)cogWheel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				GameService.Content.PlaySoundEffectByName("button-click");
				menu.Show(GameService.Input.get_Mouse().get_Position());
			});
			ContextMenuStripItem val22 = new ContextMenuStripItem("Strikes");
			((Control)val22).set_Parent((Container)(object)proofsCategory.get_Submenu());
			val22.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem strikesCategory = val22;
			AddProofEntries(strikesCategory, ProofLogix.Instance.Resources.GetItems(93781).Concat(ProofLogix.Instance.Resources.GetItemsForStrikes()));
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
					}
				});
			}
		}
	}
}
