using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Localization;

namespace RaidClears.Features.Shared.Services
{
	public class CornerIconTooltipView : Tooltip
	{
		private readonly Image _emblemIcon;

		private readonly Label _moduleName;

		private readonly Label _accountName;

		private readonly Label _motdMessage;

		public string? MotdMessage
		{
			get
			{
				return _motdMessage.get_Text();
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_motdMessage.set_Text(value);
					((Control)_motdMessage).set_Visible(true);
				}
				else
				{
					((Control)_motdMessage).set_Visible(false);
				}
				((Control)this).Invalidate();
			}
		}

		public CornerIconTooltipView()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Expected O, but got Unknown
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(48);
			((Control)val).set_Width(48);
			val.set_Texture(AsyncTexture2D.op_Implicit(Service.Textures?.SettingWindowEmblem ?? Textures.get_Pixel()));
			((Control)val).set_Location(new Point
			{
				X = 0,
				Y = 0
			});
			_emblemIcon = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Height(Control.get_Content().get_DefaultFont16().get_LineHeight());
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)_emblemIcon).get_Right() + 5, ((Control)_emblemIcon).get_Top() + 5));
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_TextColor(Colors.Chardonnay);
			val2.set_Text(Strings.Module_Title);
			_moduleName = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Height(Control.get_Content().get_DefaultFont14().get_LineHeight());
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(((Control)_moduleName).get_Left(), ((Control)_moduleName).get_Bottom() + 2));
			val3.set_Font(Control.get_Content().get_DefaultFont14());
			val3.set_TextColor(Color.get_White() * 0.8f);
			val3.set_Text("Account: " + Service.CurrentAccountName);
			_accountName = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Width(220);
			((Control)val4).set_Location(new Point(((Control)_emblemIcon).get_Left(), ((Control)_emblemIcon).get_Bottom() + 10));
			val4.set_Font(Control.get_Content().get_DefaultFont14());
			val4.set_TextColor(Color.get_White() * 0.9f);
			val4.set_WrapText(true);
			((Control)val4).set_Visible(false);
			_motdMessage = val4;
		}

		public void UpdateAccountName(string accountName)
		{
			_accountName.set_Text("Account: " + accountName);
			((Control)this).Invalidate();
		}

		public override void RecalculateLayout()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			int height = 58;
			int contentWidth = 220;
			if (_motdMessage != null && ((Control)_motdMessage).get_Visible() && !string.IsNullOrEmpty(_motdMessage.get_Text()))
			{
				((Control)_motdMessage).set_Width(contentWidth);
				((Control)_motdMessage).RecalculateLayout();
				int motdHeight = ((((Control)_motdMessage).get_Height() > 0) ? ((Control)_motdMessage).get_Height() : Control.get_Content().get_DefaultFont14().get_LineHeight());
				height += motdHeight + 10;
			}
			((Control)this).set_Size(new Point(contentWidth + 10, height));
			((Container)this).set_ContentRegion(new Rectangle(5, 5, contentWidth, height - 10));
		}

		protected override void DisposeControl()
		{
			((Tooltip)this).DisposeControl();
		}
	}
}
