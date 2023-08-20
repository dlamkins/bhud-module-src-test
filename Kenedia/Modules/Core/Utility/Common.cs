using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Blish_HUD;
using Blish_HUD.Content;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.Core.Utility
{
	public static class Common
	{
		private static char[] s_invalids;

		public static double Now()
		{
			return GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
		}

		public static bool SetProperty<T>(ref T property, T newValue, ValueChangedEventHandler<T> OnUpdated, bool triggerOnUpdate = true)
		{
			if (SetProperty(ref property, newValue))
			{
				if (triggerOnUpdate)
				{
					OnUpdated?.Invoke(property, new ValueChangedEventArgs<T>(property, newValue));
				}
				return true;
			}
			return false;
		}

		public static bool SetProperty<T>(ref T property, T newValue, PropertyChangedEventHandler OnUpdated, bool triggerOnUpdate = true, [CallerMemberName] string propName = null)
		{
			if (SetProperty(ref property, newValue))
			{
				if (triggerOnUpdate)
				{
					OnUpdated?.Invoke(property, new PropertyChangedEventArgs(propName));
				}
				return true;
			}
			return false;
		}

		public static bool SetProperty<T>(ref T property, T newValue, Action OnUpdated, bool triggerOnUpdate = true)
		{
			if (SetProperty(ref property, newValue))
			{
				if (triggerOnUpdate)
				{
					OnUpdated?.Invoke();
				}
				return true;
			}
			return false;
		}

		public static bool SetProperty<T>(ref T property, T newValue)
		{
			if (object.Equals(property, newValue))
			{
				return false;
			}
			property = newValue;
			return true;
		}

		public static T GetPropertyValue<T>(object obj, string propName)
		{
			PropertyInfo p = obj.GetType().GetProperty(propName);
			if (p == null)
			{
				return default(T);
			}
			object o = p.GetValue(obj, null);
			if (o == null)
			{
				return default(T);
			}
			if (o.GetType() == typeof(T))
			{
				return (T)o;
			}
			return default(T);
		}

		public static string GetPropertyValueAsString(object obj, string propName)
		{
			PropertyInfo p = obj.GetType().GetProperty(propName);
			if (p == null)
			{
				return null;
			}
			return p.GetValue(obj, null)?.ToString();
		}

		public static int GetAssetIdFromRenderUrl(this RenderUrl? url)
		{
			if (!url.HasValue)
			{
				return 0;
			}
			string s = url.ToString();
			int pos = s.LastIndexOf("/") + 1;
			if (!int.TryParse(s.Substring(pos, s.Length - pos - 4), out var id))
			{
				return 0;
			}
			return id;
		}

		public static int GetAssetIdFromRenderUrl(this RenderUrl url)
		{
			string s = ((object)(RenderUrl)(ref url)).ToString();
			int pos = s.LastIndexOf("/") + 1;
			if (!int.TryParse(s.Substring(pos, s.Length - pos - 4), out var id))
			{
				return 0;
			}
			return id;
		}

		public static int GetAssetIdFromRenderUrl(string s)
		{
			int pos = s.ToString().LastIndexOf("/") + 1;
			if (!int.TryParse(s.Substring(pos, s.Length - pos - 4), out var id))
			{
				return 0;
			}
			return id;
		}

		public static AsyncTexture2D GetAssetFromRenderUrl(this RenderUrl? url)
		{
			if (!url.HasValue)
			{
				return null;
			}
			string s = url.ToString();
			int pos = url.ToString().LastIndexOf("/") + 1;
			if (int.TryParse(s.Substring(pos, s.Length - pos - 4), out var id))
			{
				return AsyncTexture2D.FromAssetId(id);
			}
			return null;
		}

		public static string MakeValidFileName(string text, char? replacement = '_', bool fancy = true)
		{
			StringBuilder sb = new StringBuilder(text.Length);
			char[] invalids = s_invalids ?? (s_invalids = Path.GetInvalidFileNameChars());
			bool changed = false;
			foreach (char c in text)
			{
				if (invalids.Contains(c))
				{
					changed = true;
					char repl = replacement.GetValueOrDefault();
					if (fancy)
					{
						switch (c)
						{
						case '"':
							repl = '”';
							break;
						case '\'':
							repl = '’';
							break;
						case '/':
							repl = '⁄';
							break;
						}
					}
					if (repl != 0)
					{
						sb.Append(repl);
					}
				}
				else
				{
					sb.Append(c);
				}
			}
			if (sb.Length != 0)
			{
				if (!changed)
				{
					return text;
				}
				return sb.ToString();
			}
			return "_";
		}
	}
}
