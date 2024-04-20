using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.ArcDps.Models;
using Blish_HUD.Content;
using Gw2Sharp.Models;
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

			public bool Autocropped { get; set; }

			public Color Color { get; set; } = Color.get_White();


			public override Size2 Size { get; } = new Size2(side, side);


			public IconFragment(float side)
			{
			}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)

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
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
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
							Rectangle? val3 = ((iconFragment.Autocropped && CTextModule.Settings.AutocropIcons.Value) ? CTextModule.IconBBoxes.GetIconBBox(iconFragment.AssetId) : null);
							SpriteBatch spriteBatch = target.SpriteBatch;
							Point2 val4 = new Point2(position.X, rect.Y);
							Rectangle absoluteBounds2 = target.Control.get_AbsoluteBounds();
							spriteBatch.Draw(val2, Point2.op_Implicit(val4 + Size2.op_Implicit(((Rectangle)(ref absoluteBounds2)).get_Location())), val3, iconFragment.Color * alpha, 0f, Vector2.get_Zero(), Math.Min(iconFragment.Size.Width / (float)(val3?.Width ?? val2.get_Width()), iconFragment.Size.Height / (float)(val3?.Height ?? val2.get_Height())), (SpriteEffects)0, 0f);
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

		public static (int id, bool knownElite)? GetProfIcon(ProfessionType prof, uint eliteId)
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

		public static IEnumerable<Fragment> FinalParse(PreFragment frag, Color? resultColor, Color srcProfColor, Color dstProfColor, (ProfessionType prof, uint eliteId) srcSpec, (ProfessionType prof, uint eliteId) dstSpec, Color? receiverColor, BitmapFont font, Message message, IReadOnlyList<Message> messages, List<List<Message>> resultGroups)
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
					int num3 = messages2.Sum((Message x) => x.Value);
					return (new List<Fragment>(1) { newString($"{num3:N0}", resultColor) }, num3 != 0 || messages2.Any((Message x) => x.LandedDamage));
				},
				["%b"] = delegate
				{
					int num2 = messages2.Sum((Message x) => x.Barrier);
					return (new List<Fragment>(1) { newString(string.Format("{0}{1:N0}{2}", "{", num2, "}"), CTextModule.Style.BarrierColor) }, num2 != 0);
				},
				["%r"] = delegate
				{
					string text2 = ((resultGroups2.Count != 1) ? null : GetResultFormat(message.Result)?.Text);
					return (new List<Fragment>(1) { newString(text2 ?? "", resultColor) }, text2 != null);
				},
				["%f"] = () => combinedAgents(messages2.Select((Message x) => x.Src), srcProfColor, "sources"),
				["%t"] = () => combinedAgents(messages2.Select((Message x) => x.Dst), dstProfColor, "targets"),
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
							Autocropped = true,
							AssetId = iconAssetId
						}
					}, ((val != null) ? val.get_Texture() : null) != null);
				},
				["%m"] = () => agentIcon(messages2.Select((Message x) => x.Src), srcSpec),
				["%o"] = () => agentIcon(messages2.Select((Message x) => x.Dst), dstSpec)
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
			(List<Fragment> result, bool isSignificant) agentIcon(IEnumerable<Ag> agents, (ProfessionType prof, uint eliteId) spec)
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				int num = agents.DistinctBy((Ag x) => x.get_Id()).Count();
				(int, bool)? profIcon = GetProfIcon(spec.prof, spec.eliteId);
				bool isSelf = num == 1 && agents.First().get_Self() == 1;
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
			(List<Fragment> result, bool isSignificant) combinedAgents(IEnumerable<Ag> agents, Color color, string plural)
			{
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				int count = agents.DistinctBy((Ag x) => x.get_Id()).Count();
				string obj = ((count == 1 && agents.First().get_Name() != "0") ? agents.First().get_Name() : null);
				bool isSelf2 = count == 1 && agents.First().get_Self() == 1;
				string j = obj;
				int c = count;
				(string, bool) tuple3;
				if (j != null)
				{
					if (c != 1)
					{
						goto IL_009c;
					}
					tuple3 = (j, true);
				}
				else
				{
					if (c != 1)
					{
						goto IL_009c;
					}
					tuple3 = ("???", false);
				}
				goto IL_00b6;
				IL_00b6:
				var (value, known) = tuple3;
				return (new List<Fragment>(1) { newString(value, color) }, (count > 1 || known) && !isSelf2);
				IL_009c:
				tuple3 = ($"({c} {plural})", true);
				goto IL_00b6;
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
