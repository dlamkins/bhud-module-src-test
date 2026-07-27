using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Frtal.LorebookReader
{
	public static class ShortStoryLibrary
	{
		public const string Expansion = "Short Stories";

		public static readonly ShortStory[] All = new ShortStory[30]
		{
			new ShortStory
			{
				Title = "Mr. Sparkles, A Tale of the Asura",
				Page = "Mr._Sparkles,_A_Tale_of_the_Asura",
				Writer = "Jeff Grubb",
				Published = "January 28, 2012",
				Timeline = "Before the personal story"
			},
			new ShortStory
			{
				Title = "Braham's Story",
				Page = "Braham's_Story",
				Writer = "Angel McCoy",
				Published = "March 28, 2013",
				Timeline = "Before Living World Season 1"
			},
			new ShortStory
			{
				Title = "Rox's Tale",
				Page = "Rox's_Tale",
				Writer = "Angel McCoy",
				Published = "May 6, 2013",
				Timeline = "Before Living World Season 1"
			},
			new ShortStory
			{
				Title = "Welcome to Paradise",
				Page = "Welcome_to_Paradise",
				Writer = "Scott McGough",
				Published = "May 16, 2013",
				Timeline = "Preceding The Secret of Southsun"
			},
			new ShortStory
			{
				Title = "Canach's Story: An After-Hours Meeting",
				Page = "Canach's_Story:_An_After-Hours_Meeting",
				Writer = "Scott McGough",
				Published = "May 22, 2013",
				Timeline = "Preceding Last Stand at Southsun"
			},
			new ShortStory
			{
				Title = "Marjory's Story: The Last Straw",
				Page = "Marjory's_Story:_The_Last_Straw",
				Writer = "Angel McCoy",
				Published = "June 19–21, 2013",
				Timeline = "Before Living World Season 1"
			},
			new ShortStory
			{
				Title = "Aetherblade Pirates: Look up!",
				Page = "Aetherblade_Pirates:_Look_up!",
				Writer = "Angel McCoy",
				Published = "June 26, 2013",
				Timeline = "Preceding Sky Pirates of Tyria"
			},
			new ShortStory
			{
				Title = "The Trek of the Zephyrites",
				Page = "Short_Story:_The_Trek_of_the_Zephyrites",
				Writer = "Angel McCoy",
				Published = "July 11, 2013",
				Timeline = "Approx. 1320 AE"
			},
			new ShortStory
			{
				Title = "Evon Gnashblade Disembarks",
				Page = "Short_Story:_Evon_Gnashblade_Disembarks",
				Writer = "Angel McCoy",
				Published = "July 26, 2013",
				Timeline = "Before Living World Season 1"
			},
			new ShortStory
			{
				Title = "A Shipwreck: How Ellen Met Magnus",
				Page = "A_Shipwreck:_How_Ellen_Met_Magnus",
				Writer = "Angel McCoy",
				Published = "July 26, 2013",
				Timeline = "Before Living World Season 1"
			},
			new ShortStory
			{
				Title = "Delegation",
				Page = "Short_Story:_Delegation",
				Writer = "Scott McGough",
				Published = "August 8, 2013",
				Timeline = "Preceding Queen's Jubilee"
			},
			new ShortStory
			{
				Title = "What Scarlet Saw",
				Page = "Short_Story:_What_Scarlet_Saw",
				Writer = "Scott McGough",
				Published = "August 23, 2013",
				Timeline = "Approx. 1304–1321 AE"
			},
			new ShortStory
			{
				Title = "A Message from Queen Jennah",
				Page = "A_Message_from_Queen_Jennah",
				Writer = "Writer not credited",
				Published = "September 3, 2013",
				Timeline = "After Queen's Jubilee"
			},
			new ShortStory
			{
				Title = "Shadowbox",
				Page = "Short_Story:_Shadowbox",
				Writer = "John Ryan",
				Published = "September 4, 2013",
				Timeline = "Before Living World Season 1"
			},
			new ShortStory
			{
				Title = "Twilight Preparations",
				Page = "Short_Story:_Twilight_Preparations",
				Writer = "Scott McGough",
				Published = "September 30, 2013",
				Timeline = "Preceding Twilight Assault"
			},
			new ShortStory
			{
				Title = "The Family Business",
				Page = "The_Family_Business",
				Writer = "John Ryan",
				Published = "October 17, 2013",
				Timeline = "Approx. 825 AE"
			},
			new ShortStory
			{
				Title = "Scarlet's Dossier",
				Page = "Scarlet's_Dossier:_A_history_of_Scarlet's_attacks_on_Tyria",
				Writer = "Rubi Bayer",
				Published = "January 7–13, 2014",
				Timeline = "Preceding The Origins of Madness"
			},
			new ShortStory
			{
				Title = "Lionguard Security Force",
				Page = "Lionguard_Security_Force",
				Writer = "Writer not credited",
				Published = "February 28, 2014",
				Timeline = "During Escape from and Battle for Lion's Arch"
			},
			new ShortStory
			{
				Title = "The Reaper's Bounty",
				Page = "The_Reaper's_Bounty",
				Writer = "John Smith",
				Published = "October 21–24, 2014",
				Timeline = "Timeline placement not stated"
			},
			new ShortStory
			{
				Title = "Notes from Rata Novus",
				Page = "Notes_from_Rata_Novus",
				Writer = "Ross Beeley",
				Published = "July 13–20, 2016",
				Timeline = "Between Heart of Thorns and Living World Season 3"
			},
			new ShortStory
			{
				Title = "An Interview with Queen Jennah",
				Page = "An_Interview_with_Queen_Jennah",
				Writer = "Aaron Linde",
				Published = "July 21, 2016",
				Timeline = "Between Heart of Thorns and Living World Season 3"
			},
			new ShortStory
			{
				Title = "Tyrian Travels: Chapter One",
				Page = "Tyrian_Travels:_Chapter_One",
				Writer = "Anatoli Ingram",
				Published = "August 31, 2016",
				Timeline = "After Heart of Thorns"
			},
			new ShortStory
			{
				Title = "Tyrian Travels: Chapter Two",
				Page = "Tyrian_Travels:_Chapter_Two",
				Writer = "Anatoli Ingram",
				Published = "September 12, 2016",
				Timeline = "After Heart of Thorns"
			},
			new ShortStory
			{
				Title = "Tyrian Travels: Chapter Three",
				Page = "Tyrian_Travels:_Chapter_Three",
				Writer = "Anatoli Ingram",
				Published = "October 10, 2016",
				Timeline = "After Heart of Thorns"
			},
			new ShortStory
			{
				Title = "Tyrian Travels: Chapter Four",
				Page = "Tyrian_Travels:_Chapter_Four",
				Writer = "Anatoli Ingram",
				Published = "October 31, 2016",
				Timeline = "After Heart of Thorns"
			},
			new ShortStory
			{
				Title = "Tyrian Travels: Chapter Five",
				Page = "Tyrian_Travels:_Chapter_Five",
				Writer = "Anatoli Ingram",
				Published = "December 1, 2016",
				Timeline = "After Heart of Thorns"
			},
			new ShortStory
			{
				Title = "Tyrian Travels: Chapter Six",
				Page = "Tyrian_Travels:_Chapter_Six",
				Writer = "Anatoli Ingram",
				Published = "January 12, 2017",
				Timeline = "After Heart of Thorns"
			},
			new ShortStory
			{
				Title = "Requiem: Rytlock",
				Page = "Requiem:_Rytlock",
				Writer = "Alex Kain, Samantha Wallschlaeger",
				Published = "January 29, 2019",
				Timeline = "Between All or Nothing and War Eternal"
			},
			new ShortStory
			{
				Title = "Requiem: Zafirah",
				Page = "Requiem:_Zafirah",
				Writer = "Alex Kain, Samantha Wallschlaeger",
				Published = "April 9, 2019",
				Timeline = "Between All or Nothing and War Eternal"
			},
			new ShortStory
			{
				Title = "Requiem: Caithe",
				Page = "Requiem:_Caithe",
				Writer = "Alex Kain, Samantha Wallschlaeger",
				Published = "May 7, 2019",
				Timeline = "Between All or Nothing and War Eternal"
			}
		};

		public const string ImagePrefix = "⟦IMG:";

		public const string ImageSuffix = "⟧";

		public static async Task<string> FetchAsync(ShortStory story, string imageDir = null, CancellationToken ct = default(CancellationToken))
		{
			string obj = await DownloadAsync(story.Url, ct).ConfigureAwait(continueOnCapturedContext: false);
			Match open = Regex.Match(obj, "<div[^>]*class=\"[^\"]*mw-parser-output[^\"]*\"[^>]*>");
			if (!open.Success)
			{
				throw new InvalidOperationException("Unexpected wiki page layout — article body not found.");
			}
			string body = obj.Substring(open.Index + open.Length);
			string[] array = new string[4] { "<div class=\"printfooter\"", "<div id=\"catlinks\"", "id=\"catlinks\"", "<!-- NewPP" };
			foreach (string stop in array)
			{
				int cut = body.IndexOf(stop, StringComparison.Ordinal);
				if (cut > 0)
				{
					body = body.Substring(0, cut);
					break;
				}
			}
			string[] parts = Regex.Split(body, "<h2[^>]*>");
			string intro = ((parts.Length != 0) ? parts[0] : "");
			string textSection = "";
			for (int i = 1; i < parts.Length; i++)
			{
				string headline = HeadlineOf(parts[i]);
				if (headline.Equals("Text", StringComparison.OrdinalIgnoreCase) || headline.StartsWith("Text", StringComparison.OrdinalIgnoreCase))
				{
					textSection = parts[i];
					break;
				}
			}
			if (textSection.Length == 0 && parts.Length > 1)
			{
				textSection = parts[1];
			}
			textSection = AfterHeading(textSection);
			textSection = MarkImages(textSection);
			StringBuilder sb = new StringBuilder();
			string note = CleanHtml(StripBanners(intro));
			if (note.Length > 0)
			{
				sb.Append("Note from Wiki").Append("\n\n").Append(note)
					.Append("\n\n")
					.Append("· · ·")
					.Append("\n\n");
			}
			sb.Append(CleanHtml(textSection));
			sb.Append("\n\n").Append("· · ·").Append("\n\n")
				.Append("Source: Guild Wars 2 Wiki — ")
				.Append(story.Title)
				.Append('\n')
				.Append(story.Url);
			string text = sb.ToString().Trim();
			return string.IsNullOrEmpty(imageDir) ? Regex.Replace(text, "⟦IMG:[^⟧]+⟧\\s*", "") : (await DownloadImagesAsync(text, imageDir, ct).ConfigureAwait(continueOnCapturedContext: false));
		}

		private static string MarkImages(string html)
		{
			return Regex.Replace(html, "<img[^>]*>", delegate(Match m)
			{
				Match match = Regex.Match(m.Value, "src=\"([^\"]+)\"");
				if (!match.Success)
				{
					return " ";
				}
				string value = match.Groups[1].Value;
				value = Regex.Replace(value, "/thumb(/.+?\\.(?:png|jpg|jpeg|gif))/[^/]+$", "$1", RegexOptions.IgnoreCase);
				if (value.StartsWith("//"))
				{
					value = "https:" + value;
				}
				else if (value.StartsWith("/"))
				{
					value = "https://wiki.guildwars2.com" + value;
				}
				return (value.IndexOf("/skins/", StringComparison.OrdinalIgnoreCase) >= 0) ? " " : ("\n\n⟦IMG:" + value + "⟧\n\n");
			}, RegexOptions.IgnoreCase);
		}

		private static async Task<string> DownloadImagesAsync(string text, string imageDir, CancellationToken ct)
		{
			Directory.CreateDirectory(imageDir);
			MatchCollection matches = Regex.Matches(text, "⟦IMG:([^⟧]+)⟧");
			int done = 0;
			foreach (Match i in matches)
			{
				string url = i.Groups[1].Value;
				string marker = i.Value;
				if (done >= 8)
				{
					text = text.Replace(marker, "");
					continue;
				}
				try
				{
					string name = SafeFileName(url);
					string path = Path.Combine(imageDir, name);
					if (!File.Exists(path))
					{
						byte[] data = await DownloadBytesAsync(url, ct).ConfigureAwait(continueOnCapturedContext: false);
						if (data.Length < 512)
						{
							throw new Exception("too small");
						}
						File.WriteAllBytes(path, data);
					}
					text = text.Replace(marker, "⟦IMG:" + name + "⟧");
					done++;
				}
				catch
				{
					text = text.Replace(marker, "");
				}
			}
			return text;
		}

		private static string SafeFileName(string url)
		{
			string leaf = url.Substring(url.LastIndexOf('/') + 1);
			leaf = Uri.UnescapeDataString(leaf);
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char c in invalidFileNameChars)
			{
				leaf = leaf.Replace(c, '_');
			}
			if (leaf.Length <= 80)
			{
				return leaf;
			}
			return leaf.Substring(leaf.Length - 80);
		}

		private static async Task<byte[]> DownloadBytesAsync(string url, CancellationToken ct)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			request.UserAgent = "LorebookCodexAndTTS/0.8 (Blish HUD module; https://github.com/frtocheeese-ops/lorebook-reader)";
			request.Timeout = 20000;
			using (ct.Register(delegate
			{
				try
				{
					request.Abort();
				}
				catch
				{
				}
			}))
			{
				using HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(continueOnCapturedContext: false));
				using Stream stream = response.GetResponseStream();
				using MemoryStream ms = new MemoryStream();
				await stream.CopyToAsync(ms).ConfigureAwait(continueOnCapturedContext: false);
				return ms.ToArray();
			}
		}

		private static async Task<string> DownloadAsync(string url, CancellationToken ct)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";
			request.UserAgent = "LorebookCodexAndTTS/0.8 (Blish HUD module; https://github.com/frtocheeese-ops/lorebook-reader)";
			request.Timeout = 20000;
			using (ct.Register(delegate
			{
				try
				{
					request.Abort();
				}
				catch
				{
				}
			}))
			{
				using HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(continueOnCapturedContext: false));
				using Stream stream = response.GetResponseStream();
				using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
				return await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private static string Slice(string src, string from, string to)
		{
			int a = src.IndexOf(from, StringComparison.Ordinal);
			if (a < 0)
			{
				return "";
			}
			a += from.Length;
			int b = src.IndexOf(to, a, StringComparison.Ordinal);
			if (b >= 0)
			{
				return src.Substring(a, b - a);
			}
			return src.Substring(a);
		}

		private static string AfterHeading(string sectionHtml)
		{
			int end = sectionHtml.IndexOf("</h2>", StringComparison.Ordinal);
			if (end >= 0)
			{
				return sectionHtml.Substring(end + 5);
			}
			return sectionHtml;
		}

		private static string HeadlineOf(string sectionHtml)
		{
			Match i = Regex.Match(sectionHtml, "id=\"([^\"]+)\"");
			if (!i.Success)
			{
				return "";
			}
			return i.Groups[1].Value.Replace('_', ' ');
		}

		private static string StripBanners(string html)
		{
			html = Regex.Replace(html, "<table[^>]*>.*?</table>", " ", RegexOptions.Singleline);
			html = Regex.Replace(html, "<div[^>]*class=\"[^\"]*(messagebox|notice|dablink|hatnote)[^\"]*\"[^>]*>.*?</div>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			html = Regex.Replace(html, "This article is copied[^<]*", " ", RegexOptions.IgnoreCase);
			return html;
		}

		private static List<string> ImageCaptions(string html)
		{
			List<string> res = new List<string>();
			foreach (Match item in Regex.Matches(html, "<img[^>]*alt=\"([^\"]+)\""))
			{
				string alt = Decode(item.Groups[1].Value).Trim();
				if (alt.Length > 2 && !res.Contains(alt))
				{
					res.Add(alt);
				}
			}
			return res;
		}

		private static string CleanHtml(string html)
		{
			html = Regex.Replace(html, "<(script|style)[^>]*>.*?</\\1>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			html = Regex.Replace(html, "<span[^>]*class=\"mw-editsection\".*?</span>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			html = Regex.Replace(html, "<br\\s*/?>", "\n", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "</(p|div|li|h[1-6]|blockquote)>", "\n\n", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<li[^>]*>", "- ", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<[^>]+>", "");
			html = Decode(html);
			html = html.Replace("\r", "");
			html = Regex.Replace(html, "[ \\t]+", " ");
			html = Regex.Replace(html, " ?\\n ?", "\n");
			html = Regex.Replace(html, "\\n{3,}", "\n\n");
			return DropWikiChrome(html).Trim();
		}

		private static string DropWikiChrome(string text)
		{
			string[] drop = new string[7] { "From Guild Wars 2 Wiki", "Jump to navigation", "Jump to search", "This article is copied", "verbatim from an official source", "Retrieved from", "[edit]" };
			List<string> keep = new List<string>();
			string[] array = text.Split('\n');
			foreach (string line in array)
			{
				string t = line.Trim();
				bool bad = false;
				string[] array2 = drop;
				foreach (string d in array2)
				{
					if (t.IndexOf(d, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						bad = true;
						break;
					}
				}
				if (!bad)
				{
					keep.Add(line);
				}
			}
			return string.Join("\n", keep);
		}

		private static string Decode(string s)
		{
			return WebUtility.HtmlDecode(s ?? "").Replace('\u00a0', ' ');
		}
	}
}
