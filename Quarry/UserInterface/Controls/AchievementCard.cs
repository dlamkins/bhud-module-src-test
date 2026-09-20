using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Quarry.Interfaces;
using Quarry.Models;
using Quarry.Services;
using Quarry.WikiData.Achievement;

namespace Quarry.UserInterface.Controls
{
	public class AchievementCard : Container
	{
		private const int IconSize = 56;

		private const int Pad = 8;

		private const int TierEdgeHeight = 2;

		private const int HideGlyphSize = 16;

		private const int EyeSize = 22;

		private const int PipHeight = 4;

		private const int PipGap = 2;

		private const int MaxDiscretePips = 40;

		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IAchievementService achievementService;

		private readonly ITextureService textureService;

		private readonly IHuntService huntService;

		private readonly AchievementTableEntry achievement;

		private readonly GuidanceInfo guidance;

		private readonly IHereCardActions hereCardActions;

		private readonly int? rank;

		private readonly AsyncTexture2D icon;

		private readonly bool isComplete;

		private readonly (int Current, int Max, string Text, string RemainingText, double Fraction) progress;

		private readonly string placeText;

		private readonly bool placeIsBold;

		private readonly string title;

		private GlowButton eye;

		private Label hideLabel;

		private Label badgeLabel;

		private ContextMenuStrip hereMenu;

		private bool hovering;

