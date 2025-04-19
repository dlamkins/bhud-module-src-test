using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Achievements.Tooltips;
using SL.Common;
using SL.Common.Controls;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	public sealed class AchievementTile : Container
	{
		private readonly DetailsButton _detailsButton;

		private readonly TextBox _chatLink = new TextBox();

		public AchievementTileViewModel ViewModel { get; }

		public AchievementTile(AchievementTileViewModel viewModel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Expected O, but got Unknown
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Expected O, but got Unknown
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Expected O, but got Unknown
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Expected O, but got Unknown
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Expected O, but got Unknown
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Expected O, but got Unknown
			AchievementTileViewModel viewModel2 = viewModel;
			((Container)this)._002Ector();
			AchievementTile achievementTile = this;
			ThrowHelper.ThrowIfNull(viewModel2, "viewModel");
			ViewModel = viewModel2;
			((Control)this).set_Width(320);
			((Control)this).set_Height(90);
			DetailsButton val = new DetailsButton();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_Text(viewModel2.Name);
			_detailsButton = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)_detailsButton);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)2);
			val2.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel toolbar = val2;
			int chatLinkWidth = 170;
			_detailsButton.set_Icon((AsyncTexture2D)(viewModel2.Locked ? ((object)AsyncTexture2D.FromAssetId(240704)) : ((object)(viewModel2.GetIcon() ?? AsyncTexture2D.FromAssetId(155865)))));
			if (viewModel2.Progression == null)
			{
				Image val3 = new Image();
				((Control)val3).set_Parent((Container)(object)toolbar);
				((Control)val3).set_Size(new Point(32));
				val3.set_Texture(AsyncTexture2D.FromAssetId(1508665));
				((Control)val3).set_BasicTooltipText(ViewModel.AchievementProgressUnavailable);
				Image info5 = val3;
				chatLinkWidth -= ((Control)info5).get_Width();
				if (!viewModel2.Locked)
				{
					_detailsButton.set_MaxFill(viewModel2.Achievement.Tiers[0].Count);
					_detailsButton.set_ShowFillFraction(true);
				}
			}
			else if ((object)viewModel2.Progress == null)
			{
				if (ViewModel.IsDaily)
				{
					Image val4 = new Image();
					((Control)val4).set_Parent((Container)(object)toolbar);
					((Control)val4).set_Size(new Point(32));
					val4.set_Texture(AsyncTexture2D.FromAssetId(1508665));
					((Control)val4).set_BasicTooltipText(ViewModel.DailyAchievementProgressUnavailable);
					Image info4 = val4;
					chatLinkWidth -= ((Control)info4).get_Width();
				}
				else if (viewModel2.IsWeekly)
				{
					Image val5 = new Image();
					((Control)val5).set_Parent((Container)(object)toolbar);
					((Control)val5).set_Size(new Point(32));
					val5.set_Texture(AsyncTexture2D.FromAssetId(1508665));
					((Control)val5).set_BasicTooltipText(ViewModel.WeeklyAchievementProgressUnavailable);
					Image info3 = val5;
					chatLinkWidth -= ((Control)info3).get_Width();
				}
				else if (viewModel2.IsMonthly)
				{
					Image val6 = new Image();
					((Control)val6).set_Parent((Container)(object)toolbar);
					((Control)val6).set_Size(new Point(32));
					val6.set_Texture(AsyncTexture2D.FromAssetId(1508665));
					((Control)val6).set_BasicTooltipText(ViewModel.MonthlyAchievementProgressUnavailable);
					Image info2 = val6;
					chatLinkWidth -= ((Control)info2).get_Width();
				}
				else if (viewModel2.IsPerCharacter)
				{
					Image val7 = new Image();
					((Control)val7).set_Parent((Container)(object)toolbar);
					((Control)val7).set_Size(new Point(32));
					val7.set_Texture(AsyncTexture2D.FromAssetId(1508665));
					((Control)val7).set_BasicTooltipText(ViewModel.PerCharacterAchievementProgressUnavailable);
					Image info = val7;
					chatLinkWidth -= ((Control)info).get_Width();
				}
				else if (!viewModel2.Locked)
				{
					_detailsButton.set_MaxFill(viewModel2.Achievement.Tiers[0].Count);
					_detailsButton.set_ShowFillFraction(true);
				}
			}
			else if (!viewModel2.Progress!.Done || viewModel2.Achievement.Flags.Repeatable)
			{
				_detailsButton.set_MaxFill(viewModel2.Progress!.Max);
				_detailsButton.set_CurrentFill(viewModel2.Progress!.Current);
				_detailsButton.set_ShowFillFraction(true);
			}
			else
			{
				_detailsButton.set_ShowVignette(false);
				_detailsButton.set_IconDetails(viewModel2.CompletedLabel);
			}
			if (ViewModel.Achievement.Flags.Hidden)
			{
				StackedImage stackedImage = new StackedImage();
				((Control)stackedImage).set_Parent((Container)(object)toolbar);
				((Control)stackedImage).set_Size(new Point(32));
				((Control)stackedImage).set_BasicTooltipText(viewModel2.HiddenLabel);
				StackedImage eye = stackedImage;
				eye.Textures.Add((AsyncTexture2D.FromAssetId(528726), Color.get_White()));
				if ((object)viewModel2.Progress == null)
				{
					eye.Textures.Add((AsyncTexture2D.FromAssetId(154983), Color.get_OrangeRed()));
				}
				chatLinkWidth -= ((Control)eye).get_Width();
			}
			TextBox val8 = new TextBox();
			((Control)val8).set_Parent((Container)(object)toolbar);
			((Control)val8).set_Height(35);
			((Control)val8).set_Width(chatLinkWidth);
			((TextInputBase)val8).set_Text(viewModel2.ChatLink);
			val8.set_HideBackground(true);
			_chatLink = val8;
			((TextInputBase)_chatLink).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)ChatLinkFocusChanged);
			GlowButton val9 = new GlowButton();
			((Control)val9).set_Parent((Container)(object)toolbar);
			val9.set_Icon(AsyncTexture2D.FromAssetId(2208345));
			val9.set_ActiveIcon(AsyncTexture2D.FromAssetId(2208347));
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)OnCopyClicked);
			((Control)this).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[4]
			{
				achievementTile.ViewModel.CopyNameCommand.ToMenuItem(() => achievementTile.ViewModel.CopyNameLabel),
				achievementTile.ViewModel.CopyChatLinkCommand.ToMenuItem(() => viewModel2.CopyChatLinkLabel),
				achievementTile.ViewModel.OpenWikiCommand.ToMenuItem(() => viewModel2.OpenWikiLabel),
				achievementTile.ViewModel.OpenApiCommand.ToMenuItem(() => viewModel2.OpenApiLabel)
			}))));
		}

		private void OnCopyClicked(object sender, MouseEventArgs e)
		{
			Soundboard.Click();
			ViewModel.CopyChatLinkCommand.Execute();
		}

		private void ChatLinkFocusChanged(object sender, ValueEventArgs<bool> args)
		{
			if (args.get_Value())
			{
				((TextInputBase)_chatLink).set_SelectionStart(0);
				((TextInputBase)_chatLink).set_SelectionEnd(((TextInputBase)_chatLink).get_Length());
			}
			else
			{
				((TextInputBase)_chatLink).set_SelectionStart(((TextInputBase)_chatLink).get_SelectionEnd());
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0034: Expected O, but got Unknown
			if (((Control)this).get_MouseOver())
			{
				DetailsButton detailsButton = _detailsButton;
				if (((Control)detailsButton).get_Tooltip() == null)
				{
					Tooltip val = new Tooltip((ITooltipView)(object)new AchievementTooltipView(ViewModel.CreateAchievementTooltipViewModel()));
					Tooltip val2 = val;
					((Control)detailsButton).set_Tooltip(val);
				}
			}
			else
			{
				Tooltip tooltip = ((Control)_detailsButton).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)_detailsButton).set_Tooltip((Tooltip)null);
			}
			((Container)this).UpdateContainer(gameTime);
		}

		protected override void DisposeControl()
		{
			ViewModel.Dispose();
			((Container)this).DisposeControl();
		}
	}
}
