using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.Models;
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

		private readonly IReadOnlyList<TemplateParser.PreFragment> _parsedTemplate;

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
			_parsedTemplate = receiver.PreFragments;
			_parsedFragments = new List<TemplateParser.Fragment>();
			_messages = new List<Message>();
			base._002Ector();
		}

		public void AddMessage(TimeSpan time, Message message)
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			LastTime = time;
			if (!_messages.Any())
			{
				Time = time;
			}
			_messages.Add(message);
			List<List<Message>> resultGroups;
			Style.ResultFormat x4;
			Style.ResultFormat resultFormat = (byGroup<EventResult, Style.ResultFormat>(_messages, (Message x) => x.Result, CTextModule.Style.ResultFormats, out resultGroups, out x4) ? x4 : null);
			List<List<Message>> groups5;
			Color x3;
			Color srcProfColor = (byGroup<ProfessionType, Color>(_messages, (Message x) => (ProfessionType)(byte)x.Src.get_Profession(), CTextModule.Style.ProfessionColors, out groups5, out x3) ? x3 : CTextModule.Style.DefaultEntityColor);
			Color? petColor3 = CTextModule.Style.PetColor;
			if (petColor3.HasValue)
			{
				Color petColor2 = petColor3.GetValueOrDefault();
				if ((from x in groups5.First()
					where !x.SrcIsPet
					select x).Count() < _messages.Where((Message x) => x.SrcIsPet).Count())
				{
					srcProfColor = petColor2;
				}
			}
			List<List<Message>> groups4;
			Color x2;
			Color dstProfColor = (byGroup<ProfessionType, Color>(_messages, (Message x) => (ProfessionType)(byte)x.Dst.get_Profession(), CTextModule.Style.ProfessionColors, out groups4, out x2) ? x2 : CTextModule.Style.DefaultEntityColor);
			petColor3 = CTextModule.Style.PetColor;
			if (petColor3.HasValue)
			{
				Color petColor = petColor3.GetValueOrDefault();
				if ((from x in groups4.First()
					where !x.DstIsPet
					select x).Count() < _messages.Where((Message x) => x.DstIsPet).Count())
				{
					dstProfColor = petColor;
				}
			}
			byGroup<(uint, uint), object>(_messages, (Message x) => (x.Src.get_Profession(), x.Src.get_Elite()), new Dictionary<(uint, uint), object>(), out var groups3, out var value2);
			(ProfessionType, uint) srcSpec = ((ProfessionType)(byte)groups3[0][0].Src.get_Profession(), groups3[0][0].Src.get_Elite());
			byGroup<(uint, uint), object>(_messages, (Message x) => (x.Dst.get_Profession(), x.Dst.get_Elite()), new Dictionary<(uint, uint), object>(), out var groups2, out value2);
			(ProfessionType, uint) dstSpec = ((ProfessionType)(byte)groups2[0][0].Dst.get_Profession(), groups2[0][0].Dst.get_Elite());
			SizeDeltaX = 0f;
			_parsedFragments.Clear();
			foreach (TemplateParser.PreFragment item in _parsedTemplate)
			{
				foreach (TemplateParser.Fragment parsed in TemplateParser.FinalParse(item, resultFormat?.Color, srcProfColor, dstProfColor, srcSpec, dstSpec, _receiver.Color, _font, message, _messages, resultGroups))
				{
					TemplateParser.StringFragment s = parsed as TemplateParser.StringFragment;
					if (s != null && s.Text.EndsWith(" "))
					{
						s.Text += " ";
					}
					_parsedFragments.Add(parsed);
					Size2 size = parsed.Size;
					SizeDeltaX = SizeDelta.X + size.Width;
					SizeDeltaY = Math.Max(SizeDelta.Y, size.Height);
				}
			}
			Value += message.Value;
			static bool byGroup<TKey, TValue>(IEnumerable<Message> source, Func<Message, TKey> selector, IReadOnlyDictionary<TKey, TValue> dict, out List<List<Message>> groups, out TValue value) where TKey : notnull where TValue : notnull
			{
				List<List<Message>> list = new List<List<Message>>();
				list.AddRange(from x in source.GroupBy(selector)
					select x.ToList() into x
					orderby x.Count descending
					select x);
				groups = list;
				TKey mostCommonResult = selector(groups[0][0]);
				return dict.TryGetValue(mostCommonResult, out value);
			}
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
