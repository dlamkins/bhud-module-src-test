using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public static class RunnerUI
	{
		public static RectAnchor Construct(RaceRunner racer)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			RectAnchor rect = new RectAnchor();
			RectAnchor inner = rect.AddChild(new RectAnchor());
			inner.AddChild(new SimpleRectangle
			{
				Color = Color.get_Black(),
				Thickness = 1f
			});
			TimeSpan current = TimeSpan.Zero;
			TimeSpan last = TimeSpan.Zero;
			TimeSpan best = TimeSpan.Zero;
			SizedTextLabel raceName = addLine(GameService.Content.get_DefaultFont18());
			SizedTextLabel raceAuthor = addLine(GameService.Content.get_DefaultFont12());
			addLine().WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = Strings.CheckpointLabel.Format(racer.CurrentStep, racer.Checkpoints.Count);
			});
			addLine().WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"{racer.Progress:P2}";
			}).With(delegate(SizedTextLabel x)
			{
				x.SuffixModel = $"{0.99:P2}";
			});
			addLine(GameService.Content.get_DefaultFont32()).WithUpdate(delegate(SizedTextLabel x)
			{
				if (racer.RaceTime != TimeSpan.Zero)
				{
					current = racer.RaceTime;
				}
				x.Text = current.Formatted();
			}).With(delegate(SizedTextLabel x)
			{
				x.SuffixModel = TimeSpan.Zero.Formatted();
			});
			if (racer.IsTesting)
			{
				addLine().With(delegate(SizedTextLabel x)
				{
					x.Text = Strings.TestingModeNotice;
				});
			}
			else
			{
				addLine().WithUpdate(delegate(SizedTextLabel x)
				{
					x.Text = StringExtensions.Format(Strings.LastLabel, last.Formatted());
				});
				addLine().WithUpdate(delegate(SizedTextLabel x)
				{
					x.Text = StringExtensions.Format(Strings.BestLabel, best.Formatted());
				});
			}
			racer.RaceLoaded += delegate
			{
				current = (best = (last = TimeSpan.Zero));
				raceName.Text = racer.Race?.Name;
				SizedTextLabel sizedTextLabel = raceAuthor;
				FullRace fullRace = racer.FullRace;
				sizedTextLabel.Text = ((fullRace != null && !fullRace.IsLocal) ? StringExtensions.Format(Strings.ByInfo, racer.FullRace.Meta.AuthorName) : null);
			};
			racer.RaceStarted += delegate
			{
				current = TimeSpan.Zero;
			};
			racer.RaceFinished += delegate(Race _, TimeSpan time)
			{
				ScreenNotification.ShowNotification(StringExtensions.Format(Strings.FinishTimeLabel, time.Formatted()), (NotificationType)0, (Texture2D)null, 4);
				current = (last = time);
				if (time < best || best == TimeSpan.Zero)
				{
					best = time;
				}
			};
			racer.RaceCancelled += delegate
			{
				current = TimeSpan.Zero;
			};
			SizedTextLabel ghost = addLine();
			racer.GhostLoaded += delegate(FullGhost fullGhost)
			{
				string text = fullGhost.Describe();
				ghost.Text = ((text != null) ? StringExtensions.Format(Strings.GhostLabel, text) : null);
			};
			RacingModule.Settings.RacerAnchorX.OnChangedAndNow(delegate(float v)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				inner.Anchor = new Vector2(v, inner.AnchorMin.Y);
			});
			RacingModule.Settings.RacerAnchorY.OnChangedAndNow(delegate(float v)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				inner.Anchor = new Vector2(inner.AnchorMin.X, v);
			});
			RacingModule.Settings.RacerHAlignment.OnChangedAndNow(delegate(RacingSettings.HorizontalAlignment v)
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				float num = RacingSettings.AlignmentPercentage(v);
				foreach (RectAnchor current2 in inner.Children)
				{
					Vector2 val3 = (current2.Anchor = (current2.Pivot = new Vector2(num, current2.Pivot.Y)));
				}
			});
			RacingModule.Settings.RacerVAlignment.OnChangedAndNow(delegate(RacingSettings.VerticalAlignment v)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				inner.Pivot = new Vector2(inner.Pivot.X, RacingSettings.AlignmentPercentage(v));
			});
			return rect;
			SizedTextLabel addLine(BitmapFont font = null)
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				SizedTextLabel line = inner.AddChild(new SizedTextLabel
				{
					Position = new Vector2(0f, inner.SizeDelta.Y),
					Font = (font ?? GameService.Content.get_DefaultFont16()),
					Color = Color.get_White(),
					Stroke = true,
					StrokeDistance = 1,
					Text = " "
				});
				inner.SizeDelta = new Vector2(0f, inner.SizeDelta.Y + line.SizeDelta.Y);
				return line;
			}
		}
	}
}
