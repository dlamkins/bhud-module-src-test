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
	public class RunnerHUD : RectAnchor, IDisposable
	{
		private readonly DisposableCollection _dc = new DisposableCollection();

		public RunnerHUD(RaceRunner racer)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			RaceRunner racer2 = racer;
			base._002Ector();
			RectAnchor inner = AddChild(new RectAnchor());
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
				x.Text = Strings.LabelCheckpoint.Format(racer2.PassedPoints, racer2.Checkpoints.Count);
			});
			addLine().WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = $"{racer2.Progress:P2}";
			}).With(delegate(SizedTextLabel x)
			{
				x.SuffixModel = $"{0.99:P2}";
			});
			addLine(GameService.Content.get_DefaultFont32()).WithUpdate(delegate(SizedTextLabel x)
			{
				if (racer2.RaceTime != TimeSpan.Zero)
				{
					current = racer2.RaceTime;
				}
				x.Text = current.Formatted();
			}).With(delegate(SizedTextLabel x)
			{
				x.SuffixModel = TimeSpan.Zero.Formatted();
			});
			if (racer2.IsTesting)
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
					x.Text = StringExtensions.Format(Strings.LabelLast, last.Formatted());
				});
				addLine().WithUpdate(delegate(SizedTextLabel x)
				{
					x.Text = StringExtensions.Format(Strings.LabelBest, best.Formatted());
				});
			}
			racer2.RaceLoaded += delegate(FullRace? race)
			{
				current = (best = (last = TimeSpan.Zero));
				raceName.Text = race?.Race.Name ?? "";
				raceAuthor.Text = ((race != null && !race!.IsLocal) ? StringExtensions.Format(Strings.ByInfo, race!.Meta.AuthorName) : "");
			};
			racer2.RaceStarted += delegate
			{
				current = TimeSpan.Zero;
			};
			racer2.RaceFinished += delegate(Race _, Ghost ghost)
			{
				ScreenNotification.ShowNotification(StringExtensions.Format(Strings.LabelFinishTime, ghost.Time.Formatted()), (NotificationType)0, (Texture2D)null, 4);
				current = (last = ghost.Time);
				if (ghost.Time < best || best == TimeSpan.Zero)
				{
					best = ghost.Time;
				}
			};
			racer2.RaceCancelled += delegate
			{
				current = TimeSpan.Zero;
			};
			SizedTextLabel ghost2 = addLine();
			racer2.GhostLoaded += delegate(FullGhost? fullGhost)
			{
				string text = fullGhost?.Describe() ?? null;
				ghost2.Text = ((text != null) ? StringExtensions.Format(Strings.GhostLabel, text) : "");
			};
			_dc.Add(RacingModule.Settings.RacerAnchorX.OnChangedAndNow(delegate(float v)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				inner.Anchor = new Vector2(v, inner.AnchorMin.Y);
			}));
			_dc.Add(RacingModule.Settings.RacerAnchorY.OnChangedAndNow(delegate(float v)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				inner.Anchor = new Vector2(inner.AnchorMin.X, v);
			}));
			_dc.Add(RacingModule.Settings.RacerHAlignment.OnChangedAndNow(delegate(RacingSettings.HorizontalAlignment v)
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
			}));
			_dc.Add(RacingModule.Settings.RacerVAlignment.OnChangedAndNow(delegate(RacingSettings.VerticalAlignment v)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				inner.Pivot = new Vector2(inner.Pivot.X, RacingSettings.AlignmentPercentage(v));
			}));
			SizedTextLabel addLine(BitmapFont? font = null)
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

		public void Dispose()
		{
			_dc.Dispose();
		}
	}
}
