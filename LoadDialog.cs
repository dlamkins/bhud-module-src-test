using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using HomeDesigner;
using Microsoft.Xna.Framework;

public class LoadDialog : StandardWindow
{
	private FlowPanel _filePanel;

	private StandardButton _loadButton;

	private string _selectedFile;

	private StandardButton _selectedButton;

	private readonly ContentsManager _contents;

	private InputBlocker inputBlocker = new InputBlocker();

	public event Action<string> TemplateSelected;

	public LoadDialog(ContentsManager contents)
		: this(contents.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 750), new Rectangle(70, 71, 839, 644))
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		_contents = contents;
		((WindowBase2)this).set_Title("Load Template");
		((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		((Control)this).set_Size(new Point(500, 500));
		((Control)this).set_Location(new Point(600, 300));
		((WindowBase2)this).set_SavesPosition(true);
		((WindowBase2)this).set_SavesSize(true);
		((WindowBase2)this).set_CanResize(true);
		((Control)this).set_ZIndex(100);
		BuildLayout();
		RefreshList();
		((Control)inputBlocker).set_ZIndex(99);
		((Control)inputBlocker).set_Visible(true);
		((Control)this).add_Hidden((EventHandler<EventArgs>)delegate
		{
			((Control)inputBlocker).set_Visible(false);
		});
	}

	private void BuildLayout()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		FlowPanel val = new FlowPanel();
		((Control)val).set_Parent((Container)(object)this);
		((Control)val).set_Width(((Control)this).get_Width());
		((Control)val).set_Height(((Container)this).get_ContentRegion().Height - 50);
		((Control)val).set_Location(new Point(((Container)this).get_ContentRegion().X, 10));
		val.set_FlowDirection((ControlFlowDirection)3);
		((Panel)val).set_CanScroll(true);
		val.set_ControlPadding(new Vector2(4f, 4f));
		_filePanel = val;
		StandardButton val2 = new StandardButton();
		((Control)val2).set_Parent((Container)(object)this);
		val2.set_Text("Load");
		((Control)val2).set_Width(((Container)this).get_ContentRegion().Width - 20);
		((Control)val2).set_Location(new Point(((Container)this).get_ContentRegion().X + 10, ((Container)this).get_ContentRegion().Height - 40));
		((Control)val2).set_Enabled(false);
		_loadButton = val2;
		((Control)_loadButton).add_Click((EventHandler<MouseEventArgs>)OnLoadClicked);
	}

	private void RefreshList()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		((Container)_filePanel).ClearChildren();
		_selectedFile = null;
		_selectedButton = null;
		((Control)_loadButton).set_Enabled(false);
		IOrderedEnumerable<string> files = from f in Directory.GetFiles(GetHomesteadFolder(), "*.xml")
			orderby f
			select f;
		if (!files.Any())
		{
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_filePanel);
			val.set_Text("No template found");
			val.set_AutoSizeWidth(true);
			return;
		}
		foreach (string file in files)
		{
			string name = Path.GetFileName(file);
			string mapName = LoadMapName(file) ?? "Unknown Map";
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_filePanel);
			((Control)val2).set_Width(((Container)_filePanel).get_ContentRegion().Width - 20);
			((Control)val2).set_Height(30);
			val2.set_ShowBorder(false);
			Panel row = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(name);
			((Control)val3).set_Width(((Control)row).get_Width() / 2 - 10);
			((Control)val3).set_Height(30);
			((Control)val3).set_Location(new Point(0, 0));
			StandardButton btn = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(mapName);
			val4.set_AutoSizeWidth(false);
			((Control)val4).set_Width(((Control)row).get_Width() / 2 - 10);
			((Control)val4).set_Height(30);
			((Control)val4).set_Location(new Point(((Control)btn).get_Width() + 10, 0));
			val4.set_VerticalAlignment((VerticalAlignment)1);
			((Control)btn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectFile(file, btn);
			});
		}
	}

	private void SelectFile(string filePath, StandardButton button)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (_selectedButton != null)
		{
			((Control)_selectedButton).set_BackgroundColor(Color.get_Transparent());
		}
		_selectedButton = button;
		_selectedFile = filePath;
		((Control)button).set_BackgroundColor(Color.get_LightGreen());
		((Control)_loadButton).set_Enabled(true);
	}

	private void OnLoadClicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(_selectedFile))
		{
			this.TemplateSelected?.Invoke(_selectedFile);
			((Control)inputBlocker).set_Visible(false);
			((Control)this).Hide();
		}
	}

	private string LoadMapName(string filePath)
	{
		try
		{
			XElement decorations = XDocument.Load(filePath).Element("Decorations");
			if (decorations != null)
			{
				return decorations.Attribute("mapName")?.Value;
			}
		}
		catch
		{
		}
		return null;
	}

	private string GetHomesteadFolder()
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "Homesteads");
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		return path;
	}
}
