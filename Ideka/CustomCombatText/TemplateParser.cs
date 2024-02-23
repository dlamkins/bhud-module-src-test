using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.ArcDps.Models;
using Blish_HUD.Content;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Ideka.BHUDCommon;
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
		}

		public class IconFragment : Fragment
		{
			public AsyncTexture2D? Icon { get; set; }

			public int? AssetId { get; set; }

			public override Size2 Size { get; } = new Size2(side, side);


			public IconFragment(float side)
			{
			}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)

		}

		public class PreFragment
		{
			public string Text { get; set; } = "";


			public bool IsProfessionColor { get; set; }

			public Color? Color { get; set; }
		}

		public static readonly Dictionary<int, int> StaticIcons = new Dictionary<int, int>
		{
			[717] = 102834,
			[718] = 102835,
			[719] = 102836,
			[720] = 102837,
			[721] = 102838,
			[722] = 102839,
			[723] = 102840,
			[725] = 102842,
			[726] = 102843,
			[727] = 102844,
			[736] = 102848,
			[737] = 102849,
			[738] = 102850,
			[740] = 102852,
			[742] = 102853,
			[743] = 102854,
			[791] = 102869,
			[861] = 102880,
			[873] = 2440718,
			[1122] = 415959,
			[1187] = 1012835,
			[19426] = 598887,
			[26766] = 961397,
			[26980] = 961398,
			[27705] = 1228472,
			[30328] = 1938787,
			[17495] = 102835,
			[17674] = 102835,
			[21632] = 598887
		};

		public static void DrawFragments(this AnchoredRect.RectTarget target, IEnumerable<Fragment> text, float alpha, bool shadow, Vector2 shadowDistance, bool stroke, float strokeDistance = 1f)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<Fragment> text2 = text;
			RectangleF rect2 = target.Rect;
			Point2 position2 = ((RectangleF)(ref rect2)).get_Position();
			Rectangle absoluteBounds = target.Control.get_AbsoluteBounds();
			Point2 val = position2 + Size2.op_Implicit(((Rectangle)(ref absoluteBounds)).get_Location());
			rect2 = target.Rect;
			RectangleF rect = new RectangleF(val, ((RectangleF)(ref rect2)).get_Size());
			if (shadow)
			{
				loop(delegate(Fragment frag, Vector2 position)
				{
					//IL_0022: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0058: Unknown result type (might be due to invalid IL or missing references)
					//IL_005a: Unknown result type (might be due to invalid IL or missing references)
					//IL_005f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0064: Unknown result type (might be due to invalid IL or missing references)
					//IL_006f: Unknown result type (might be due to invalid IL or missing references)
					StringFragment stringFragment3 = frag as StringFragment;
					if (stringFragment3 != null)
					{
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment3.Font, stringFragment3.Text, position, Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment3.Font, stringFragment3.Text, position + shadowDistance, Color.get_Black() * alpha, (Rectangle?)null);
					}
				});
			}
			if (stroke)
			{
				loop(delegate(Fragment frag, Vector2 position)
				{
					//IL_0022: Unknown result type (might be due to invalid IL or missing references)
					//IL_002f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Unknown result type (might be due to invalid IL or missing references)
					//IL_003f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0069: Unknown result type (might be due to invalid IL or missing references)
					//IL_0077: Unknown result type (might be due to invalid IL or missing references)
					//IL_007c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0087: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
					//IL_0104: Unknown result type (might be due to invalid IL or missing references)
					//IL_0109: Unknown result type (might be due to invalid IL or missing references)
					//IL_0114: Unknown result type (might be due to invalid IL or missing references)
					//IL_013e: Unknown result type (might be due to invalid IL or missing references)
					//IL_014a: Unknown result type (might be due to invalid IL or missing references)
					//IL_014f: Unknown result type (might be due to invalid IL or missing references)
					//IL_015a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0184: Unknown result type (might be due to invalid IL or missing references)
					//IL_0192: Unknown result type (might be due to invalid IL or missing references)
					//IL_0197: Unknown result type (might be due to invalid IL or missing references)
					//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
					//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
					//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
					//IL_01de: Unknown result type (might be due to invalid IL or missing references)
					//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
					//IL_0213: Unknown result type (might be due to invalid IL or missing references)
					//IL_0222: Unknown result type (might be due to invalid IL or missing references)
					//IL_0227: Unknown result type (might be due to invalid IL or missing references)
					//IL_0232: Unknown result type (might be due to invalid IL or missing references)
					StringFragment stringFragment2 = frag as StringFragment;
					if (stringFragment2 != null)
					{
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, 0f, 0f - strokeDistance), Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, strokeDistance, 0f - strokeDistance), Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, strokeDistance, 0f), Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, strokeDistance, strokeDistance), Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, 0f, strokeDistance), Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, 0f - strokeDistance, strokeDistance), Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, 0f - strokeDistance, 0f), Color.get_Black() * alpha, (Rectangle?)null);
						BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment2.Font, stringFragment2.Text, Vector2Extension.OffsetBy(position, 0f - strokeDistance, 0f - strokeDistance), Color.get_Black() * alpha, (Rectangle?)null);
					}
				});
			}
			loop(delegate(Fragment frag, Vector2 position)
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				StringFragment stringFragment = frag as StringFragment;
				if (stringFragment != null)
				{
					BitmapFontExtensions.DrawString(target.SpriteBatch, stringFragment.Font, stringFragment.Text, position, stringFragment.Color * alpha, (Rectangle?)null);
				}
				else
				{
					IconFragment iconFragment = frag as IconFragment;
					if (iconFragment != null)
					{
						AsyncTexture2D? icon = iconFragment.Icon;
						Texture2D val2 = ((icon != null) ? icon!.get_Texture() : null);
						if (val2 != null)
						{
							Rectangle? val3 = (CTextModule.Settings.AutocropIcons.Value ? CTextModule.IconBBoxes.GetIconBBox(iconFragment.AssetId) : null);
							SpriteBatch spriteBatch = target.SpriteBatch;
							Point2 val4 = new Point2(position.X, rect.Y);
							Rectangle absoluteBounds2 = target.Control.get_AbsoluteBounds();
							spriteBatch.Draw(val2, Point2.op_Implicit(val4 + Size2.op_Implicit(((Rectangle)(ref absoluteBounds2)).get_Location())), val3, Color.get_White() * alpha, 0f, Vector2.get_Zero(), Math.Min(iconFragment.Size.Width / (float)(val3?.Width ?? val2.get_Width()), iconFragment.Size.Height / (float)(val3?.Height ?? val2.get_Height())), (SpriteEffects)0, 0f);
						}
					}
				}
			}, populateSpacings: true);
			void loop(Action<Fragment, Vector2> act, bool populateSpacings = false)
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

		public static List<PreFragment> PreParse(string template)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			PreFragment frag = new PreFragment();
			List<PreFragment> frags = new List<PreFragment>();
			int i = 0;
			Color color = default(Color);
			while (i < template.Length)
			{
				string sub = template.Substring(i);
				if (sub.StartsWith("[col="))
				{
					int end = sub.IndexOf(']');
					if (end > 0)
					{
						finishFragment();
						string colorString = sub.Substring(5, end - 5);
						if (colorString == "%c")
						{
							frag.IsProfessionColor = true;
						}
						else
						{
							frag.Color = (ColorUtil.TryParseHex(colorString, ref color) ? new Color?(color) : null);
						}
						i += end + 1;
						continue;
					}
				}
				if (sub.StartsWith("[/col]"))
				{
					finishFragment();
					i += 6;
				}
				else
				{
					frag.Text += sub[0];
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
					frag = new PreFragment();
				}
			}
		}

		public static string? GetSkillName(Message cbt)
		{
			int skillId = (int)cbt.Ev.get_SkillId();
			object obj;
			if (StaticIcons.ContainsKey(skillId))
			{
				string name = cbt.SkillName;
				if (name != null)
				{
					obj = name;
					goto IL_0057;
				}
			}
			Skill? skill = cbt.Skill;
			obj = ((skill != null) ? skill!.get_Name() : null);
			if (obj == null)
			{
				Trait? trait = cbt.Trait;
				obj = ((trait != null) ? trait!.get_Name() : null);
				if (obj == null)
				{
					return cbt.SkillName;
				}
			}
			goto IL_0057;
			IL_0057:
			return (string?)obj;
		}

		public static int? GetIconAssetId(Message message)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (!StaticIcons.TryGetValue((int)message.Ev.get_SkillId(), out var assetId))
			{
				Skill? skill = message.Skill;
				RenderUrl? val = ((skill != null) ? skill!.get_Icon() : null);
				RenderUrl? url;
				if (!val.HasValue)
				{
					Trait? trait = message.Trait;
					url = ((trait != null) ? new RenderUrl?(trait!.get_Icon()) : null);
				}
				else
				{
					url = val;
				}
				return ApiCache.TryExtractAssetId(url) ?? message.SkillFallback?.AssetId;
			}
			return assetId;
		}

		private static Style.ResultFormat? GetResultFormat(EventResult result)
		{
			if (!CTextModule.Style.ResultFormats.TryGetValue(result, out var format))
			{
				return null;
			}
			return format;
		}

		public static IEnumerable<Fragment> FinalParse(PreFragment frag, Color? resultColor, Color srcProfColor, Color dstProfColor, Color? receiverColor, BitmapFont font, Message message, IReadOnlyList<Message> messages, List<List<Message>> resultGroups)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font2 = font;
			IReadOnlyList<Message> messages2 = messages;
			List<List<Message>> resultGroups2 = resultGroups;
			Color? fragColor = (frag.IsProfessionColor ? new Color?(srcProfColor) : frag.Color);
			float height = font2.MeasureString(" ").Height;
			Dictionary<string, Func<(List<Fragment> result, bool isSignificant)>> repDict = new Dictionary<string, Func<(List<Fragment>, bool)>>
			{
				["%v"] = delegate
				{
					int num4 = messages2.Sum((Message x) => x.Value);
					return (new List<Fragment>(1) { newString($"{num4:N0}", resultColor) }, num4 != 0 || messages2.Any((Message x) => x.LandedDamage));
				},
				["%b"] = delegate
				{
					int num3 = messages2.Sum((Message x) => x.Barrier);
					return (new List<Fragment>(1) { newString(string.Format("{0}{1:N0}{2}", "{", num3, "}"), CTextModule.Style.BarrierColor) }, num3 != 0);
				},
				["%r"] = delegate
				{
					string text6 = ((resultGroups2.Count != 1) ? null : GetResultFormat(message.Result)?.Text);
					return (new List<Fragment>(1) { newString(text6 ?? "", resultColor) }, text6 != null);
				},
				["%f"] = delegate
				{
					//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
					(string? text, int count) tuple6 = joinNames(messages2.Select((Message x) => x.Src));
					string item3 = tuple6.text;
					int item4 = tuple6.count;
					string text4 = item3;
					int num2 = item4;
					(string, bool) tuple7;
					if (text4 != null)
					{
						if (num2 != 1)
						{
							goto IL_006f;
						}
						tuple7 = (text4, true);
					}
					else
					{
						if (num2 != 1)
						{
							goto IL_006f;
						}
						tuple7 = ("???", false);
					}
					goto IL_0088;
					IL_0088:
					var (text5, flag2) = tuple7;
					return (new List<Fragment>(1) { newString(text5, srcProfColor) }, (item4 > 1 || flag2) && text5 != GameService.Gw2Mumble.get_PlayerCharacter().get_Name());
					IL_006f:
					tuple7 = ($"({num2} sources)", true);
					goto IL_0088;
				},
				["%t"] = delegate
				{
					//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
					(string? text, int count) tuple3 = joinNames(messages2.Select((Message x) => x.Dst));
					string item = tuple3.text;
					int item2 = tuple3.count;
					string text2 = item;
					int num = item2;
					(string, bool) tuple4;
					if (text2 != null)
					{
						if (num != 1)
						{
							goto IL_006f;
						}
						tuple4 = (text2, true);
					}
					else
					{
						if (num != 1)
						{
							goto IL_006f;
						}
						tuple4 = ("???", false);
					}
					goto IL_0088;
					IL_0088:
					var (text3, flag) = tuple4;
					return (new List<Fragment>(1) { newString(text3, dstProfColor) }, (item2 > 1 || flag) && text3 != GameService.Gw2Mumble.get_PlayerCharacter().get_Name());
					IL_006f:
					tuple4 = ($"({num} targets)", true);
					goto IL_0088;
				},
				["%s"] = delegate
				{
					string skillName = GetSkillName(message);
					return (new List<Fragment>(1) { newString(skillName ?? $"(({(int)message.Ev.get_SkillId()}))") }, skillName != null && !StaticIcons.ContainsKey((int)message.Ev.get_SkillId()));
				},
				["%n"] = () => ((from x in resultGroups2.SelectMany((List<Message> x) => new List<(string, Color?)>
					{
						("/", null),
						($"{x.Count}", GetResultFormat(x.First().Result)?.Color)
					}).Skip(1)
					select newString(x.text, x.color)).Cast<Fragment>().ToList(), messages2.Skip(1).Any()),
				["%i"] = delegate
				{
					int? iconAssetId = GetIconAssetId(message);
					AsyncTexture2D val = ((!iconAssetId.HasValue) ? null : AsyncTexture2D.FromAssetId(iconAssetId.Value));
					return (new List<Fragment>(1)
					{
						new IconFragment(height)
						{
							Icon = val,
							AssetId = iconAssetId
						}
					}, ((val != null) ? val.get_Texture() : null) != null);
				}
			};
			StringFragment fragment = newString();
			int optionStart = -1;
			int repStart = -1;
			for (int i = 0; i < frag.Text.Length; i++)
			{
				char chr = frag.Text[i];
				if (chr == ']' && optionStart >= 0 && repStart > 0 && repDict.TryGetValue(frag.Text.Substring(repStart, 2), out var func2))
				{
					(List<Fragment>, bool) tuple = func2();
					List<Fragment> result2;
					(result2, _) = tuple;
					if (tuple.Item2)
					{
						fragment.Text += frag.Text.Substring(optionStart + 1, repStart - optionStart - 1);
						if (fragment.Text != "")
						{
							yield return fragment;
						}
						foreach (Fragment r2 in result2)
						{
							StringFragment s2 = r2 as StringFragment;
							if (s2 == null || s2.Text != "")
							{
								yield return r2;
							}
						}
						fragment = newString();
						fragment.Text += frag.Text.Substring(repStart + 2, i - repStart - 2);
					}
					repStart = -1;
					optionStart = -1;
					continue;
				}
				if (chr == '%' && i + 1 < frag.Text.Length)
				{
					repStart = i;
					if (optionStart >= 0)
					{
						i++;
						continue;
					}
					if (repDict.TryGetValue(frag.Text.Substring(repStart, 2), out var func))
					{
						if (fragment.Text != "")
						{
							yield return fragment;
							fragment = newString();
						}
						List<Fragment> result = func().Item1;
						foreach (Fragment r in result)
						{
							StringFragment s = r as StringFragment;
							if (s == null || s.Text != "")
							{
								yield return r;
							}
						}
						repStart = -1;
						i++;
						continue;
					}
					func = null;
				}
				if (chr == '[')
				{
					optionStart = i;
				}
				else if (optionStart < 0)
				{
					fragment.Text += chr;
				}
			}
			if (fragment.Text != "")
			{
				yield return fragment;
			}
			static (string? text, int count) joinNames(IEnumerable<Ag> agents)
			{
				int count = agents.DistinctBy((Ag x) => x.get_Id()).Count();
				return ((count != 1 || agents.First().get_Name() == "0") ? null : agents.First().get_Name(), count);
			}
			StringFragment newString(string text = "", Color? color = null)
			{
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				return new StringFragment(font2)
				{
					Text = text,
					Color = (Color)(((_003F?)fragColor) ?? ((_003F?)color) ?? ((_003F?)receiverColor) ?? CTextModule.Style.BaseColor)
				};
			}
		}
	}
}
