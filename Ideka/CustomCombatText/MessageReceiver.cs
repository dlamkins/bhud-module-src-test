using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	public class MessageReceiver
	{
		private (string template, List<TemplateParser.MarkupFragment> preFragments)? _markupCache;

		public string Name { get; set; } = "";


		public bool Enabled { get; set; } = true;


		public HashSet<MessageCategory> Categories { get; set; } = new HashSet<MessageCategory>();


		public HashSet<EventResult> Results { get; set; } = new HashSet<EventResult>();


		public EntityFilter EntityFilter { get; set; }

		public string Template { get; set; } = "";


		public Color? Color { get; set; }

		public string? FontName { get; set; }

		public int? FontSize { get; set; }

		[JsonIgnore]
		public string Describe
		{
			get
			{
				if (!string.IsNullOrEmpty(Name))
				{
					return Name;
				}
				return "(unnamed)";
			}
		}

		[JsonIgnore]
		public BitmapFont Font => CTextModule.FontAssets.Get(FontName, FontSize);

		[JsonIgnore]
		public IReadOnlyList<TemplateParser.MarkupFragment> MarkupFrags
		{
			get
			{
				if (_markupCache?.template != Template)
				{
					_markupCache = new(string, List<TemplateParser.MarkupFragment>)?((Template, TemplateParser.ParseMarkup(Template)));
				}
				return _markupCache.Value.preFragments;
			}
		}

		public bool Matches(Message message)
		{
			if (Enabled && (!Categories.Any() || Categories.Contains(message.Category)) && (!Results.Any() || Results.Contains(message.Result)) && (EntityFilter != EntityFilter.TargetOnly || message.IsOnTarget || message.IsFromTarget))
			{
				if (EntityFilter == EntityFilter.NonSelf)
				{
					return !message.IsSelf;
				}
				return true;
			}
			return false;
		}
	}
}
