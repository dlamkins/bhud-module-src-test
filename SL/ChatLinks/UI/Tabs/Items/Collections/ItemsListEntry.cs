using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;

namespace SL.ChatLinks.UI.Tabs.Items.Collections
{
	public sealed class ItemsListEntry : FlowPanel
	{
		private static readonly Color ActiveColor = new Color(109, 100, 69, 0);

		private static readonly Color HoverColor = new Color(109, 100, 69, 127);

		private readonly Image _image;

		private readonly Panel _labelHolder;

		private readonly Label _name;

		public ItemsListViewModel ViewModel { get; }

		public ItemsListEntry(ItemsListViewModel viewModel)
			: this()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			ViewModel = viewModel;
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(35));
			val.set_Texture(viewModel.GetIcon());
			_image = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)this);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(35);
			((Container)val2).set_HorizontalScrollOffset(-5);
			_labelHolder = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_labelHolder);
			val3.set_Text(viewModel.Item.Name);
			val3.set_TextColor(viewModel.Color);
			((Control)val3).set_Height(35);
			((Control)val3).set_Width(395);
			val3.set_WrapText(true);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			_name = val3;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_00a1: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00cd: Expected O, but got Unknown
			if (ViewModel.IsSelected)
			{
				((Control)_labelHolder).set_BackgroundColor(ActiveColor);
				_name.set_ShowShadow(true);
			}
			else if (((Control)this).get_MouseOver())
			{
				((Control)_labelHolder).set_BackgroundColor(HoverColor);
				_name.set_ShowShadow(true);
			}
			else
			{
				((Control)_labelHolder).set_BackgroundColor(Color.get_Transparent());
				_name.set_ShowShadow(false);
			}
			if (((Control)this).get_MouseOver())
			{
				Image image = _image;
				if (((Control)image).get_Tooltip() == null)
				{
					Tooltip val = new Tooltip((ITooltipView)(object)new ItemTooltipView(ViewModel.CreateTooltipViewModel()));
					Tooltip val2 = val;
					((Control)image).set_Tooltip(val);
				}
				Label name = _name;
				if (((Control)name).get_Tooltip() == null)
				{
					Tooltip val3 = new Tooltip((ITooltipView)(object)new ItemTooltipView(ViewModel.CreateTooltipViewModel()));
					Tooltip val2 = val3;
					((Control)name).set_Tooltip(val3);
				}
			}
			((Container)this).UpdateContainer(gameTime);
		}

		protected override void DisposeControl()
		{
			((Control)_image).Dispose();
			((Control)_name).Dispose();
			((FlowPanel)this).DisposeControl();
		}
	}
}
