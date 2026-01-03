using Blish_HUD.Controls;
using FontStashSharp;
using LoreBridge.Controls;
using LoreBridge.Modules.Chat.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Chat.Controls
{
	public sealed class TranslationItemPanel : FlowPanel
	{
		private readonly FormattedLabelCustom _translationItemLabel;

		public TranslationItemPanel(Message listItem, SpriteFontBase font)
		{
			SpriteFontBase font2 = font;
			Message listItem2 = listItem;
			((FlowPanel)this)._002Ector();
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			FormattedLabelBuilderCustom builder = new FormattedLabelBuilderCustom();
			if (!string.IsNullOrEmpty(listItem2.Time))
			{
				builder.CreatePart("[" + listItem2.Time + "] ", delegate(FormattedLabelPartBuilderCustom b)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					b.SetTextColor(Color.get_Gray()).SetFont(font2);
				});
			}
			if (!string.IsNullOrEmpty(listItem2.Name))
			{
				builder.CreatePart(listItem2.Name ?? "", delegate(FormattedLabelPartBuilderCustom b)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					b.SetTextColor(listItem2.NameColor).SetFont(font2);
				});
				builder.CreatePart(": ", delegate(FormattedLabelPartBuilderCustom b)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					b.SetTextColor(Color.get_LightGray()).SetFont(font2);
				});
			}
			builder.CreatePart(listItem2.Text, delegate(FormattedLabelPartBuilderCustom b)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				b.SetTextColor(Color.get_LightGray()).SetFont(font2);
			});
			_translationItemLabel = builder.SetWidth(((Control)this)._size.X).AutoSizeHeight().Wrap()
				.ShowShadow()
				.Build();
			((Control)_translationItemLabel).set_Parent((Container)(object)this);
		}

		public void UpdateFont(SpriteFontBase font)
		{
			_translationItemLabel.Font = font;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((Control)_translationItemLabel).set_Width(e.get_CurrentSize().X - 8);
			((Container)this).OnResized(e);
		}
	}
}
