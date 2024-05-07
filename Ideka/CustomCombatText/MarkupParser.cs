using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public static class MarkupParser
	{
		public class Fragment
		{
			public string Text { get; set; } = "";


			public Color? Color { get; set; }
		}

		public class Syntax<T> where T : Fragment
		{
			public string ColorTagOpenA { get; init; } = "\0";


			public char ColorTagOpenB { get; init; }

			public HashSet<string> ColorTagClose { get; init; } = new HashSet<string>();


			public Dictionary<string, Color> KnownColors { get; init; } = new Dictionary<string, Color>();


			public Dictionary<string, Action<string, T>> SpecialColors { get; init; } = new Dictionary<string, Action<string, T>>();

		}

		public static List<T> Parse<T>(string template, Syntax<T> syntax) where T : Fragment, new()
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
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
				string close = syntax.ColorTagClose.FirstOrDefault(sub.StartsWith);
				if (close != null)
				{
					finishFragment();
					i += close.Length;
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
