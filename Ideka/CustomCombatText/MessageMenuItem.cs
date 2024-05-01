using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.CustomCombatText
{
	public class MessageMenuItem : MenuItem
	{
		private class MessageDraw : AnchoredRect
		{
			public List<TemplateParser.Fragment> ParsedFragments { get; set; } = new List<TemplateParser.Fragment>();


			protected override void EarlyDraw(RectTarget target)
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				base.EarlyDraw(target);
				if (CTextModule.Settings.Debug.Value)
				{
					ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_Black(), 1f, 0f);
				}
				target.DrawFragments(ParsedFragments, 1f, shadow: true, Vector2.get_One() * 1.2f, stroke: false);
			}
		}

		private const int Spacing = 5;

		private readonly Message _message;

		private readonly MessageDraw _rect;

		public float InnerHeight { get; private set; }

		public MessageMenuItem(Message message, List<TemplateParser.TemplatePreFrag> preFrags, int fontSize)
			: this()
		{
			_message = message;
			_rect = new MessageDraw();
			UpdateVisuals(preFrags, fontSize);
		}

		public void UpdateVisuals(List<TemplateParser.TemplatePreFrag> preFrags, int fontSize)
		{
			List<TemplateParser.Fragment> parsed = TemplateParser.FinalParse(preFrags, null, CTextModule.FontAssets.Get(null, fontSize), _message, new _003C_003Ez__ReadOnlyArray<Message>(new Message[1] { _message })).ToList();
			_rect.ParsedFragments = parsed;
			InnerHeight = (parsed.Any() ? parsed.Max((TemplateParser.Fragment x) => x.Size.Height) : 0f) + 5f;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			RectangleF rect = _rect.Target(RectangleF.op_Implicit(((Control)this).get_AbsoluteBounds()));
			rect.Width -= 5f;
			rect.X += 2f;
			_rect.Draw(spriteBatch, (Control)(object)this, rect);
		}
	}
}
