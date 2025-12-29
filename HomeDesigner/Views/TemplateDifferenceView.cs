using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using HomeDesigner.Loader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomeDesigner.Views
{
	public class TemplateDifferenceView : View
	{
		private FlowPanel _originalTemplatesPanel;

		private FlowPanel _cutOutTemplatesPanel;

		private readonly ContentsManager contents;

		private List<XDocument> _loadedOriginalTemplates = new List<XDocument>();

		private List<XDocument> _loadedCutOutTemplates = new List<XDocument>();

		private XDocument differedTemplate;

		public TemplateDifferenceView(ContentsManager contents)
			: this()
		{
			this.contents = contents;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Text("Original Template");
			val.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val).set_Location(new Point(40, 10));
			val.set_AutoSizeWidth(true);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(buildPanel.get_ContentRegion().Width - 40, 50));
			((Control)val2).set_Location(new Point(20, 60));
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_CanScroll(true);
			((Panel)val2).set_ShowBorder(true);
			val2.set_ControlPadding(new Vector2(4f, 4f));
			_originalTemplatesPanel = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent(buildPanel);
			val3.set_Text("Load Template");
			((Control)val3).set_Width(180);
			((Control)val3).set_Location(new Point(20, 130));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LoadDialog loadDialog2 = new LoadDialog(contents);
				loadDialog2.TemplateSelected += delegate(string path)
				{
					XDocument template3 = XmlLoader.LoadXml(path);
					ClearLoadedTemplates(_loadedOriginalTemplates, (Panel)(object)_originalTemplatesPanel);
					AddTemplate(_loadedOriginalTemplates, (Panel)(object)_originalTemplatesPanel, template3, Path.GetFileName(path));
				};
				((Control)loadDialog2).Show();
			});
			Label val4 = new Label();
			((Control)val4).set_Parent(buildPanel);
			val4.set_Text("Cut Out Template");
			val4.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val4).set_Location(new Point(40, 190));
			val4.set_AutoSizeWidth(true);
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_Size(new Point(buildPanel.get_ContentRegion().Width - 40, 50));
			((Control)val5).set_Location(new Point(20, 230));
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val5).set_CanScroll(true);
			((Panel)val5).set_ShowBorder(true);
			val5.set_ControlPadding(new Vector2(4f, 4f));
			_cutOutTemplatesPanel = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent(buildPanel);
			val6.set_Text("Load Template");
			((Control)val6).set_Width(180);
			((Control)val6).set_Location(new Point(20, 300));
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LoadDialog loadDialog = new LoadDialog(contents);
				loadDialog.TemplateSelected += delegate(string path)
				{
					XDocument template2 = XmlLoader.LoadXml(path);
					ClearLoadedTemplates(_loadedCutOutTemplates, (Panel)(object)_cutOutTemplatesPanel);
					AddTemplate(_loadedCutOutTemplates, (Panel)(object)_cutOutTemplatesPanel, template2, Path.GetFileName(path));
				};
				((Control)loadDialog).Show();
			});
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent(buildPanel);
			val7.set_Text("Search Differences");
			((Control)val7).set_Width(180);
			((Control)val7).set_Location(new Point(20, 350));
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_loadedOriginalTemplates.Count < 1 || _loadedCutOutTemplates.Count < 1)
				{
					ScreenNotification.ShowNotification("To find the differences, two templates must be selected.", (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					ScreenNotification.ShowNotification("Search successfull.", (NotificationType)0, (Texture2D)null, 4);
					differedTemplate = CompareXDocuments(_loadedOriginalTemplates[0], _loadedCutOutTemplates[0]);
				}
			});
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent(buildPanel);
			val8.set_Text("Save Template");
			((Control)val8).set_Width(180);
			((Control)val8).set_Location(new Point(220, 350));
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (differedTemplate == null)
				{
					ScreenNotification.ShowNotification("Nothing to save yet.", (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					XDocument template = differedTemplate;
					SaveDialog saveDialog = new SaveDialog(contents, template);
					saveDialog.TemplateSaved += delegate
					{
						ScreenNotification.ShowNotification("Template Saved", (NotificationType)0, (Texture2D)null, 4);
					};
					((Control)saveDialog).Show();
				}
			});
			Label val9 = new Label();
			((Control)val9).set_Parent(buildPanel);
			val9.set_Text("The idea: \nTo cut out specific construction from your template \nyou can do this:\n\n- Save your original template\n- Remove the decorations you want to keep\n- Save your 'cut out' template\n Now you can compare both files to get the missing decorations.\nThat's where this tool helps you.");
			((Control)val9).set_Width(400);
			((Control)val9).set_Location(new Point(40, 390));
			val9.set_AutoSizeHeight(true);
			Label val10 = new Label();
			((Control)val10).set_Parent(buildPanel);
			val10.set_Text("How to: \n\n-Load your original template in the first slot\n- Load the template with removed decorations in the 2nd slot\n- Click 'Search Differences'\n- Save the new template\nThat's it. You are done. :)");
			((Control)val10).set_Width(400);
			((Control)val10).set_Location(new Point(500, 390));
			val10.set_AutoSizeHeight(true);
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)resize);
		}

		private void resize(object sender, ResizedEventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)_originalTemplatesPanel).get_Parent() != null)
			{
				((Control)_originalTemplatesPanel).set_Width(((Control)_originalTemplatesPanel).get_Parent().get_ContentRegion().Width - 40);
			}
			if (((Control)_cutOutTemplatesPanel).get_Parent() != null)
			{
				((Control)_cutOutTemplatesPanel).set_Width(((Control)_cutOutTemplatesPanel).get_Parent().get_ContentRegion().Width - 40);
			}
		}

		private void AddTemplate(List<XDocument> list, Panel pan, XDocument template, string displayName)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			if (template != null && !list.Contains(template))
			{
				list.Add(template);
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)pan);
				((Control)val).set_Width(500);
				((Control)val).set_Height(40);
				val.set_ShowBorder(true);
				Panel row = val;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)row);
				val2.set_Text(displayName);
				((Control)val2).set_Location(new Point(5, 5));
				val2.set_AutoSizeWidth(true);
				string mapName = "Unknown Map";
				XElement decorations = template.Element("Decorations");
				if (decorations != null && decorations.Attribute("mapName") != null)
				{
					mapName = decorations.Attribute("mapName").Value;
				}
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)row);
				val3.set_Text(mapName ?? "");
				((Control)val3).set_Location(new Point(220, 5));
				val3.set_AutoSizeWidth(true);
				StandardButton val4 = new StandardButton();
				((Control)val4).set_Parent((Container)(object)row);
				val4.set_Text("X");
				((Control)val4).set_Size(new Point(25, 25));
				((Control)val4).set_Location(new Point(450, 2));
				((Control)val4).set_BasicTooltipText("Remove Template");
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					list.Remove(template);
					((Control)row).Dispose();
				});
			}
		}

		private void ClearLoadedTemplates(List<XDocument> list, Panel pan)
		{
			Control[] array = ((Container)pan).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			list.Clear();
		}

		public static XDocument CompareXDocuments(XDocument docA, XDocument docB)
		{
			if (docA?.Root == null || docB?.Root == null)
			{
				throw new ArgumentException("Beide XDocument-Objekte müssen eine gültige Root enthalten.");
			}
			HashSet<string> propsBKeys = docB.Root.Elements("prop").Select(BuildPropKey).ToHashSet();
			List<XElement> missingProps = (from p in docA.Root.Elements("prop")
				where !propsBKeys.Contains(BuildPropKey(p))
				select new XElement(p)).ToList();
			XElement decorations = new XElement("Decorations", docA.Root.Attributes(), missingProps);
			return new XDocument(new XDeclaration("1.0", "UTF-8", null), decorations);
			static string BuildPropKey(XElement prop)
			{
				IEnumerable<string> attribs = from a in prop.Attributes()
					where a.Name.LocalName != "pos"
					orderby a.Name.LocalName
					select a.Name.LocalName + "=" + a.Value;
				string payload = (from p in prop.Elements("payload")
					select p.Attribute("pt")?.Value + "|" + p.Attribute("v")?.Value + "|" + p.Value).DefaultIfEmpty("").FirstOrDefault();
				return string.Join("|", attribs) + "|payload:" + payload;
			}
		}
	}
}
