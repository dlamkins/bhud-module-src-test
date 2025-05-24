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
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Expected O, but got Unknown
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Expected O, but got Unknown
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
				Image info2 = val3;
				chatLinkWidth -= ((Control)info2).get_Width();
				if (!viewModel2.Locked)
				{
					_detailsButton.set_MaxFill(viewModel2.Achievement.Tiers[0].Count);
					_detailsButton.set_ShowFillFraction(true);
				}
			}
			else if ((object)viewModel2.Progress == null)
			{
				if (viewModel2.IsPerCharacter)
				{
					Image val4 = new Image();
					((Control)val4).set_Parent((Container)(object)toolbar);
					((Control)val4).set_Size(new Point(32));
					val4.set_Texture(AsyncTexture2D.FromAssetId(1508665));
					((Control)val4).set_BasicTooltipText(ViewModel.PerCharacterAchievementProgressUnavailable);
					Image info = val4;
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
			TextBox val5 = new TextBox();
			((Control)val5).set_Parent((Container)(object)toolbar);
			((Control)val5).set_Height(35);
			((Control)val5).set_Width(chatLinkWidth);
			((TextInputBase)val5).set_Text(viewModel2.ChatLink);
			val5.set_HideBackground(true);
			_chatLink = val5;
			((TextInputBase)_chatLink).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)ChatLinkFocusChanged);
			GlowButton val6 = new GlowButton();
			((Control)val6).set_Parent((Container)(object)toolbar);
			val6.set_Icon(AsyncTexture2D.FromAssetId(2208345));
			val6.set_ActiveIcon(AsyncTexture2D.FromAssetId(2208347));
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)OnCopyClicked);
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
