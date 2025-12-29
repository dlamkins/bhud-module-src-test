using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using HomeDesigner;
using HomeDesigner.Loader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SaveDialog : StandardWindow
{
	private FlowPanel _filePanel;

	private TextBox _fileNameBox;

	private StandardButton _saveButton;

	private readonly ContentsManager _contents;

	private readonly XDocument _template;

	private InputBlocker inputBlocker = new InputBlocker();

	public event Action<string> TemplateSaved;

	public SaveDialog(ContentsManager contents, XDocument template)
		: this(contents.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 750), new Rectangle(70, 71, 839, 644))
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		_contents = contents;
		_template = template;
		((WindowBase2)this).set_Title("Save Template");
		((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		((Control)this).set_Size(new Point(500, 550));
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
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		FlowPanel val = new FlowPanel();
		((Control)val).set_Parent((Container)(object)this);
		((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
		((Control)val).set_Height(((Container)this).get_ContentRegion().Height - 120);
		((Control)val).set_Location(new Point(((Container)this).get_ContentRegion().X, 0));
		val.set_FlowDirection((ControlFlowDirection)3);
		((Panel)val).set_CanScroll(true);
		val.set_ControlPadding(new Vector2(4f, 4f));
		((Panel)val).set_ShowBorder(true);
		_filePanel = val;
		TextBox val2 = new TextBox();
		((Control)val2).set_Parent((Container)(object)this);
		((TextInputBase)val2).set_PlaceholderText("File name...");
		((Control)val2).set_Width(((Container)this).get_ContentRegion().Width - 20);
		((Control)val2).set_Location(new Point(((Container)this).get_ContentRegion().X + 10, ((Control)_filePanel).get_Bottom() + 10));
		_fileNameBox = val2;
		StandardButton val3 = new StandardButton();
		((Control)val3).set_Parent((Container)(object)this);
		val3.set_Text("Save");
		((Control)val3).set_Width(((Container)this).get_ContentRegion().Width - 20);
		((Control)val3).set_Location(new Point(((Container)this).get_ContentRegion().X + 10, ((Control)_fileNameBox).get_Bottom() + 10));
		_saveButton = val3;
		((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)OnSaveClicked);
	}

	private void RefreshList()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		((Container)_filePanel).ClearChildren();
		IOrderedEnumerable<string> files = from f in Directory.GetFiles(GetHomesteadFolder(), "*.xml")
			orderby f
			select f;
		if (!files.Any())
		{
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_filePanel);
			val.set_Text("No Template Found.");
			val.set_AutoSizeWidth(true);
			return;
		}
		foreach (string item in files)
		{
			string fileName = Path.GetFileName(item);
			string mapName = XmlLoader.GetMapNameFromPath(item);
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_filePanel);
			((Control)val2).set_Width(((Container)_filePanel).get_ContentRegion().Width - 10);
			((Control)val2).set_Height(30);
			Panel row = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(fileName);
			((Control)val3).set_Location(new Point(5, 5));
			val3.set_AutoSizeWidth(true);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)row);
			val4.set_Text(mapName ?? "");
			((Control)val4).set_Location(new Point(180, 5));
			val4.set_AutoSizeWidth(true);
		}
	}

	private void OnSaveClicked(object sender, EventArgs e)
	{
		string fileName = ((TextInputBase)_fileNameBox).get_Text()?.Trim();
		if (string.IsNullOrWhiteSpace(fileName))
		{
			ScreenNotification.ShowNotification("Enter a File Name", (NotificationType)0, (Texture2D)null, 4);
			return;
		}
		if (!fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
		{
			fileName += ".xml";
		}
		string folder = GetHomesteadFolder();
		string filePath = Path.Combine(folder, fileName);
		if (File.Exists(filePath))
		{
			ConfirmDialog confirmDialog = new ConfirmDialog(_contents, "Do you really want to overwrite this file?");
			confirmDialog.confirmed += delegate(bool result)
			{
				if (!result)
				{
					ScreenNotification.ShowNotification("Template NOT saved", (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					try
					{
						_template.Save(filePath);
						this.TemplateSaved?.Invoke(filePath);
						((Control)this).Hide();
					}
					catch (Exception ex2)
					{
						ScreenNotification.ShowNotification("An Error occoured. Template NOT saved: \n" + ex2.Message, (NotificationType)0, (Texture2D)null, 4);
					}
				}
			};
			((Control)confirmDialog).Show();
		}
		else
		{
			try
			{
				_template.Save(filePath);
				this.TemplateSaved?.Invoke(filePath);
				((Control)inputBlocker).set_Visible(false);
				((Control)this).Hide();
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("An Error occoured. Template NOT saved: \n" + ex.Message, (NotificationType)0, (Texture2D)null, 4);
			}
		}
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
