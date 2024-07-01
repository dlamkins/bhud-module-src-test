using System;
using Blish_HUD;
using Ideka.BHUDCommon.AnchoredRect;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.RacingMeter
{
	public class OnlineHUD : AnchoredRect, IDisposable
	{
		private readonly DisposableCollection _dc = new DisposableCollection();

		public OnlineHUD(RaceOnline racer)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			RaceOnline racer2 = racer;
			base._002Ector();
			AnchoredRect inner = AddChild(new AnchoredRect());
			inner.AddChild(new SimpleRectangle
			{
				Color = Color.get_Black(),
				Thickness = 1f
			});
			_ = TimeSpan.Zero;
			addLine(GameService.Content.get_DefaultFont18()).WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = racer2.Race?.Name ?? "";
			});
			inner.SizeDelta = new Vector2(0f, inner.SizeDelta.Y + 5f);
			addLine(GameService.Content.get_DefaultFont32()).WithUpdate(delegate(SizedTextLabel x)
			{
				int place = racer2.Place;
				x.Text = ((place < 1) ? "-" : place.Ordinalize());
			});
			addLine().WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = Strings.LabelLap.Format(racer2.CompletedLaps + 1, racer2.TotalLaps);
			});
			addLine().WithUpdate(delegate(SizedTextLabel x)
			{
				x.Text = Strings.LabelCheckpoint.Format(racer2.PassedPoints, racer2.TotalPoints);
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
				x.Text = racer2.RaceTime.Formatted();
			}).With(delegate(SizedTextLabel x)
			{
				x.SuffixModel = TimeSpan.Zero.Formatted();
			});
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
				foreach (AnchoredRect current in inner.Children)
				{
					Vector2 val3 = (current.Anchor = (current.Pivot = new Vector2(num, current.Pivot.Y)));
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
