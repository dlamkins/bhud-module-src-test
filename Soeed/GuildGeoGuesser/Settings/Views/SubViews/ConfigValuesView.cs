using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Settings.Views.SubViews
{
	public class ConfigValuesView : View
	{
		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlowFill(new FlowPanel(), buildPanel);
			panel.set_OuterControlPadding(new Vector2(0f, 0f));
			((Control)panel).set_Height(((Control)buildPanel).get_Height());
			((Panel)panel).set_CanScroll(true);
			panel.AddString("Configuration Values");
			panel.AddSpace(20);
			panel.AddString("Static Configuration (ConfigModel)");
			panel.AddSpace(10);
			DisplayObjectProperties(Service.Config, panel, "Config");
			panel.AddSpace(30);
			panel.AddString("Score Configuration");
			panel.AddSpace(10);
			DisplayScores(Service.Config.Scores, panel);
		}

		private void DisplayObjectProperties(object obj, FlowPanel panel, string prefix)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			foreach (PropertyInfo property in from p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
				where p.CanRead && !p.Name.Contains("Scores")
				select p)
			{
				object value = property.GetValue(obj);
				string displayValue = FormatValue(value);
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)panel);
				val.set_Text(prefix + "." + property.Name);
				val.set_AutoSizeWidth(true);
				val.set_AutoSizeHeight(true);
				val.set_Font(GameService.Content.get_DefaultFont18());
				val.set_TextColor(Color.get_LightGoldenrodYellow());
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)panel);
				val2.set_Text(displayValue);
				val2.set_AutoSizeWidth(true);
				val2.set_AutoSizeHeight(true);
				val2.set_Font(GameService.Content.get_DefaultFont14());
				val2.set_TextColor(Color.get_LightGray());
				panel.AddSpace(5);
			}
		}

		private void DisplayScores(List<ScoreModel> scores, FlowPanel panel)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			if (scores == null || scores.Count == 0)
			{
				Label val = new Label();
				val.set_Text("No scores configured");
				val.set_AutoSizeWidth(true);
				val.set_Font(GameService.Content.get_DefaultFont18());
				val.set_TextColor(Color.get_LightGoldenrodYellow());
				Label noScoresLabel = val;
				panel.AddControl<Label>(noScoresLabel);
				return;
			}
			for (int i = 0; i < scores.Count; i++)
			{
				ScoreModel score = scores[i];
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)panel);
				val2.set_Text($"Score {i + 1}");
				val2.set_AutoSizeWidth(true);
				val2.set_AutoSizeHeight(true);
				val2.set_Font(GameService.Content.get_DefaultFont18());
				val2.set_TextColor(Color.get_LightGoldenrodYellow());
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)panel);
				val3.set_Text($"Range: {score.Min}-{score.Max} | Value: {score.Value} | Color: {score.Color} | VFX: {score.Vfx.ToString().ToLower()}");
				val3.set_AutoSizeWidth(true);
				val3.set_AutoSizeHeight(true);
				val3.set_Font(GameService.Content.get_DefaultFont14());
				val3.set_TextColor(Color.get_LightGray());
				panel.AddSpace(5);
			}
		}

		private string FormatValue(object? value)
		{
			if (value == null)
			{
				return "null";
			}
			string str = value as string;
			if (str != null)
			{
				return "\"" + str + "\"";
			}
			if (value is bool)
			{
				return ((bool)value).ToString().ToLower();
			}
			if (value is int || value is long || value is double || value is float)
			{
				return value!.ToString();
			}
			List<ScoreModel> scores = value as List<ScoreModel>;
			if (scores != null)
			{
				return $"{scores.Count} score(s)";
			}
			return value!.ToString() ?? "null";
		}

		public ConfigValuesView()
			: this()
		{
		}
	}
}
