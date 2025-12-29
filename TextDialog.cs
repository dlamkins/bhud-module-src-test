using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using HomeDesigner;
using Microsoft.Xna.Framework;

public class TextDialog : StandardWindow
{
	private TextBox _fileNameBox;

	private StandardButton _confirmButton;

	private InputBlocker inputBlocker = new InputBlocker();

	public event Action<string> TextConfirmed;

	public TextDialog(ContentsManager contents)
		: this(contents.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 750), new Rectangle(50, 41, 863, 709))
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		((WindowBase2)this).set_Title("Text of your Decoration");
		((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		((Control)this).set_Size(new Point(450, 200));
		((Control)this).set_Location(new Point(600, 300));
		((WindowBase2)this).set_SavesPosition(true);
		((WindowBase2)this).set_SavesSize(true);
		((WindowBase2)this).set_CanResize(true);
		((Control)this).set_ZIndex(100);
		BuildLayout();
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
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		TextBox val = new TextBox();
		((Control)val).set_Parent((Container)(object)this);
		((TextInputBase)val).set_PlaceholderText("Text...");
		((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 20);
		((Control)val).set_Location(new Point(5, 0));
		_fileNameBox = val;
		StandardButton val2 = new StandardButton();
		((Control)val2).set_Parent((Container)(object)this);
		val2.set_Text("Confirm");
		((Control)val2).set_Width(((Container)this).get_ContentRegion().Width - 20);
		((Control)val2).set_Location(new Point(5, ((Control)_fileNameBox).get_Bottom() + 15));
		_confirmButton = val2;
		((Control)_confirmButton).add_Click((EventHandler<MouseEventArgs>)OnConfirmClicked);
	}

	private void OnConfirmClicked(object sender, EventArgs e)
	{
		((TextInputBase)_fileNameBox).get_Text()?.Trim();
		this.TextConfirmed?.Invoke(((TextInputBase)_fileNameBox).get_Text());
		((Control)inputBlocker).set_Visible(false);
		((Control)this).Hide();
	}
}
