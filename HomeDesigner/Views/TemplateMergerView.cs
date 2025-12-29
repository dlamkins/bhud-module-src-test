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
	public class TemplateMergerView : View
	{
		private FlowPanel _loadedTemplatesPanel;

		private readonly ContentsManager contents;

		private List<XDocument> _loadedTemplates = new List<XDocument>();

		private XDocument mergedTemplate;

		public TemplateMergerView(ContentsManager contents)
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
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Text("Loaded Templates");
			val.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val).set_Location(new Point(40, 10));
			val.set_AutoSizeWidth(true);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(buildPanel.get_ContentRegion().Width - 40, 150));
			((Control)val2).set_Location(new Point(20, 40));
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_CanScroll(true);
			((Panel)val2).set_ShowBorder(true);
			val2.set_ControlPadding(new Vector2(4f, 4f));
			_loadedTemplatesPanel = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent(buildPanel);
			val3.set_Text("Load Template");
			((Control)val3).set_Width(180);
			((Control)val3).set_Location(new Point(20, 220));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LoadDialog loadDialog = new LoadDialog(contents);
				loadDialog.TemplateSelected += delegate(string path)
				{
					XDocument template2 = XmlLoader.LoadXml(path);
					AddTemplate(template2, Path.GetFileName(path));
				};
				((Control)loadDialog).Show();
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent(buildPanel);
			val4.set_Text("Merge Templates");
			((Control)val4).set_Width(180);
			((Control)val4).set_Location(new Point(200, 220));
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_loadedTemplates.Count != 0)
				{
					List<string> list = new List<string>();
					foreach (XDocument loadedTemplate in _loadedTemplates)
					{
						XElement xElement = loadedTemplate.Element("Decorations");
						list.Add(xElement.Attribute("mapId").Value.ToString());
					}
					if (list.Distinct().Count() == 1)
					{
						mergedTemplate = XmlLoader.MergeTemplates(_loadedTemplates);
						ClearLoadedTemplates();
						AddTemplate(mergedTemplate, "Merged Template");
						ScreenNotification.ShowNotification("Templates merged!", (NotificationType)0, (Texture2D)null, 4);
					}
					else
					{
						ScreenNotification.ShowNotification("Can't merge Templates of different Maps!", (NotificationType)0, (Texture2D)null, 4);
					}
				}
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent(buildPanel);
			val5.set_Text("Save Template");
			((Control)val5).set_Width(180);
			((Control)val5).set_Location(new Point(380, 220));
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_loadedTemplates.Count != 1)
				{
					ScreenNotification.ShowNotification("To save, there must be > one < template in the list.", (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					XDocument template = _loadedTemplates.First();
					SaveDialog saveDialog = new SaveDialog(contents, template);
					saveDialog.TemplateSaved += delegate
					{
						ScreenNotification.ShowNotification("Template Saved", (NotificationType)0, (Texture2D)null, 4);
					};
					((Control)saveDialog).Show();
				}
			});
			Label val6 = new Label();
			((Control)val6).set_Parent(buildPanel);
			val6.set_Text("This tool helps merging different templates into one.\nJust load your templates of choice, click merge and save the new file.");
			((Control)val6).set_Height(200);
			((Control)val6).set_Location(new Point(40, 270));
			val6.set_AutoSizeWidth(true);
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)resize);
		}

		private void resize(object sender, ResizedEventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)_loadedTemplatesPanel).get_Parent() != null)
			{
				((Control)_loadedTemplatesPanel).set_Width(((Control)_loadedTemplatesPanel).get_Parent().get_ContentRegion().Width - 40);
			}
		}

		private void AddTemplate(XDocument template, string displayName)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			if (template != null && !_loadedTemplates.Contains(template))
			{
				_loadedTemplates.Add(template);
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_loadedTemplatesPanel);
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
					_loadedTemplates.Remove(template);
					((Control)row).Dispose();
				});
			}
		}

		private void ClearLoadedTemplates()
		{
			Control[] array = ((Container)_loadedTemplatesPanel).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			_loadedTemplates.Clear();
		}
	}
}
