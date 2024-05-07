using System;
using System.Collections.Generic;
using System.Linq;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.CustomCombatText
{
	public class MessageView : AnchoredRect
	{
		private readonly MessageReceiver _receiver;

		private readonly BitmapFont _font;

		private readonly IReadOnlyList<TemplateParser.MarkupFragment> _markupFrags;

		private readonly List<TemplateParser.Fragment> _parsedFragments;

		private readonly List<Message> _messages;

		public TimeSpan Time { get; set; }

		public TimeSpan LastTime { get; private set; }

		public int Value { get; private set; }

		public float Alpha { get; set; } = 1f;


		public bool Stroke { get; set; }

		public int StrokeDistance { get; set; }

		public bool Shadow { get; set; } = true;


		public float ShadowOffset { get; set; } = 1.2f;


		public MessageView(MessageReceiver receiver)
		{
			_receiver = receiver;
			_font = receiver.Font;
			_markupFrags = receiver.MarkupFrags;
			_parsedFragments = new List<TemplateParser.Fragment>();
			_messages = new List<Message>();
			base._002Ector();
		}

		public void AddMessage(TimeSpan time, Message message)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			LastTime = time;
			if (!_messages.Any())
			{
				Time = time;
			}
			_messages.Add(message);
			SizeDeltaX = 0f;
			_parsedFragments.Clear();
			foreach (TemplateParser.Fragment parsed in TemplateParser.FinalParse(_markupFrags, _receiver.Color, _font, message, _messages))
			{
				_parsedFragments.Add(parsed);
				Size2 size = parsed.Size;
				SizeDeltaX = SizeDelta.X + size.Width;
				SizeDeltaY = Math.Max(SizeDelta.Y, size.Height);
			}
			Value += message.Value;
		}

		public bool WithinMergeTimeout(TimeSpan totalGameTime)
		{
			double passed = totalGameTime.TotalMilliseconds - LastTime.TotalMilliseconds;
			return _messages.Any((Message x) => passed < (double)(x.Ev.get_Buff() ? CTextModule.Settings.MergeMaxMsBuffs.Value : CTextModule.Settings.MergeMaxMsStrikes.Value));
		}

		public bool CanMerge(MessageReceiver receiver, Message message)
		{
			if (_receiver == receiver)
			{
				if (_messages.Any())
				{
					return _messages.Last().CanMerge(message);
				}
				return true;
			}
			return false;
		}

		public bool CanMergeAny(MessageView other)
		{
			MessageView other2 = other;
			if (_receiver == other2._receiver)
			{
				return _messages.Any((Message x) => other2._messages.Any((Message y) => y.CanMerge(x)));
			}
			return false;
		}

		protected override void EarlyDraw(RectTarget target)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			base.EarlyDraw(target);
			if (CTextModule.Settings.Debug.Value)
			{
				ShapeExtensions.DrawRectangle(target.SpriteBatch, target.Rect, Color.get_Black(), 1f, 0f);
			}
			target.DrawFragments(_parsedFragments, Alpha, Shadow, Vector2.get_One() * ShadowOffset, stroke: false);
		}
	}
}