		public AchievementCard(AchievementTableEntry achievement, IAchievementTrackerService achievementTrackerService, IAchievementService achievementService, ITextureService textureService, IHuntService huntService, INearestObjectiveService nearestObjectiveService, ICurrentMapService currentMapService, string icon, GuidanceInfo guidance, IHereCardActions hereCardActions, int? rank)
			: this()
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			this.achievement = achievement;
			this.achievementTrackerService = achievementTrackerService;
			this.achievementService = achievementService;
			this.textureService = textureService;
			this.huntService = huntService;
			this.guidance = guidance ?? GuidanceInfo.None;
			this.hereCardActions = hereCardActions;
			this.rank = rank;
			this.icon = this.textureService.GetTexture(icon);
			isComplete = this.achievementService.HasFinishedAchievement(achievement.Id);
			IReadOnlyList<RemainingObjective> nearest = Array.Empty<RemainingObjective>();
			if (nearestObjectiveService.HasAnyObjectives(achievement.Id) && this.achievementService.PlayerAchievements != null)
			{
				int mapId = currentMapService.MapId;
				Vector3 player = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
				nearest = nearestObjectiveService.GetRemaining(achievement.Id, mapId, player);
			}
			progress = AchievementProgress.Get(achievementService, achievement, nearest);
			if (nearest.Count == 0)
			{
				placeText = "no route on this map";
				placeIsBold = false;
			}
			else
			{
				RemainingObjective top = nearest[0];
				string distance = string.Format("{0:F0} m{1}", top.DistanceMetres, top.GroundDistanceOnly ? " (ground)" : string.Empty);
				string place = top.AreaHint ?? top.Name;
				placeText = place + " · " + distance;
				placeIsBold = true;
			}
			((Control)this).set_Height(92);
			title = achievement.Name.Trim();
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				hovering = true;
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				hovering = false;
			});
			if (!isComplete)
			{
				((Control)this).add_Click((EventHandler<MouseEventArgs>)AchievementCard_Click);
			}
			((Control)this).set_BasicTooltipText(progress.RemainingText);
			this.achievementTrackerService.AchievementUntracked += Tracker_AchievementUntracked;
			BuildChildren();
		}

		private void BuildChildren()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Expected O, but got Unknown
			if (guidance.Tier != 0)
			{
				bool canPeek = huntService.CanPeek;
				bool packBacked = guidance.Tier == GuidanceTier.Tagged || guidance.Tier == GuidanceTier.Route;
				string description = guidance.Describe();
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(guidance.Label);
				val.set_Font(UiStyle.BodyFont);
				val.set_HorizontalAlignment((HorizontalAlignment)2);
				((Control)val).set_Width(90);
				((Control)val).set_Height(16);
				badgeLabel = val;
				UiStyle.ApplyShadow(badgeLabel, guidance.Color);
				if (canPeek && packBacked)
				{
					((Control)badgeLabel).set_BasicTooltipText(description + "\n\nClick to show this route in Pathing.");
					((Control)badgeLabel).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						huntService.Peek(achievement.Id);
					});
				}
				else
				{
					((Control)badgeLabel).set_BasicTooltipText(description);
				}
			}
			if (hereCardActions != null)
			{
				BuildHereCardActions();
			}
			if (!isComplete)
			{
				GlowButton val2 = new GlowButton();
				((Control)val2).set_Parent((Container)(object)this);
				((Control)val2).set_Width(22);
				((Control)val2).set_Height(22);
				val2.set_ActiveIcon(textureService.GetRefTexture("track_enabled.png"));
				val2.set_Icon(textureService.GetRefTexture("track_disabled.png"));
				val2.set_ToggleGlow(true);
				val2.set_Checked(achievementTrackerService.IsBeingTracked(achievement.Id));
				((Control)val2).set_BasicTooltipText(achievementTrackerService.IsBeingTracked(achievement.Id) ? "Drop this" : "Target this");
				eye = val2;
			}
		}

		private void BuildHereCardActions()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			bool isHidden = hereCardActions.IsHidden(achievement.Id);
			if (isHidden)
			{
				((Control)this).set_Opacity(0.55f);
				((Control)this).set_BasicTooltipText(hereCardActions.DescribeExclusion(achievement.Id));
			}
			hereMenu = new ContextMenuStrip();
			if (isHidden)
			{
				((Control)hereMenu.AddMenuItem("Unhide")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					hereCardActions.Unhide(achievement.Id);
				});
			}
			else
			{
				ContextMenuStripItem obj = hereMenu.AddMenuItem("Not today");
				((Control)obj).set_BasicTooltipText("Hide from Here until the daily reset (00:00 UTC).");
				((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					hereCardActions.SnoozeUntilReset(achievement.Id);
				});
				ContextMenuStripItem obj2 = hereMenu.AddMenuItem("Not interested");
				((Control)obj2).set_BasicTooltipText("Hide from Here until you un-hide it.");
				((Control)obj2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					hereCardActions.HideIndefinitely(achievement.Id);
				});
			}
			((Control)this).set_Menu(hereMenu);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(isHidden ? "+" : "x");
			val.set_Font(UiStyle.BodyFont);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val).set_Width(16);
			((Control)val).set_Height(16);
			((Control)val).set_BasicTooltipText(isHidden ? "Show this achievement in Here again" : "Hide this from Here (also on right-click)");
			hideLabel = val;
			UiStyle.ApplyShadow(hideLabel, UiStyle.Rank);
			((Control)hideLabel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				hereMenu.Show(GameService.Input.get_Mouse().get_Position());
			});
		}

		public override void RecalculateLayout()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			int width = ((Control)this).get_Width();
			if (hideLabel != null)
			{
				((Control)hideLabel).set_Location(new Point(width - 8 - 16, 6));
			}
			if (eye != null)
			{
				((Control)eye).set_Location(new Point(width - 8 - 22, ((Control)this).get_Height() - 8 - 22));
			}
			if (badgeLabel != null)
			{
				int eyeReserve = ((eye != null) ? 26 : 0);
				((Control)badgeLabel).set_Location(new Point(width - 8 - eyeReserve - ((Control)badgeLabel).get_Width(), ((Control)this).get_Height() - 8 - 18));
			}
		}

		private void Tracker_AchievementUntracked(int achievementId)
		{
			if (achievement.Id == achievementId && eye != null)
			{
				eye.set_Checked(false);
				((Control)eye).set_BasicTooltipText("Target this");
			}
		}

		private void AchievementCard_Click(object sender, MouseEventArgs e)
		{
			if (achievementTrackerService.IsBeingTracked(achievement.Id))
			{
				achievementTrackerService.RemoveAchievement(achievement.Id);
				if (eye != null)
				{
					eye.set_Checked(false);
					((Control)eye).set_BasicTooltipText("Target this");
				}
				return;
			}
			bool trackSuccess = achievementTrackerService.TrackAchievement(achievement.Id);
			if (eye != null)
			{
				eye.set_Checked(trackSuccess);
				((Control)eye).set_BasicTooltipText(trackSuccess ? "Drop this" : "Target this");
			}
			if (!trackSuccess)
			{
				ScreenNotification.ShowNotification("You can have a maximum of 15 achievements tracked concurrently.\n Untrack one to add a new one.", (NotificationType)0, (Texture2D)null, 4);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			Color background = (isComplete ? UiStyle.Complete : (hovering ? UiStyle.CardBackgroundHover : UiStyle.CardBackground));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 2, bounds.Width, bounds.Height - 2), background);
			Color edgeColor = ((guidance.Tier != 0) ? guidance.Color : UiStyle.CardBorder);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, bounds.Width, 2), edgeColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 2, 1, bounds.Height - 2), UiStyle.CardBorder);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bounds.Width - 1, 2, 1, bounds.Height - 2), UiStyle.CardBorder);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, bounds.Height - 1, bounds.Width, 1), UiStyle.CardBorder);
			if (icon != null && icon.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(icon), new Rectangle(8, 8, 56, 56));
			}
			int textX = 74;
			int rankWidth = 0;
			if (rank.HasValue)
			{
				string rankText = rank.Value.ToString();
				rankWidth = (int)UiStyle.BodyFont.MeasureString(rankText).Width + 6;
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, rankText, UiStyle.BodyFont, new Rectangle(textX, 8, rankWidth, 18), UiStyle.Rank, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			int hideReserve = ((hideLabel != null) ? 22 : 0);
			int titleWidth = Math.Max(40, bounds.Width - textX - rankWidth - 8 - hideReserve);
			string trimmedTitle = StringUtils.TrimNameToWidth(title, titleWidth, UiStyle.TitleFont);
			DrawShadowed(spriteBatch, trimmedTitle, UiStyle.TitleFont, new Rectangle(textX + rankWidth, 8, titleWidth, 20), UiStyle.TextPrimary);
			Color placeColor = (placeIsBold ? UiStyle.TextSecondary : UiStyle.Rank);
			int placeWidth = bounds.Width - textX - 8;
			DrawShadowed(spriteBatch, placeText, UiStyle.BodyFont, new Rectangle(textX, 28, placeWidth, 18), placeColor);
			int numeralY = bounds.Height - 8 - 18;
			if (isComplete)
			{
				DrawShadowed(spriteBatch, "Complete", UiStyle.NumeralFont, new Rectangle(textX, numeralY, 140, 20), UiStyle.Complete);
				return;
			}
			Color numeralColor = ((progress.Fraction >= 0.75) ? UiStyle.NearDone : UiStyle.TextPrimary);
			string numeralText = progress.Text ?? string.Empty;
			DrawShadowed(spriteBatch, numeralText, UiStyle.NumeralFont, new Rectangle(textX, numeralY, 70, 20), numeralColor);
			if (progress.Max > 0)
			{
				int numeralWidth = (int)UiStyle.NumeralFont.MeasureString(numeralText).Width + 10;
				int badgeReserve = ((badgeLabel != null) ? (((Control)badgeLabel).get_Width() + 6) : 0);
				int eyeReserve = ((eye != null) ? 28 : 0);
				int pipsX = textX + numeralWidth;
				int pipsWidth = bounds.Width - pipsX - badgeReserve - eyeReserve - 8;
				DrawPips(spriteBatch, pipsX, numeralY + 8, pipsWidth);
			}
		}

		private void DrawPips(SpriteBatch spriteBatch, int x, int y, int width)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			if (width <= 0)
			{
				return;
			}
			int max = progress.Max;
			int current = Math.Min(progress.Current, max);
			Color pipColor = ((progress.Fraction >= 0.75) ? UiStyle.NearDone : UiStyle.TextPrimary);
			if (max > 40)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(x, y, width, 4), UiStyle.PipTodo);
				int filledWidth = (int)((double)width * progress.Fraction);
				if (filledWidth > 0)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(x, y, filledWidth, 4), pipColor);
				}
				return;
			}
			int pipWidth = Math.Max(1, (width - (max - 1) * 2) / max);
			for (int i = 0; i < max; i++)
			{
				int pipX = x + i * (pipWidth + 2);
				if (pipX + pipWidth <= x + width)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(pipX, y, pipWidth, 4), (i < current) ? pipColor : UiStyle.PipTodo);
					continue;
				}
				break;
			}
		}

		private void DrawShadowed(SpriteBatch spriteBatch, string text, BitmapFont font, Rectangle destRect, Color color)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(text))
			{
				Rectangle shadowRect = default(Rectangle);
				((Rectangle)(ref shadowRect))._002Ector(destRect.X + 1, destRect.Y + 1, destRect.Width, destRect.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, font, shadowRect, UiStyle.ShadowColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, font, destRect, color, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		protected override void DisposeControl()
		{
			achievementTrackerService.AchievementUntracked -= Tracker_AchievementUntracked;
			ContextMenuStrip obj = hereMenu;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			hereMenu = null;
			((Container)this).DisposeControl();
		}
	}
}
