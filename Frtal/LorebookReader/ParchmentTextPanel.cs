using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	public sealed class ParchmentTextPanel : Panel
	{
		private readonly ParchmentContent _content;

		private float _fontSize = 18f;

		private string _text = "";

		public float FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				if (!(Math.Abs(_fontSize - value) < 0.1f))
				{
					_fontSize = value;
					_content.SetFontSize(_fontSize);
				}
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value ?? "";
				_content.SetContent(_text, _fontSize, EffectiveWidth());
			}
		}

		public ParchmentTextPanel(TextRenderer tr, Texture2D parchment)
			: this()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).set_CanScroll(true);
			((Panel)this).set_ShowBorder(true);
			ParchmentContent parchmentContent = new ParchmentContent(tr, parchment);
			((Control)parchmentContent).set_Parent((Container)(object)this);
			((Control)parchmentContent).set_Location(Point.get_Zero());
			_content = parchmentContent;
		}

		private int EffectiveWidth()
		{
			return Math.Max(50, ((Control)this).get_Width() - 20);
		}

		public void ApplyWrap()
		{
			_content?.SetContent(_text, _fontSize, EffectiveWidth());
		}

		public override void RecalculateLayout()
		{
			((Panel)this).RecalculateLayout();
			_content?.SetWrapWidth(EffectiveWidth());
		}
	}
}
