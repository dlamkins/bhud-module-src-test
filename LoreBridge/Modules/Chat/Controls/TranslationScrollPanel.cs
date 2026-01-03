using Blish_HUD.Controls;
using FontStashSharp;
using LoreBridge.Controls;
using LoreBridge.Modules.Chat.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Chat.Controls
{
	public sealed class TranslationScrollPanel : Panel
	{
		private const int ScrollBarWidth = 12;

		private const int ScrollBarOffsetLeft = 5;

		private const int ScrollBarOffsetRight = 6;

		private const int ScrollBarOffsetY = 10;

		private readonly ScrollbarCustom _scrollBar;

		private readonly TranslationPanel _scrollPanel;

		public TranslationScrollPanel(Messages messages, SpriteFontBase font)
			: this()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)2);
			TranslationPanel translationPanel = new TranslationPanel(messages, font);
			((Control)translationPanel).set_Parent((Container)(object)this);
			((Control)translationPanel).set_Location(new Point(23, 0));
			_scrollPanel = translationPanel;
			ScrollbarCustom scrollbarCustom = new ScrollbarCustom((Container)(object)_scrollPanel);
			((Control)scrollbarCustom).set_Parent((Container)(object)this);
			((Control)scrollbarCustom).set_Location(new Point(5, 10));
			scrollbarCustom.ScrollDistance = 1f;
			_scrollBar = scrollbarCustom;
		}

		public void UpdateFont(SpriteFontBase font)
		{
			_scrollPanel.UpdateFont(font);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			if (_scrollBar != null)
			{
				((Control)_scrollBar).set_Height(((Control)this).get_Height() - 20);
				((Control)_scrollBar).set_Location(new Point(5, 10));
			}
			if (_scrollPanel != null)
			{
				((Control)_scrollPanel).set_Height(((Control)this).get_Height());
				((Control)_scrollPanel).set_Width(((Control)this).get_Width() - 5 - 6 - 12);
				((Control)_scrollPanel).set_Location(new Point(23, 0));
			}
		}
	}
}
