using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.ArcDps.Models;
using Blish_HUD.Content;
using Gw2Sharp.Models;
using Ideka.BHUDCommon.AnchoredRect;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.CustomCombatText
{
	public static class TemplateParser
	{
		public abstract class Fragment
		{
			public abstract Size2 Size { get; }

			public abstract void Draw(AnchoredRect.RectTarget target, Vector2 position, float alpha);
		}

		public class StringFragment : Fragment
		{
			private (BitmapFont font, string text, Size2 size)? _cache;

			public string Text { get; set; } = "";


			public Color Color { get; set; }

			public BitmapFont Font { get; set; }

			public override Size2 Size
			{
				get
				{
					//IL_0066: Unknown result type (might be due to invalid IL or missing references)
					//IL_0085: Unknown result type (might be due to invalid IL or missing references)
					if (_cache?.font != Font || _cache?.text != Text)
					{
						_cache = new(BitmapFont, string, Size2)?((Font, Text, Font.MeasureString(Text)));
					}
					return _cache.Value.size;
				}
			}

			public StringFragment(BitmapFont font)
			{
				Font = font;
				base._002Ector();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				BitmapFontExtensions.DrawString(spriteBatch, Font, Text, position, color, (Rectangle?)null);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void DrawShadow(SpriteBatch spriteBatch, Vector2 position, Vector2 shadowDistance, float alpha)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				Color color = Color.get_Black() * alpha;
				Draw(spriteBatch, position, color);
				Draw(spriteBatch, position + shadowDistance, color);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void DrawStroke(SpriteBatch spriteBatch, Vector2 position, float strokeDistance, float alpha)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				Color color = Color.get_Black() * alpha;
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, 0f, 0f - strokeDistance), color);
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, strokeDistance, 0f - strokeDistance), color);
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, strokeDistance, 0f), color);
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, strokeDistance, strokeDistance), color);
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, 0f, strokeDistance), color);
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, 0f - strokeDistance, strokeDistance), color);
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, 0f - strokeDistance, 0f), color);
				Draw(spriteBatch, Vector2Extension.OffsetBy(position, 0f - strokeDistance, 0f - strokeDistance), color);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public override void Draw(AnchoredRect.RectTarget target, Vector2 position, float alpha)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Draw(target.SpriteBatch, position, Color * alpha);
			}
		}

		public class IconFragment : Fragment
		{
			public AsyncTexture2D? Icon { get; set; }

			public int? AssetId { get; set; }

			public bool Autocropped { get; set; }

			public Color Color { get; set; } = Color.get_White();


			public override Size2 Size { get; } = new Size2(side, side);


			public IconFragment(float side)
			{
			}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public override void Draw(AnchoredRect.RectTarget target, Vector2 position, float alpha)
			{
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_010b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0111: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_011e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_013a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0140: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				AsyncTexture2D? icon = Icon;
				Texture2D texture = ((icon != null) ? icon!.get_Texture() : null);
				if (texture != null)
				{
					Rectangle? bbox = ((Autocropped && CTextModule.Settings.AutocropIcons.Value) ? CTextModule.IconBBoxes.GetIconBBox(AssetId) : null);
					float width = bbox?.Width ?? texture.get_Width();
					float height = bbox?.Height ?? texture.get_Height();
					float aspectRatio = width / height;
					Vector2 offset = ((aspectRatio > Size.Width / Size.Height) ? new Vector2(0f, (Size.Height - Size.Width / aspectRatio) / 2f) : new Vector2((Size.Width - Size.Height * aspectRatio) / 2f, 0f));
					target.SpriteBatch.Draw(texture, Point2.op_Implicit(new Point2(position.X, position.Y) + offset), bbox, Color * alpha, 0f, Vector2.get_Zero(), Math.Min(Size.Width / width, Size.Height / height), (SpriteEffects)0, 0f);
				}
			}
		}

		public class MultiIconFragment : Fragment
		{
			public IconFragment[] Inner { get; set; } = Array.Empty<IconFragment>();


			public override Size2 Size { get; } = new Size2(side, side);


			public MultiIconFragment(float side)
			{
			}//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public override void Draw(AnchoredRect.RectTarget target, Vector2 position, float alpha)
			{
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				if (!CTextModule.Settings.MultiIconMessages.Value && Inner.Any())
				{
					Inner.Last().Draw(target, position, alpha);
					return;
				}
				foreach (var item2 in Inner.Enumerate())
				{
					int i = item2.index;
					IconFragment item = item2.item;
					float offset = ((float)i - (float)(Inner.Length - 1) / 2f) * 4f;
					item.Draw(target, Vector2Extension.OffsetBy(position, offset, offset), alpha);
				}
			}
		}

		public class PreFrag
		{
			public string Text { get; set; } = "";


			public Color? Color { get; set; }
		}

		public class TemplatePreFrag : PreFrag
		{
			public bool IsProfessionColor { get; set; }
		}

		public readonly struct Syntax<T> where T : PreFrag
		{
			public string ColorTagOpenA { get; init; }

			public char ColorTagOpenB { get; init; }

			public string ColorTagClose { get; init; }

			public Dictionary<string, Color> KnownColors { get; init; }

			public Dictionary<string, Action<string, T>> SpecialColors { get; init; }
		}

		public const string Help = "Templates:\n%i - skill icon\n%v - damage taken/damage healed/barrier applied\n%b - barrier damage taken\n%r - result's associated text, if any (e.g. block, invuln)\n%f - message source\n%t - message target\n%s - skill name\n%n - number of messages (for combined messages)\n%m - mesage source elite spec icon\n%o - mesage target elite spec icon\n\n[col=XXXXXX][/col] tags can be used to override default colors.\n%c can be used in [col] tags to use the source profession's color.\nSquare brackets can be used around templates (e.g. [%b]) to mark them as \"optional\". The content of the brackets will only be shown if the template within is relevant to the current message.\nOptional brackets must contain exactly one template.\n[col] tags only work outside optional brackets (but can have optional brackets inside).";

		private static readonly Syntax<TemplatePreFrag> BaseSyntax = new Syntax<TemplatePreFrag>
		{
			ColorTagOpenA = "[col=",
			ColorTagOpenB = ']',
			ColorTagClose = "[/col]",
			KnownColors = new Dictionary<string, Color>(),
			SpecialColors = new Dictionary<string, Action<string, TemplatePreFrag>> { ["%c"] = delegate(string _, TemplatePreFrag frag)
			{
				frag.IsProfessionColor = true;
			} }
		};

		private static (int id, bool knownElite)? GetProfIcon(ProfessionType prof, uint eliteId)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected I4, but got Unknown
			return (prof - 1) switch
			{
				0 => eliteId switch
				{
					0u => new(int, bool)?((156634, true)), 
					27u => new(int, bool)?((1128573, true)), 
					62u => new(int, bool)?((1770211, true)), 
					65u => new(int, bool)?((2479354, true)), 
					_ => new(int, bool)?((156634, false)), 
				}, 
				1 => eliteId switch
				{
					0u => new(int, bool)?((156643, true)), 
					18u => new(int, bool)?((1128567, true)), 
					61u => new(int, bool)?((1770223, true)), 
					68u => new(int, bool)?((2491566, true)), 
					_ => new(int, bool)?((156643, false)), 
				}, 
				2 => eliteId switch
				{
					0u => new(int, bool)?((156632, true)), 
					43u => new(int, bool)?((1128581, true)), 
					57u => new(int, bool)?((1770225, true)), 
					70u => new(int, bool)?((2503659, true)), 
					_ => new(int, bool)?((156632, false)), 
				}, 
				3 => eliteId switch
				{
					0u => new(int, bool)?((156640, true)), 
					5u => new(int, bool)?((1128575, true)), 
					55u => new(int, bool)?((1770215, true)), 
					72u => new(int, bool)?((2503663, true)), 
					_ => new(int, bool)?((156640, false)), 
				}, 
				4 => eliteId switch
				{
					0u => new(int, bool)?((156641, true)), 
					7u => new(int, bool)?((1128571, true)), 
					58u => new(int, bool)?((1770213, true)), 
					71u => new(int, bool)?((2503667, true)), 
					_ => new(int, bool)?((156641, false)), 
				}, 
				5 => eliteId switch
				{
					0u => new(int, bool)?((156630, true)), 
					48u => new(int, bool)?((1128583, true)), 
					56u => new(int, bool)?((1670506, true)), 
					67u => new(int, bool)?((2491558, true)), 
					_ => new(int, bool)?((156630, false)), 
				}, 
				6 => eliteId switch
				{
					0u => new(int, bool)?((156636, true)), 
					40u => new(int, bool)?((1128569, true)), 
					59u => new(int, bool)?((1770217, true)), 
					66u => new(int, bool)?((2479358, true)), 
					_ => new(int, bool)?((156636, false)), 
				}, 
				7 => eliteId switch
				{
					0u => new(int, bool)?((156638, true)), 
					34u => new(int, bool)?((1128579, true)), 
					60u => new(int, bool)?((1770221, true)), 
					64u => new(int, bool)?((2479362, true)), 
					_ => new(int, bool)?((156638, false)), 
				}, 
				8 => eliteId switch
				{
					0u => new(int, bool)?((961390, true)), 
					52u => new(int, bool)?((1128577, true)), 
					63u => new(int, bool)?((1770219, true)), 
					69u => new(int, bool)?((2491562, true)), 
					_ => new(int, bool)?((961390, false)), 
				}, 
				_ => null, 
			};
		}

		private static Style.ResultFormat? GetResultFormat(EventResult result)
		{
			if (!CTextModule.Style.ResultFormats.TryGetValue(result, out var format))
			{
				return null;
			}
			return format;
		}

		public static IEnumerable<Fragment> FinalParse(IEnumerable<TemplatePreFrag> pFrags, Color? receiverColor, BitmapFont font, Message lastMessage, IReadOnlyList<Message> allMessages)
		{
			BitmapFont font2 = font;
			List<List<Message>> resultGroups;
			Style.ResultFormat x2;
			Style.ResultFormat resultFormat = (byGroup<EventResult, Style.ResultFormat>(allMessages, (Message x) => x.Result, CTextModule.Style.ResultFormats, out resultGroups, out x2) ? x2 : null);
			List<List<Message>> groups2;
			Color x3;
			Color srcProfColor = (byGroup<ProfessionType, Color>(allMessages, (Message x) => (ProfessionType)(byte)x.Src.get_Profession(), CTextModule.Style.ProfessionColors, out groups2, out x3) ? x3 : CTextModule.Style.DefaultEntityColor);
			Color? petColor3 = CTextModule.Style.PetColor;
			if (petColor3.HasValue)
			{
				Color petColor = petColor3.GetValueOrDefault();
				if ((from x in groups2.First()
					where !x.SrcIsPet
					select x).Count() < allMessages.Where((Message x) => x.SrcIsPet).Count())
				{
					srcProfColor = petColor;
				}
			}
			List<List<Message>> groups3;
			Color x4;
			Color dstProfColor = (byGroup<ProfessionType, Color>(allMessages, (Message x) => (ProfessionType)(byte)x.Dst.get_Profession(), CTextModule.Style.ProfessionColors, out groups3, out x4) ? x4 : CTextModule.Style.DefaultEntityColor);
			petColor3 = CTextModule.Style.PetColor;
			if (petColor3.HasValue)
			{
				Color petColor2 = petColor3.GetValueOrDefault();
				if ((from x in groups3.First()
					where !x.DstIsPet
					select x).Count() < allMessages.Where((Message x) => x.DstIsPet).Count())
				{
					dstProfColor = petColor2;
				}
			}
			byGroup<(uint, uint), object>(allMessages, (Message x) => (x.Src.get_Profession(), x.Src.get_Elite()), new Dictionary<(uint, uint), object>(), out var groups4, out var value4);
			(ProfessionType, uint) srcSpec = ((ProfessionType)(byte)groups4[0][0].Src.get_Profession(), groups4[0][0].Src.get_Elite());
			byGroup<(uint, uint), object>(allMessages, (Message x) => (x.Dst.get_Profession(), x.Dst.get_Elite()), new Dictionary<(uint, uint), object>(), out var groups5, out value4);
			(ProfessionType, uint) dstSpec = ((ProfessionType)(byte)groups5[0][0].Dst.get_Profession(), groups5[0][0].Dst.get_Elite());
			float height = font2.MeasureString(" ").Height;
			Dictionary<string, (List<Fragment> result, bool isSignificant)> templates = new Dictionary<string, (List<Fragment>, bool)>();
			int value3 = allMessages.Sum((Message x) => x.Value);
			templates["%v"] = (new List<Fragment>(1) { newString($"{value3:N0}", resultFormat?.Color) }, value3 != 0 || allMessages.Any((Message x) => x.LandedDamage));
			int value2 = allMessages.Sum((Message x) => x.Barrier);
			templates["%b"] = (new List<Fragment>(1) { newString(string.Format("{0}{1:N0}{2}", "{", value2, "}"), CTextModule.Style.BarrierColor) }, value2 != 0);
			string text2 = ((resultGroups.Count != 1) ? null : GetResultFormat(lastMessage.Result)?.Text);
			templates["%r"] = (new List<Fragment>(1) { newString(text2 ?? "", resultFormat?.Color) }, text2 != null);
			templates["%f"] = combinedAgents(allMessages.Select((Message x) => x.Src), srcProfColor, "sources");
			templates["%t"] = combinedAgents(allMessages.Select((Message x) => x.Dst), dstProfColor, "targets");
			templates["%s"] = (new List<Fragment>(1) { newString(lastMessage.SkillName ?? $"(({(int)lastMessage.Ev.get_SkillId()}))") }, lastMessage.SkillName != null && !lastMessage.IsBoonOrCondi);
			IEnumerable<(string, Color?)> parts = resultGroups.SelectMany((List<Message> x) => new List<(string, Color?)>
			{
				("/", null),
				($"{x.Count}", GetResultFormat(x.First().Result)?.Color)
			});
			templates["%n"] = ((from x in parts.Skip(1)
				select newString(x.text, x.color)).Cast<Fragment>().ToList(), allMessages.Skip(1).Any());
			MultiIconFragment mi = new MultiIconFragment(height)
			{
				Inner = allMessages.DistinctBy((Message x) => x.SkillId).Select(delegate(Message message)
				{
					AsyncTexture2D icon3 = ((!message.SkillIconId.HasValue) ? null : AsyncTexture2D.FromAssetId(message.SkillIconId.Value));
					return new IconFragment(height)
					{
						Icon = icon3,
						Autocropped = true,
						AssetId = message.SkillIconId
					};
				}).ToArray()
			};
			templates["%i"] = (new List<Fragment>(1) { mi }, mi.Inner.Any(delegate(IconFragment x)
			{
				AsyncTexture2D? icon2 = x.Icon;
				return ((icon2 != null) ? icon2!.get_Texture() : null) != null;
			}));
			templates["%m"] = agentIcon(allMessages.Select((Message x) => x.Src), srcSpec);
			templates["%o"] = agentIcon(allMessages.Select((Message x) => x.Dst), dstSpec);
			foreach (TemplatePreFrag pFrag2 in pFrags)
			{
				TemplatePreFrag pFrag = pFrag2;
				Color? fragColor = (pFrag.IsProfessionColor ? new Color?(srcProfColor) : pFrag.Color);
				foreach (Fragment frag in process())
				{
					StringFragment str = frag as StringFragment;
					if (str != null)
					{
						if (str.Text == "")
						{
							continue;
						}
						if (str.Text.EndsWith(" "))
						{
							str.Text += " ";
						}
						str.Color = (Color)(((_003F?)fragColor) ?? str.Color);
					}
					yield return frag;
				}
				IEnumerable<Fragment> process()
				{
					StringFragment frag2 = newString();
					int optionStart = -1;
					int templateStart = -1;
					for (int i = 0; i < pFrag.Text.Length; i++)
					{
						char chr = pFrag.Text[i];
						if (chr == ']' && optionStart >= 0 && templateStart > 0 && templates.TryGetValue(pFrag.Text.Substring(templateStart, 2), out var template))
						{
							if (template.isSignificant)
							{
								frag2.Text += pFrag.Text.Substring(optionStart + 1, templateStart - optionStart - 1);
								yield return frag2;
								foreach (Fragment item in template.result)
								{
									yield return item;
								}
								frag2 = newString();
								frag2.Text += pFrag.Text.Substring(templateStart + 2, i - templateStart - 2);
							}
							templateStart = -1;
							optionStart = -1;
						}
						else
						{
							template = default((List<Fragment>, bool));
							if (chr == '%' && i + 1 < pFrag.Text.Length)
							{
								templateStart = i;
								if (optionStart >= 0)
								{
									i++;
									continue;
								}
								if (templates.TryGetValue(pFrag.Text.Substring(templateStart, 2), out template))
								{
									yield return frag2;
									frag2 = newString();
									foreach (Fragment item2 in template.result)
									{
										yield return item2;
									}
									templateStart = -1;
									i++;
									continue;
								}
								template = default((List<Fragment>, bool));
							}
							if (chr == '[')
							{
								optionStart = i;
							}
							else if (optionStart < 0)
							{
								frag2.Text += chr;
							}
						}
					}
					yield return frag2;
				}
			}
			(List<Fragment> result, bool isSignificant) agentIcon(IEnumerable<Ag> agents, (ProfessionType prof, uint eliteId) spec)
			{
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				bool isSelf = agents.DistinctBy((Ag x) => x.get_Id()).Count() == 1 && agents.First().get_Self() == 1;
				(int, bool)? profIcon = GetProfIcon(spec.prof, spec.eliteId);
				AsyncTexture2D icon = ((!profIcon.HasValue) ? null : AsyncTexture2D.FromAssetId(profIcon.Value.Item1));
				return (new List<Fragment>(1)
				{
					new IconFragment(height)
					{
						Icon = icon,
						Autocropped = false,
						Color = ((profIcon.HasValue && !profIcon.GetValueOrDefault().Item2) ? CTextModule.Style.DefaultEntityColor : Color.get_White()),
						AssetId = profIcon?.Item1
					}
				}, ((icon != null) ? icon.get_Texture() : null) != null && !isSelf);
			}
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
			(List<Fragment> result, bool isSignificant) combinedAgents(IEnumerable<Ag> agents, Color color, string plural)
			{
				//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
				int count = agents.DistinctBy((Ag x) => x.get_Id()).Count();
				string obj = ((count == 1 && agents.First().get_Name() != "0") ? agents.First().get_Name() : null);
				bool isSelf2 = count == 1 && agents.First().get_Self() == 1;
				string j = obj;
				int c = count;
				(string, bool) tuple;
				if (j == null || j == "")
				{
					if (c != 1)
					{
						goto IL_00aa;
					}
					tuple = ("???", false);
				}
				else
				{
					if (c != 1)
					{
						goto IL_00aa;
					}
					tuple = (j, true);
				}
				goto IL_00c4;
				IL_00aa:
				tuple = ($"({c} {plural})", true);
				goto IL_00c4;
				IL_00c4:
				var (value5, known) = tuple;
				return (new List<Fragment>(1) { newString(value5, color) }, (count > 1 || known) && !isSelf2);
			}
			StringFragment newString(string text = "", Color? color = null)
			{
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				return new StringFragment(font2)
				{
					Text = text,
					Color = (Color)(((_003F?)color) ?? ((_003F?)receiverColor) ?? CTextModule.Style.BaseColor)
				};
			}
		}

		public static void DrawFragments(this AnchoredRect.RectTarget target, IEnumerable<Fragment> text, float alpha, bool shadow, Vector2 shadowDistance, bool stroke, float strokeDistance = 1f)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<Fragment> text2 = text;
			RectangleF rect2 = target.Rect;
			Point2 position2 = ((RectangleF)(ref rect2)).get_Position();
			rect2 = target.Rect;
			RectangleF rect = new RectangleF(position2, ((RectangleF)(ref rect2)).get_Size());
			loop(delegate(Fragment frag, Vector2 position)
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				if (CTextModule.Settings.Debug.Value)
				{
					ShapeExtensions.DrawRectangle(target.SpriteBatch, position, frag.Size, Color.get_Black(), 1f, 0f);
				}
				StringFragment stringFragment = frag as StringFragment;
				if (stringFragment != null)
				{
					if (shadow)
					{
						stringFragment.DrawShadow(target.SpriteBatch, position, shadowDistance, alpha);
					}
					if (stroke)
					{
						stringFragment.DrawStroke(target.SpriteBatch, position, strokeDistance, alpha);
					}
				}
			});
			loop(delegate(Fragment frag, Vector2 position)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				frag.Draw(target, position, alpha);
			});
			void loop(Action<Fragment, Vector2> act)
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				float x = rect.X;
				foreach (Fragment frag2 in text2)
				{
					float y = rect.Y + rect.Height / 2f - frag2.Size.Height / 2f;
					act(frag2, new Vector2(x, y));
					x += frag2.Size.Width;
				}
			}
		}

		public static List<TemplatePreFrag> PreParse(string template)
		{
			return PreParse(template, BaseSyntax);
		}

		public static List<T> PreParse<T>(string template, Syntax<T> syntax) where T : PreFrag, new()
		{
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			T frag = new T();
			List<T> frags = new List<T>();
			int i = 0;
			Color y = default(Color);
			while (i < template.Length)
			{
				string sub = template.Substring(i);
				if (sub.StartsWith(syntax.ColorTagOpenA))
				{
					int end = sub.IndexOf(syntax.ColorTagOpenB);
					if (end > 0)
					{
						finishFragment();
						string colorString = sub.Substring(syntax.ColorTagOpenA.Length, end - syntax.ColorTagOpenA.Length);
						frag.Color = (syntax.KnownColors.TryGetValue(colorString.ToLower(), out var x) ? new Color?(x) : null) ?? (ColorUtil.TryParseHex(colorString, ref y) ? new Color?(y) : null);
						if (syntax.SpecialColors.TryGetValue(colorString, out var f))
						{
							f(colorString, frag);
						}
						i += end + 1;
						continue;
					}
				}
				if (sub.StartsWith(syntax.ColorTagClose))
				{
					finishFragment();
					i += syntax.ColorTagClose.Length;
				}
				else
				{
					ref T reference = ref frag;
					reference.Text += sub[0];
					i++;
				}
			}
			finishFragment();
			return frags;
			void finishFragment()
			{
				if (frag.Text != "")
				{
					frags.Add(frag);
					frag = new T();
				}
			}
		}
	}
}
