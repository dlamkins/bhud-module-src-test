using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Domain.Converters;
using Nekres.Musician.UI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nekres.Musician.Core.Models
{
	internal class MusicSheet
	{
		[JsonProperty("guid")]
		public Guid Id { get; set; }

		[JsonProperty("artist")]
		public string Artist { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("user")]
		public string User { get; set; }

		[JsonProperty("instrument")]
		[JsonConverter(typeof(StringEnumConverter))]
		public Instrument Instrument { get; set; }

		[JsonProperty("tempo")]
		[JsonConverter(typeof(MetronomeConverter))]
		public Metronome Tempo { get; set; }

		[JsonProperty("algorithm")]
		[JsonConverter(typeof(StringEnumConverter))]
		public Algorithm Algorithm { get; set; }

		[JsonProperty("melody")]
		[JsonConverter(typeof(ChordOffsetConverter))]
		public IEnumerable<ChordOffset> Melody { get; set; }

		public MusicSheet()
		{
			Id = Guid.NewGuid();
		}

		public MusicSheetModel ToModel()
		{
			return new MusicSheetModel
			{
				Id = Id,
				Artist = Artist,
				Title = Title,
				User = User,
				Instrument = Instrument,
				Algorithm = Algorithm,
				Melody = string.Join(" ", Melody.Select((ChordOffset c) => c.ToString())),
				Tempo = Tempo.ToString()
			};
		}

		public static MusicSheet FromModel(MusicSheetModel model)
		{
			return new MusicSheet
			{
				Id = model.Id,
				Artist = model.Artist,
				Title = model.Title,
				User = model.User,
				Instrument = model.Instrument,
				Algorithm = model.Algorithm,
				Melody = ChordOffset.MelodyFromString(model.Melody),
				Tempo = Metronome.FromString(model.Tempo)
			};
		}

		private static MusicSheet FromXml(XDocument xDocument)
		{
			string title = xDocument.Elements().SingleOrDefault()?.Elements("title").SingleOrDefault()?.Value ?? "???";
			string artist = xDocument.Elements().SingleOrDefault()?.Elements("artist").SingleOrDefault()?.Value ?? "Unknown Artist";
			string user = xDocument.Elements().SingleOrDefault()?.Elements("user").SingleOrDefault()?.Value ?? string.Empty;
			if (!Enum.TryParse<Instrument>(xDocument.Elements().SingleOrDefault()?.Elements("instrument").SingleOrDefault()?.Value, ignoreCase: true, out var instrument))
			{
				return null;
			}
			string tempo = xDocument.Elements().SingleOrDefault()?.Elements("tempo").SingleOrDefault()?.Value;
			string meter = xDocument.Elements().SingleOrDefault()?.Elements("meter").SingleOrDefault()?.Value;
			if (string.IsNullOrEmpty(tempo) || string.IsNullOrEmpty(meter))
			{
				return null;
			}
			string melody = xDocument.Elements().SingleOrDefault()?.Elements("melody").SingleOrDefault()?.Value;
			if (string.IsNullOrEmpty(melody))
			{
				return null;
			}
			if (!Enum.TryParse<Algorithm>(xDocument.Elements().SingleOrDefault()?.Elements("algorithm").Single().Value.Replace(" ", string.Empty), ignoreCase: true, out var algorithm))
			{
				return null;
			}
			return new MusicSheet
			{
				Title = title,
				Artist = artist,
				User = user,
				Instrument = instrument,
				Tempo = Metronome.FromString(tempo + " " + meter),
				Melody = ChordOffset.MelodyFromString(melody),
				Algorithm = algorithm
			};
		}

		public static MusicSheet FromXml(string path)
		{
			DateTime timeout = DateTime.UtcNow.AddMilliseconds(10000.0);
			while (DateTime.UtcNow < timeout)
			{
				try
				{
					return FromXml(XDocument.Load(path));
				}
				catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
				{
					if (DateTime.UtcNow < timeout)
					{
						continue;
					}
					MusicianModule.Logger.Warn(ex, ex.Message);
					break;
				}
				catch (Exception ex2) when (ex2 is XmlException || ex2 is FormatException)
				{
					MusicianModule.Logger.Info(ex2, "Invalid format or corrupted data: " + Path.GetFileName(path));
					break;
				}
			}
			return null;
		}

		public static bool TryParseXml(string xml, out MusicSheet sheet)
		{
			sheet = null;
			if (string.IsNullOrEmpty(xml))
			{
				return false;
			}
			try
			{
				sheet = FromXml(XDocument.Parse(xml));
				return true;
			}
			catch (XmlException e)
			{
				MusicianModule.Logger.Warn((Exception)e, e.Message);
			}
			return false;
		}
	}
}
