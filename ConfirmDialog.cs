using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using HomeDesigner;
using Microsoft.Xna.Framework;

public class ConfirmDialog : StandardWindow
{
	private readonly ContentsManager _contents;

	private InputBlocker inputBlocker = new InputBlocker();

	public event Action<bool> confirmed;

	public ConfirmDialog(ContentsManager contents, string text)
		: this(contents.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 750), new Rectangle(70, 71, 839, 644))
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		_contents = contents;
		((WindowBase2)this).set_Title("Confirm");
		((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		((Control)this).set_Size(new Point(400, 230));
		((Control)this).set_Location(new Point(700, 450));
		((WindowBase2)this).set_CanResize(false);
		((Control)this).set_ZIndex(999);
		((Control)inputBlocker).set_ZIndex(998);
		((Control)inputBlocker).set_Visible(true);
		((Control)this).add_Hidden((EventHandler<EventArgs>)delegate
		{
			((Control)inputBlocker).set_Visible(false);
		});
		Label val = new Label();
		((Control)val).set_Parent((Container)(object)this);
		val.set_Text(text);
		((Control)val).set_Width(((Container)this).get_ContentRegion().Width - 20);
		((Control)val).set_Location(new Point(10, 0));
		val.set_WrapText(true);
		val.set_AutoSizeHeight(true);
		StandardButton val2 = new StandardButton();
		((Control)val2).set_Parent((Container)(object)this);
		val2.set_Text("Confirm");
		((Control)val2).set_Width(((Container)this).get_ContentRegion().Width / 2);
		((Control)val2).set_Location(new Point(10, 35));
		StandardButton confirmButton = val2;
		((Control)confirmButton).add_Click((EventHandler<MouseEventArgs>)OnConfirmClicked);
		StandardButton val3 = new StandardButton();
		((Control)val3).set_Parent((Container)(object)this);
		val3.set_Text("Cancel");
		((Control)val3).set_Width(((Container)this).get_ContentRegion().Width / 2);
		((Control)val3).set_Location(new Point(((Control)confirmButton).get_Width() + 10, 35));
		((Control)val3).add_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
	}

	private void OnCancelClicked(object sender, MouseEventArgs e)
	{
		((Control)inputBlocker).set_Visible(false);
		this.confirmed?.Invoke(obj: false);
		((Control)this).Hide();
	}

	private void OnConfirmClicked(object sender, MouseEventArgs e)
	{
		((Control)inputBlocker).set_Visible(false);
		this.confirmed?.Invoke(obj: true);
		((Control)this).Hide();
	}
}
