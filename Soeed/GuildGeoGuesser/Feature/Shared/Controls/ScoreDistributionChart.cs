using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class ScoreDistributionChart : Control, IDisposable
	{
		private readonly List<GuessUser> _guesses;

		private readonly Location _puzzleLocation;

		private readonly Dictionary<string, int> _scoreDistribution;

		private readonly Dictionary<string, Color> _scoreColors;

		private List<string> _orderedScores;

		private int _barWidth;

		private const int CHART_HEIGHT = 120;

		private const int LABEL_HEIGHT = 40;

		private const int BAR_SPACING = 10;

		private const int CHART_PADDING = 20;

		public ScoreDistributionChart(List<GuessUser> guesses, Location puzzleLocation)
			: this()
		{
			_guesses = guesses ?? new List<GuessUser>();
			_puzzleLocation = puzzleLocation;
			_scoreDistribution = new Dictionary<string, int>();
			_scoreColors = new Dictionary<string, Color>();
			BuildColorMapping();
			((Control)this).set_Width(700);
			((Control)this).set_Height(200);
			CalculateScoreDistribution();
			CalculateLayout();
		}

		private void BuildColorMapping()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			_scoreColors.Clear();
			foreach (ScoreModel scoreModel in Service.Config.Scores)
			{
				Color color = ParseHexColor(scoreModel.Color);
				_scoreColors[scoreModel.Value] = color;
			}
			Color wrongMapColor = ParseHexColor(Service.Config.ScoreWrongMapColor);
			_scoreColors[Service.Config.ScoreWrongMap] = wrongMapColor;
		}

		private Color ParseHexColor(string hexColor)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (hexColor.StartsWith("#"))
				{
					hexColor = hexColor.Substring(1);
				}
				if (hexColor.Length == 6)
				{
					int num = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
					int g2 = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
					int b2 = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
					return new Color(num, g2, b2);
				}
				if (hexColor.Length == 8)
				{
					int num2 = int.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
					int g = int.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
					int b = int.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
					int a = int.Parse(hexColor.Substring(6, 2), NumberStyles.HexNumber);
					return new Color(num2, g, b, a);
				}
			}
			catch
			{
			}
			return Color.get_White();
		}

		private void CalculateScoreDistribution()
		{
			_scoreDistribution.Clear();
			foreach (ScoreModel scoreModel in Service.Config.Scores)
			{
				_scoreDistribution[scoreModel.Value] = 0;
			}
			_scoreDistribution[Service.Config.ScoreWrongMap] = 0;
			foreach (GuessUser guess in _guesses)
			{
				string score = guess.PuzzleGuess.Location.Score(_puzzleLocation);
				if (_scoreDistribution.ContainsKey(score))
				{
					_scoreDistribution[score]++;
				}
			}
		}

		private void CalculateLayout()
		{
			_orderedScores = Service.Config.Scores.Select((ScoreModel s) => s.Value).Concat<string>(new string[1] { Service.Config.ScoreWrongMap }).ToList();
			_barWidth = (((Control)this).get_Width() - 40 - (_orderedScores.Count - 1) * 10) / _orderedScores.Count;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			if (!_guesses.Any())
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "No guesses yet", Control.get_Content().get_DefaultFont16(), new Rectangle(20, ((Control)this).get_Height() / 2 - 10, ((Control)this).get_Width() - 40, 20), Color.get_White(), true, (HorizontalAlignment)0, (VerticalAlignment)1);
				return;
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Score Distribution", Control.get_Content().get_DefaultFont18(), new Rectangle(0, 5, ((Control)this).get_Width(), 25), Color.get_LightGoldenrodYellow(), true, (HorizontalAlignment)0, (VerticalAlignment)1);
			int maxCount = _scoreDistribution.Values.Max();
			if (maxCount == 0)
			{
				return;
			}
			int chartTop = 30;
			int barStartY = chartTop + 120;
			Rectangle shadowRect = default(Rectangle);
			Rectangle backgroundRect = default(Rectangle);
			Rectangle labelRect = default(Rectangle);
			for (int i = 0; i < _orderedScores.Count; i++)
			{
				string score = _orderedScores[i];
				int count = _scoreDistribution[score];
				int barHeight = (int)((double)count / (double)maxCount * 120.0);
				int barX = 20 + i * (_barWidth + 10);
				int barY = barStartY - barHeight;
				Color barColor = (_scoreColors.ContainsKey(score) ? _scoreColors[score] : Color.get_White());
				int actualBarHeight = Math.Max(1, barHeight);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(barX, barY, _barWidth, actualBarHeight), barColor);
				string textValue = count.ToString();
				int centerX = barX + _barWidth / 2;
				int centerY = chartTop + 60;
				int shadowSize = 25;
				((Rectangle)(ref shadowRect))._002Ector(centerX - shadowSize / 2 + 3, centerY - shadowSize / 2 + 3, shadowSize, shadowSize);
				((Rectangle)(ref backgroundRect))._002Ector(centerX - shadowSize / 2, centerY - shadowSize / 2, shadowSize, shadowSize);
				((Rectangle)(ref labelRect))._002Ector(centerX - shadowSize / 2 + 5, centerY - shadowSize / 2, shadowSize, shadowSize);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), shadowRect, Color.get_Black() * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), backgroundRect, Color.get_Gray() * 0.8f);
				try
				{
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, textValue, Control.get_Content().get_DefaultFont16(), labelRect, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
				catch (Exception)
				{
				}
				string labelText = score;
				if (labelText.Length > 22)
				{
					labelText = labelText.Substring(0, 19) + "...";
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, labelText, Control.get_Content().get_DefaultFont12(), new Rectangle(barX, barStartY - 10, _barWidth, 40), Color.get_LightGray(), true, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"Total Guesses: {_guesses.Count}", Control.get_Content().get_DefaultFont14(), new Rectangle(0, ((Control)this).get_Height() - 20, ((Control)this).get_Width(), 20), Color.get_LightGoldenrodYellow(), true, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Control)this).OnResized(e);
			CalculateLayout();
		}

		protected override void DisposeControl()
		{
		}
	}
}
